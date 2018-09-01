using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class Window
    {
        public Rectangle StartRectangle;
        public Rectangle MyRectangle;
        public Rectangle HoverRectangle;
        public float MoveSpeed=0.1f;
        public List<Form> Children=new List<Form>();
        public bool Changing = false;
        Rectangle MoveToRect;
        public RenderTarget2D RenderTarget;
        public Rectangle ScrollRectangle;
        public Rectangle ScrollTarget;
        public Button LeftScroll;
        public Button RightScroll;
        public Point MousePoint;
        bool ScrollLR;
        bool ScrollUD;
        public bool UpdateMousePoint = true;
        public float HoverAlpha = 0;
        public bool MouseHovering = false;
        public Vector4 BackgroundColor = new Vector4(0.5f);
        public Vector4 EdgeColor = new Vector4(1);

        public Window(Rectangle MyRectangle)
        {
            this.MyRectangle = MyRectangle;
            Create();
        }

        public Window(Rectangle MyRectangle, Rectangle HoverRectangle,bool ScrollLR,bool ScrollUD)
        {
            this.MyRectangle = MyRectangle;
            this.HoverRectangle = HoverRectangle;
            this.ScrollLR = ScrollLR;

            



            Create();
        }

        public void Create()
        {
            StartRectangle = EditorStatic.CloneRectangle(MyRectangle);
            RenderTarget = new RenderTarget2D(Game1.graphicsDevice, MyRectangle.Width*2, MyRectangle.Height, false, SurfaceFormat.Alpha8, DepthFormat.None);
            ScrollRectangle = EditorStatic.SizeCloneRectangle(MyRectangle);
            ScrollTarget = EditorStatic.SizeCloneRectangle(ScrollRectangle);

            if (ScrollLR)
            {
                AddForm(LeftScroll = new Button(Game1.contentManager.Load<Texture2D>("Editor/ScrollArrow2"), new Rectangle(26, MyRectangle.Height / 2, 32, 32), new Rectangle(26, MyRectangle.Height / 2, 48, 48), 4, ScrollLeft));
                AddForm(RightScroll = new Button(Game1.contentManager.Load<Texture2D>("Editor/ScrollArrow"), new Rectangle(MyRectangle.Width - 26, MyRectangle.Height / 2, 32, 32), new Rectangle(MyRectangle.Width - 26, MyRectangle.Height / 2, 48, 48), 4,ScrollRight));

            }

        }

        public void ScrollLeft(Button button)
        {

        }

        public void ScrollRight(Button button)
        {

        }

        public virtual void Load()
        {
        }

        public void AddForm(Form NewForm,int WidthMargin,int HeightMargin,int Xsteps,int Ysteps,bool OverStepX)
        {

            int x = WidthMargin;
            int y = HeightMargin;

            for (int i = 0; i < 1000; i++)
            {
                bool DoubleBreak = false;
                Rectangle MarginRect=new Rectangle(x-WidthMargin,y-HeightMargin,WidthMargin*2+NewForm.MyRectangle.Width,HeightMargin*2+NewForm.MyRectangle.Height);

                bool PlaceFree = true;

                for (int b = 0; b < Children.Count(); b++)
                {
                    Form form = Children[b];
                    if(form!=null)
                    if (form.MyRectangle.Intersects(MarginRect))
                    {
                        PlaceFree = false;
                        break;
                    }
                }
                if (PlaceFree)
                {
                    NewForm.SetPosition(new Rectangle(x, y, NewForm.MyRectangle.Width, NewForm.MyRectangle.Height));
                    break;
                }

                x += Xsteps;
                if (x > MyRectangle.Width - WidthMargin&&!OverStepX)
                {
                    x = WidthMargin;
                    y += Ysteps;
                }

                if (DoubleBreak)
                    break;
            }

            AddForm(NewForm);
        }

        public void AddForm(Form NewForm)
        {
            Children.Add(NewForm);
            NewForm.Create();
            NewForm.ParentWindow = this;

            if (ScrollLR)
            {
                Children.Remove(LeftScroll);
                Children.Remove(RightScroll);
                Children.Add(LeftScroll);
                Children.Add(RightScroll);
            }
        }

        public virtual void Update()
        {
            if (MouseHovering)
                HoverAlpha += 0.1f;
            else
                HoverAlpha -= 0.1f;

            HoverAlpha = MathHelper.Clamp(HoverAlpha, 0.5f, 1);

            if(UpdateMousePoint)
            MousePoint = new Point(
                (int)MathHelper.Clamp(WindowManager.mouseState.X - MyRectangle.X, 0, Game1.self.Window.ClientBounds.Width),
                (int)MathHelper.Clamp(WindowManager.mouseState.Y - MyRectangle.Y, 0, Game1.self.Window.ClientBounds.Width)
                );

            if (!Changing)
            {
                if (MouseHovering)
                    MoveToRect = HoverRectangle;
                else
                    MoveToRect = StartRectangle;
                Changing = true;
            }

            Vector2 MoveVec2 = new Vector2(MoveToRect.X, MoveToRect.Y);
            Vector2 MyVec2 = new Vector2(MyRectangle.X, MyRectangle.Y);

            if (Vector2.Distance(MoveVec2, MyVec2) > 2)
            {
                Vector2 MoveAmount = MyVec2+ (MoveVec2-MyVec2)*MoveSpeed;
                MoveAmount += Vector2.Normalize(MoveVec2-MyVec2);

                MyRectangle.X = (int)MoveAmount.X;
                MyRectangle.Y = (int)MoveAmount.Y;
            }
            else
            {
                MyRectangle.X = MoveToRect.X;
                MyRectangle.Y = MoveToRect.Y;
                Changing = false;
            }


            MoveVec2 = new Vector2(ScrollTarget.X, ScrollTarget.Y);
            MyVec2 = new Vector2(ScrollRectangle.X, ScrollRectangle.Y);

            if (Vector2.Distance(MoveVec2, MyVec2) > 2)
            {
                Vector2 MoveAmount = MyVec2 + (MoveVec2 - MyVec2) * MoveSpeed;
                MoveAmount += Vector2.Normalize(MoveVec2 - MyVec2);

                ScrollRectangle.X = (int)MoveAmount.X;
                ScrollRectangle.Y = (int)MoveAmount.Y;
            }
            else
            {
                ScrollRectangle.X = ScrollTarget.X;
                ScrollRectangle.Y = ScrollTarget.Y;
            }

            for (int i = 0; i < Children.Count(); i++)
                if(Children[i]!=null)
                Children[i].Update();


            if (ScrollLR)
            {
                LeftScroll.SetPosition(new Rectangle(ScrollRectangle.X + 26, ScrollRectangle.Height / 2, LeftScroll.MyRectangle.Width, LeftScroll.MyRectangle.Height));
                RightScroll.SetPosition(new Rectangle(ScrollRectangle.X - 26 + ScrollRectangle.Width, ScrollRectangle.Height / 2, RightScroll.MyRectangle.Width, RightScroll.MyRectangle.Height));
            }

        }

        public virtual void ContainsMouse()
        {
            MouseHovering = true;

            for (int i = 0; i < Children.Count(); i++)
                if(Children[i]!=null)
            {
                if (Children[i].MyRectangle.Contains(MousePoint))
                    Children[i].ContainsMouse();
                else
                    Children[i].DoesNotContainMouse();
            }
        }

        public virtual void DoesNotContainMouse()
        {
            MouseHovering = false;

            for (int i = 0; i < Children.Count(); i++)
                if(Children[i]!=null)
                Children[i].DoesNotContainMouse();
        }

        public void DeselectButtons()
        {
            for (int i = 0; i < Children.Count(); i++)
                if (Children[i] != null)
                    if (Children[i].GetType() == typeof(Button))
                    {
                        Button button = Children[i] as Button;
                        button.Selected = false;
                    }
        }

        public virtual void PreDraw()
        {
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,SamplerState.AnisotropicClamp,DepthStencilState.None,RasterizerState.CullNone,null,Matrix.CreateTranslation(ScrollRectangle.X,ScrollRectangle.Y,0));
            Game1.graphicsDevice.SetRenderTarget(RenderTarget);
            Game1.graphicsDevice.Clear(new Color(0, 0, 0, 0));


            EditorStatic.DrawRectangle(ScrollRectangle, new Color(BackgroundColor * HoverAlpha), true);
            EditorStatic.DrawRectangle(ScrollRectangle, new Color(EdgeColor * HoverAlpha), false);
        }

        public virtual void Draw()
        {


            if (ScrollLR)
            {
                LeftScroll.Draw(HoverAlpha);
                RightScroll.Draw(HoverAlpha);
            }


            for (int i = 0; i < Children.Count(); i++)
                if (Children[i] != null)
            {
                float Alpha = 1;
                if (Children[i] != LeftScroll && Children[i] != RightScroll)
                {
                    if (Children[i].MyRectangle.X < 96)
                        Alpha = (32 - (96 - Children[i].MyRectangle.X)) / 32;
                    if (Children[i].MyRectangle.X+Children[i].MyRectangle.Width > MyRectangle.Width-96)
                        Alpha = ((MyRectangle.Width-96) - (MyRectangle.X+Children[i].MyRectangle.Width)) / 32;
                }

                Alpha = 1;

                Children[i].Draw(Alpha*HoverAlpha);
            }

            Game1.spriteBatch.End();
        }

    }
}
