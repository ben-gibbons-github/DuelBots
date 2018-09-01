using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class Camera
    {
        public Vector4 MyRectangle;
        public Vector4 TargetRectangle;
        public Vector2 EditorOffset;
        public float MoveSpeed=0.25f;

        public List<BasicObject> Targets = new List<BasicObject>();


        public Camera(Vector4 NewRectangle)
        {
           // EditorOffset = new Vector2(NewRectangle.X + NewRectangle.Z, NewRectangle.Y + NewRectangle.W );
            MyRectangle = NewRectangle;
            TargetRectangle = NewRectangle;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 Min = new Vector2(100000);
            Vector2 Max= new Vector2(-100000);

            foreach (BasicObject Object in Targets)
            {
                Min = new Vector2(Math.Min(Min.X, Object.Position.X), Math.Min(Min.Y, Object.Position.Y));
                Max = new Vector2(Math.Max(Max.X, Object.Position.X + Object.Size.X), Math.Max(Max.Y, Object.Position.Y + Object.Size.Y));
            }
            Min.X = Math.Max(Min.X, GameManager.MyLevel.MyRectangle.X);
            Max.X = Math.Min(Max.X, GameManager.MyLevel.MyRectangle.X+GameManager.MyLevel.MyRectangle.Width);
            if (Max.Y > GameManager.MyLevel.MyRectangle.Height / 4f + GameManager.MyLevel.MyRectangle.Y)
                Max.Y += (Max.Y - (GameManager.MyLevel.MyRectangle.Height / 4f + GameManager.MyLevel.MyRectangle.Y))/3;
            Max.Y = Math.Min(Max.Y, GameManager.MyLevel.MyRectangle.Height + GameManager.MyLevel.MyRectangle.Y);
            //Min.Y = Math.Min(Math.Max(Min.Y, GameManager.MyLevel.MyRectangle.Y), GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Width / 2);
           // Max.Y = Math.Min(Max.Y, GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Width/2);
            Vector2 Pos = (Min + Max) / 2;
           // Pos.X = Math.Max(MyRectangle.X-500, Pos.X);
          //  Pos.X = Math.Min(MyRectangle.X+MyRectangle.Z + 500, Pos.X+500)-500;
           // Pos.Y = Math.Max(MyRectangle.Y - 1500, Pos.Y-500)+500;
           // Pos.Y = Math.Min(-GameManager.MyLevel.MyRectangle.Y+ GameManager.MyLevel.MyRectangle.Height - MyRectangle.W, Pos.Y);
            Vector2 Scale = Vector2.Distance(Min, Max)*1.5f * new Vector2(1280, 720) / 1000;
            Scale.X = Math.Max(1280, Scale.X);
            Scale.Y = Math.Max(720, Scale.Y);
            Scale.X = Math.Min(1280*1.5f, Scale.X);
            Scale.Y = Math.Min(720*1.5f, Scale.Y);
            Pos.Y = Math.Min(Pos.Y, GameManager.MyLevel.MyRectangle.Height + GameManager.MyLevel.MyRectangle.Y - Scale.Y / 2-16);
            

           // Vector2 Scale = new Vector2(MyRectangle.Z, MyRectangle.W);
            TargetRectangle = new Vector4(-Pos.X + Scale.X / 2, -Pos.Y + Scale.Y / 2, Scale.X, Scale.Y);
            

            //TargetRectangle = MyRectangle;
            float TempMoveSpeed = MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;

            MyRectangle += (TargetRectangle - MyRectangle)*0.05f;

            if (Vector4.Distance(MyRectangle, TargetRectangle) > TempMoveSpeed)
            {
                MyRectangle += Vector4.Normalize(TargetRectangle - MyRectangle) * TempMoveSpeed;

            }
            else
                MyRectangle = TargetRectangle;
             

          //  MyRectangle += (TargetRectangle-MyRectangle)/100;
        }
        public Matrix ReturnMatrix()
        {
            return
                              Matrix.CreateTranslation(MyRectangle.X, MyRectangle.Y, 0) * Matrix.CreateScale(Game1.self.Window.ClientBounds.Width / MyRectangle.Z, Game1.self.Window.ClientBounds.Height / MyRectangle.W, 1)
                ;

            //return Matrix.Identity;
        }

    }
}
