using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    class TimeLineWindow : Window
    {

        Texture2D HoverBox;
        Vector2 MousePos;

        public Vector2 CameraDragPos;
        public Vector2 MoveDragPos;

        public Rectangle DragRectangle = new Rectangle();

        bool DraggingCamera = false;
        bool DraggingMove = false;
        bool DraggingSelect = false;

        const int GridMargin = 32;

        public TimeLineWindow(Rectangle MyRectangle, Rectangle HoverRectangle, bool ScrollLR, bool ScrollUD)
            : base(MyRectangle, HoverRectangle, ScrollLR, ScrollUD)
        {
            UpdateMousePoint = false;
        }

        public void LeftClick()
        {

            if (MasterEditor.mouseMode == MouseMode.Select || MasterEditor.mouseMode == MouseMode.Square)
            {
                DraggingSelect = true;
                DragRectangle.X = MousePoint.X;
                DragRectangle.Y = MousePoint.Y;
            }
            if (MasterEditor.mouseMode == MouseMode.Move)
            {
                DraggingMove = true;
                MoveDragPos = MousePos;
            }

        }
        
        public override void ContainsMouse()
        {
            if (!DraggingCamera)
            {
                if (WindowManager.JustRightClicked && WindowManager.KeyState.IsKeyDown(Keys.LeftShift))
                {
                    DraggingCamera = true;
                    CameraDragPos = MousePos;
                }

                if (WindowManager.JustLeftClicked)
                {
                    LeftClick();
                }

                if (GameManager.MyLevel.TimeRectangle.Contains(MousePoint))
                {
                    if (WindowManager.mouseState.LeftButton.Equals(ButtonState.Pressed))
                        if (MasterEditor.mouseMode == MouseMode.Place)
                        {
                            int x = (int)(Math.Floor(MousePos.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                            int y = (int)(Math.Floor(MousePos.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);

                            Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);

                            bool PlaceFree = false;

                            if (GameManager.MyLevel.TimeRectangle.Contains(CheckRect))
                            {
                                PlaceFree = true;
                                foreach (TimeBasic Time in GameManager.MyLevel.TimeEvents)
                                    if (Time.MyRectangle.Intersects(CheckRect))
                                        PlaceFree = false;
                            }

                            if (PlaceFree)
                            {
                                TimeBasic NewObject = (TimeBasic)Instancer.CreateInstanceOf(MasterEditor.SelectedObjectCreater, GameManager.MyLevel);
                                NewObject.Create(MasterEditor.GridSize, new Vector2(x, y));
                            }

                        }

                }
                if (WindowManager.mouseState.RightButton.Equals(ButtonState.Pressed) && WindowManager.KeyState.IsKeyUp(Keys.LeftShift))
                {
                    List<TimeBasic> DeleteObjects = new List<TimeBasic>(GameManager.MyLevel.TimeEvents.Count);

                    int x = (int)(Math.Floor(MousePos.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                    int y = (int)(Math.Floor(MousePos.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);

                    Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);

                    foreach (TimeBasic Time in GameManager.MyLevel.TimeEvents)
                        if (Time.MyRectangle.Intersects(CheckRect))
                            DeleteObjects.Add(Time);

                    foreach (TimeBasic Object in DeleteObjects)
                        Object.Delete();

                }
            }


            base.ContainsMouse();
        }

        public override void Update()
        {
            MousePos = Vector2.Transform(new Vector2(WindowManager.mouseState.X, WindowManager.mouseState.Y), Matrix.Invert(GameManager.MyLevel.MyCamera.ReturnMatrix()));
            MousePoint = new Point((int)MousePos.X, (int)MousePos.Y);

            if (WindowManager.KeyState.IsKeyDown(Keys.Delete))
            {
                List<TimeBasic> DeleteList = new List<TimeBasic>();
                foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                    if (Object.Selected)
                        DeleteList.Add(Object);

                foreach (TimeBasic Object in DeleteList)
                    Object.Delete();
            }
            if (WindowManager.mouseState.LeftButton == ButtonState.Released)
            {
                if (DraggingMove)
                {
                    GameManager.MyLevel.HasChanged = true;

                    foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                        if (Object.Selected)
                        {
                            int x = (int)(Math.Floor(Object.Position.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                            int y = (int)(Math.Floor(Object.Position.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);
                            Object.ChangePosition(new Vector2(x, y));
                        }
                }

                if (DraggingSelect)
                {
                    if (MasterEditor.mouseMode == MouseMode.Select)
                    {
                        foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                            Object.ChangeSelect(false);

                        foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                            if (Object.MyRectangle.Intersects(DragRectangle))
                                Object.ChangeSelect(true);
                    }
                    else
                    {
                        Rectangle LvlRect = GameManager.MyLevel.MyRectangle;
                        for (int x = (int)(Math.Floor(LvlRect.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X); x < LvlRect.X + LvlRect.Width; x += (int)MasterEditor.GridSize.X)
                            for (int y = (int)(Math.Floor(LvlRect.Y / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X); y < LvlRect.Y + LvlRect.Height; y += (int)MasterEditor.GridSize.Y)
                            {

                                Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);
                                if (CheckRect.Intersects(DragRectangle))
                                {
                                    bool PlaceFree = false;

                                    if (GameManager.MyLevel.TimeRectangle.Contains(CheckRect))
                                    {
                                        PlaceFree = true;
                                        foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                                            if (Object.MyRectangle.Intersects(CheckRect))
                                                PlaceFree = false;
                                    }

                                    if (PlaceFree)
                                    {
                                        TimeBasic Object = (TimeBasic)Instancer.CreateInstanceOf(MasterEditor.SelectedObjectCreater, GameManager.MyLevel);
                                        Object.Create(MasterEditor.GridSize, new Vector2(x, y));
                                    }
                                }
                            }

                    }
                }

                DraggingMove = false;
                DraggingSelect = false;
            }
            if (WindowManager.mouseState.RightButton == ButtonState.Released)
                DraggingCamera = false;


            if (DraggingSelect)
            {
                DragRectangle.Width = MousePoint.X - DragRectangle.X;
                DragRectangle.Height = MousePoint.Y - DragRectangle.Y;
            }

            if (DraggingMove)
            {
                Vector2 Change = MousePos - MoveDragPos;
                foreach (TimeBasic Object in GameManager.MyLevel.TimeEvents)
                    if (Object.Selected)
                    {
                        Object.ChangePosition(Object.Position + Change);
                    }
                MoveDragPos = MousePos;
            }


            if (DraggingCamera)
            {
                GameManager.MyLevel.MyCamera.EditorOffset += (MousePos - CameraDragPos)/2 ;
                CameraDragPos = MousePos;
            }

            if (GameManager.MyLevel != null)
                GameManager.MyLevel.MyCamera.MyRectangle = new Vector4(
                    GameManager.MyLevel.MyCamera.EditorOffset.X + (Game1.self.Window.ClientBounds.Width - GameManager.MyLevel.MyRectangle.Width) / 2,
                    GameManager.MyLevel.MyCamera.EditorOffset.Y + (Game1.self.Window.ClientBounds.Height - GameManager.MyLevel.MyRectangle.Height) / 2,
                    //200,200,
                    Game1.self.Window.ClientBounds.Width,
                    Game1.self.Window.ClientBounds.Height
                );

            base.Update();
        }

        public override void Load()
        {
            HoverBox = Game1.contentManager.Load<Texture2D>("Blank");
            base.Load();
        }

        public override void PreDraw()
        {
            // if (GameManager.MyLevel != null)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, GameManager.MyLevel.MyCamera.ReturnMatrix());
                Game1.graphicsDevice.SetRenderTarget(RenderTarget);
                Game1.graphicsDevice.Clear(new Color(0, 0, 0, 0));
            }

        }

        public override void Draw()
        {

            if (DraggingSelect)
            {
                DragRectangle.Width = MousePoint.X - DragRectangle.X;
                DragRectangle.Height = MousePoint.Y - DragRectangle.Y;
            }

            Game1.spriteBatch.Draw(HoverBox, GameManager.MyLevel.TimeRectangle, new Color(new Vector4(0.125f)));

            Render.DrawLine(new Vector2(0, GameManager.MyLevel.TimeRectangle.Height), new Vector2(GameManager.MyLevel.TimeRectangle.Width, GameManager.MyLevel.TimeRectangle.Height),Color.White);
            Render.DrawLine(new Vector2(0,0), new Vector2(GameManager.MyLevel.TimeRectangle.Width, 0), Color.White);


            int Change = 32;
            for (int x = -(int)Math.Min(0, Math.Floor(GameManager.MyLevel.MyCamera.MyRectangle.X / Change) * Change); x < -(int)GameManager.MyLevel.MyCamera.MyRectangle.X+GameManager.MyLevel.MyCamera.MyRectangle.Z; x+=Change)
                Render.DrawLine(new Vector2(x, 0), new Vector2(x, GameManager.MyLevel.TimeRectangle.Height), new Color(new Vector4(0.25f)));
            Change = 32*10;
            for (int x = -(int)Math.Min(0, Math.Floor(GameManager.MyLevel.MyCamera.MyRectangle.X / Change) * Change); x < -(int)GameManager.MyLevel.MyCamera.MyRectangle.X + GameManager.MyLevel.MyCamera.MyRectangle.Z; x += Change)
            {
                Game1.spriteBatch.DrawString(MenuManager.Font, ((int)(x / 16)).ToString(), new Vector2(x ,- 32), Color.White);
                Render.DrawLine(new Vector2(x, 0), new Vector2(x, GameManager.MyLevel.TimeRectangle.Height), new Color(new Vector4(0.5f)));
            }
                Change = 32 * 60;
            for (int x = -(int)Math.Min(0, Math.Floor(GameManager.MyLevel.MyCamera.MyRectangle.X / Change) * Change); x < -(int)GameManager.MyLevel.MyCamera.MyRectangle.X + GameManager.MyLevel.MyCamera.MyRectangle.Z; x += Change)
                Render.DrawLine(new Vector2(x, 0), new Vector2(x, GameManager.MyLevel.TimeRectangle.Height), new Color(new Vector4(1)));
            

                if (GameManager.MyLevel != null)
                    GameManager.MyLevel.DrawTimeLine();

            if (DraggingSelect)
            {
                Game1.spriteBatch.Draw(HoverBox, DragRectangle, Color.White);
            }
            Game1.spriteBatch.End();

        }
    }
}
