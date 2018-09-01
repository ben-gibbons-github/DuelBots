using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class Form
    {
        public Rectangle MyRectangle;
        public Window ParentWindow;
        public bool MouseHovering = false;

        public virtual void Create()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void SetPosition(Rectangle NewRectangle)
        {
            MyRectangle = NewRectangle;
            
        }

        public virtual void ContainsMouse()
        {
            MouseHovering = true;
        }

        public virtual void DoesNotContainMouse()
        {
            MouseHovering = false;
        }

        public virtual void Draw(float alpha)
        {
        }


    }
}
