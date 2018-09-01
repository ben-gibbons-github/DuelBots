using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    class DrillerCreator : BasicObjectCreator
    {
        public static Texture2D DrillerTexture;

        public override void Create()
        {
            IsTimeLineCreator = true;
            DrillerTexture = Game1.contentManager.Load<Texture2D>("Game/Driller");
            MyObjectName = "EnemyDriller";
            base.Create();
        }

        public override void TimeEvent(Vector2 Position)
        {
            GameManager.MyLevel.AddDynamic(new EnemyDriller().Create(Vector2.Zero, Position));

            base.TimeEvent(Position);
        }

        public override BasicObject ReturnObject()
        {
            return new TimeBasic();
        }

        public override void Load()
        {
            IconTexture = DrillerTexture;
            base.Load();
        }
    }
}
