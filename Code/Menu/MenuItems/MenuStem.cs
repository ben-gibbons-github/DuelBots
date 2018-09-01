using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public delegate void StemAction();

    public class MenuStem:MenuItem
    {
        public StemAction MyAction;
        public Vector2 TargetPosition;
        public string MyText;
        public Vector4 MyColor = Vector4.One;
        public Vector2 Speed = Vector2.Zero;
        public Vector2 Margins = new Vector2(8);

        public static Vector2 HoverOffset = new Vector2(25, 0);

        public MenuStem(StemAction MyAction,Vector4 MyColor, string MyText)
        {
            this.MyText = MyText;
            this.MyColor = MyColor;
            this.MyAction = MyAction;
        }

        public override MenuItem Create(Vector2 Position, Vector2 Size, int X, int Y,MenuBasic Parent)
        {
            DestroyOnFade = false;
            Fade = false;

            if (Size == Vector2.Zero)
            {
                Size = MenuManager.Font.MeasureString(MyText) * new Vector2(2.5f, 1) + Margins;
            }

            TargetPosition = Position + HoverOffset;
            return base.Create(Position, Size, X, Y,Parent);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Selected)
            {
                if (!Discarded)
                    Position = StartPos + (TargetPosition - StartPos) * HoverAlpha;
                else
                {
                    Speed.X -= 1 * (float)gameTime.ElapsedGameTime.Milliseconds / 1000 * 60;
                    Position += Speed;
                    if (Position.X + Size.X < 0)
                        Destroy();
                }
            }
            else
            {
                Position += (TargetPosition - Position) * 0.1f * (float)gameTime.ElapsedGameTime.Milliseconds / 1000f *60f;
                if (Vector2.Distance(Position, TargetPosition) < 50)
                    MenuManager.ActiveMenu.Alive = true;
            }


                ChangePosition();
        }

        public override void Draw()
        {
            Color MyColor = new Color(Vector4.One * (HoverAlpha/2+0.25f)*MenuAlpha);
            Game1.spriteBatch.Draw(MenuManager.BarTexture, MyRectangle, MyColor);
           
            Game1.spriteBatch.Draw(MenuManager.BarTexture, new Rectangle(MyRectangle.X+2,MyRectangle.Y+2,MyRectangle.Width,MyRectangle.Height), new Color(new Vector4(0, 0, 0, 0.5f) * MenuAlpha));
            Game1.spriteBatch.DrawString(MenuManager.Font, MyText, Position + Size / 2 + new Vector2(2) - MenuManager.Font.MeasureString(MyText) / 2, new Color(new Vector4(0, 0, 0, 0.5f) * MenuAlpha));
            Game1.spriteBatch.DrawString(MenuManager.Font, MyText, Position + Size / 2 - MenuManager.Font.MeasureString(MyText) / 2,new Color(Vector4.One*MenuAlpha));

            base.Draw();
        }

        public override void Discard()
        {
            
        }
    }
}
