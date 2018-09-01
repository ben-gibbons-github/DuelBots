using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class FireMode
    {
        public GunBasic ParentGun;

        public float Rof = 0;
        public int BurstSize = 0;
        public float BurstTime = 0;
        public int BulletNumb = 1;

        public float MaxRof = 0;
        public int MaxBurstSize = 0;
        public float MaxBurstTime = 0;


        public virtual FireMode Create(GunBasic ParentGun)
        {
            this.ParentGun = ParentGun;
            return this;
        }

        public virtual void Shoot(Vector2 ShootFrom,Vector2 Direction)
        {
            if (BurstSize > 0)
            {
                if (Rof > MaxRof)
                {
                    BurstTime = 0;
                    Rof = 0;
                    BurstSize--;

                    for (int i = 0; i < BulletNumb; i++)
                    {
                        Bullet NewBullet;
                        GameManager.MyLevel.AddDynamic(NewBullet = CreateBullet());
                        NewBullet.CreateBullet(Vector2.Zero, ShootFrom, Direction, ParentGun.Creator);
                        ParticleSystem.Add(ParticleType.Spark, ShootFrom, Vector2.Zero, 0,ParentGun.MyColor,10f);
                    }
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Rof++;

            {
                BurstTime += gameTime.ElapsedGameTime.Milliseconds;
                if (BurstTime > MaxBurstTime)
                {
                    BurstTime = 0;
                    BurstSize = MaxBurstSize;
                }
            }
           
        }

        public virtual Bullet CreateBullet()
        {
            return null;
        }
    }
}
