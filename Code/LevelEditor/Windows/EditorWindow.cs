using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    class EditorWindow:Window
    {

        Texture2D HoverBox;
        Texture2D HoverBox2;
        Texture2D Node;

        Vector2 MousePos;

        public Rectangle NodeRectangleLowerRight;
        public Rectangle NodeRectangleLowerLeft;
        public Rectangle NodeRectangleUpperRight;
        public Rectangle NodeRectangleUpperLeft;

        public Vector2 DragPos;
        public Vector2 CameraDragPos;
        public Vector2 MoveDragPos;

        public Rectangle DragRectangle = new Rectangle();



        bool Dragging = false;
        int DraggingX = 1;
        int DraggingY = 1;
        bool DraggingCamera = false;
        bool DraggingMove = false;
        bool DraggingSelect = false;


        const int GridMargin=32;

        public EditorWindow(Rectangle MyRectangle, Rectangle HoverRectangle,bool ScrollLR,bool ScrollUD)
           : base(MyRectangle,HoverRectangle,ScrollLR,ScrollUD)
        {
            UpdateMousePoint = false;
        }

        public void LeftClick()
        {
            if (!CheckDrag()) 
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
        }

        public bool CheckDrag()
        {
            bool result = false;

            if (NodeRectangleLowerRight.Contains(MousePoint))
            {
                Dragging = true;
                DragPos = MousePos;
                DraggingX = 1;
                DraggingY = 1;
                result = true;
            }

            if (NodeRectangleLowerLeft.Contains(MousePoint))
            {
                Dragging = true;
                DragPos = MousePos;
                DraggingX = -1;
                DraggingY = 1;
                result = true;
            }

            if (NodeRectangleUpperRight.Contains(MousePoint))
            {
                Dragging = true;
                DragPos = MousePos;
                DraggingX = 1;
                DraggingY = -1;
                result = true;
            }

            if (NodeRectangleUpperLeft.Contains(MousePoint))
            {
                Dragging = true;
                DragPos = MousePos;
                DraggingX = -1;
                DraggingY = -1;
                result = true;
            }

            return result;
        }

        public override void ContainsMouse()
        {
            if (!Dragging && !DraggingCamera)
            {
                if (WindowManager.JustRightClicked&&WindowManager.KeyState.IsKeyDown(Keys.LeftShift))
                {
                    DraggingCamera = true;
                    CameraDragPos = MousePos;
                }

                if (WindowManager.JustLeftClicked)
                {
                    LeftClick();
                }

                if (GameManager.MyLevel.MyRectangle.Contains(MousePoint))
                {
                    if (WindowManager.mouseState.LeftButton.Equals(ButtonState.Pressed))
                        if (MasterEditor.mouseMode == MouseMode.Place)
                        {
                            int x = (int)(Math.Floor(MousePos.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                            int y = (int)(Math.Floor(MousePos.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);

                            Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);

                            bool PlaceFree = false;

                            if (GameManager.MyLevel.MyRectangle.Contains(CheckRect))
                            {
                                PlaceFree = true;
                                foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                                    if (Object.MyRectangle.Intersects(CheckRect))
                                        PlaceFree = false;
                            }

                            if (PlaceFree)
                            {
                                BasicObject NewObject = (BasicObject)Instancer.CreateInstanceOf(MasterEditor.SelectedObjectCreater,GameManager.MyLevel);
                                NewObject.Create(MasterEditor.GridSize, new Vector2(x, y));
                            }

                        }
                    
                }
                if (WindowManager.mouseState.RightButton.Equals(ButtonState.Pressed)&&WindowManager.KeyState.IsKeyUp(Keys.LeftShift))
                {
                    List<BasicObject> DeleteObjects = new List<BasicObject>(GameManager.MyLevel.ObjectList.Count);

                    int x = (int)(Math.Floor(MousePos.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                    int y = (int)(Math.Floor(MousePos.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);

                    Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);

                    foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                        if (Object.MyRectangle.Intersects(CheckRect))
                            DeleteObjects.Add(Object);

                    foreach (BasicObject Object in DeleteObjects)
                        Object.Delete();

                }
            }


            base.ContainsMouse();
        }

        public override void Update()
        {
            MousePos= Vector2.Transform(new Vector2(WindowManager.mouseState.X, WindowManager.mouseState.Y), Matrix.Invert(GameManager.MyLevel.MyCamera.ReturnMatrix()));
            MousePoint = new Point((int)MousePos.X, (int)MousePos.Y);

            if (WindowManager.KeyState.IsKeyDown(Keys.Delete))
            {
                List<Object> DeleteList = new List<object>();
                foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                    if (Object.Selected)
                        DeleteList.Add(Object);

                foreach (BasicObject Object in DeleteList)
                    Object.Delete();
            }
            if (WindowManager.mouseState.LeftButton == ButtonState.Released)
            {
                if (DraggingMove)
                {
                    GameManager.MyLevel.HasChanged = true;

                    foreach(BasicObject Object in GameManager.MyLevel.ObjectList)
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
                        foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                            Object.ChangeSelect(false);

                        foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                            if (Object.MyRectangle.Intersects(DragRectangle))
                                Object.ChangeSelect(true);
                    }
                    else
                    {
                        Rectangle LvlRect= GameManager.MyLevel.MyRectangle;
                        for (int x = (int)(Math.Floor(LvlRect.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X); x < LvlRect.X + LvlRect.Width; x += (int)MasterEditor.GridSize.X)
                            for (int y = (int)(Math.Floor(LvlRect.Y / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X); y < LvlRect.Y + LvlRect.Height; y += (int)MasterEditor.GridSize.Y)
                            {

                                Rectangle CheckRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);
                                if (CheckRect.Intersects(DragRectangle))
                                {
                                    bool PlaceFree = false;

                                    if (GameManager.MyLevel.MyRectangle.Contains(CheckRect))
                                    {
                                        PlaceFree = true;
                                        foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                                            if (Object.MyRectangle.Intersects(CheckRect))
                                                PlaceFree = false;
                                    }

                                    if (PlaceFree)
                                    {


                                        BasicObject Object = (BasicObject)Instancer.CreateInstanceOf(MasterEditor.SelectedObjectCreater, GameManager.MyLevel);
                                        Object.Create(MasterEditor.GridSize, new Vector2(x, y));

                                    }
                                }
                            }

                    }
                }

                DraggingMove = false;
                DraggingSelect = false;
                Dragging = false;
            }
            if (WindowManager.mouseState.RightButton == ButtonState.Released)
                DraggingCamera = false;
            

            if (Dragging)
            {
                Rectangle PreviousRect = EditorStatic.CloneRectangle(GameManager.MyLevel.MyRectangle);
                Rectangle LevelRect = GameManager.MyLevel.MyRectangle;

                LevelRect.Width += (int)(MousePos.X - DragPos.X)*DraggingX;
                LevelRect.Height += (int)(MousePos.Y - DragPos.Y)*DraggingY;

                LevelRect.X += (int)(MousePos.X - DragPos.X) * Math.Abs( Math.Min(0, DraggingX));
                LevelRect.Y += (int)(MousePos.Y - DragPos.Y) * Math.Abs(Math.Min(0, DraggingY));

                LevelRect.Width = Math.Max(100, LevelRect.Width);
                LevelRect.Height = Math.Max(100, LevelRect.Height);

                GameManager.MyLevel.MyRectangle = LevelRect;

                GameManager.MyLevel.HasResized = true;

                GameManager.MyLevel.MyCamera.EditorOffset += new Vector2(LevelRect.Width - PreviousRect.Width, LevelRect.Height - PreviousRect.Height)/2*new Vector2(Math.Abs( DraggingX),Math.Abs( DraggingY));
                DragPos = MousePos;
            }

            if (DraggingSelect)
            {
                DragRectangle.Width = MousePoint.X - DragRectangle.X;
                DragRectangle.Height = MousePoint.Y - DragRectangle.Y;
            }

            if (DraggingMove)
            {
                Vector2 Change = MousePos - MoveDragPos;
                foreach(BasicObject Object in GameManager.MyLevel.ObjectList)
                    if (Object.Selected)
                    {
                        Object.ChangePosition(Object.Position + Change);
                    }
                MoveDragPos = MousePos;
            }


            if (DraggingCamera)
            {
                GameManager.MyLevel.MyCamera.EditorOffset += (MousePos - CameraDragPos)/2;
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
            HoverBox = Game1.contentManager.Load<Texture2D>("Editor/HoverBox");
            HoverBox2 = Game1.contentManager.Load<Texture2D>("Editor/HoverBox2");
            Node = Game1.contentManager.Load<Texture2D>("Editor/Node");
            base.Load();
        }

        public override void PreDraw()
        {
           // if (GameManager.MyLevel != null)
            {
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, GameManager.MyLevel.MyCamera.ReturnMatrix());
               // Game1.spriteBatch.Begin();
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

            //Vector2 NewPosition = Vector2.Transform(new Vector2(GameManager.MyLevel.MyRectangle.X, GameManager.MyLevel.MyRectangle.Y), GameManager.MyLevel.MyCamera.ReturnMatrix());
           // Vector2 NewSize = Vector2.Transform(new Vector2(GameManager.MyLevel.MyRectangle.Width, GameManager.MyLevel.MyRectangle.Height), GameManager.MyLevel.MyCamera.ReturnMatrix());

           // EditorStatic.DrawRectangle(new Rectangle((int)NewPosition.X, (int)NewPosition.Y, (int)NewSize.X, (int)NewSize.Y), new Color(new Vector4(0.25f * HoverAlpha)), false);
            Game1.spriteBatch.Draw(HoverBox2, GameManager.MyLevel.MyRectangle, new Color(new Vector4(1)));
            NodeRectangleLowerRight= new Rectangle(GameManager.MyLevel.MyRectangle.X+GameManager.MyLevel.MyRectangle.Width,GameManager.MyLevel.MyRectangle.Y+GameManager.MyLevel.MyRectangle.Height,16,16);
            NodeRectangleLowerLeft = new Rectangle(GameManager.MyLevel.MyRectangle.X -16, GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Height, 16, 16);
            NodeRectangleUpperRight = new Rectangle(GameManager.MyLevel.MyRectangle.X + GameManager.MyLevel.MyRectangle.Width, GameManager.MyLevel.MyRectangle.Y - 16, 16, 16);
            NodeRectangleUpperLeft = new Rectangle(GameManager.MyLevel.MyRectangle.X - 16, GameManager.MyLevel.MyRectangle.Y -16, 16, 16);
            

            Game1.spriteBatch.Draw(Node, NodeRectangleUpperRight, new Color(new Vector4(1) * HoverAlpha));
            Game1.spriteBatch.Draw(Node, NodeRectangleUpperLeft, new Color(new Vector4(1) * HoverAlpha));
            Game1.spriteBatch.Draw(Node, NodeRectangleLowerRight, new Color(new Vector4(1) * HoverAlpha));
            Game1.spriteBatch.Draw(Node, NodeRectangleLowerLeft, new Color(new Vector4(1) * HoverAlpha));

            if(GameManager.MyLevel!=null)
            GameManager.MyLevel.Draw();

            if (DraggingSelect)
            {
                Game1.spriteBatch.Draw(HoverBox, DragRectangle, Color.White);
            }

          if (MouseHovering&&GameManager.MyLevel.MyRectangle.Contains(MousePoint)&&MasterEditor.mouseMode==MouseMode.Place&&WindowManager.mouseState.RightButton.Equals(ButtonState.Released))
            {

                int x = (int)(Math.Floor(MousePos.X / MasterEditor.GridSize.X) * (float)MasterEditor.GridSize.X);
                int y = (int)(Math.Floor(MousePos.Y / MasterEditor.GridSize.Y) * (float)MasterEditor.GridSize.Y);

                Rectangle DrawRect = new Rectangle(x, y, (int)MasterEditor.GridSize.X, (int)MasterEditor.GridSize.Y);
                Color col = new Color(Vector4.One * HoverAlpha);
                Game1.spriteBatch.Draw(HoverBox, DrawRect, col);
            }
            //if (GameManager.MyLevel != null)
                Game1.spriteBatch.End();

        }
    }
}
