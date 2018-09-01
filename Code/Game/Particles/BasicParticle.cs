using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DuelBots
{
    public class BasicParticle
    {
        public ParticleSystem Parent;
        public float StartSize = 0;
        public float EndSize = 0;
        public float Rot = 0;
        public float CurrentRotSpeed = 0;
        public float RotSpeed = 0;
        public int LifeTime = 0;
        public int MaxLifeTime = 0;
        public Vector2 Gravity;
        public Vector2 Speed;
        public Vector2 Position;
        public Texture2D MyTexture;
        public bool Active = false;
        public Color MyColor;
        public float SizeMult=1;

        public BasicParticle(ParticleSystem Parent,float StartSize, float EndSize, float Rot, float RotSpeed, int MaxLifeTime,Texture2D MyTexture,Vector2 Gravity,Color MyColor)
        {
            this.Parent = Parent;
            this.StartSize = StartSize;
            this.EndSize = EndSize;
            this.Rot = Rot;
            this.RotSpeed = RotSpeed;
            this.MaxLifeTime = MaxLifeTime;
            this.MyTexture = MyTexture;
            this.Gravity = Gravity;
            this.MyColor = MyColor;
        }

        public void Start(Vector2 Position, Vector2 Speed,float Rot)
        {
            this.Position = Position;
            this.Speed = Speed;
            this.Rot = Rot;
            LifeTime = 0;
            CurrentRotSpeed = (float)(Bullet.NewRandom.NextDouble() * RotSpeed * 2 - RotSpeed);
            Active = true;
            
        }

        public void Start(Vector2 Position, Vector2 Speed, float Rot,Color NewColor,float SizeMult)
        {
            Start(Position, Speed, Rot);
            MyColor = NewColor;
            this.SizeMult = SizeMult;
        }

        public void Draw()
        {
            float Normal = (float)LifeTime/MaxLifeTime;
            float Size= StartSize+(EndSize-StartSize)*Normal;
            Rectangle MyRectangle = new Rectangle((int)(Position.X - Size / 2 * SizeMult), (int)(Position.Y - Size / 2 * SizeMult), (int)(Size * SizeMult), (int)(Size * SizeMult));
            Game1.spriteBatch.Draw(MyTexture, MyRectangle, MyColor*(1-Normal));
        }

        public void Update(GameTime gameTime)
        {
            Speed += Gravity * gameTime.ElapsedGameTime.Milliseconds;
            Position += Speed * gameTime.ElapsedGameTime.Milliseconds;

            LifeTime += gameTime.ElapsedGameTime.Milliseconds;
            if (LifeTime > MaxLifeTime)
                Destroy();
        }

        public void Destroy()
        {
            Active = false;
            Parent.Remove(this);
        }

        public void Reset()
        {
            Active = false;
        }
    }
}
