using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class MenuItem
    {
        public MenuBasic Parent;

        public Rectangle MyRectangle;
        public Vector2 Position;
        public Vector2 Size;
        public Vector2 StartPos;

        public float HoverAlpha = 0;
        public float HoverAlphaChange = 0.1f;
        public float MenuAlpha = 0;
        public float MenuAlphaChange = 0.1f;

        public bool Fade = true;
        public bool DestroyOnFade = true;

        public int X;
        public int Y;

        public bool Hovering = false;
        public bool Discarded = false;
        public bool Selected = false;

        public virtual MenuItem Create(Vector2 Position, Vector2 Size, int X, int Y, MenuBasic Parent)
        {
            this.X = X;
            this.Y = Y;

            StartPos = Position;
            ChangeSize(Size);
            ChangePosition(Position);

            this.Parent = Parent;

            return this;
        }

        public virtual void Update(GameTime gameTime)
        {

            if (Selected || Hovering && !Discarded)
                HoverAlpha = Math.Min(1, (float)(HoverAlpha + (float)gameTime.ElapsedGameTime.Milliseconds / 1000 * 60 * HoverAlphaChange));
            else
                HoverAlpha = Math.Max(0, HoverAlpha - (float)gameTime.ElapsedGameTime.Milliseconds / 1000 * 60 * HoverAlphaChange);


            if (!Discarded || Selected)
                MenuAlpha = Math.Min(1, MenuAlpha + (float)gameTime.ElapsedGameTime.Milliseconds / 1000 * 60 * MenuAlphaChange);
            else if (Fade) 
            {
                MenuAlpha = Math.Max(0, MenuAlpha - (float)gameTime.ElapsedGameTime.Milliseconds / 1000 * 60 * MenuAlphaChange);
                if (MenuAlpha == 0 && DestroyOnFade)
                    Destroy();
            }
            
        }

        public virtual void Destroy()
        {
            Parent.DestroyItem(this);
        }

        public virtual void Discard()
        {
            Discarded = true;
        }

        public virtual void TakeInput(MenuInput Input)
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void ChangeSize(Vector2 NewSize)
        {
            Size = NewSize;
            MyRectangle.Width = (int)NewSize.X;
            MyRectangle.Height = (int)NewSize.Y;
        }

        public virtual void ChangePosition(Vector2 NewPosition)
        {
            Position = NewPosition;
            MyRectangle.X = (int)NewPosition.X;
            MyRectangle.Y = (int)NewPosition.Y;
        }

        public virtual void ChangePosition()
        {
            MyRectangle.X = (int)Position.X;
            MyRectangle.Y = (int)Position.Y;
        }

    }
}
