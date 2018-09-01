using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class EnemyDrone:BasicEnemy
    {
        int MaxBounces = 15;
        int DieTime = 0;
        int MaxDieTime = 2000 * 1000;
        


        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            Push = 0.5f;
            DamageToBlock = 0.15f;
            Life = 50;

            MyGun = new DroneGun().Create(this);

            return base.Create(Size, Position);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(DroneCreator.DroneTexture, MyRectangle, Color.Lime);
            base.Draw();
        }

        public override void CollideWithEnemy(BasicObject Enemy)
        {
            //Bounces++;
           // Enemy.TakeDamage(25, this, Vector2.Normalize(Enemy.Position - Position));
        }

        public override void Update(GameTime gameTime)
        {
            if (Bounces >= MaxBounces)
            {
                Speed *= 0.95f;
                DieTime += gameTime.ElapsedGameTime.Milliseconds;

                if (DieTime < MaxDieTime)
                    Explode();
            }
            if (PushTime < 0)
            {
                BasicObject NearestEnemy = GameManager.MyLevel.GetNearestEnemy(this);

                Speed *= 0.95f;


                if (NearestEnemy != null)
                {
                    Direction = Vector2.Normalize(NearestEnemy.Position - Position);

                    if (Vector2.Distance(Position, NearestEnemy.Position) > 100)
                        Speed += 1f * Direction * (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
                    else
                        Speed -= 1f * Direction * (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;


                }
                
            }
            base.Update(gameTime);
        }


        public void Explode()
        {
            //new BasicExplosion(Position,150, 250, 0.025f);
            //Die();
        }
    }
}
