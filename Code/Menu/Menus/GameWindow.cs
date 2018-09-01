using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DuelBots
{
    public class GameWindow : MenuBasic
    {
        public override MenuBasic Create()
        {
            this.NeedsInput = false;
            Ready = true;
            LevelEditorWindow.EditorMode = false;
            LevelEditorWindow.masterEditor = new MasterEditor();
            LevelEditorWindow.masterEditor.Load();
            LevelEditorWindow.gameManager = new GameManager();


            MasterEditor.LoadNewLevel(DialogManager.ReadFile(new BinaryReader(File.Open("Content/Game/Levels/"+SettingsHolder.map.ToString() + ".lvl", FileMode.Open))));
            GameManager.MyLevel.Reset();
            MasterEditor.Run();

            return base.Create();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (MenuManager.ActiveMenu != this)
                Destroy();

            else
            {
                if (LevelEditorWindow.EditorMode)
                    LevelEditorWindow.masterEditor.Update();
                else
                    LevelEditorWindow.gameManager.Update(gameTime);

                base.Update(gameTime);
            }
        }

        public override void Draw()
        {
            GameManager.MyLevel.PreDraw();

            if (LevelEditorWindow.EditorMode)
                LevelEditorWindow.masterEditor.Draw();
            else
                LevelEditorWindow.gameManager.Draw();

        }


    }
}
