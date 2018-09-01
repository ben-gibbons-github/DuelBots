using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class LaserTurret : Bullet
    {
        bool ReadyToDestroy = false;
        LaserGun MyGun;
        public int ParticleTime = 0;
        public int MaxParticleTime = 25;
        public static Texture2D Texture;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            if (Texture == null)
                Texture = Game1.contentManager.Load<Texture2D>("Game/Turret");

            MyGun = (LaserGun)new LaserGun().Create(Creator);
            Accuracy = 0;
            FireSpeed = 0.6f;
            Reps = 1;
            Damage = 0;
            Push = 0f;
            MaxLifeTime = 1500;
            Range = 0;
            FriendlyFire = true;

            Size = new Vector2(24);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            if(LifeTime>1)
            ParticleTime += (int)Math.Max(1,(float)gameTime.ElapsedGameTime.Milliseconds/Math.Max(1,LifeTime/75));
            while (ParticleTime > MaxParticleTime)
            {
                ParticleTime -= MaxParticleTime;
                ParticleSystem.Add(ParticleType.Square, Position + Size / 2, Vector2.Zero, (float)MathHelper.ToRadians((float)Bullet.NewRandom.NextDouble() * 360), new Color(1, 0.25f, 0.25f), 2);
            }

            MyGun.Update(gameTime);

            Speed *= 0.975f;
            BasicObject Enemy= GameManager.MyLevel.GetNearestEnemy(Creator);

            if (Enemy != null)
                MyGun.Primary.Shoot(Position + Size / 2, Vector2.Normalize((Enemy.Position + Enemy.Size / 2) - (Position + Size / 2)));

            if (MyGun.Primary.BurstSize > 0)
                ReadyToDestroy = true;
            else if (ReadyToDestroy)
                Destroy();

            base.Update(gameTime);
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            Speed *= 0.85f;
            Speed = Object.Bounce(MyRectangle, Speed, (int)Math.Max(Speed.X, Speed.Y) * gameTime.ElapsedGameTime.Milliseconds);
            return true;
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(Texture, MyRectangle, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }
    }
}
