using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
                

    public class ObjectSelectorWindow : Window
    {
        int ModX = 64;

        public ObjectSelectorWindow(Rectangle MyRectangle, Rectangle HoverRectangle, bool ScrollLR, bool ScrollUD)
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

            bool PlaceSelected = true;

            Button NewButton = null;

            foreach (BasicObjectCreator Creator in BasicObjectCreator.BasicObjectTypes)
                if(Creator.IsTimeLineCreator == MasterEditor.TimeLineMode)
            {
                AddForm(
                   NewButton = new Button(Creator.IconTexture,
                        new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
                        new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, Select)
                    );
                PlaceX += ModX;

                NewButton.Selected = PlaceSelected;
                if (PlaceSelected)
                    MasterEditor.SelectedObjectCreater = Creator;

                PlaceSelected = false;
            }
        }

        void Select(Button button)
        {
            DeselectButtons();
            button.Selected = true;

            foreach (BasicObjectCreator Creator in BasicObjectCreator.BasicObjectTypes)
                if (Creator.IconTexture == button.Image)
                    MasterEditor.SelectedObjectCreater = Creator;

        }

    }
}
