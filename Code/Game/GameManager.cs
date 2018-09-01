using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class GameManager
    {
        public static Level MyLevel;
        public static GameManager self;
        public static KeyboardState KeyState;


        public GameManager()
        {
            self = this;
        }

        public void Update(GameTime gameTime)
        {
            if (MyLevel != null)
            {
                KeyState = Keyboard.GetState();

                if (!LevelEditorWindow.EditorMode)
                    if (KeyState.IsKeyDown(Keys.Escape))
                    {
                        MyLevel.EndGame();
                        LevelEditorWindow.EditorMode = true;
                    }

                MyLevel.Update(gameTime);
            }
        }

        public void Draw()
        {
            if (MyLevel != null)
            {
            

                MyLevel.PreDraw();

                Game1.graphicsDevice.SetRenderTarget(null);
                Game1.graphicsDevice.Clear(Color.Black);

                MyLevel.BackgroundDraw();

                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, GameManager.MyLevel.MyCamera.ReturnMatrix());
                MyLevel.Draw();
                ParticleSystem.DrawAlpha();
                Game1.spriteBatch.End();

                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, GameManager.MyLevel.MyCamera.ReturnMatrix());
                ParticleSystem.DrawAdditive();
                Game1.spriteBatch.End();

                Game1.spriteBatch.Begin();
                MyLevel.DrawHUD();
                Game1.spriteBatch.End();
            }
        }

    }
}
