using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    class EnemySpawnCreator : BasicObjectCreator
    {
        public override void Create()
        {
            MyObjectName = "EnemySpawn";
            base.Create();
        }

        public override BasicObject ReturnObject()
        {
            return new EnemySpawn();
        }

        public override void Load()
        {
            IconTexture = BlockCreator.BlockTipUp;
            base.Load();
        }
    }
}
