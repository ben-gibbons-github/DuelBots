using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public  class MenuInput
    {
        public PlayerIndex MyIndex;
        public GamePadState PadState;
        public GamePadState PreviousPadState;

        public MenuInput(PlayerIndex MyIndex)
        {
            this.MyIndex = MyIndex;

        }

        public void Update(GameTime gameTime)
        {
            PreviousPadState = PadState;
            PadState = GamePad.GetState(MyIndex);
        }

        public bool CheckJustPressed(Buttons button)
        {
            if (PadState.IsButtonDown(button) && PreviousPadState.IsButtonUp(button))
                return true;
            else
                return false;
        }

        public bool CheckAnyButton()
        {
            foreach(Buttons button in Enum.GetValues(typeof(Buttons)))
                if(PadState.IsButtonDown(button))
                    return true;

            return false;
        }

    }
}
