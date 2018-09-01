using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class Instancer
    {
        public static BasicObject CreateInstanceOf(BasicObjectCreator Creator,Level level)
        {
            BasicObject NewObject = null;

            if (!Creator.HasLoaded)
                Creator.Load();

            NewObject = Creator.ReturnObject();

            if (NewObject != null)
            {
                NewObject.CreatorString = Creator.MyObjectName;

                if (NewObject.GetType().Equals(typeof(TimeBasic)))
                {
                    level.TimeEvents.Add((TimeBasic)NewObject);
                    SetAsTime((TimeBasic)NewObject, Creator);
                }
                else
                    level.AddObject(NewObject);
            }


            return NewObject;
        }


        public static BasicObject CreateInstanceOf(string Type, Level level)
        {
            BasicObject NewObject = null;

            foreach (BasicObjectCreator Creator in BasicObjectCreator.BasicObjectTypes)
            {

                if (Type == Creator.MyObjectName)
                {
                    if (!Creator.HasLoaded)
                        Creator.Create();
                    NewObject = Creator.ReturnObject();



                    if (NewObject != null)
                    {
                        NewObject.CreatorString = Creator.MyObjectName;

                        if (NewObject.GetType().Equals(typeof(TimeBasic)))
                        {
                            level.TimeEvents.Add((TimeBasic)NewObject);
                            SetAsTime((TimeBasic)NewObject, Creator);
                        }
                        else
                            level.AddObject(NewObject);
                    }

                }
            }
            Console.Write(Type);



            return NewObject;
        }


        public static void SetAsTime(TimeBasic NewObject, BasicObjectCreator Creator)
        {
            NewObject.MyEvent = Creator.TimeEvent;
            NewObject.MyTexture = Creator.IconTexture;
        }
    }
}
