﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class CaveBack : BackgroundBasic
    {
        public override BackgroundBasic Create(int i)
        {
            AddLayers(new BackgroundLayer(0, new Rectangle(0, 0, Game1.ResolutionX, Game1.ResolutionY), Game1.contentManager.Load<Texture2D>("Game/Backgrounds/DigitalBack"), Color.White));
            AddLayers(new BackgroundLayer(0.1f, new Rectangle(0, 0, 3000, 2000), Game1.contentManager.Load<Texture2D>("Game/Backgrounds/DigitalMiddle"), new Color(Vector3.One * 2)));
            AddLayers(new BackgroundLayer(0f, new Rectangle(0, 0, 2000, 2000), Game1.contentManager.Load<Texture2D>("Game/Backgrounds/DigitalFront"), Color.White));

            return base.Create(i);
        }
    }
}
