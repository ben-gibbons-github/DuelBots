using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class RocketLauncher : GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {
            MyColor = new Color(1, 0.5f, 0.5f);

            LeftImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/laser_holdL");
            RightImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/laser_hold");

            Primary = new RocketPrimary().Create(this);
            Secondary = new RocketSecondary().Create(this);
            return base.Create(Creator);
        }
    }
}
