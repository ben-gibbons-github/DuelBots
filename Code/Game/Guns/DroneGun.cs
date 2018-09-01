using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class DroneGun : GunBasic
    {
        public override GunBasic Create(BasicObject Creator)
        {
            Primary = new DronePrimary().Create(this);
            return base.Create(Creator);
        }
    }
}
