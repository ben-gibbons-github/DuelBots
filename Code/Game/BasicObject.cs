using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace DuelBots
{
    public class BasicObject
    {
        public BasicObject LastDamager;

        public bool Alive = true;
        public bool FriendlyFire = false;

        public Vector2 Direction = Vector2.Zero;
        public Vector2 Speed;
        public Vector2 Gravity;

        public BasicObject Creator;

        public Vector2 Position;
        public Vector2 Size;
        public Rectangle MyRectangle;

        public float Life = 100;
        public bool Died = false;
        public float Damage = 0;
        public float Push = 0;
        public float WallDamageMult = 1;

        public bool Selected = false;
        public bool Alert = false;
        public bool VisibleInGame = true;
        public bool Solid = false;

        public bool Changed = false;

        public int Team = 0;

        public BlockMode blockMode = BlockMode.Full;

        public string CreatorString;

        public Facing MyFacing = Facing.Right;

        public virtual BasicObject Create(Vector2 Size, Vector2 Position)
        {
            MyRectangle = new Rectangle();
            ChangeSize(Size);
            ChangePosition(Position);

            return this;

        }

        public virtual bool CheckCollision(Rectangle CheckRect)
        {
            return CheckRect.Intersects(MyRectangle);
        }

        public virtual Vector2 Bounce(Rectangle CheckRect, Vector2 Direction, int Exact)
        {
            Vector2 Result = Direction;

            if (Math.Abs((MyRectangle.X + MyRectangle.Width / 2) - (CheckRect.X + CheckRect.Width / 2)) <
                Math.Abs((MyRectangle.Y + MyRectangle.Height / 2) - (CheckRect.Y + CheckRect.Height / 2)))
            {
                if (CheckRect.Y + CheckRect.Height / 2 > MyRectangle.Y + MyRectangle.Height / 2)
                    Result.Y = Math.Abs(Direction.Y);
                else
                    Result.Y = -Math.Abs(Direction.Y);
            }
            else
            {
                if (CheckRect.X + CheckRect.Width / 2 > MyRectangle.X + MyRectangle.Width / 2)
                    Result.X = Math.Abs(Direction.X);
                else
                    Result.X = -Math.Abs(Direction.X);
            }
            return Result;
        }

        public virtual void Delete()
        {
            GameManager.MyLevel.DeleteObject(this);
        }

        public virtual void Destroy()
        {
            Died = true;
            GameManager.MyLevel.DestroyObject(this);
        }

        public virtual void ChangeSelect(bool Is)
        {
            if (Selected != Is)
            {
                GameManager.MyLevel.HasChanged = true;
                Selected = Is;
            }
        }

        public virtual void ChangeSize(Vector2 NewSize)
        {
            Size = NewSize;
            MyRectangle.Width = (int)NewSize.X;
            MyRectangle.Height = (int)NewSize.Y;

            GameManager.MyLevel.HasChanged = true;
        }

        public virtual void ChangePosition(Vector2 NewPosition)
        {
            Position = NewPosition;
            MyRectangle.X = (int)NewPosition.X;
            MyRectangle.Y = (int)NewPosition.Y;

            GameManager.MyLevel.HasChanged = true;
        }

        public virtual void ChangePosition()
        {
            MyRectangle.X = (int)Position.X;
            MyRectangle.Y = (int)Position.Y;

            GameManager.MyLevel.HasChanged = true;
        }


        public virtual void Update(GameTime gameTime)
        {
            
        }

        public virtual void Reset()
        {
            Damage = 0;
            LastDamager = null;
            Speed = Vector2.Zero;
        }

        public virtual void Draw()
        {
            if (Selected)
                Game1.spriteBatch.Draw(MasterEditor.SelectedBox,MyRectangle,Color.White);
            if (Alert)
                Game1.spriteBatch.Draw(MasterEditor.Alert, MyRectangle, Color.White);
        }

        public virtual void DrawUpdate()
        {
            Changed = false;
        }

        public virtual void PreWrite(BinaryWriter Writer)
        {
            Writer.Write(CreatorString);
            DialogManager.WriteRectangle(Writer, MyRectangle);

        }

        public virtual void Write(BinaryWriter Writer)
        {

        }

        public virtual void PreRead(BinaryReader Reader)
        {
            Rectangle NewRect = DialogManager.ReadRectangle(Reader);
            ChangePosition(new Vector2(NewRect.X,NewRect.Y));
            ChangeSize(new Vector2(NewRect.Width, NewRect.Height));
        }

        public virtual void Read(BinaryReader Reader)
        {

        }

        public virtual void TakeDamage(float Damage, BasicObject Damager, Vector2 Direction)
        {
            bool GO = true;

            if (Damager != null)
                if (Damager.Team == Team && !Damager.FriendlyFire)
                    GO = false;

            if(GO)
            {


                if (GetType().IsSubclassOf(typeof(DynamicObject)) || Damager == null)
                    this.Damage += Damage;
                else
                    this.Damage += Damage * Damager.WallDamageMult;

                if (this.Damage > Life)
                    Die();

                if (Damager != null)
                    LastDamager = Damager.Creator;
            }
        }

        public virtual void Die()
        {
            Destroy();
        }

    }
}
