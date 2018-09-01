using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class MachineGrenade : Bullet
    {
        public Vector2 PreviousPosition = Vector2.Zero;
        public int Hits = 3;
        public int ParticleTime = 0;
        public int MaxParticleTime = 50;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            Gravity= new Vector2(0,0.001f);
            PreviousPosition = Position;
            Accuracy = 0;
            FireSpeed = 0.5f;
            Reps = 1;
            Damage = 150;
            Push = 0.15f;
            MaxLifeTime = 3000;
            Range = 150;
            FriendlyFire = true;

            Size = new Vector2(24);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);

            Speed += new Vector2(0, -0.3f);
        }

        public override void Update(GameTime gameTime)
        {
            ParticleTime += gameTime.ElapsedGameTime.Milliseconds;
            while (ParticleTime > MaxParticleTime)
            {
                ParticleTime -= MaxParticleTime;
                ParticleSystem.Add(ParticleType.Smoke, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians( (float)Bullet.NewRandom.NextDouble()*360), new Color(Vector4.One * 500), 1);
                ParticleSystem.Add(ParticleType.Hex, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(1,0.25f,0.25f), 2);
            
            }

            PreviousPosition = Position;
            base.Update(gameTime);
        }


        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            if (Object.Damage < Object.Life)
            {
                if (Hits > 0 && Object.GetType().Equals(typeof(Block)) && Vector2.Distance(Position, GameManager.MyLevel.GetNearestEnemy(this).Position) > Range)
                {
                    Hits--;
                    Object.TakeDamage(1000, this, Vector2.Zero);
                    Speed *=0.5f;
                }
                else
                    Destroy();
            }
            return true;
        }

        public override void Destroy()
        {
            GameManager.MyLevel.DistanceDamage(this);

            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(1f, 0.66f, 0.33f), 1);
            for (int i = 0; i < 4; i++)
                ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.005f) - Vector2.Normalize(Speed) * 0.005f, 0, new Color(1, 0.66f, 0.33f), 25);
            for (int i = 0; i < 1; i++)
                ParticleSystem.Add(ParticleType.Ring, Position, Vector2.Zero, 0, new Color(1, 0.66f, 0.33f), 1);
            for (int i = 0; i < 4; i++)
                ParticleSystem.Add(ParticleType.Explosion, Position, Bullet.RandomSpeed(0.05f), 0);

            base.Destroy();
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(MachineSecondary.GrenadeTexture, MyRectangle, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }
    }
}
