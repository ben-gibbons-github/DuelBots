using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class SlimeGrenade : Bullet
    {
        float MaxSpeed = 0.1f;
        bool Stuck = false;
        BasicObject StuckTo;
        Vector2 StuckOffset;
        Vector2 PreviousPosition;
        public float StuckDetonateTime = 500;

        public int ParticleTime = 0;
        public int MaxParticleTime = 50;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            HitObjectsOnce = false;
            Gravity = new Vector2(0, 0.001f);
            Accuracy = 0;
            FireSpeed = 0.5f;
            Reps = 1;
            Damage = 150;
            Push = 0.15f;
            MaxLifeTime = 30000;
            Range = 150;
            FriendlyFire = true;

            Size = new Vector2(16);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);

            Speed += new Vector2(0, -0.3f);
        }

        public override void Update(GameTime gameTime)
        {
            ParticleTime += gameTime.ElapsedGameTime.Milliseconds;
            while (ParticleTime > MaxParticleTime)
            {
                ParticleTime -= MaxParticleTime;
                ParticleSystem.Add(ParticleType.Circle, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(0.25f,1, 0.5f), 2);

                if (LifeTime > MaxLifeTime - StuckDetonateTime)
                {
                     ParticleSystem.Add(ParticleType.Glow, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(0.25f, 1, 0.5f), (LifeTime - (MaxLifeTime - StuckDetonateTime)) / (StuckDetonateTime) * 10);
                }
            }

            PreviousPosition = Position;
            if (!Stuck)
            {
                BasicObject Enemy = GameManager.MyLevel.GetNearestEnemy(this);

                if (Enemy != null)
                {
                    float Distance = Vector2.Distance(Enemy.Position + Enemy.Size / 2, Position + Size / 2);

                    if (Distance < Range)
                        Speed = Vector2.Distance(Speed, Vector2.Zero) * Vector2.Normalize((Enemy.Position+Enemy.Size/2) - (Position+Size/2));
                    {
                        if (Distance < 24)
                            StickTo(Enemy);
                    }
                }
 
            }
            else
            {
                if (StuckTo != null)
                {
                    bool Go = true;
                    if (StuckTo.GetType().Equals(typeof(Player)))
                    {
                        Player p = StuckTo as Player;
                        if (p.IsDashing)
                            Go = false;
                    }

                    if (Go)
                    {
                        Position = StuckTo.Position - StuckOffset;
                        Speed = Vector2.Zero;
                        Gravity = Vector2.Zero;
                        Reps = 0;
                        ChangePosition();
                        LifeTime = (int)Math.Max(LifeTime, MaxLifeTime - StuckDetonateTime);
                    }
                    else
                        Stuck = false;
                }

            }

            base.Update(gameTime);
        }

        public void StickTo(BasicObject other)
        {
            Stuck = true;
            StuckTo = other;
            StuckOffset = other.Position-Position;
            Speed = Vector2.Zero;
        }

        public override bool HitObject(BasicObject Object,GameTime gameTime)
        {
            if (Object.GetType().Equals(typeof(Block)))
            {
                if (Vector2.Distance(Vector2.Zero, Speed) < MaxSpeed)
                {
                    Speed = Vector2.Zero;
                    StickTo(Object);
                }
                else
                {
                    Speed *= 0.85f;
                    Speed = Object.Bounce(MyRectangle, Speed, (int)Math.Max(Speed.X, Speed.Y) * gameTime.ElapsedGameTime.Milliseconds);
                }
            }
            return true;
        }

        public override void Destroy()
        {

            for (int i = 0; i < 20; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.25f), 0, new Color(0.33f, 1, 0.33f), 1);
            for (int i = 0; i < 4; i++)
                ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.005f), 0, new Color(0.33f, 1, 0.33f), 25);
            for (int i = 0; i < 1; i++)
                ParticleSystem.Add(ParticleType.Ring, Position, Vector2.Zero, 0, new Color(0.33f , 1, 0.33f), 1);
            for (int i = 0; i < 4; i++)
                ParticleSystem.Add(ParticleType.Explosion, Position, Bullet.RandomSpeed(0.05f), 0);

            GameManager.MyLevel.DistanceDamage(this);
            base.Destroy();
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(SlimeSecondary.GrenadeTexture, MyRectangle, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }
    }
}
