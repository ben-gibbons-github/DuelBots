using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class SlimePrimary : FireMode
    {
        public static Texture2D SlimeTexture;

        public override FireMode Create(GunBasic ParentGun)
        {
            SlimeTexture = Game1.contentManager.Load<Texture2D>("Game/Particles/SlimeSquare");
            MaxRof = 15f;
            MaxBurstSize = 3;
            MaxBurstTime = 500;
            BulletNumb = 3;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new SlimeBullet();
        }

    }
}
