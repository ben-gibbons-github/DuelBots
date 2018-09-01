using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class DamageBar:HUDBasic
    {
        public Color MyColor=Color.White;
        public Color BackgroundColor = new Color(0.4f, 0.4f, 0.4f, 0.4f);
        public Rectangle MyRectangle;
        public Player MyPlayer;
        public SpriteFont font;
        public Vector2 DamagePosition;

        public DamageBar(Rectangle MyRectangle,Player MyPlayer,Color MyColor)
        {
            this.MyPlayer = MyPlayer;
            this.MyRectangle = MyRectangle;
            font = Game1.contentManager.Load<SpriteFont>("Game/LargeFont");
            DamagePosition = new Vector2(MyRectangle.X+MyRectangle.Width/2, MyRectangle.Y+MyRectangle.Height/2);
        }

        public override void Draw()
        {
            string MyString = ((int)Math.Min(9999,MyPlayer.Damage)).ToString();
            Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, BackgroundColor);
            Game1.spriteBatch.DrawString(font, MyString, DamagePosition - font.MeasureString(MyString) / 2, MyColor);
            base.Draw();
        }
    }
}
