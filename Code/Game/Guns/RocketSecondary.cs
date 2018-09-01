using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class RocketSecondary : FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {

            MaxRof = 0;
            MaxBurstSize = 1;
            MaxBurstTime = 2000;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new LaserTurret();
        }

    }
}
