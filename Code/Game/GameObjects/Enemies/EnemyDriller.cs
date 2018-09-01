using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class EnemyDriller : BasicEnemy
    {




        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            MyPush = 0.5f;
            Push = 0.5f;
            DamageToBlock = 15f;
            Life = 150;
            Size = new Vector2(96, 96);

            return base.Create(Size, Position);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(DrillerCreator.DrillerTexture, MyRectangle, Color.Tomato);
            base.Draw();
        }

        public override void CollideWithEnemy(BasicObject Enemy)
        {
            Bounces++;
            Enemy.TakeDamage(50, this, Vector2.Normalize(Enemy.Position - Position));
        }

        public override void Update(GameTime gameTime)
        {

            Speed.Y = Math.Max(0, Speed.Y + 0.1f*(float)gameTime.ElapsedGameTime.Milliseconds/1000f);
            Position.Y += 1;


            base.Update(gameTime);
        }


        public void Explode()
        {
            //new BasicExplosion(Position,150, 250, 0.025f);
            //Die();
        }
    }
}
