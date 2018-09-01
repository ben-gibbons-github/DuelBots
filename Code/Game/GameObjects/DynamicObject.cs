using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class DynamicObject:BasicObject
    {

        public override void ChangePosition()
        {
            MyRectangle.X = (int)Position.X;
            MyRectangle.Y = (int)Position.Y;
        }

        public override void ChangePosition(Microsoft.Xna.Framework.Vector2 NewPosition)
        {
            Position = NewPosition;
            MyRectangle.X = (int)NewPosition.X;
            MyRectangle.Y = (int)NewPosition.Y;
        }

        public override void Destroy()
        {
            Died = true;
            GameManager.MyLevel.DestroyDynamic(this);
        }

    }
}
