using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class LocalGameWindow : MenuBasic
    {
        MenuStemBlock MyBlock = null;

        public override void TakeInput(MenuInput Input)
        {
            if (Input.CheckJustPressed(Buttons.B))
                MenuManager.SwitchActive(new MainWindow().Create(), true, false);
            if (Input.CheckJustPressed(Buttons.A))
            {
                if (ScrollY == 0)
                    MenuManager.SwitchActive(new MapWindow().Create(), true, true);
                if (ScrollY == 1)
                    MenuManager.SwitchActive(new BotLevelWindow().Create(), true, true);
                if (this.ScrollY == 2)
                {
                    this.Discard(Vector2.Zero, false);
                    MenuManager.SwitchActive(new GameWindow().Create(), true, false);
                }
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
            return (MenuStem)new MenuStem(null, Vector4.One, "Local Game").Create(HeaderPosition - new Vector2(-200, 0), Vector2.Zero, -1, -1, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentStep == 1)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Map: " + SettingsHolder.map.ToString()), 8);
                Steps++;
            }

            if (CurrentStep == 2)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Bot Strength: " + SettingsHolder.botLevel.ToString()), 8);
                Steps++;
            }

            if (CurrentStep == 3)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Play"), 8);
                Steps++;
            }


            if (CurrentStep > 3)
                Ready = true;

            base.Update(gameTime);
        }
    }
}
