using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    class BlockCreator:BasicObjectCreator
    {

        public static Texture2D BlockFullLeft;
        public static Texture2D BlockFullRight;
        public static Texture2D BlockFullUp;
        public static Texture2D BlockFullDown;

        public static Texture2D BlockBevelLeft;
        public static Texture2D BlockBevelRight;
        public static Texture2D BlockBevelDLeft;
        public static Texture2D BlockBevelDRight;

        public static Texture2D BlockDoubleUD;
        public static Texture2D BlockDoubleLR;

        public static Texture2D BlockTipUp;
        public static Texture2D BlockTipDown;
        public static Texture2D BlockTipRight;
        public static Texture2D BlockTipLeft;

        public static Texture2D BlockBlack;


        public override void Create()
        {
            MyObjectName = "Block";
            base.Create();
        }

        public override BasicObject ReturnObject()
        {
            return new Block();
        }

        public override void Load()
        {
            BlockFullRight = Game1.contentManager.Load<Texture2D>("Game/BlockFullRight");
            BlockFullUp = Game1.contentManager.Load<Texture2D>("Game/BlockFullUp");
            BlockFullDown = Game1.contentManager.Load<Texture2D>("Game/BlockFullDown");
            BlockFullLeft = Game1.contentManager.Load<Texture2D>("Game/BlockFullLeft");

            BlockBevelRight = Game1.contentManager.Load<Texture2D>("Game/BlockBevelRight");
            BlockBevelDRight = Game1.contentManager.Load<Texture2D>("Game/BlockBevelDRight");
            BlockBevelLeft = Game1.contentManager.Load<Texture2D>("Game/BlockBevelLeft");
            BlockBevelDLeft = Game1.contentManager.Load<Texture2D>("Game/BlockBevelDLeft");

            BlockDoubleLR = Game1.contentManager.Load<Texture2D>("Game/BlockDoubleLR");
            BlockDoubleUD = Game1.contentManager.Load<Texture2D>("Game/BlockDoubleUD");

            BlockTipUp = Game1.contentManager.Load<Texture2D>("Game/BlockTip");
            BlockTipRight = Game1.contentManager.Load<Texture2D>("Game/BlockTipRight");
            BlockTipLeft = Game1.contentManager.Load<Texture2D>("Game/BlockTipLeft");
            BlockTipDown = Game1.contentManager.Load<Texture2D>("Game/BlockTipDown");

            BlockBlack = Game1.contentManager.Load<Texture2D>("Game/BlockBlack");


            IconTexture = BlockDoubleLR;
            base.Load();
        }
    }
}
