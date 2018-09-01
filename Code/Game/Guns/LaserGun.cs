using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class LaserGun : GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {
            Primary = new LaserPrimary().Create(this);
            return base.Create(Creator);
        }
    }
}
