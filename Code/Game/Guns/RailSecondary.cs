using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class RailSecondary : FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {

            MaxRof = 0f;
            MaxBurstSize = 1;
            MaxBurstTime = 1800;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new EmpBullet();
        }

    }
}
