using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class TimeManager
    {
        public int CurrentSecond;
        public int TimeTillNextSecond;
        public Level MyLevel;

        public List<TimeBasic>[] TimeArray;
        public List<TimeBasic> ActiveEvents = new List<TimeBasic>();

        public int EventTime;
        public int MaxEventTime = 250;


        public TimeManager Create(Level MyLevel)
        {
            TimeTillNextSecond = 1000;
            this.MyLevel = MyLevel;

            GenerateLists();
            return this;
        }

        public void GenerateLists()
        {
            int TimeSize = 0;

            foreach (TimeBasic Time in MyLevel.TimeEvents)
                TimeSize = (int)Math.Max(TimeSize, Time.MyTime);

            TimeArray = new List<TimeBasic>[TimeSize+1];

            foreach (TimeBasic Time in MyLevel.TimeEvents)
            {
                if (TimeArray[Time.MyTime] == null)
                    TimeArray[Time.MyTime] = new List<TimeBasic>();

                TimeArray[Time.MyTime].Add(Time);
            }

            CurrentSecond = 0;
            TimeTillNextSecond = 1000;
        }


        public void DumpLists()
        {
            TimeArray = null;
            CurrentSecond = 0;
            TimeTillNextSecond = 1000;
            ActiveEvents.Clear();
        }

        public void Update(GameTime gameTime)
        {
            TimeTillNextSecond -= gameTime.ElapsedGameTime.Milliseconds;
            if (TimeTillNextSecond <= 0)
            {
                TimeTillNextSecond += 1000;
                CurrentSecond += 1;
                if (CurrentSecond < TimeArray.Length)
                    if (TimeArray[CurrentSecond] != null)
                        foreach (TimeBasic Time in TimeArray[CurrentSecond])
                            ActiveEvents.Add(Time);
            }

            if (ActiveEvents.Count > 0)
            {
                EventTime += gameTime.ElapsedGameTime.Milliseconds;
                if (EventTime > MaxEventTime)
                {
                    EventTime -= MaxEventTime;
                    ActiveEvents[ActiveEvents.Count - 1].DoEvent();
                    ActiveEvents.Remove(ActiveEvents[ActiveEvents.Count - 1]);
                }
            }
            else
                EventTime = 0;

        }

        public void Draw()
        {
            Game1.spriteBatch.DrawString(MenuManager.Font, CurrentSecond.ToString() + " " + TimeTillNextSecond.ToString(), -new Vector2(GameManager.MyLevel.MyCamera.MyRectangle.X, GameManager.MyLevel.MyCamera.MyRectangle.Y) + new Vector2(100), Color.White);
        }

    }
}
