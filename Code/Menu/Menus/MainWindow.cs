using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class MainWindow : MenuBasic
    {
        MenuStemBlock MyBlock = null;

        public override void TakeInput(MenuInput Input)
        {
            if (Input.CheckJustPressed(Buttons.B))
                MenuManager.SwitchActive(new StartWindow().Create(), true, false);
            if (Input.CheckJustPressed(Buttons.A))
            {
                if (ScrollY == 0)
                    MenuManager.SwitchActive(new LocalGameWindow().Create(), true, true);

                if (this.ScrollY == 3)
                    MenuManager.SwitchActive(new LevelEditorWindow().Create(), true, false);
                if (ScrollY == 4)
                    Game1.self.Exit();
            }
            base.TakeInput(Input);
        }

        public override MenuBasic Create()
        {
            AddItem(MyBlock = (MenuStemBlock)new MenuStemBlock(0, 0,200).Create(new Vector2(200), new Vector2(175, 200), 0, 0, this));
            HeaderPosition = new Vector2(200, 200);

            return base.Create();
        }

        public override MenuStem MakeHeader()
        {
            return (MenuStem)new MenuStem(null, Vector4.One, "Start").Create(HeaderPosition - new Vector2(-200, 0), Vector2.Zero, -1, -1, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentStep == 1)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Local Game"), 8);
                Steps++;
            }

            if (CurrentStep == 2)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Online Game"), 8);
                Steps++;
            }

            if (CurrentStep == 3)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Options"), 8);
                Steps++;
            }

            if (CurrentStep == 4)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Level Editor"), 8);
                Steps++;
            }

            if (CurrentStep == 5)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Exit"), 8);
                Steps++;
            }


            if (CurrentStep > 5)
                Ready = true;

            base.Update(gameTime);
        }
    }
}
