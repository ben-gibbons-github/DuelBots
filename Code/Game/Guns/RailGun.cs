using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class RailGun : GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {
            MyColor = new Color(0.5f, 0.33f, 1);

            LeftImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/railgun_holdL");
            RightImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/railgun_hold");

            Primary = new RailPrimary().Create(this);
            Secondary = new RailSecondary().Create(this);
            return base.Create(Creator);
        }
    }
}
