using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class SlimeGun : GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {

            MyColor = new Color(0.33f, 1, 0.66f);

            LeftImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/green_holdL");
            RightImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/green_hold");

            Primary = new SlimePrimary().Create(this);
            Secondary = new SlimeSecondary().Create(this);
            return base.Create(Creator);
        }
    }
}
