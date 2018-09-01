using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace DuelBots
{
    public class EnemySpawn : BasicObject
    {
        Color MyColor = Color.White;

        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            VisibleInGame = false;
            return base.Create(Size, Position);
        }

        public override void Draw()
        {
            Game1.spriteBatch.Draw(SpawnCreator.PlayerSphere, MyRectangle, MyColor);
            base.Draw();
        }

    }
}
