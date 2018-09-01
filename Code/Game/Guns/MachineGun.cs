using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class MachineGun:GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {
            MyColor = new Color(1, 0.66f, 0.33f);

            LeftImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/mg_holdL");
            RightImage = Game1.contentManager.Load<Microsoft.Xna.Framework.Graphics.Texture2D>("Game/Weapons/mg_hold");

            Primary = new MachinePrimary().Create(this);
            Secondary = new MachineSecondary().Create(this);
            return base.Create(Creator);
        }
    }
}
