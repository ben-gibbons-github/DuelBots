using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class RailBullet : Bullet
    {
        Vector2 PreviousPosition;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            PreviousPosition = Position;
            Accuracy = 0;
            FireSpeed = 0.5f;
            Reps = 20;
            Damage = 100;
            Push = 0.35f;
            MaxLifeTime = 200;

            Size = new Vector2(16);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }

        public override void Update(GameTime gameTime)
        {
            LifeTime += gameTime.ElapsedGameTime.Milliseconds;
            if (LifeTime > MaxLifeTime)
                Destroy();

            for (int i = 0; i < Reps; i++)
            {
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(0.5f, 0.33f, 1), 1);
                ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.025f) - Vector2.Normalize(Speed) * 0.025f, 0, new Color(0.5f, 0.33f, 1), 4);

                Speed += Gravity * gameTime.ElapsedGameTime.Milliseconds;
                Vector2 ToPosition = Position += Speed * gameTime.ElapsedGameTime.Milliseconds;



                List<BasicObject> List = GameManager.MyLevel.CheckForAllList(new Rectangle((int)ToPosition.X, (int)ToPosition.Y, (int)Size.X, (int)Size.Y));

                bool StopMove = false;


                foreach (BasicObject Other in List)
                    if (Other != Creator)
                        if (!Victims.Contains(Other) || !HitObjectsOnce)
                            if (Other.GetType().Equals(typeof(DynamicObject)) || !IgnoreWalls)
                            {
                                Victims.Add(Other);
                                if (HitObject(Other, gameTime))
                                    StopMove = true;
                            }

                if (!StopMove)
                    Position = ToPosition;
                else
                    break;


            }
            ChangePosition();
        }


        public override void Destroy()
        {
            for (int i = 0; i < 20; i++)
            {
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(0.5f, 0.33f, 1), 1);
                ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.025f) - Vector2.Normalize(Speed) * 0.025f, 0, new Color(0.5f, 0.33f, 1), 4);
            }
            
            base.Destroy();
        }

        public override void Draw()
        {
            Render.DrawLine(Position, PreviousPosition, Color.Purple);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            if (Object.GetType().IsSubclassOf(typeof(DynamicObject)))
            {
                return base.HitObject(Object, gameTime);
            }

            return false;
        }
    }
}
