using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{


    public class SinglePlayerWindow : Window
    {
        public SinglePlayerWindow(Rectangle MyRectangle, Rectangle HoverRectangle, bool ScrollLR, bool ScrollUD)
            : base(MyRectangle, HoverRectangle, false, false)
        {
            int PlaceX = 40;
            int PlaceY = 40;
            int SizeX = 48;
            int SizeY = 48;

            AddForm(
                new Button(Game1.contentManager.Load<Texture2D>("Editor/OnePlayer"),
                    new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, OnePlayer)
                );

            PlaceX += 64;

            Button NewButton = null;

            AddForm(NewButton =
    new Button(Game1.contentManager.Load<Texture2D>("Editor/TwoPlayer"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, TwoPlayer)
    );
            NewButton.Selected = true;

           
        }

        public void OnePlayer(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            GameManager.MyLevel.SinglePlayer = true;
        }
        public void TwoPlayer(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            GameManager.MyLevel.SinglePlayer = false;
        }
       
    }
}
