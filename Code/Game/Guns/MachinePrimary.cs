using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class MachinePrimary:FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {

            MaxRof = 3.5f;
            MaxBurstSize = 8;
            MaxBurstTime = 500;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new MachineBullet();
        }

    }
}
