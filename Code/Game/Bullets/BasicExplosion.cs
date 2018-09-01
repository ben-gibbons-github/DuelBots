using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class BasicExplosion:Bullet
    {

        public BasicExplosion(Vector2 Position,float Damage, float Range, float Push)
        {
            this.ChangePosition(Position);
            this.Damage = Damage;
            this.Range = Range;
            this.Push = Push;
            GameManager.MyLevel.DistanceDamage(this);
        }
    }
}
