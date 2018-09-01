using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public enum ParticleType
    {
        Spark,
        BlockBit,
        Ring,
        Explosion,
        Smoke,
        Glow,
        Dust,
        Circle,
        Hex,
        Square,
        Diamond
    }


    public class ParticleSystem
    {

        public static ParticleSystem[] ParticleSystems =
        {
            new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,600,"Spark",ParticleType.Spark,new Vector2(0,0.0005f),new Color(1,1,1)),
            new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,1800,"Spark",ParticleType.Glow,new Vector2(0,0),new Color(1,1,1)),
            new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,1800,"Dust",ParticleType.Dust,new Vector2(0,0),new Color(1,1,1)),
            new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,1800,"Circle",ParticleType.Circle,new Vector2(0,0),new Color(1,1,1)),
            new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,1800,"Square",ParticleType.Square,new Vector2(0,0),new Color(1,1,1)),
             new ParticleSystem(BlendState.Additive,200,10,0,true,0,0,1800,"Hex",ParticleType.Hex,new Vector2(0,0),new Color(1,1,1)),
            new ParticleSystem(BlendState.AlphaBlend,100,12,0,true,0,0,1500,"BlockBit",ParticleType.BlockBit,new Vector2(0,0.0005f),Color.White),
            new ParticleSystem(BlendState.Additive,4,0,300,true,0,0,1000,"Ring",ParticleType.Ring,new Vector2(0,0),Color.White),
            new ParticleSystem(BlendState.Additive,50,100,200,true,0,0,2000,"Ex",ParticleType.Explosion,new Vector2(0,0),Color.White),
            new ParticleSystem(BlendState.AlphaBlend,50,0,200,true,0,0,1500,"Smoke",ParticleType.Smoke,new Vector2(0,0),Color.White),
            new ParticleSystem(BlendState.AlphaBlend,50,0,200,true,0,0,1500,"Diamond",ParticleType.Diamond,new Vector2(0,0),Color.White),
        };

        public static List<ParticleSystem> AddativeParticles;
        public static List<ParticleSystem> AlphaParticles;

        public Color MyColor;
        public bool RandomRotation = false;
        public BlendState MyBlendState;
        public ParticleType MyType;
        public float StartSize = 0;
        public float EndSize = 0;
        public float RotSpeed = 0;
        public int LifeTime = 0;
        public int MaxLifeTime = 0;
        public float Rot;
        public Vector2 Gravity;
        public Texture2D MyTexture;
        public string MyTexturePath;
        public Queue<BasicParticle> ParticleQeue;
        public BasicParticle[] Particles;

        public ParticleSystem(BlendState MyBlendState,int MaxParticles, float StartSize, float EndSize, bool RandomRotation, float Rot, float RotSpeed,
            int MaxLifeTime, string MyTexturePath, ParticleType MyType, Vector2 Gravity,Color MyColor)
        {
            this.MyBlendState = MyBlendState;
            this.RandomRotation = RandomRotation;
            this.MyType = MyType;
            this.StartSize = StartSize;
            this.EndSize = EndSize;
            this.RotSpeed = RotSpeed;
            this.MaxLifeTime = MaxLifeTime;
            this.MyTexturePath = MyTexturePath;
            this.Gravity = Gravity;

            this.Rot = Rot;
            this.MyColor = MyColor;

            Particles = new BasicParticle[MaxParticles];
            ParticleQeue = new Queue<BasicParticle>(MaxParticles);

        }

        public static void UpdateAll(GameTime gameTime)
        {
            foreach (ParticleSystem system in ParticleSystems)
            {
                system.Update(gameTime);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (BasicParticle p in Particles)
            {
                if (!p.Active)
                    continue;
                p.Update(gameTime);
            }

        }

        public static void Add(ParticleType Type, Vector2 Position, Vector2 Speed, float Rotation,Color NewColor,float SizeMult)
        {
            foreach (ParticleSystem system in ParticleSystems)
            {
                if (system.MyType == Type)
                    system.Add(Position, Speed, Rotation,NewColor,SizeMult);
            }

        }

        public static void Add(ParticleType Type, Vector2 Position, Vector2 Speed, float Rotation)
        {
            foreach (ParticleSystem system in ParticleSystems)
            {
                if (system.MyType == Type)
                    system.Add(Position, Speed, Rotation);
            }
               
        }

        public void Add(Vector2 Position, Vector2 Speed, float Rotation)
        {
            if (ParticleQeue.Count() > 0)
            {
                BasicParticle p = ParticleQeue.Dequeue();
                p.Start(Position, Speed, Rotation);
            }
        }

        public void Add(Vector2 Position, Vector2 Speed, float Rotation, Color NewColor, float SizeMult)
        {
            if (ParticleQeue.Count() > 0)
            {
                BasicParticle p = ParticleQeue.Dequeue();
                p.Start(Position, Speed, Rotation,NewColor,SizeMult);
            }
        }


        public void Remove(BasicParticle Particle)
        {
            Particle.Reset();
            ParticleQeue.Enqueue(Particle);
        }

        public void Load()
        {
            if (MyBlendState == BlendState.AlphaBlend)
                AlphaParticles.Add(this);
            else
                AddativeParticles.Add(this);

            if (MyTexture == null)
            {
                MyTexture = Game1.contentManager.Load<Texture2D>("Game/Particles/" + MyTexturePath);

                for (int i = 0; i < Particles.Count(); i++)
                    Particles[i] = new BasicParticle(this,StartSize, EndSize, Rot, RotSpeed, MaxLifeTime, MyTexture, Gravity,MyColor);
            }
            Reset();
        }

        public void Draw()
        {
            for (int i = 0; i < Particles.Count(); i++)
            {
                if (!Particles[i].Active)
                    continue;

                Particles[i].Draw();
            }
        }

        public void Reset()
        {
            ParticleQeue.Clear();
            for (int i = 0; i < Particles.Count(); i++)
            {
                Particles[i].Reset();
                ParticleQeue.Enqueue(Particles[i]);
            }
        }

        public static void LoadAll()
        {
            AlphaParticles = new List<ParticleSystem>();
            AddativeParticles = new List<ParticleSystem>();

            foreach (ParticleSystem System in ParticleSystems)
                System.Load();
        }

        public static void DrawAlpha()
        {
            foreach (ParticleSystem System in AlphaParticles)
                System.Draw();
        }

        public static void DrawAdditive()
        {
            foreach (ParticleSystem System in AddativeParticles)
                System.Draw();
        }
    }
}
