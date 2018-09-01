using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class EmpBullet : Bullet
    {
        public int CollisionLifeTime = 250;
        public int ParticleTime = 0;
        public int MaxParticleTime = 50;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            WallDamageMult = 0.5f;
            Accuracy = 0;
            FireSpeed = 0.45f;
            Reps = 1;
            Damage = 100;
            Push = 0.75f;
            MaxLifeTime = 1500;
            Range = 150;

            Size = new Vector2(16);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            ParticleTime += gameTime.ElapsedGameTime.Milliseconds;
            while (ParticleTime > MaxParticleTime)
            {
                ParticleTime -= MaxParticleTime;
                ParticleSystem.Add(ParticleType.Dust, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(0.2f,0.33f,1), 4);
                ParticleSystem.Add(ParticleType.Hex, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(0.2f, 0.33f, 1), 4);
           
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(SpawnCreator.PlayerSphere, MyRectangle, Color.Purple);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            if (Object.GetType().IsSubclassOf(typeof(DynamicObject)))
            {
                return base.HitObject(Object, gameTime);
            }
            else
            {
                //if (LifeTime < MaxLifeTime - CollisionLifeTime)
                LifeTime = MaxLifeTime - CollisionLifeTime;
                return false;
            }
        }

        public override void Destroy()
        {
            for (int i = 0; i < 20; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.25f), 0, new Color(0.2f, 0.33f, 1), 1);
            for (int i = 0; i < 4; i++)
                ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.005f), 0, new Color(0.2f, 0.33f, 1), 25);
            for (int i = 0; i < 1; i++)
                ParticleSystem.Add(ParticleType.Ring, Position, Vector2.Zero, 0, new Color(0.2f, 0.33f, 1), 1);
            for (int i = 0; i < 1; i++)
                ParticleSystem.Add(ParticleType.Explosion, Position, Bullet.RandomSpeed(0.05f), 0);


            GameManager.MyLevel.DistanceDamage(this);
            base.Destroy();
        }
    }
}
