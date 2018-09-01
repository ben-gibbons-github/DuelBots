using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class MenuManager
    {
        public static List<MenuBasic> CurrentMenus = new List<MenuBasic>();
        public static MenuBasic ActiveMenu;

        public static List<MenuInput> Inputs = new List<MenuInput>();

        public static MenuProfile[] Profiles = new MenuProfile[2];

        public static Texture2D BarTexture;
        public static SpriteFont Font;
        

        public static void Load()
        {
            for (int i = 0; i < 4; i++)
            {
                Inputs.Add(new MenuInput((PlayerIndex)i));
            }

            Font = Game1.contentManager.Load<SpriteFont>("Font");
            BarTexture = Game1.contentManager.Load<Texture2D>("blank");

            ActiveMenu = new LevelEditorWindow().Create();
            CurrentMenus.Add(ActiveMenu);
            ActiveMenu.Alive = true;
        }

        public static void Update(GameTime gameTime)
        {
            if(ActiveMenu!=null)
            if (ActiveMenu.NeedsInput)
                TakeInput(gameTime);

            for (int i = 0; i < CurrentMenus.Count;i++ )
                CurrentMenus[i].Update(gameTime);
        }

        public static void TakeInput(GameTime gameTime)
        {
            for (int i = 0; i < 2;i++)
                if (Profiles[i] == null)
                {
                    foreach (MenuInput Input in Inputs)
                    {
                        Input.Update(gameTime);
                        if (Input.CheckAnyButton())
                            Profiles[i] = new MenuProfile(Input);
                    }
                }
                else
                {
                    Profiles[i].Input.Update(gameTime);
                    ActiveMenu.TakeInput(Profiles[i].Input);
                }
        }

        public static void SwitchActive(MenuBasic NewMenu, bool DiscardAll,bool AddSelected)
        {
            if (ActiveMenu.Ready)
            {
                if (DiscardAll)
                {
                    foreach (MenuBasic Menu in CurrentMenus)
                        Menu.Discard(NewMenu.HeaderPosition,false);

                    NewMenu.Header = ActiveMenu.Discard(NewMenu.HeaderPosition,AddSelected);


                }

                ActiveMenu = NewMenu;
                CurrentMenus.Add(NewMenu);
            }
        }
        public static void RemoveMenu(MenuBasic Menu)
        {
            CurrentMenus.Remove(Menu);
            if (Menu == ActiveMenu)
                ActiveMenu = null;
        }

        public static void Draw()
        {
            Game1.graphicsDevice.Clear(Color.Black);

            foreach (MenuBasic Menu in CurrentMenus)
                Menu.Draw();
        }

    }
}
