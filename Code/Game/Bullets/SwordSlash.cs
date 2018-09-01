using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class SwordSlash : Bullet
    {
        SpriteEffects fx = SpriteEffects.None;
        public Rectangle DrawFrom;
        public float Frame = 0;

        public override void CreateBullet(Vector2 Size, Vector2 Position, Vector2 Direction, BasicObject Creator)
        {
            
            if (Creator.MyFacing == Facing.Left)
                fx = SpriteEffects.FlipHorizontally;
            Accuracy = 0;
            FireSpeed = 0.5f;
            Reps = 4;
            Damage = 3.5f;
            Push = 10f;
            MaxLifeTime = 600;
            this.Direction = Vector2.Normalize(Direction);
            HitObjectsOnce = false;

            Size = new Vector2(80);

            base.CreateBullet(Size, Position - Size / 2, Direction, Creator);
        }


        public override void Update(GameTime gameTime)
        {
            Frame = (float)LifeTime / MaxLifeTime * 32;

            Creator.Position = Position + Size / 2 - Creator.Size / 2;
            Creator.Speed = Vector2.Zero;
            if (Creator.GetType().Equals(typeof(Player)))
            {
                Player p = Creator as Player;
                p.GunInvisibleTime = 3;
            }

            if (LifeTime > 125)
            {
                Slash();  
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            if (Creator != null)
            {
                

                DrawFrom = new Rectangle(((int)Frame) * 64, 0, 64, 64);
                Game1.spriteBatch.Draw(Sword.SwordTexture,
                    new Rectangle((int)(Position.X - Size.X + Creator.Size.X / 2), (int)(Position.Y - Size.Y + Creator.Size.Y / 2), (int)Size.X * 2, (int)Size.Y * 2)
                    , DrawFrom, Color.White, 0, Vector2.Zero, fx, 0);
                // Game1.spriteBatch.Draw(EditorStatic.BlankTexture, MyRectangle, Color.White);
            }
            base.Draw();
        }

        public override bool HitObject(BasicObject Object, GameTime gameTime)
        {
            if(Object!=Creator)
            if (LifeTime < 400)
            {

                Object.TakeDamage((float)Damage * gameTime.ElapsedGameTime.Milliseconds / 1000f * 60f, this, Direction);
                if (Object.GetType().IsSubclassOf(typeof(DynamicObject)))
                {
                    for (int i = 0; i < 3; i++)
                        ParticleSystem.Add(ParticleType.Spark, (Position + Size / 2 + Object.Position + Object.Size / 2) / 2, Bullet.RandomSpeed(0.35f) * new Vector2(1, 5), 0, new Color(1f, 1, 1f), 1.5f);
                    ParticleSystem.Add(ParticleType.Spark, (Position + Size / 2 + Object.Position + Object.Size / 2) / 2, Bullet.RandomSpeed(0.1f) *new Vector2(1,5), 0, new Color(1f, 1, 1f), 5);
    
                    if (Vector2.Distance(Position + Size / 2, Object.Position + Object.Size / 2) < 25)
                        Slash();
                    else
                        Speed = Vector2.Normalize((Object.Position + Object.Size / 2) - (Position + Size / 2)) * Vector2.Distance(Vector2.Zero, Speed);
                }
            }
                return false;
        }

        public void Slash()
        {
            LifeTime = Math.Max(LifeTime, MaxLifeTime / 2);
            Speed = Vector2.Zero;
            Gravity = Vector2.Zero;
            WallDamageMult = 40;
        }
    }
}
