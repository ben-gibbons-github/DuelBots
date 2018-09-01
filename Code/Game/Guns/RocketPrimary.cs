using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class RocketPrimary : FireMode
    {
        public static Texture2D LaserTexture;

        public override FireMode Create(GunBasic ParentGun)
        {
            LaserTexture = Game1.contentManager.Load<Texture2D>("Game/Particles/LaserSquare");

            MaxRof = 3;
            MaxBurstSize = 4;
            MaxBurstTime = 800;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new Rocket();
        }

    }
}
