using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class SwordDart : Bullet
    {
        Vector2 PreviousPosition;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            Accuracy = 3;
            FireSpeed = 0.35f;
            Reps = 2;
            Damage = 12;
            Push = 1f;
            MaxLifeTime = 6000;

            Size = new Vector2(16);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }


        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Render.DrawSquare(Position, Position-Speed*200, 5, SwordSecondary.DartSquare, Color.White);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override void Destroy()
        {
            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(1f, 1, 1f), 1);
            ParticleSystem.Add(ParticleType.Spark, Position, Vector2.Zero, 0, new Color(1f, 1f, 1f), 10);
            base.Destroy();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            if (Object.GetType().IsSubclassOf(typeof(DynamicObject)))
            {
                return base.HitObject(Object, gameTime);
            }
            else
            {
                Object.TakeDamage(gameTime.ElapsedGameTime.Milliseconds / 1000f * 60f*Damage, this, Vector2.Normalize(Speed));
                return false;
            }
        }
    }
}
