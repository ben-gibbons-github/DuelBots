using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class SlimeBullet : Bullet
    {
        public Vector2 PreviousPosition = Vector2.Zero;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            PreviousPosition = Position;
            Accuracy = 25;
            FireSpeed = 0.35f;
            Reps = 3;
            Damage = 26;
            Push = 0.725f;
            MaxLifeTime = 3000;
            Gravity = new Vector2(0, 0.00005f);
            WallDamageMult = 3;

            Size = new Vector2(12);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition += (Position - PreviousPosition)*0.15f;
            base.Update(gameTime);
        }


        public override void Draw()
        {
            Render.DrawSquare(Position, PreviousPosition, 10, SlimePrimary.SlimeTexture, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(0.33f, 1, 0.33f), 1);
            ParticleSystem.Add(ParticleType.Spark, Position, Vector2.Zero, 0, new Color(0.33f, 1, 0.33f), 10);
            return base.HitObject(Object, gameTime);
        }
    }
}
