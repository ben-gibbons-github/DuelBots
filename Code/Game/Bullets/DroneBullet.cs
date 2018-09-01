using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class DroneBullet : Bullet
    {

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            Accuracy = 10;
            FireSpeed = 0.1f;
            Reps = 1;
            Damage = 25;
            Push = 0.5f;
            MaxLifeTime = 30000;

            Size = new Vector2(16);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }


        public override void Update(GameTime gameTime)
        {
            Speed *= (1 + (float)gameTime.ElapsedGameTime.Milliseconds / 1000f);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(Render.BlankTexture, MyRectangle, Color.Green);
            // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            base.Draw();
        }
    }
}
