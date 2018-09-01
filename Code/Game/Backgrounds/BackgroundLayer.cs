using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class BackgroundLayer
    {
        public float CameraFollow = 0;
        public Rectangle MyRectangle = new Rectangle(0, 0, Game1.ResolutionX, Game1.ResolutionY);
        public Texture2D MyTexture;
        public Color MyColor;

        public BackgroundLayer(float CameraFollow, Rectangle MyRectangle, Texture2D MyTexture, Color MyColor)
        {
            this.MyColor = MyColor;
            this.CameraFollow = CameraFollow;
            this.MyRectangle = MyRectangle;
            this.MyTexture = MyTexture;
        }

        public void Draw()
        {
            if (CameraFollow > 0)
            {

               /* Rectangle DrawRectangle = new Rectangle(
                    MyRectangle.Width - Game1.ResolutionX + (int)(GameManager.MyLevel.MyCamera.MyRectangle.X * CameraFollow)+GameManager.MyLevel.MyRectangle.X,
                    MyRectangle.Height - Game1.ResolutionY + (int)(GameManager.MyLevel.MyCamera.MyRectangle.Y * CameraFollow) + GameManager.MyLevel.MyRectangle.Y,
                    MyRectangle.Width, MyRectangle.Height);*/

                Rectangle DrawRectangle = new Rectangle(
                    GameManager.MyLevel.MyRectangle.X+GameManager.MyLevel.MyRectangle.Width/2-MyRectangle.Width/2,
                    GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Height  - MyRectangle.Height,
                    MyRectangle.Width, MyRectangle.Height);

                Game1.spriteBatch.Draw(MyTexture, DrawRectangle,MyColor);
            }
            else
                Game1.spriteBatch.Draw(MyTexture, MyRectangle, MyColor);
        }
    }
}
