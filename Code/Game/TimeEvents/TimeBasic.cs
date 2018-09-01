using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public delegate void TimeEvent(Vector2 Position);

    

    public class TimeBasic:BasicObject
    {

        public Vector2 SpawnPosition;

        public int MyTime;
        public TimeEvent MyEvent;

        public int MaxEventPause;
        public int EventPause;

        public int EventReps;
        public int MaxEventReps;

        public Texture2D MyTexture;


        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            MyTime = (int)Math.Floor(Position.X / 16);
            return base.Create(Size, Position);
        }

        public virtual void DoEvent()
        {
            if (MyEvent != null)
                MyEvent(GetSpawnPosition());
        }


        public Vector2 GetSpawnPosition()
        {
            List<EnemySpawn> Choices = new List<EnemySpawn>();

            foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                if (Object.GetType().Equals(typeof(EnemySpawn)))
                    if (GameManager.MyLevel.CheckForAllCollision(Object.MyRectangle,Object) == null)
                        Choices.Add((EnemySpawn)Object);

            if (Choices.Count() > 0)
                return Choices[(int)Math.Floor(new Random().NextDouble() * Choices.Count())].Position;
            else
                return Vector2.Zero;
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(MyTexture, MyRectangle, Color.White);
            base.Draw();
        }

        public override void Delete()
        {
            GameManager.MyLevel.DeleteTime(this);
            base.Delete();
        }

        public override void Read(System.IO.BinaryReader Reader)
        {
            MyTime = (int)Math.Floor(Position.X / 16);
            base.Read(Reader);
        }

    }
}
