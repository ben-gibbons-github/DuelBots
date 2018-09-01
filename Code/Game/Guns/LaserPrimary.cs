using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class LaserPrimary : FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {

            MaxRof = 25;
            MaxBurstSize = 3;
            MaxBurstTime = 150;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new LaserBullet();
        }

    }
}
