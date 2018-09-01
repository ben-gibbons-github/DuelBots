using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public enum MouseMode
    {
        Select,
        Place,
        Move,
        Square
    }

    public class MasterEditor
    {
        public static WindowManager windowManager;
        public static EditorStatic editorStatic;
        public static Vector2 GridSize = new Vector2(32, 32);
        public static bool UnSaved = false;
        public static bool TimeLineMode = false;

        public static Texture2D SelectedBox;
        public static Texture2D Alert;

        public static DialogManager dialogManager = new DialogManager();

        public static MouseMode mouseMode = MouseMode.Place;
        public static BasicObjectCreator SelectedObjectCreater = BasicObjectCreator.BasicObjectTypes[0];

        public void Load()
        {
            TimeLineMode = false;

            for (int i = 0; i < BasicObjectCreator.BasicObjectTypes.Length; i++)
            {
                BasicObjectCreator.BasicObjectTypes[i].Create();
                BasicObjectCreator.BasicObjectTypes[i].Load();
            }

            windowManager = new WindowManager();
            editorStatic = new EditorStatic();

            SelectedBox = Game1.contentManager.Load<Texture2D>("Editor/Selected");
            Alert = Game1.contentManager.Load<Texture2D>("Editor/Warning");



            CreateNewLevel();
        }

        public static void CreateNewLevel()
        {
            GameManager.MyLevel = new Level(new Rectangle(0, 0, 640, 480), new Camera(new Vector4(200, 200, 640+400, 480+400)));
        }

        public static void LoadNewLevel(Level NewLevel)
        {
            GameManager.MyLevel = NewLevel;
        }

        public static void Run()
        {
            LevelEditorWindow.EditorMode = false;
            GameManager.MyLevel.NewGame();
        }

        public static void TimeLine()
        {
            MasterEditor.TimeLineMode = !MasterEditor.TimeLineMode;

            ObjectSelectorWindow win =
            WindowManager.TopWindow as ObjectSelectorWindow;
            win.ResetIcons();

            WindowManager.Children.Remove(WindowManager.editorWindow);

            if (TimeLineMode)
                WindowManager.AddWindow(WindowManager.editorWindow = new TimeLineWindow(new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), false, false));
            else
                WindowManager.AddWindow(WindowManager.editorWindow = new EditorWindow(new Rectangle(0, 0, 800, 600), new Rectangle(0, 0, 800, 600), false, false));

            WindowManager.self.UpdateGui();
        }

        public void Update()
        {
            if (!dialogManager.InUse)
                windowManager.Update();
            else
                dialogManager.Update();
        }

        public void Draw()
        {
            Game1.graphicsDevice.SetRenderTarget(null);
            Game1.graphicsDevice.Clear(Color.Black);

            windowManager.Draw();
        }
    }
}
