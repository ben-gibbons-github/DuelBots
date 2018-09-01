using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class MachineSecondary : FireMode
    {
        public static Texture2D GrenadeTexture;

        public override FireMode Create(GunBasic ParentGun)
        {
            GrenadeTexture = Game1.contentManager.Load<Texture2D>("Game/Particles/Grenade");
            MaxRof = 0;
            MaxBurstSize = 1;
            MaxBurstTime = 1200;

            return base.Create(ParentGun);
        }

        public override Bullet CreateBullet()
        {
            return new MachineGrenade();
        }

    }
}
