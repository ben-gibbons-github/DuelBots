using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class Sword : GunBasic
    {
        public static Texture2D SwordTexture;

        public override GunBasic Create(BasicObject Creator)
        {
            MyColor = Color.White;

            if (SwordTexture == null)
                SwordTexture = Game1.contentManager.Load<Texture2D>("Game/Sword");

            LeftImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/sword_holdL");
            RightImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/sword_hold");

            Primary = new SwordPrimary().Create(this);
            Secondary = new SwordSecondary().Create(this);
            return base.Create(Creator);
        }
    }
}
