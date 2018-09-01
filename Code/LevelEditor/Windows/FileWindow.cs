using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class FileWindow:Window
    {
        public FileWindow(Rectangle MyRectangle, Rectangle HoverRectangle, bool ScrollLR, bool ScrollUD)
            : base(MyRectangle, HoverRectangle, false, false)
        {
            int PlaceX = 48;
            int PlaceY = 40;
            int SizeX = 48;
            int SizeY = 48;


            AddForm(
    new Button(Game1.contentManager.Load<Texture2D>("Editor/New"),
        new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
        new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, NewProject)
    );
            PlaceY += 64;

            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/Load"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, LoadProject)
);
            PlaceY += 64;


            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/Save"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, SaveProject)
);
            PlaceY += 64;


            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/Run"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, RunProject)
);
            PlaceY += 64;

            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/TimeLine"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, TimeLine)
);
            PlaceY += 64;

            AddForm(
new Button(Game1.contentManager.Load<Texture2D>("Editor/Exit"),
new Rectangle(PlaceX, PlaceY, SizeX, SizeY),
new Rectangle(PlaceX, PlaceY, SizeX + 16, SizeY + 16), 4, Exit)
);
            PlaceY += 64;
        }

        void NewProject(Button button)
        {
            MasterEditor.CreateNewLevel();
        }

        void LoadProject(Button button)
        {
            MasterEditor.dialogManager.Load();
        }

        void SaveProject(Button button)
        {
            MasterEditor.dialogManager.Save();
        }

        void RunProject(Button button)
        {
            MasterEditor.Run();
        }

        void TimeLine(Button button)
        {
            MasterEditor.TimeLine();
        }

        void Exit(Button button)
        {
            MenuManager.SwitchActive(new StartWindow().Create(), true, false);
        }
    }
}
