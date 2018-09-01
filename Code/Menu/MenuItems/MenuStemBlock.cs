using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class MenuStemBlock:MenuItem
    {
        public List<MenuStem> Children = new List<MenuStem>();

        public int DiscardTimer;

        Vector2 PlacePosition;

        Vector2 Seperation;
        Vector2 Margins = new Vector2(25, 10);

        Vector2 TargetSize;

        float ItemWidth;

        int PlaceX = 0;
        int PlaceY = 0;

        public MenuStemBlock(int PlaceX, int PlaceY,float ItemWidth)
        {
            this.ItemWidth = ItemWidth;
            this.Seperation = Margins;
            this.PlaceX = PlaceX;
            this.PlaceY = PlaceY;
        }

        public override MenuItem Create(Vector2 Position, Vector2 Size, int X, int Y,MenuBasic Parent)
        {
            DestroyOnFade = false;

            PlacePosition = Position + new Vector2(Seperation.X);

            Size.Y = 0;

            return base.Create(Position, Size, X, Y,Parent);

            
        }

        public void AddStem(MenuStem Stem,int Margins)
        {
            Vector2 Size = new Vector2(ItemWidth, MenuManager.Font.MeasureString(Stem.MyText).Y+Margins);
            Children.Add((MenuStem)Stem.Create(PlacePosition,Size, PlaceX, PlaceY, Parent));

            PlaceY++;
            PlacePosition.Y += MenuManager.Font.MeasureString(Stem.MyText).Y + Margins + Seperation.Y;

            Vector2 MaxSize= Size;
            float SizeY=this.Margins.Y;

           // foreach (MenuStem child in Children)
            {
            //    MaxSize.X = Math.Max(MaxSize.X, child.Size.X);

            }



            foreach (MenuStem child in Children)
            {
             //   child.ChangeSize(new Vector2(MaxSize.X,child.Size.Y));
                SizeY = Math.Max(SizeY, (child.Position.Y - Position.Y) + child.Size.Y + this.Margins.Y);
            }

            Parent.AddItem(Stem);

            TargetSize = new Vector2(Size.X, Math.Max(Size.Y, SizeY + this.Margins.Y));
        }

        public override void Update(GameTime gameTime)
        {
            if (Discarded)
            {
                DiscardTimer += gameTime.ElapsedGameTime.Milliseconds;

                for (int i = 0; i < Children.Count(); i++)
                {
                    if (DiscardTimer > i * 100)
                        if (!Children[i].Selected)
                        Children[i].Discarded = true;
                }
                if (DiscardTimer > Children.Count * 100 && MenuAlpha == 0)
                    Destroy();
            }
            else
            {
                if (TargetSize.Y > Size.Y)
                    ChangeSize(new Vector2(Size.X, Size.Y + 7f));
            }

            base.Update(gameTime);
        }

        public override void Discard()
        {
            if (!Discarded)
                DiscardTimer = 0;
            base.Discard();
        }

        public override void Draw()
        {
            Color MyColor= new Color(new Vector4(0.4f)*MenuAlpha);
            Game1.spriteBatch.Draw(MenuManager.BarTexture, MyRectangle, MyColor);
            base.Draw();
        }
    }
}
