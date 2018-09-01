using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DuelBots
{
    public class EditorStatic
    {
        public static EditorStatic self;
        public static Texture2D BlankTexture;

        public EditorStatic()
        {
            self = this;
            BlankTexture = Game1.contentManager.Load<Texture2D>("blank");
        }

        public static Rectangle CloneRectangle(Rectangle Base)
        {
            return new Rectangle(Base.X, Base.Y, Base.Width, Base.Height);
        }
        public static Rectangle SizeCloneRectangle(Rectangle Base)
        {
            return new Rectangle(0, 0, Base.Width, Base.Height);
        }

        public static void DrawRectangle(Rectangle DestinationRectangle,Color color, bool Filled)
        {
            if (Filled)
                Game1.spriteBatch.Draw(BlankTexture, DestinationRectangle, color);
            else
            {
                Game1.spriteBatch.Draw(BlankTexture, new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, 1, DestinationRectangle.Height), color);
                Game1.spriteBatch.Draw(BlankTexture, new Rectangle(DestinationRectangle.X + DestinationRectangle.Width-1, DestinationRectangle.Y, 1, DestinationRectangle.Height), color);
                Game1.spriteBatch.Draw(BlankTexture, new Rectangle(DestinationRectangle.X, DestinationRectangle.Y, DestinationRectangle.Width, 1), color);
                Game1.spriteBatch.Draw(BlankTexture, new Rectangle(DestinationRectangle.X , DestinationRectangle.Y + DestinationRectangle.Height-1, DestinationRectangle.Width, 1), color);
            }
        }

    }
}
