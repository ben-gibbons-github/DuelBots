using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class Render
    {
        public static Texture2D BlankTexture;

        public static void Create()
        {
            BlankTexture = Game1.contentManager.Load<Texture2D>("Blank");
        }

        public static void DrawLine(Vector2 StartPos, Vector2 EndPos,Color color)
        {
            Game1.spriteBatch.Draw(
                EditorStatic.BlankTexture,
                new Rectangle((int)StartPos.X,(int)StartPos.Y,(int)Vector2.Distance(StartPos,EndPos),1),
                null,
                color,
                (float)Math.Atan2(StartPos.Y - EndPos.Y, StartPos.X - EndPos.X) - (float)Math.PI,
                Vector2.Zero,
                SpriteEffects.None,
                0
                );
        }


        public static void DrawSquare(Vector2 StartPos, Vector2 EndPos,int Width,Texture2D Texture, Color color)
        {
            Game1.spriteBatch.Draw(
                Texture,
                new Rectangle((int)StartPos.X+Width/2, (int)StartPos.Y, (int)Vector2.Distance(StartPos, EndPos), Width),
                null,
                color,
                (float)Math.Atan2(StartPos.Y - EndPos.Y, StartPos.X - EndPos.X) - (float)Math.PI,
                Vector2.Zero,
                SpriteEffects.None,
                0
                );
        }

    }
}
