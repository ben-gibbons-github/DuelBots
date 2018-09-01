using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public class Level
    {
        public List<BasicObject> ObjectList = new List<BasicObject>();
        public List<BasicObject> DynamicObjects = new List<BasicObject>();
        public List<BasicObject> SolidObjects = new List<BasicObject>();
        public List<BasicObject> DestroyedObjects = new List<BasicObject>();
        public List<BasicObject> DynamicCollidables = new List<BasicObject>();
        public List<BasicObject> RespawningPlayers = new List<BasicObject>();
        public List<BasicObject> Players = new List<BasicObject>();
        public List<HUDBasic> HudItems = new List<HUDBasic>();

        public List<TimeBasic> TimeEvents = new List<TimeBasic>();
        public TimeManager MyTimeManager = new TimeManager();

        public int BackGroundNumb;

        public long MyTime = 0;
        public bool SinglePlayer = false;
        public Camera TimeCamera;
        public Rectangle TimeRectangle;

        public Rectangle MyRectangle;
        public Camera MyCamera;

        public Rectangle ClientRectangle;

        public RenderTarget2D MyRenderTarget;
        public RenderTarget2D FinalRenderTarget;

        public bool HasChanged = false;
        public bool HasResized = true;

        public int ChangeTimer = 0;
        public int CountdownTimer = 3;
        public int TimeTillNextCountdown;
        public int MaxCountdownTime = 1000;
        public int LightHeight = 480;
        public int LightWidth = 64;

        public float CountDownSize = 1;

        public SpriteFont CountFont;

        public int Wave = 0;
        public BackgroundBasic MyBackground;
        public Effect LevelEffect;

        public Texture2D RedGlow;

        public BasicObject CheckForAllCollision(Rectangle TestRect)
        {
            BasicObject Collision = null;

            Collision = CheckForSolidCollision(TestRect);
            if (Collision == null)
                Collision = CheckForDynamicCollision(TestRect);

            return Collision;
        }

        public List<BasicObject> CheckForAllList(Rectangle TestRect)
        {
            List<BasicObject> Collision = new List<BasicObject>();

            foreach (BasicObject Object in CheckForDynamicList(TestRect))
                Collision.Add(Object);
            foreach (BasicObject Object in CheckForSolidList(TestRect))
                Collision.Add(Object);

            return Collision;
        }


        public BasicObject CheckForAllCollision(Rectangle TestRect,BasicObject Me)
        {
            BasicObject Collision = null;

            Collision = CheckForSolidCollision(TestRect);
            if (Collision == null)
                Collision = CheckForDynamicCollision(TestRect,Me);

            return Collision;
        }


        public void DistanceDamage(Bullet Damager)
        {
            for (int i = 0; i < ObjectList.Count; i++)
            {
                BasicObject Object = ObjectList[i];

                if (Object != null)
                {
                    float Distance = Vector2.Distance(Object.Position+Object.Size/2, Damager.Position+Damager.Size/2);

                    if (Distance <= Damager.Range)
                        Object.TakeDamage(
                            (Damager.Range - Distance) / Damager.Range * Damager.Damage
                            , Damager
                            , Vector2.Normalize(Object.Position - Damager.Position));
                }
            }

            for (int i = 0; i < DynamicCollidables.Count; i++)
            {
                BasicObject Object = DynamicCollidables[i];

                if (Object != null)
                {
                    float Distance = Vector2.Distance(Object.Position, Damager.Position);

                    if (Distance <= Damager.Range)
                        Object.TakeDamage((Damager.Range - Distance) / Damager.Range * Damager.Damage, Damager, Vector2.Normalize(Object.Position - Damager.Position));
                }
            }
        }

        public BasicObject CheckForSolidCollision(Vector2 Position, Vector2 Size)
        {
            return CheckForSolidCollision(new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y));
        }

        public BasicObject CheckForSolidCollision(Rectangle TestRect)
        {
            BasicObject Collision=null;

            foreach (BasicObject Object in GameManager.MyLevel.SolidObjects)
                    if(Object.CheckCollision(TestRect))
                {
                    Collision = Object;
                    break;
                }

            return Collision;
        }

        public List<BasicObject> CheckForSolidList(Rectangle TestRect)
        {
            List<BasicObject> Collision = new List<BasicObject>();

            foreach (BasicObject Object in GameManager.MyLevel.SolidObjects)
                if (Object.CheckCollision(TestRect))
                {
                    Collision.Add(Object);
                }

            return Collision;
        }

        public List<BasicObject> CheckForDynamicList(Rectangle TestRect)
        {
            List<BasicObject> Collision = new List<BasicObject>();

            foreach (BasicObject Object in GameManager.MyLevel.DynamicCollidables)
                if (Object.CheckCollision(TestRect))
                {
                    Collision.Add(Object);
                }

            return Collision;
        }

        public BasicObject CheckForDynamicCollision(Rectangle TestRect)
        {
            BasicObject Collision = null;

            foreach (BasicObject Object in GameManager.MyLevel.DynamicCollidables)
                if (Object.CheckCollision(TestRect))
                {
                    Collision = Object;
                    break;
                }

            return Collision;
        }

        public BasicObject CheckForDynamicCollision(Rectangle TestRect,BasicObject Me)
        {
            BasicObject Collision = null;

            foreach (BasicObject Object in GameManager.MyLevel.DynamicCollidables)
                if(Object!=Me)
                if (Object.CheckCollision(TestRect))
                {
                    Collision = Object;
                    break;
                }

            return Collision;
        }

        public void NewGame()
        {
            ParticleSystem.LoadAll();

            CountDownSize = 3;
            CountdownTimer = 3;
            MaxCountdownTime = 1000;
            TimeTillNextCountdown = 0;

            float PlayerLife = float.PositiveInfinity;

            AddDynamic(new Player(PlayerIndex.One, 0, Color.Red, 1,PlayerLife).CreateAndSpawn());

            int TeamTwo=2;
            if(GameManager.MyLevel.SinglePlayer)
                TeamTwo=1;

           // if(!GameManager.MyLevel.SinglePlayer)
                AddDynamic(new Player(PlayerIndex.Two, 1, Color.Blue, TeamTwo, PlayerLife).CreateAndSpawn());

            foreach (BasicObject Object in DynamicObjects)
            {
                MyCamera.Targets.Add(Object);
                Players.Add(Object);
            }

            Change(true);

            MyTimeManager.GenerateLists();

        }

        public void GenerateLists()
        {
            SolidObjects.Clear();

            foreach (BasicObject Object in ObjectList)
                if (Object.Solid)
                    SolidObjects.Add(Object);

            MyTimeManager.GenerateLists();
        }

        public void EndGame()
        {
            Change(true);

            DynamicObjects.Clear();
            SolidObjects.Clear();
            MyCamera.Targets.Clear();
            Players.Clear();
            DynamicCollidables.Clear();
            Players.Clear();
            MyTimeManager.DumpLists();
            HudItems.Clear();

            this.Reset();

        }

        public void Reset()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            List<BasicObject> SaveObjects = new List<BasicObject>();

            foreach (BasicObject Object in DestroyedObjects)
            {
                if (CheckForDynamicCollision(Object.MyRectangle) == null)
                    ObjectList.Add(Object);
                else
                    SaveObjects.Add(Object);
            }

            foreach (BasicObject Object in ObjectList)
                Object.Reset();

            DestroyedObjects = SaveObjects;


            GenerateLists();

            Change(true);
        }

        public BasicObject GetNearestEnemy(BasicObject Searcher)
        {
            BasicObject Result= null;
            float BestDistance = 10000000;

            foreach(BasicObject Object in DynamicCollidables)
                if (Object.Team != Searcher.Team)
                {
                    float MyDist = Vector2.Distance(Searcher.Position, Object.Position);
                    if (MyDist < BestDistance)
                    {
                        BestDistance = MyDist;
                        Result = Object;
                    }
                }

            return Result;
        }

        public List<BasicObject> GetAllEnemies(BasicObject Searcher)
        {
            List<BasicObject> Result = new List<BasicObject>();

            foreach (BasicObject Object in DynamicCollidables)
                if (Object.Team != Searcher.Team)
                {
                    Result.Add(Object);
                }

            return Result;
        }

        public void AddDynamic(BasicObject NewObject)
        {

            DynamicObjects.Add(NewObject);

            if (NewObject.Solid)
                DynamicCollidables.Add(NewObject);
        }

        public Level(Rectangle MyRectangle,Camera MyCamera)
        {
            MyBackground = new SpaceBack().Create(0);
            RedGlow = Game1.contentManager.Load<Texture2D>("Game/RedGlow");
            MyTimeManager = new TimeManager().Create(this);
            this.MyRectangle = MyRectangle;
            this.MyCamera = MyCamera;
            TimeRectangle = new Rectangle(0, 0, 1000000, 640);
            UpdateResize();
            LevelEffect = Game1.contentManager.Load<Effect>("Game/Backgrounds/LevelEffect");

            CountFont = Game1.contentManager.Load<SpriteFont>("Game/CountFont");
        }

        public void AddObject(BasicObject NewObject)
        {
            ObjectList.Add(NewObject);
            Change(true);

            if (NewObject.Solid)
                SolidObjects.Add(NewObject);
        }

        public void AddHud(HUDBasic NewHud)
        {
            HudItems.Add(NewHud);   
        }

        public void DeleteObject(BasicObject Object)
        {
            ObjectList.Remove(Object);
            Change(Object.Position, Object.Size);
        }

        public void DeleteTime(TimeBasic Time)
        {
            TimeEvents.Remove(Time);
        }

        public void DestroyObject(BasicObject Object)
        {
            ObjectList.Remove(Object);
            DestroyedObjects.Add(Object);
            Change(Object.Position, Object.Size);

            if (Object.Solid)
                SolidObjects.Remove(Object);
        }

        public void DestroyDynamic(BasicObject Object)
        {
            DynamicObjects.Remove(Object);

            if (Object.Solid)
                DynamicCollidables.Remove(Object);

            if (MyCamera.Targets.Contains(Object))
                MyCamera.Targets.Remove(Object);
        }

        public void Update(GameTime gameTime)
        {
            ParticleSystem.UpdateAll(gameTime);

            if (CountdownTimer > -1)
            {
                CountDownSize = 1 + (CountDownSize - 1) * 0.975f;
                TimeTillNextCountdown += gameTime.ElapsedGameTime.Milliseconds;
                if (TimeTillNextCountdown > MaxCountdownTime)
                {
                    TimeTillNextCountdown -= MaxCountdownTime;
                    CountdownTimer--;
                    if (CountdownTimer > 0)
                        CountDownSize = 2;
                }
            }
            else
                CountDownSize *= 0.9f;

            UpdateTime(gameTime);

            if (Game1.self.Window.ClientBounds.Width != ClientRectangle.Width || Game1.self.Window.ClientBounds.Height != ClientRectangle.Height)
            {
                ClientRectangle.Width = Game1.self.Window.ClientBounds.Width;
                ClientRectangle.Height = Game1.self.Window.ClientBounds.Height;

                HasResized = true;
            }

            if (SinglePlayer)
                MyTimeManager.Update(gameTime);

            MyCamera.Update(gameTime);



            for (int i = 0; i < DynamicObjects.Count(); i++)
            {
                BasicObject Object = DynamicObjects[i];
                if(Object!=null)
                Object.Update(gameTime);
            }

            for (int i = 0; i < RespawningPlayers.Count(); i++)
            {
                BasicObject Object = RespawningPlayers[i];
                if (Object != null)
                    Object.Update(gameTime);
            }
        }

        private void UpdateTime(GameTime gameTime)
        {
            MyTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public void KillPlayer(Player player)
        {
            DynamicObjects.Remove(player);
            RespawningPlayers.Add(player);
            Rebuild();
        }

        public void RevivePlayer(Player player)
        {
            DynamicObjects.Add(player);
            RespawningPlayers.Remove(player);
        }

        public void Change(Vector2 Position,Vector2 Size)
        {
            HasChanged = true;

            float SizeAdd = Size.X + Size.Y;

            foreach (BasicObject Object in ObjectList)
                if (Vector2.Distance(Object.Position + Object.Size / 2, Position + Size / 2) < Object.Size.X + Object.Size.Y + SizeAdd) 
                    Object.Changed = true;
        }

        public void Change(bool All)
        {
            HasChanged = true;
            if(All)
            foreach (BasicObject Object in ObjectList)
                    Object.Changed = true;
        }

        public void BackgroundDraw()
        {
            Game1.spriteBatch.Begin();
            MyBackground.Layers[0].Draw();
            Game1.spriteBatch.End();
        }

        public void PreDraw()
        {

            ChangeTimer++;

            if (HasResized)
                UpdateResize();
            if (HasChanged)
                UpdateDraw();

        }

        public void Draw()
        {
            if (!LevelEditorWindow.EditorMode)
            MyBackground.Layers[1].Draw();

            Game1.spriteBatch.Draw(FinalRenderTarget, MyRectangle, Color.White);

            foreach (BasicObject Object in DynamicObjects)
                Object.Draw();

            //MyTimeManager.Draw();
        }


        public void DrawHUD()
        {
            foreach (HUDBasic Hud in HudItems)
                Hud.Draw();

            if (CountDownSize>0)
            {
                string Text =string.Empty;
                if (CountdownTimer > 0)
                    Text = CountdownTimer.ToString();
                else
                    Text = "Go!";
                
                Vector2 Pos= new Vector2(Game1.ResolutionX, Game1.ResolutionY) /2- CountFont.MeasureString(Text) / 2*CountDownSize;
                
                Game1.spriteBatch.DrawString(CountFont, Text,Pos, new Color(0, 0, 0, 0.5f),0,Vector2.Zero,CountDownSize,SpriteEffects.None,0);
                Game1.spriteBatch.DrawString(CountFont, Text, Pos-new Vector2(10*CountDownSize),Color.White, 0, Vector2.Zero, CountDownSize, SpriteEffects.None, 0);

            }
        }

        public void DrawTimeLine()
        {
            foreach (TimeBasic Time in TimeEvents)
                Time.Draw();
        }

        public void UpdateDraw()
        {
            HasChanged = false;



            if (LevelEditorWindow.EditorMode)
            {
                GenerateLists();

                ChangeTimer = 0;

                foreach (BasicObject Object in ObjectList)
                {
                    Object.Alert = false;
                    if (!MyRectangle.Contains(Object.MyRectangle))
                        Object.Alert = true;
                }

                foreach(BasicObject Object in ObjectList)
                    foreach(BasicObject Object2 in ObjectList)
                        if (Object.MyRectangle.Intersects(Object2.MyRectangle)&&Object!=Object2)
                        {
                            Object.Alert = true;
                            Object2.Alert = true;
                        }
            }

            Game1.graphicsDevice.SetRenderTarget(MyRenderTarget);
            Game1.graphicsDevice.Clear(new Color(Vector4.Zero));
            Game1.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.AnisotropicWrap,DepthStencilState.Default,RasterizerState.CullNone,null,Matrix.CreateTranslation(-MyRectangle.X,-MyRectangle.Y,0));

            for (int i = 0; i < ObjectList.Count(); i++)

                if (LevelEditorWindow.EditorMode || ObjectList[i].VisibleInGame)
                {
                    if (ObjectList[i].Changed)
                        ObjectList[i].DrawUpdate();

                    if (i < ObjectList.Count())
                        if (ObjectList[i] != null)
                            ObjectList[i].Draw();
                }



            Game1.spriteBatch.End();
            Game1.graphicsDevice.SetRenderTarget(FinalRenderTarget);
            Game1.graphicsDevice.Clear(new Color(Vector4.Zero));
            if(MyBackground.Layers.Count>0)
            Game1.graphicsDevice.Textures[1] = MyBackground.Layers[2].MyTexture;
            if (!LevelEditorWindow.EditorMode)
                Game1.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, LevelEffect);
            else
                Game1.spriteBatch.Begin();
            Game1.spriteBatch.Draw(MyRenderTarget, new Rectangle(0, 0, MyRectangle.Width, MyRectangle.Height), Color.White);
            Game1.spriteBatch.End();

            Game1.spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.Additive);
            for (int x = 0; x < MyRectangle.Width + 1; x += 16)
            {
                Rectangle CheckRect= new Rectangle((int)x+MyRectangle.X+4,MyRectangle.Y + MyRectangle.Height - LightHeight+256,8,LightHeight);
              int ListCount=CheckForSolidList(CheckRect).Count;
                if (ListCount<7)
                    Game1.spriteBatch.Draw(RedGlow, new Rectangle(x + (LightWidth - 32) / 2 - 32, MyRectangle.Height - LightHeight * (7 - ListCount) / 7 * 2, LightWidth, LightHeight * (7 - ListCount) / 7 * 2), new Color(Vector4.One * (7 - ListCount) / 7));
            }
            Game1.spriteBatch.End();

        }

        public void UpdateResize()
        {
            HasResized = false;
            Change(false);

            MyRenderTarget = new RenderTarget2D(Game1.graphicsDevice, MyRectangle.Width, MyRectangle.Height, false, SurfaceFormat.Color, DepthFormat.None);
            FinalRenderTarget = new RenderTarget2D(Game1.graphicsDevice, MyRectangle.Width, MyRectangle.Height, false, SurfaceFormat.Color, DepthFormat.None);
        }
    }
}
