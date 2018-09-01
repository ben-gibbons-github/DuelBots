using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class BasicEnemy:DynamicObject
    {
        public float MaxPushTime = 20;
        public float PushTime = 0;
        public int Bounces = 0;
        public float DamageToBlock = 0;
        public GunBasic MyGun;
        public float MyPush = 1;

        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
         //   GameManager.MyLevel.MyCamera.Targets.Add(this);

            Team = 3;
            Solid = true;
            Size = new Vector2(32);
            return base.Create(Size, Position);
        }

        public virtual void CollideWithEnemy(BasicObject Enemy)
        {

        }

        public override void Update(GameTime gameTime)
        {
            PushTime -= 1f * (float)gameTime.ElapsedGameTime.Milliseconds / (1000f / 60f);
            if (PushTime < 0)
                if (MyGun != null)
                {
                    MyGun.Update(gameTime);

                    if (MyGun.Primary != null)
                        MyGun.Primary.Shoot(Position + Size / 2, Direction);
                    if (MyGun.Secondary != null)
                        MyGun.Secondary.Shoot(Position + Size / 2, Direction);

                }

            Vector2 ToPosition = Position + ((Speed * gameTime.ElapsedGameTime.Milliseconds));
            int Reps = 1;
            Rectangle ToRectangle = new Rectangle((int)ToPosition.X, (int)ToPosition.Y, (int)Size.X, (int)Size.Y);

            if (GameManager.MyLevel.CheckForSolidCollision(ToRectangle) != null)
                Reps = Math.Max(1, (int)Vector2.Distance(Vector2.Zero, Speed * gameTime.ElapsedGameTime.Milliseconds));

            bool CountedBounce = false;

            for (int i = 1; i < Reps + 1; i++)
            {
                ToPosition = Position + ((Speed * gameTime.ElapsedGameTime.Milliseconds) / Reps);
                ToRectangle = new Rectangle((int)ToPosition.X, (int)ToPosition.Y, (int)Size.X, (int)Size.Y);


                BasicObject Other2 = GameManager.MyLevel.CheckForSolidCollision(ToRectangle);

                if (Other2 == null)
                    Position = ToPosition;
                else
                {
                    if (!CountedBounce&&Other2.GetType().Equals(typeof(Block)))
                    {
                        Other2.TakeDamage(DamageToBlock, null, Vector2.Zero);
                        CountedBounce = true;
                        Bounces++;
                    }

                    Speed *= 0.75f;
                    if (Other2.Damage <= Other2.Life)
                        Speed = Other2.Bounce(MyRectangle, Speed, (int)Math.Max(Speed.X, Speed.Y) * gameTime.ElapsedGameTime.Milliseconds / Reps);
                    Position = ToPosition;
                }
            }

            ChangePosition();

            foreach (BasicObject Object in GameManager.MyLevel.DynamicCollidables)
                if (Object.Team != Team)
                    if (Object.MyRectangle.Intersects(MyRectangle))
                    {
                        CollideWithEnemy(Object);
                        break;
                    }

            base.Update(gameTime);
        }

        public override void TakeDamage(float Damage, BasicObject Damager, Vector2 Direction)
        {
            if(Damager!=null)
            PushTime = Math.Max(PushTime,
                    Damage * (this.Damage + 750) / 100 * Damager.Push
                    )*MyPush;

            Speed = (Vector2.Normalize(Vector2.Normalize(Direction) + new Vector2(0, -0.35f))) *
                Damage * (this.Damage + 750) / 18000 * Damager.Push * MyPush;

            base.TakeDamage(Damage, Damager, Direction);

        }



    }
}
