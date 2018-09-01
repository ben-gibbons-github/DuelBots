using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class SwordSecondary : FireMode
    {
        public static Texture2D DartSquare;
        public override FireMode Create(GunBasic ParentGun)
        {
            if (DartSquare == null)
                DartSquare = Game1.contentManager.Load<Texture2D>("Game/Particles/DartSquare");
            MaxRof = 15f;
            MaxBurstSize = 3;
            MaxBurstTime = 1200;
            BulletNumb = 1;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new SwordDart();
        }

    }
}
