using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class MachineBullet:Bullet
    {
        public Vector2 PreviousPosition = Vector2.Zero;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            PreviousPosition = Position;
            Accuracy = 20;
            FireSpeed = 0.95f;
            Reps = 10;
            Damage = 16;
            Push = 0.7f;
            MaxLifeTime = 300;
            WallDamageMult = 2f;

            Size = new Vector2(4);

            base.CreateBullet(Size, Position-Size/2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            base.Update(gameTime);
        }


        public override void Die()
        {
            
            base.Die();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            for (int i = 0; i < 10;i++ )
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f)-Vector2.Normalize(Speed)*0.25f, 0,new Color(1f,0.66f,0.33f),1);
            ParticleSystem.Add(ParticleType.Spark, Position,Vector2.Zero, 0, new Color(1f, 0.66f, 0.33f), 10);
            return base.HitObject(Object, gameTime);
        }

        public override void Draw()
        {
            Render.DrawLine(Position, PreviousPosition,Color.White);
           // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }
    }
}
