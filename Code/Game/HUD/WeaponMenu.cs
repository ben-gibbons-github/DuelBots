using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class WeaponMenu : MenuBasic
    {
        MenuStemBlock MyBlock = null;
        Vector2 Position;
        Player MyPlayer;

        public WeaponMenu(Vector2 Position,Player MyPlayer)
        {
            this.MyPlayer = MyPlayer;
            this.Position = Position;
        }
        public void Select()
        {
            switch (ScrollY)
            {
                case 0:
                    MyPlayer.GunCurrent = new MachineGun().Create(MyPlayer);
                    break;
                case 1:
                    MyPlayer.GunCurrent = new SlimeGun().Create(MyPlayer);
                    break;
                case 2:
                    MyPlayer.GunCurrent = new Sword().Create(MyPlayer);
                    break;
                case 3:
                    MyPlayer.GunCurrent = new RailGun().Create(MyPlayer);
                    break;
                case 4:
                    MyPlayer.GunCurrent = new RocketLauncher().Create(MyPlayer);
                    break;

            }
            this.Discard(Vector2.Zero, false);
        }

        public override void TakeInput(MenuInput Input)
        {
            if (Input.CheckJustPressed(Buttons.A))
            {
                Select();
            }

            base.TakeInput(Input);
        }

        public override MenuBasic Create()
        {
            AddItem(MyBlock = (MenuStemBlock)new MenuStemBlock(0, 0, 200).Create(Position, new Vector2(175, 200), 0, 0, this));
            HeaderPosition = Position;

            return base.Create();
        }

        public override MenuStem MakeHeader()
        {
            return (MenuStem)new MenuStem(null, Vector4.One, "Weapon: ").Create(HeaderPosition - new Vector2(-200, 0), Vector2.Zero, -1, -1, this);
        }

        public override void Update(GameTime gameTime)
        {
            if (CurrentStep == 1)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Machine Gun"), 8);
                Steps++;
            }

            if (CurrentStep == 2)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Slime Gun"), 8);
                Steps++;
            }

            if (CurrentStep == 3)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Sword"), 8);
                Steps++;
            }

            if (CurrentStep == 4)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Railgun"), 8);
                Steps++;
            }


            if (CurrentStep == 5)
            {
                MyBlock.AddStem(new MenuStem(null, Vector4.One, "Rocket Launcher"), 8);
                Steps++;
            }

            if (CurrentStep > 6)
                Ready = true;

            base.Update(gameTime);
        }

        public override void Draw()
        {
            foreach (MenuItem Item in Children)
                Item.Draw();
        }
        public override void Destroy()
        {
            MyPlayer.GunMenu = null;
        }
    }
}
