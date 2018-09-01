using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuelBots
{
    public class MenuBasic
    {
        public int ScrollX = 0;
        public int ScrollY = 0;
        public int MaxScrollX = 0;
        public int MaxScrollY = 0;

        public bool NeedsInput = true;

        public bool Active = true;
        public bool Visible = true;

        public bool IsDiscarded = false;
        public bool Alive = false;
        public bool Selected = false;
        public bool Ready = false;

        public int AliveTime = 0;
        public int Steps = 0;
        public int CurrentStep;

        public bool LocalHeader = false;
 

        public Vector2 HeaderPosition;

        public List<MenuItem> Children = new List<MenuItem>();
        public MenuItem Header;

        public virtual MenuBasic Create()
        {
            return this;
        }


        public virtual void Update(GameTime gameTime)
        {
            if (Header == null)
            {

                Header = MakeHeader();
                if (Header != null)
                {
                    LocalHeader = true;
                    Header.Selected = true;
                    MenuStem Stem = (MenuStem)Header;
                    Stem.TargetPosition = HeaderPosition - new Vector2(0, Stem.Size.Y + 25);
                    Children.Add(Header);
                }
                else
                    Alive = true;
            }

            CurrentStep = -1;

            if (Alive)
            {
                AliveTime += gameTime.ElapsedGameTime.Milliseconds;

                if (!IsDiscarded)
                    if (AliveTime > Steps * 100)
                    {
                        CurrentStep = (int)Math.Floor(AliveTime / 100f);
                        Steps = CurrentStep;
                    }

                if (AliveTime > 100)
                    for (int i = 0; i < Children.Count; i++)
                    {
                        MenuItem Item = Children[i];

                        if (Item != null)
                        {
                            if (ScrollX == Item.X && ScrollY == Item.Y)
                                Item.Hovering = true;
                            else
                                Item.Hovering = false;

                            Item.Update(gameTime);
                        }
                    }
            }
            if(LocalHeader&&Header!=null)
            Header.Update(gameTime);

            if (IsDiscarded && Children.Count == 0)
                Destroy();
        }

        public virtual void TakeInput(MenuInput Input)
        {
            if (NeedsInput)
            {
                if (Math.Abs(Input.PadState.ThumbSticks.Left.Y) > 0.1f)
                    if (Math.Abs(Input.PreviousPadState.ThumbSticks.Left.Y) < 0.1f)
                {
                    if (Input.PadState.ThumbSticks.Left.Y < 0)
                        ScrollY = Math.Min(ScrollY + 1, MaxScrollY);
                    else
                        ScrollY = Math.Max(ScrollY - 1, 0);
                }

                foreach (MenuItem Item in Children)
                    Item.TakeInput(Input);


            }
        }

        public virtual void Draw()
        {
            Game1.spriteBatch.Begin();

            foreach (MenuItem Item in Children)
                Item.Draw();

            Game1.spriteBatch.End();
        }

        public virtual void AddItem(MenuItem Item)
        {
            Children.Add(Item);
            MaxScrollX = Math.Max(Item.X, MaxScrollX);
            MaxScrollY = Math.Max(Item.Y, MaxScrollY);
        }

        public virtual void Destroy()
        {
            MenuManager.RemoveMenu(this);
        }

        public virtual void DestroyItem(MenuItem Item)
        {
            Children.Remove(Item);
        }

        public virtual MenuStem MakeHeader()
        {
            return null;
        }

        public virtual MenuItem Discard(Vector2 NewMenuTop, bool AddSelected)
        {
            MenuItem Result = null;
            IsDiscarded = true;

            if (Header != null)
            {
                Header.Selected = false;
                Header.Discard();
                Header.Discarded = true;
            }

            foreach (MenuItem Item in Children)
            {
                if (AddSelected && Item.Hovering && Item.GetType().Equals(typeof(MenuStem)))
                {
                    Result = Item;
                    Item.Selected = true;
                    MenuStem Stem = (MenuStem)Item;
                    Stem.TargetPosition = NewMenuTop - new Vector2(0, Stem.Size.Y + 25);
                }
                else
                    Item.Discard();
            }

            return Result;
        }
    }
}
