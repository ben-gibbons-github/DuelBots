using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class GunBasic
    {
        public BasicObject Creator;

        public FireMode Primary;
        public FireMode Secondary;

        public Texture2D LeftImage;
        public Texture2D RightImage;

        public Color MyColor;

        public virtual GunBasic Create(BasicObject Creator)
        {
            this.Creator = Creator;
            return this;
        }

        public virtual void Update(GameTime gameTime)
        {
            Primary.Update(gameTime);
            if (Secondary != null) 
            Secondary.Update(gameTime);
        }


    }
}
