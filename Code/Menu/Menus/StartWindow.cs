using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class StartWindow:MenuBasic
    {
        MenuStemBlock MyBlock = null;

        public override MenuBasic Create()
        {
            AddItem(MyBlock = (MenuStemBlock)new MenuStemBlock(0, 0,200).Create(new Vector2(200), new Vector2(175,200), 0, 0, this));
            HeaderPosition = new Vector2(200, 200);

            return base.Create();
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentStep ==1)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Start"), 8);
                Steps++;
            }
            if (CurrentStep > 2)
                Ready = true;
            
            base.Update(gameTime);
        }

        public override void TakeInput(MenuInput Input)
        {
            if (Input.CheckJustPressed(Buttons.A))
                MenuManager.SwitchActive(new MainWindow().Create(), true, true);
            if (Input.CheckJustPressed(Buttons.B))
                MenuManager.SwitchActive(new StartWindow().Create(), true, false);


            base.TakeInput(Input);
        }
    }
}
