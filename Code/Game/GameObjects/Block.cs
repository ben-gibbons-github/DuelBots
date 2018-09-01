using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public enum BlockMode
    {
        None,
        Full,
        Left,
        Right,
        DLeft,
        DRight,
        TipUp,
        TipRight,
        TipLeft,
        TipDown
    }

    public class Block:BasicObject
    {
        public Vector2 Reflection;
        public bool IsBox;
        public Texture2D MyTexture;
        public bool IsTip;

        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            Life = 225;

            Solid = true;
            return base.Create(Size, Position);
           
        }



        public override Vector2 Bounce(Rectangle CheckRect, Vector2 Direction,int Exact)
        {
          //  if (IsBox||IsTip)
            if(true)
            {
                Vector2 Result= Direction;

                /*
                if (CheckRect.Intersects(new Rectangle(MyRectangle.X, MyRectangle.Y, MyRectangle.Width, Exact)))
                    Result.Y *= -1;
                if (CheckRect.Intersects(new Rectangle(MyRectangle.X, MyRectangle.Y+MyRectangle.Height-Exact, MyRectangle.Width, Exact)))
                    Result.Y *= -1;


                if (CheckRect.Intersects(new Rectangle(MyRectangle.X, MyRectangle.Y, Exact, MyRectangle.Height)))
                    Result.X *= -1;
                if (CheckRect.Intersects(new Rectangle(MyRectangle.X + MyRectangle.Width - Exact, MyRectangle.Y, Exact, MyRectangle.Height)))
                    Result.X *= -1;
                */


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

            else
            return Vector2.Reflect(Direction, Vector2.Normalize(Reflection));
        } 

        public override bool CheckCollision(Rectangle CheckRect)
        {
            bool IsCollision = false;

            if (blockMode != BlockMode.None) 
            if (CheckRect.Intersects(MyRectangle))
            {
                switch (blockMode)
                {
                    case BlockMode.Full:
                        IsCollision = true;
                        break;

                    case BlockMode.Left:
                        if (CheckRect.X + CheckRect.Y + CheckRect.Width + CheckRect.Height > MyRectangle.X + MyRectangle.Y + (MyRectangle.Width + MyRectangle.Height) / 2)
                            IsCollision = true;
                        break;


                    case BlockMode.Right:
                        if (CheckRect.X - MyRectangle.X + 1 < (CheckRect.Y+CheckRect.Height) - MyRectangle.Y)
                            IsCollision = true;
                        break;

                    case BlockMode.DRight:
                        if (CheckRect.X + CheckRect.Y < MyRectangle.X + MyRectangle.Y + (MyRectangle.Width + MyRectangle.Height) / 2)
                            IsCollision = true;
                        break;

                    case BlockMode.DLeft:
                        if ((CheckRect.X + CheckRect.Width) - MyRectangle.X + 1 > (CheckRect.Y) - MyRectangle.Y)
                            IsCollision = true;
                        break;

                    case BlockMode.TipUp:
                        if (CheckRect.X - MyRectangle.X + 1 < (CheckRect.Y + CheckRect.Height) - MyRectangle.Y)
                            if (CheckRect.X + CheckRect.Y + CheckRect.Width + CheckRect.Height > MyRectangle.X + MyRectangle.Y + (MyRectangle.Width + MyRectangle.Height) / 2)
                                IsCollision = true;
                        break;

                    case BlockMode.TipRight:
                        if (CheckRect.X - MyRectangle.X + 1 < (CheckRect.Y + CheckRect.Height) - MyRectangle.Y)
                            if (CheckRect.X + CheckRect.Y < MyRectangle.X + MyRectangle.Y + (MyRectangle.Width + MyRectangle.Height) / 2)
                            IsCollision = true;
                        break;

                    case BlockMode.TipLeft:
                        if ((CheckRect.X + CheckRect.Width) - MyRectangle.X + 1 > (CheckRect.Y) - MyRectangle.Y)
                            if ((CheckRect.X + CheckRect.Width) - MyRectangle.X + 1 > (CheckRect.Y) - MyRectangle.Y)
                    
                            IsCollision = true;
                        break;


                    case BlockMode.TipDown:
                        if (CheckRect.X + CheckRect.Y < MyRectangle.X + MyRectangle.Y + (MyRectangle.Width + MyRectangle.Height) / 2)
                            if ((CheckRect.X + CheckRect.Width) - MyRectangle.X + 1 > (CheckRect.Y) - MyRectangle.Y)
                        
                            IsCollision = true;
                        break;

                    default:
                        IsCollision = true;
                        break;
                }
            }

            return IsCollision;
        }

        public override void DrawUpdate()
        {
            

            Rectangle TestRect=new Rectangle(MyRectangle.X-32,MyRectangle.Y,MyRectangle.Width,MyRectangle.Height);

            BasicObject IsLeft = GameManager.MyLevel.CheckForSolidCollision(TestRect);

            TestRect.X = MyRectangle.X + 32;
            BasicObject IsRight = GameManager.MyLevel.CheckForSolidCollision(TestRect);

            TestRect.X = MyRectangle.X;
            TestRect.Y = MyRectangle.Y - 32;
            bool IsUp = GameManager.MyLevel.CheckForSolidCollision(TestRect) != null;

            TestRect.Y = MyRectangle.Y + 32;
            bool IsDown = GameManager.MyLevel.CheckForSolidCollision(TestRect) != null;


            MyTexture = BlockCreator.BlockTipUp;
            blockMode = BlockMode.TipUp;

            if (IsRight != null)
            {
                if (IsLeft != null)
                    MyTexture = BlockCreator.BlockDoubleLR;
                else
                    MyTexture = BlockCreator.BlockBevelLeft;
            }

            if (IsLeft != null)
            {
                if (IsRight != null)
                    MyTexture = BlockCreator.BlockDoubleLR;
                else
                    MyTexture = BlockCreator.BlockBevelRight;
            }


            if (IsUp)
            {
                MyTexture = BlockCreator.BlockDoubleUD;

                if (IsLeft != null || IsRight != null)
                {
                    if (IsDown)
                    {
                        MyTexture = BlockCreator.BlockBlack;

                        if (IsRight == null)
                            MyTexture = BlockCreator.BlockFullRight;
                        if (IsLeft == null)
                            MyTexture = BlockCreator.BlockFullLeft;
                    }
                    else
                    {
                        MyTexture = BlockCreator.BlockFullDown;

                        if (IsRight == null)
                            MyTexture = BlockCreator.BlockBevelDRight;
                        if (IsLeft == null)
                            MyTexture = BlockCreator.BlockBevelDLeft;
                    }
                }

            }

            if (IsDown)
            {
                if (IsLeft != null || IsRight != null)
                {
                    if (!IsUp)
                    {
                       MyTexture = BlockCreator.BlockFullUp;

                       if (IsRight == null)
                            MyTexture = BlockCreator.BlockBevelRight;
                       if (IsLeft == null)
                            MyTexture = BlockCreator.BlockBevelLeft;
                    }
                    else
                    {
                        MyTexture = BlockCreator.BlockBlack;

                        if (IsRight == null)
                            MyTexture = BlockCreator.BlockFullRight;
                        if (IsLeft == null)
                            MyTexture = BlockCreator.BlockFullLeft;
                    }
                }
                else if(IsUp)
                    MyTexture = BlockCreator.BlockDoubleUD;
            }

            if(!IsDown)
            {
                if (MyTexture == BlockCreator.BlockBevelRight)
                    MyTexture = BlockCreator.BlockTipRight;
                if (MyTexture == BlockCreator.BlockBevelLeft)
                    MyTexture = BlockCreator.BlockTipLeft;
                if (MyTexture == BlockCreator.BlockDoubleUD)
                    MyTexture = BlockCreator.BlockTipDown;
            }
            IsBox = false;
            IsTip = false;
            Reflection = new Vector2(0, 1);
            

            if (MyTexture == BlockCreator.BlockBevelRight)
            {
                blockMode = BlockMode.Right;
                Reflection = new Vector2(1, -1);
            }
            if (MyTexture == BlockCreator.BlockBevelLeft)
            {
                blockMode = BlockMode.Left;
                Reflection = new Vector2(-1, -1);
            }
            if (MyTexture == BlockCreator.BlockBevelDLeft)
            {
                blockMode = BlockMode.DLeft;
                Reflection = new Vector2(-1, 1);
            }
            if (MyTexture == BlockCreator.BlockBevelDRight)
            {
                blockMode = BlockMode.DRight;
                Reflection = new Vector2(1, 1);
            }
            if (MyTexture == BlockCreator.BlockTipRight)
            {
                blockMode = BlockMode.TipRight;
                Reflection = new Vector2(1, 0);
                IsTip = true;
            }
            if (MyTexture == BlockCreator.BlockTipLeft)
            {
                blockMode = BlockMode.TipLeft;
                Reflection = new Vector2(-1, 0);
                IsTip = true;
            }
            if (MyTexture == BlockCreator.BlockTipDown)
            {
                blockMode = BlockMode.TipDown;
                Reflection = new Vector2(0, 1);
                IsTip = true;
            }
            if (MyTexture == BlockCreator.BlockTipUp)
            {
                blockMode = BlockMode.TipUp;
                Reflection = new Vector2(0, -1);
                IsTip = true;
            }

            if (MyTexture == BlockCreator.BlockBlack || MyTexture == BlockCreator.BlockDoubleLR || MyTexture == BlockCreator.BlockDoubleUD || MyTexture == BlockCreator.BlockFullLeft || MyTexture == BlockCreator.BlockFullRight || MyTexture == BlockCreator.BlockFullUp || MyTexture == BlockCreator.BlockFullDown)
            {
                IsBox = true;
                blockMode = BlockMode.Full;
            }

            if (!LevelEditorWindow.EditorMode)
            {
                if (blockMode == BlockMode.TipUp && IsDown == false)
                    Destroy();

                if (blockMode == BlockMode.Right && !IsDown && IsLeft.blockMode == BlockMode.Right)
                    Destroy();


                
                if (blockMode == BlockMode.Left && !IsDown && IsRight.blockMode == BlockMode.Left)
                    Destroy();

            }
            base.DrawUpdate();
        }

        public override void Draw()
        {
            if (MyTexture != null)
            {
                Color Col = Color.White;
                Game1.spriteBatch.Draw(MyTexture, MyRectangle, Col);
            }
            base.Draw();
        }
        public override void Die()
        {
            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.BlockBit, Position+Size/2, Bullet.RandomSpeed(0.25f), 0);
            for (int i = 0; i < 10; i++)
                ParticleSystem.Add(ParticleType.Spark, Position, Bullet.RandomSpeed(0.35f) - Vector2.Normalize(Speed) * 0.25f, 0, new Color(1f, 0.66f, 0.33f), 1);
            base.Die();
        }
    }
}
