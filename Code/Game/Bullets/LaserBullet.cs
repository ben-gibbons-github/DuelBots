using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class LaserBullet : Bullet
    {
        public Vector2 PreviousPosition = Vector2.Zero;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            PreviousPosition = Position;
            Accuracy = 0;
            FireSpeed = 0.75f;
            Reps = 2;
            Damage = 35;
            Push = 0.75f;
            MaxLifeTime = 1500;
            WallDamageMult = 2.5f;

            Size = new Vector2(4);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            base.Update(gameTime);
        }


        public override void Draw()
        {
            Render.DrawSquare(Position, PreviousPosition, 10, RocketPrimary.LaserTexture, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(1f, 0.33f, 0.33f), 1);
            ParticleSystem.Add(ParticleType.Spark, Position, Vector2.Zero, 0, new Color(1f, 0.33f, 0.33f), 10);
            return base.HitObject(Object, gameTime);
        }
    }
}
