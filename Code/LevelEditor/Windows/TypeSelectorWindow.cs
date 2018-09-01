using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    

    public class TypeSelectorWindow:Window
    {
        public TypeSelectorWindow(Rectangle MyRectangle, Rectangle HoverRectangle,bool ScrollLR,bool ScrollUD)
           : base(MyRectangle,HoverRectangle,false,false)
        {
            int PlaceX=40;
            int PlaceY=40;
            int SizeX = 48;
            int SizeY = 48;

            AddForm(
                new Button(Game1.contentManager.Load<Texture2D>("Editor/MouseModeSelect"),
                    new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX+16,SizeY+16), 4, SelectMouseSelect)
                );

            PlaceX += 64;

            Button NewButton = null;

            AddForm( NewButton=
    new Button(Game1.contentManager.Load<Texture2D>("Editor/MouseModePlace"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectMousePlace)
    );
            NewButton.Selected = true;

            PlaceX += 64;

            AddForm(
    new Button(Game1.contentManager.Load<Texture2D>("Editor/MouseModeMove"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                    new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectMouseMove)
    );

            PlaceX += 64;
            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/MouseModeSquare"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
        new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SelectMouseSquare)
);
        }

        public void SelectMouseMove(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            MasterEditor.mouseMode = MouseMode.Move;
        }

        public void SelectMousePlace(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            MasterEditor.mouseMode = MouseMode.Place;
        }

        public void SelectMouseSelect(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            MasterEditor.mouseMode = MouseMode.Select;
        }

        public void SelectMouseSquare(Button button)
        {
            DeselectButtons();
            button.Selected = true;
            MasterEditor.mouseMode = MouseMode.Square;
        }
    }
}
