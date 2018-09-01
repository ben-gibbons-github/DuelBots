using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class Bullet:DynamicObject
    {
        

        public static Random NewRandom = new Random(159);

        public float Accuracy = 0;
        public float FireSpeed = 0;

        public bool IgnoreWalls = false;

        public bool HitObjectsOnce = true;
       

        public int Reps = 1;

        public List<BasicObject> Victims= new List<BasicObject>();

        public float Range;

        public int LifeTime = 0;
        public int MaxLifeTime = 1000;

        public virtual void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            this.Team = Creator.Team;
            float Dire = (float)Math.Atan2(Direction.X, Direction.Y) - (float)Math.PI;
            Dire += MathHelper.ToRadians((float)(Accuracy / 2 - NewRandom.NextDouble() * Accuracy));

            Speed = -new Vector2((float)Math.Sin(Dire), (float)Math.Cos(Dire)) * FireSpeed;

            this.Creator = Creator;

            Create(Size, Position);
        }

        public override void Update(GameTime gameTime)
        {
            LifeTime += gameTime.ElapsedGameTime.Milliseconds;
            if (LifeTime > MaxLifeTime)
                Destroy();

            for (int i = 0; i < Reps; i++)
            {
                Speed += Gravity * gameTime.ElapsedGameTime.Milliseconds;
                Vector2 ToPosition = Position += Speed * gameTime.ElapsedGameTime.Milliseconds;



                List<BasicObject> List = GameManager.MyLevel.CheckForAllList(new Rectangle((int)ToPosition.X, (int)ToPosition.Y, (int)Size.X, (int)Size.Y));

                bool StopMove=false;


                    foreach(BasicObject Other in List)
                    if (Other != Creator)
                        if (!Victims.Contains(Other) || !HitObjectsOnce)
                            if (Other.GetType().Equals(typeof(DynamicObject)) || !IgnoreWalls)
                            {
                                Victims.Add(Other);
                                if(HitObject(Other, gameTime))
                                StopMove = true;
                            }

                    if (!StopMove)
                        Position = ToPosition;
                    else
                        break;


            }
            ChangePosition();


            base.Update(gameTime);
        }

        public virtual bool HitObject(BasicObject Object,GameTime gameTime)
        {
            Object.TakeDamage(Damage,this,Vector2.Normalize(Speed));

           // if (Object.GetType().Equals(typeof(Block)))
                Destroy();

                return true;
        }


        public static Vector2 RandomSpeed(float Speed)
        {
            return new Vector2((float)(NewRandom.NextDouble() * Speed * 2 - Speed), (float)(NewRandom.NextDouble() * Speed * 2 - Speed));
        }

    }
}
