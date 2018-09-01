using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class BasicObjectCreator
    {
       // public Type MyObjectType;
        public bool HasLoaded = false;
        public Texture2D IconTexture;
        public string MyObjectName;
        public bool IsTimeLineCreator=false;

        public static bool HasLoadedTypes = false;
        public static BasicObjectCreator[] BasicObjectTypes = 
        {
            new BlockCreator(),
            new SpawnCreator(),
            new EnemySpawnCreator(),
            new DroneCreator(),
            new NewWaveCreator(),
            new DrillerCreator(),
        };

       // public static Dictionary<Type, BasicObjectCreator> ObjectDictionary = new Dictionary<Type, BasicObjectCreator>();

        public static void LoadList()
        {

            if (!HasLoadedTypes)
            {
                foreach (BasicObjectCreator Creator in BasicObjectTypes)
                {
                    Creator.Create();
                }
                HasLoadedTypes = true;
            }
        }


        public virtual void Create()
        {
            if (!HasLoaded)
            {
                Load();
                HasLoaded = true;
            }
        }

        public virtual void Load()
        {

        }

        public virtual BasicObject ReturnObject()
        {
            return null;
        }

        public virtual void TimeEvent(Vector2 Position)
        {

        }
    }
}
