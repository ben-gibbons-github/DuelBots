using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public delegate void ClickEvent(Button ClickedButton);

    public class Button:Form
    {
        ClickEvent MyClickEvent;
        public Texture2D Image;
        int Margin;
        Rectangle ImageRectangle;
        Rectangle HoverRectangle;
        Rectangle StartRectangle;
        Rectangle PositionRectangle;
        bool Changing = false;
        Rectangle MoveToRect;
        float MoveSpeed = 0.2f;
        public bool Selected = false;
        float HoverAlpha = 0;
        public object DesiredObject;

        public Button(Texture2D Image,Rectangle MyRectangle,Rectangle HoverRectangle,int Margin,ClickEvent MyClickEvent)
        {
            this.Image = Image;
            this.PositionRectangle = MyRectangle;
            this.MyRectangle = PositionRectangle;
            this.Margin = Margin;
            this.HoverRectangle = HoverRectangle;
            StartRectangle = EditorStatic.CloneRectangle(MyRectangle);
            ImageRectangle = new Rectangle(MyRectangle.X + Margin, MyRectangle.Y + Margin, MyRectangle.Width - Margin, MyRectangle.Height - Margin);
            this.MyClickEvent = MyClickEvent;
        }

        public override void SetPosition(Rectangle NewRectangle)
        {
            PositionRectangle = NewRectangle;
            HoverRectangle.X = NewRectangle.X;
            HoverRectangle.Y = NewRectangle.Y;
            StartRectangle.X = NewRectangle.X;
            StartRectangle.Y = NewRectangle.Y;
        }

        public override void ContainsMouse()
        {
            if (WindowManager.JustLeftClicked)
                if (MyClickEvent != null)
                    MyClickEvent(this);

            base.ContainsMouse();
        }

        public override void Update()
        {
            if (MouseHovering)
                HoverAlpha += 0.1f;
            else
                HoverAlpha -= 0.1f;

            HoverAlpha = MathHelper.Clamp(HoverAlpha, 0, 1);

            if (!Changing)
            {
                if (MyRectangle.Contains(ParentWindow.MousePoint))
                    MoveToRect = HoverRectangle;
                else
                    MoveToRect = StartRectangle;
                Changing = true;
            }

            Vector4 MoveVec2 = new Vector4(MoveToRect.X,MoveToRect.Y ,MoveToRect.Width, MoveToRect.Height);
            Vector4 MyVec2 = new Vector4(PositionRectangle.X, PositionRectangle.Y, PositionRectangle.Width, PositionRectangle.Height);

            if (Vector4.Distance(MoveVec2, MyVec2) > 4)
            {
                Vector4 MoveAmount = MyVec2 +(MoveVec2 - MyVec2) * MoveSpeed;
                MoveAmount += Vector4.Normalize(MoveVec2 - MyVec2);

                PositionRectangle.X = (int)MoveAmount.X;
                PositionRectangle.Y = (int)MoveAmount.Y;
                PositionRectangle.Width = (int)MoveAmount.Z;
                PositionRectangle.Height = (int)MoveAmount.W;
            }
            else
            {
                PositionRectangle.X = MoveToRect.X;
                PositionRectangle.Y = MoveToRect.Y;
                PositionRectangle.Width = MoveToRect.Width;
                PositionRectangle.Height = MoveToRect.Height;
                Changing = false;
            }

            MyRectangle = new Rectangle(PositionRectangle.X - PositionRectangle.Width / 2, PositionRectangle.Y - PositionRectangle.Height / 2, PositionRectangle.Width, PositionRectangle.Height);
            ImageRectangle = new Rectangle(MyRectangle.X + Margin, MyRectangle.Y + Margin, MyRectangle.Width - Margin*2, MyRectangle.Height - Margin*2);

            base.Update();
        }

        public override void Draw(float Alpha)
        {


            float Brightness = 0.35f + 0.15f * HoverAlpha;
            EditorStatic.DrawRectangle(MyRectangle, new Color(Brightness * Alpha, Brightness * Alpha, Brightness * Alpha, 0.5f * Alpha), true);


            Vector4 col;

            if (Selected)
                col = new Vector4(0, 1, 1, 1) * Alpha;
            else
                col = new Vector4(0, 0,0, 1) * Alpha;
            EditorStatic.DrawRectangle(MyRectangle, new Color(col), false);

            if(Image!=null)
            Game1.spriteBatch.Draw(Image, ImageRectangle, new Color(Vector4.One*Alpha));

            base.Draw(Alpha);
        }
    }
}
