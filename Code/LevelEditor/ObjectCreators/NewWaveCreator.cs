using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    class NewWaveCreator : BasicObjectCreator
    {

        public override void Create()
        {
            IsTimeLineCreator = true;
            MyObjectName = "NewWave";
            base.Create();
        }

        public override void TimeEvent(Vector2 Position)
        {
            GameManager.MyLevel.Wave += 1;

            base.TimeEvent(Position);
        }

        public override BasicObject ReturnObject()
        {
            return new TimeBasic();
        }

        public override void Load()
        {
            IconTexture = Game1.contentManager.Load<Texture2D>("Editor/TimeLine");
            base.Load();
        }
    }
}
