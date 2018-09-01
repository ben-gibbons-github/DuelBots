using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuelBots
{
    public class LevelEditorWindow:MenuBasic
    {
        public static MasterEditor masterEditor;
        public static GameManager gameManager;
        public static bool EditorMode = true;

        public override MenuBasic Create()
        {
            this.NeedsInput = false;
            Ready = true;

            masterEditor = new MasterEditor();
            masterEditor.Load();
            gameManager = new GameManager();

            return base.Create();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (MenuManager.ActiveMenu != this)
                Destroy();

            else
            {
                if (EditorMode)
                    masterEditor.Update();
                else
                    gameManager.Update(gameTime);

                base.Update(gameTime);
            }
        }

        public override void Draw()
        {
            GameManager.MyLevel.PreDraw();

            if (EditorMode)
                masterEditor.Draw();
            else
                gameManager.Draw();

        }


    }
}
