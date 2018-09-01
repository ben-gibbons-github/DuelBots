using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace DuelBots
{
    public class PlayerSpawn:BasicObject
    {
        Color MyColor=Color.White;
        public PlayerIndex playerIndex=PlayerIndex.Two;

        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            VisibleInGame = false;

            int Count = 0;


            bool RedTaken = false;

            foreach (BasicObject Object in GameManager.MyLevel.ObjectList)
                if(Object.GetType().Equals(typeof(PlayerSpawn)))
            {
                Count++;
                    PlayerSpawn spawn=(PlayerSpawn)Object;

                if (spawn.MyColor.Equals(Color.Red))
                    RedTaken = true;

            }

            if (Count > 2)
                this.Delete();
            else
            {
                if (!RedTaken)
                {
                    MyColor = Color.Red;
                    playerIndex = PlayerIndex.One;
                }
                else
                {
                    MyColor = Color.Blue;
                    playerIndex = PlayerIndex.Two;
                }
            }


            return base.Create(Size, Position);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(SpawnCreator.PlayerSphere, MyRectangle, MyColor);
            base.Draw();
        }

        public override void Read(BinaryReader Reader)
        {
            MyColor = DialogManager.ReadColor(Reader);
            playerIndex = (PlayerIndex)Reader.ReadInt32();
            base.Read(Reader);
        }

        public override void Write(BinaryWriter Writer)
        {
            DialogManager.WriteColor(Writer,MyColor);
            Writer.Write((Int32)playerIndex);
            base.Write(Writer);
        }
    }
}
