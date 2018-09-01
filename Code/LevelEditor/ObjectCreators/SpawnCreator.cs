using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    class SpawnCreator : BasicObjectCreator
    {

        public static Texture2D PlayerSphere;

        public override void Create()
        {
            MyObjectName = "PlayerSpawn";
            base.Create();
        }

        public override BasicObject ReturnObject()
        {
            return new PlayerSpawn();
        }

        public override void Load()
        {
            PlayerSphere = Game1.contentManager.Load<Texture2D>("Game/sphere");
            IconTexture = PlayerSphere;
            base.Load();
        }
    }
}
