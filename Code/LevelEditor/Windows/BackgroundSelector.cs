using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{


    public class BackgroundSelectorWindow : Window
    {
        int ModX = 64;

        public BackgroundSelectorWindow(Rectangle MyRectangle, Rectangle HoverRectangle, bool ScrollLR, bool ScrollUD)
            : base(MyRectangle, HoverRectangle, false, false)
        {

            ResetIcons();

        }

        public void ResetIcons()
        {
            Children.Clear();

            int PlaceX = 40;
            int PlaceY = 40;
            int SizeX = 48;
            int SizeY = 48;

            Button NewButton = null;
            AddForm(
               NewButton = new Button(Game1.contentManager.Load<Texture2D>("Editor/New"),
                    new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectSpace)
                );
            PlaceX += ModX;

            AddForm(
               NewButton = new Button(Game1.contentManager.Load<Texture2D>("Editor/New"),
                    new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectForest)
                );
            PlaceX += ModX;

            AddForm(
               NewButton = new Button(Game1.contentManager.Load<Texture2D>("Editor/New"),
                    new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectCave)
                );
            PlaceX += ModX;

        }

        void SelectSpace(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            GameManager.MyLevel.MyBackground = BackgroundBasic.ReturnBackground(0);
        }

        void SelectForest(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            GameManager.MyLevel.MyBackground = BackgroundBasic.ReturnBackground(1);
        }

        void SelectCave(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            GameManager.MyLevel.MyBackground = BackgroundBasic.ReturnBackground(2);
        }
    }
}
