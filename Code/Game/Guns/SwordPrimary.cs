using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class SwordPrimary : FireMode
    {
        public override FireMode Create(GunBasic ParentGun)
        {
            MaxRof = 1f;
            MaxBurstSize = 1;
            MaxBurstTime = 1200;
            BulletNumb = 1;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new SwordSlash();
        }

    }
}
