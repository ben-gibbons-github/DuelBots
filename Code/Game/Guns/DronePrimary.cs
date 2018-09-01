using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class DronePrimary : FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {

            MaxRof = 10f;
            MaxBurstSize =  3;
            MaxBurstTime = 500;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new DroneBullet();
        }

    }
}
