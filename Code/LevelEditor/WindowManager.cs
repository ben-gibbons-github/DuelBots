using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class WindowManager
    {
        public static List<Window> Children = new List<Window>();
        public static WindowManager self;

        public static MouseState mouseState;
        public static MouseState PreviousMouseState;
        public static KeyboardState KeyState;
        public static KeyboardState PrevKeyState;

        public static bool JustLeftClicked = false;
        public static bool JustRightClicked = false;

        public static Point MousePoint;
        public static Rectangle WindowRectangle;

        public static Window TopWindow;
        public static Window editorWindow;
        public static Window FileWindow;
        public static Window typeSelectorWindow;
        public static Window timeLineWindow;
        public static Window backgroundSelectorWindow;

        public WindowManager()
        {
            Children.Clear();
            self = this;
            GenerateLevelEditorGui();

        }

        public static Window AddWindow(Window NewWindow)
        {
            Children.Add(NewWindow);
            NewWindow.Load();
            return NewWindow;
        }

        public void Update()
        {
            JustLeftClicked = false;
            JustRightClicked = false;



            if(Game1.self.Window.ClientBounds.Width!=WindowRectangle.Width||Game1.self.Window.ClientBounds.Height!=WindowRectangle.Height)
                UpdateGui();

            PreviousMouseState = mouseState;
            mouseState = Mouse.GetState();

            PrevKeyState = KeyState;
            KeyState = Keyboard.GetState();

            if (mouseState.LeftButton.Equals(ButtonState.Pressed) && PreviousMouseState.LeftButton.Equals(ButtonState.Released))
                JustLeftClicked = true;
            if (mouseState.RightButton.Equals(ButtonState.Pressed) && PreviousMouseState.RightButton.Equals(ButtonState.Released))
                JustRightClicked = true;

            MousePoint = new Point((int)MathHelper.Clamp(mouseState.X, 0, Game1.self.Window.ClientBounds.Width), (int)MathHelper.Clamp(mouseState.Y, 0, Game1.self.Window.ClientBounds.Height));

            Window MouseWindow = null;

            if(WindowRectangle.Contains(MousePoint))
            for (int i = 0; i < Children.Count(); i++)
            {
                Window CurrentWindow = Children[i];
                CurrentWindow.Update();

                if (MouseWindow == null && CurrentWindow.MyRectangle.Contains(MousePoint))
                {
                    MouseWindow = CurrentWindow;
                    CurrentWindow.ContainsMouse();
                }
                else
                    CurrentWindow.DoesNotContainMouse();
            }
            else
                for (int i = 0; i < Children.Count(); i++)
                {
                    Window CurrentWindow = Children[i];
                    CurrentWindow.DoesNotContainMouse();
                    CurrentWindow.Update();
                }

        }

        public void Draw()
        {
            for (int i = 0; i < Children.Count(); i++)
            {
                Children[i].PreDraw();
                Children[i].Draw();
            }
            Game1.graphicsDevice.SetRenderTarget(null);
            Game1.graphicsDevice.Clear(new Color(0, 0, 0, 0));

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend);

            for (int i = Children.Count-1; i > -1; i--)
            {
                Game1.spriteBatch.Draw(Children[i].RenderTarget, Children[i].MyRectangle, Children[i].ScrollRectangle, Color.White);
            }
            Game1.spriteBatch.End();

        }

        public void GenerateLevelEditorGui()
        {
            AddWindow(FileWindow = new FileWindow(new Rectangle(0, -320, 100, 400), new Rectangle(0, -01, 100, 400), false, false));
            AddWindow(TopWindow = new ObjectSelectorWindow(new Rectangle(100, -01, 600, 80), new Rectangle(100, -01, 600, 101-80),true,false));
            AddWindow(typeSelectorWindow = new TypeSelectorWindow(new Rectangle(700, -01, 300, 80), new Rectangle(700, -01, 300, 301),false, false));
            AddWindow(new SinglePlayerWindow(new Rectangle(1000, -01, 150, 80), new Rectangle(1000, -01, 150, 301), false, false));
            AddWindow(backgroundSelectorWindow = new BackgroundSelectorWindow(new Rectangle(0, 0, 800, 300), new Rectangle(0, 0, 800, 300), false, false));
            AddWindow(editorWindow = new EditorWindow(new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), false, false));

            UpdateGui();
        }

        public void UpdateGui()
        {
            if (Game1.self.Window.ClientBounds.Width > 0 && Game1.self.Window.ClientBounds.Height > 0)
            {

                WindowRectangle = new Rectangle(0, 0, Game1.self.Window.ClientBounds.Width, Game1.self.Window.ClientBounds.Height);

                //  TopWindow.MyRectangle.X = WindowRectangle.Width / 2 - TopWindow.MyRectangle.Width / 2;
                //  TopWindow.StartRectangle.X = TopWindow.MyRectangle.X;
                // TopWindow.HoverRectangle.X = TopWindow.MyRectangle.X;

                backgroundSelectorWindow.MyRectangle.Y = Game1.self.Window.ClientBounds.Height - 100;
                backgroundSelectorWindow.HoverRectangle.Y = Game1.self.Window.ClientBounds.Height - 100;
                backgroundSelectorWindow.StartRectangle.Y = Game1.self.Window.ClientBounds.Height - 100;
                editorWindow.MyRectangle.Width = Game1.self.Window.ClientBounds.Width;
                editorWindow.MyRectangle.Height = Game1.self.Window.ClientBounds.Height;
                editorWindow.HoverRectangle = EditorStatic.CloneRectangle(editorWindow.MyRectangle);
                editorWindow.ScrollRectangle = EditorStatic.CloneRectangle(editorWindow.MyRectangle);
                if(GameManager.MyLevel!=null)
                GameManager.MyLevel.HasResized = true;
                editorWindow.RenderTarget = new RenderTarget2D(Game1.graphicsDevice, editorWindow.MyRectangle.Width, editorWindow.MyRectangle.Height);

            }
        }

    }
}
