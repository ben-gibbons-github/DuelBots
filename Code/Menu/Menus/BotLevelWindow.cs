using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class BotLevelWindow : MenuBasic
    {
        MenuStemBlock MyBlock = null;

        public override void TakeInput(MenuInput Input)
        {
            if (Input.CheckJustPressed(Buttons.B))
                MenuManager.SwitchActive(new LocalGameWindow().Create(), true, false);
            if (Input.CheckJustPressed(Buttons.A))
            {
                SettingsHolder.botLevel = (BotLevel)this.ScrollY;
                MenuManager.SwitchActive(new LocalGameWindow().Create(), true, false);
            }
            base.TakeInput(Input);
        }

        public override MenuBasic Create()
        {
            AddItem(MyBlock = (MenuStemBlock)new MenuStemBlock(0, 0, 200).Create(new Vector2(200), new Vector2(175, 200), 0, 0, this));
            HeaderPosition = new Vector2(200, 200);

            return base.Create();
        }

        public override MenuStem MakeHeader()
        {
            return (MenuStem)new MenuStem(null, Vector4.One, "Bot Level:").Create(HeaderPosition - new Vector2(-200, 0), Vector2.Zero, -1, -1, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentStep == 1)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Easy"), 8);
                Steps++;
            }

            if (CurrentStep == 2)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Medium"), 8);
                Steps++;
            }

            if (CurrentStep == 3)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Hard"), 8);
                Steps++;
            }

            if (CurrentStep == 4)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Extreme"), 8);
                Steps++;
            }

            if (CurrentStep == 5)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Deadly"), 8);
                Steps++;
            }


            if (CurrentStep > 5)
                Ready = true;

            base.Update(gameTime);
        }
    }
}
