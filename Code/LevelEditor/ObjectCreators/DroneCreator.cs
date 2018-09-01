using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    class DroneCreator : BasicObjectCreator
    {
        public static Texture2D DroneTexture;

        public override void Create()
        {
            IsTimeLineCreator = true;
            DroneTexture = SpawnCreator.PlayerSphere;
            MyObjectName = "EnemyDrone";
            base.Create();
        }

        public override void TimeEvent(Vector2 Position)
        {
            GameManager.MyLevel.AddDynamic(new EnemyDrone().Create(Vector2.Zero,Position));

            base.TimeEvent(Position);
        }

        public override BasicObject ReturnObject()
        {
            return new TimeBasic();
        }

        public override void Load()
        {
            IconTexture = BlockCreator.BlockFullUp;
            base.Load();
        }
    }
}
