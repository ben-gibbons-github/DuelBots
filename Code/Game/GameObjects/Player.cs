using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace DuelBots
{
    public enum Facing
    {
        Left,
        Right
    }

    public class Player:DynamicObject
    {
        PlayerIndex playerIndex;
        Vector2 PlayerSize = new Vector2(48);
        Color MyColor;

        Rectangle GravityRectangle = new Rectangle();
        Rectangle MovementRectangle = new Rectangle();

        Buttons FirePrimary = Buttons.RightTrigger;
        Buttons FireSecondary = Buttons.LeftTrigger;
        Buttons Jump = Buttons.A;
        Buttons Dash = Buttons.X;

        public GunBasic GunCurrent;

        GamePadState PadState;
        GamePadState PreviousPadState;

        public bool IsRespawning = false;
        public bool IsAutoAiming = false;
        public int RespawnTime = 0;
        public int MaxRespawnTime = 500;
        public float PushTime = 0;
        public int ShieldPoints = 1;
        public float AutoAimSuccess=0;
        public int GunInvisibleTime = 1;

        public bool IsDashing = false;
        public float DashTimer;
        public float MaxDashTimer = 350;
        public float DashSpeed = 15f;
        public float DashRecharge;
        public float MaxDashRecharge = 2000;

        public int MaxJumps = 2;
        public int Jumps = 0;
        public float MoveSpeed = 0.25f;
        public float JumpSpeed = 1f;
        public float JumpTimer = 0;

        public float AimHelpAmount = 45;

        public DamageBar DamageHud;
        public WeaponMenu GunMenu;
        public MenuInput Input;

        public int MaxWeaponSelectTime = 10000;
        public int WeaponSelectTime = 0;
        public int Inv = 0;
        public int PlayerNumb;
        public bool Visible = false;

        public static bool LoadedPlayerSprites=false;
        public float AnimationFrame = 0;

        public static Texture2D ArmsLeftOne;
        public static Texture2D ArmsLeftTwo;
        public static Texture2D ArmsRightOne;
        public static Texture2D ArmsRightTwo;

        public static Texture2D LegsLeftOne;
        public static Texture2D LegsLeftTwo;
        public static Texture2D LegsRightOne;
        public static Texture2D LegsRightTwo;

        public static Texture2D TorsoLeftOne;
        public static Texture2D TorsoLeftTwo;
        public static Texture2D TorsoRightOne;
        public static Texture2D TorsoRightTwo;

        public static Texture2D Laser;

        Texture2D ArmSprite;
        Texture2D LegSprite;
        Texture2D TorsoSprite;
        Texture2D GunSprite;

        public Player(PlayerIndex playerIndex,int PlayerNumb,Color color,int PlayerTeam,float Life)
        {
            Input = new MenuInput(playerIndex);
            Solid = true;
            Alive = false;
            //IsRespawning = true;


            this.PlayerNumb = PlayerNumb;
            this.Life = Life;
            this.Team = PlayerTeam;

            GunCurrent = new SlimeGun().Create(this);

            Gravity = new Vector2(0, 0.0025f);

            this.playerIndex = playerIndex;
            MyColor = color;

            Rectangle Rect= new Rectangle(100, 100, 200, 100);
            if(PlayerNumb==1)
                Rect= new Rectangle(Game1.ResolutionX-300,100,200,100);

            GameManager.MyLevel.AddHud(DamageHud = new DamageBar(Rect, this , color));

            if(!LoadedPlayerSprites)
                LoadPlayerSprites();

            ChangePlayerSprite();
        }

        

        public override BasicObject Create(Vector2 Size, Vector2 Position)
        {
            return base.Create(PlayerSize, Position);

        }

        void LoadPlayerSprites()
        {
            Laser = Game1.contentManager.Load<Texture2D>("Game/Weapons/GunLaser");

            ArmsLeftOne = Game1.contentManager.Load<Texture2D>("Game/Player/arms_left_one");
            ArmsLeftTwo = Game1.contentManager.Load<Texture2D>("Game/Player/arms_left_two");
            ArmsRightOne = Game1.contentManager.Load<Texture2D>("Game/Player/arms_right_one");
            ArmsRightTwo = Game1.contentManager.Load<Texture2D>("Game/Player/arms_right_two");

            LegsLeftOne = Game1.contentManager.Load<Texture2D>("Game/Player/legs_left_one");
            LegsLeftTwo = Game1.contentManager.Load<Texture2D>("Game/Player/legs_left_two");
            LegsRightOne = Game1.contentManager.Load<Texture2D>("Game/Player/legs_right_one");
            LegsRightTwo = Game1.contentManager.Load<Texture2D>("Game/Player/legs_right_two");

            TorsoLeftOne = Game1.contentManager.Load<Texture2D>("Game/Player/torso_left_one");
            TorsoLeftTwo = Game1.contentManager.Load<Texture2D>("Game/Player/torso_left_two");
            TorsoRightOne = Game1.contentManager.Load<Texture2D>("Game/Player/torso_right_one");
            TorsoRightTwo = Game1.contentManager.Load<Texture2D>("Game/Player/torso_right_two");
        }

        void ChangePlayerSprite()
        {
            if (PlayerNumb == 0)
            {
                if (MyFacing == Facing.Left)
                {
                    LegSprite = LegsLeftOne;
                    ArmSprite = ArmsLeftOne;
                    TorsoSprite = TorsoLeftOne;
                }
                else
                {
                    LegSprite = LegsRightOne;
                    ArmSprite = ArmsRightOne;
                    TorsoSprite = TorsoRightOne;
                }

            }
            else
            {
                if (MyFacing == Facing.Left)
                {
                    LegSprite = LegsLeftTwo;
                    ArmSprite = ArmsLeftTwo;
                    TorsoSprite = TorsoLeftTwo;
                }
                else
                {
                    LegSprite = LegsRightTwo;
                    ArmSprite = ArmsRightTwo;
                    TorsoSprite = TorsoRightTwo;
                }

            }

            if (MyFacing == Facing.Left)
                GunSprite = GunCurrent.LeftImage;
            else
                GunSprite = GunCurrent.RightImage;
        }

        public override void Update(GameTime gameTime)
        {
            
            GunInvisibleTime -= 1;
            PreviousPadState = PadState;
            PadState = GamePad.GetState(playerIndex);

            Inv -= gameTime.ElapsedGameTime.Milliseconds;

            if (GunMenu != null)
            {
                Visible = false;
                WeaponSelectTime+=gameTime.ElapsedGameTime.Milliseconds;
                if (WeaponSelectTime > MaxWeaponSelectTime)
                    GunMenu.Select();
                Input.Update(gameTime);
                GunMenu.TakeInput(Input);
                GunMenu.Update(gameTime);
                Inv = 1;
            }

            else if(GameManager.MyLevel.CountdownTimer<0)
            {
                if (!Visible)
                {
                    Visible = true;
                    ParticleSystem.Add(ParticleType.Ring, Position, Vector2.Zero, 0, MyColor, 15);
                    for (int i = 0; i < 15; i++)
                        ParticleSystem.Add(ParticleType.Glow, Position, Bullet.RandomSpeed(0.05f), 0, MyColor, 5);
                    ChangePlayerSprite();
                }

                if (!IsRespawning)
                {

                    UpdateGravity(gameTime);
                    UpdateDash(gameTime);

                    UpdateMove(gameTime);

                    ChangePosition();

                    UpdateAim(gameTime);

                    UpdateGuns(gameTime);

                }
                else
                {
                    RespawnTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (RespawnTime > MaxRespawnTime)
                        PlaceOnSpawn();
                }
            }
            base.Update(gameTime);
        }

        private void UpdateDash(GameTime gameTime)
        {
            if (!IsDashing)
            {
                DashRecharge += gameTime.ElapsedGameTime.Milliseconds;


                if (PadState.IsButtonDown(Dash) && PreviousPadState.IsButtonUp(Dash) && DashRecharge > MaxDashRecharge && Vector2.Distance(Vector2.Zero, new Vector2(PadState.ThumbSticks.Left.X, -PadState.ThumbSticks.Left.Y))>0.01f)
                {
                    IsDashing = true;
                    Speed = Vector2.Normalize(new Vector2(PadState.ThumbSticks.Left.X, -PadState.ThumbSticks.Left.Y)) * DashSpeed;
                    DashTimer = 0;
                }
            }

            else
            {
                ParticleSystem.Add(ParticleType.Dust, Position + Size / 2, Vector2.Zero, 0 , new Color(0.25f, 0.5f, 1), 8);
                ParticleSystem.Add(ParticleType.Hex, Position + Size / 2, Vector2.Zero, 0, new Color(0.25f, 0.5f, 1), 5);
                DashTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (Position.X < GameManager.MyLevel.MyRectangle.X)
                    Speed.X = Math.Abs(Speed.X);
                //if (Position.Y < GameManager.MyLevel.MyRectangle.Y)
                 //   Speed.Y = Math.Abs(Speed.Y);

                if (Position.X + Size.X > GameManager.MyLevel.MyRectangle.X + GameManager.MyLevel.MyRectangle.Width)
                    Speed.X = -Math.Abs(Speed.X);
                if (Position.Y + Size.Y > GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Height)
                    Speed.Y = -Math.Abs(Speed.Y);


                if (DashTimer > MaxDashTimer)
                {
                    if (GameManager.MyLevel.CheckForSolidCollision(MyRectangle) == null)
                    {
                        Speed = Vector2.Zero;
                        IsDashing = false;
                        DashRecharge = 0;
                        DashTimer = 0;
                        PushTime = 0;
                    }
                    if (DashTimer > MaxDashTimer * 8)
                    {
                        DashTimer = MaxDashTimer;
                        Speed = Vector2.Normalize(
                            Vector2.Normalize( Speed) + 
                             Vector2.Normalize(
                            new Vector2(GameManager.MyLevel.MyRectangle.X + GameManager.MyLevel.MyRectangle.Width / 2, GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Height / 2)) - Position) *
                            Vector2.Distance(Speed, Vector2.Zero);

                    }
                }


                Position += Speed;



            }
        }

        private void UpdateAim(GameTime gameTime)
        {
            IsAutoAiming = false;

            float BestDistance = 100000;

            foreach (BasicObject player in GameManager.MyLevel.GetAllEnemies(this)) 
                if(player!=this)
                    if (player.Alive)
                    {
                        float MyDistance = Vector2.Distance(Position, player.Position);

                        if (MyDistance < BestDistance)
                        {
    
                            bool Go = false;

                            if (MyDistance < 150)
                                Go = true;

                            float MyDire = (float)Math.Atan2(Direction.X, Direction.Y) - (float)Math.PI;

                            float TempDire = (float)Math.Atan2(player.Position.X - Position.X, player.Position.Y - Position.Y) +MathHelper.ToRadians(180);

                            float AimDire = MathHelper.ToDegrees(Math.Min(Math.Abs(MyDire - TempDire), Math.Abs((MyDire - TempDire) + MathHelper.ToRadians(360))));
                            if (AimDire < AimHelpAmount)
                                Go = true;
                            AutoAimSuccess = AimDire;

                            if(Go)
                            {
                                IsAutoAiming = true;
                                BestDistance = MyDistance;
                                Direction = Vector2.Normalize(player.Position - Position);
                            }
                        }
                    }
                    
        }

        private void UpdateGuns(GameTime gameTime)
        {
            if (PushTime <= 0 && !IsDashing)
            {
                GunCurrent.Update(gameTime);

                if (PadState.IsButtonDown(FirePrimary))
                    GunCurrent.Primary.Shoot(Position + Size / 2 - new Vector2(0, 20) + Direction * 48, Direction);

                if (PadState.IsButtonDown(FireSecondary))
                    GunCurrent.Secondary.Shoot(Position + Size / 2 -new Vector2 (0,20) + Direction * 48, Direction);
            }
        }

        private void UpdateMove(GameTime gameTime)
        {
            if (PushTime <= 0 && !IsDashing) 
            {
                if (PadState.ThumbSticks.Left.Length() > 0f)
                {
                    Speed.X = 0;
                    Direction = Vector2.Normalize(new Vector2(PadState.ThumbSticks.Left.X, -PadState.ThumbSticks.Left.Y));

                    float MoveAmount = MathHelper.Clamp(PadState.ThumbSticks.Left.X * 2, -1, 1) * MoveSpeed * gameTime.ElapsedGameTime.Milliseconds;

                    if (MoveAmount != 0)
                    {
                        MovementRectangle.X = (int)(Position.X + MoveAmount);



                        if (GameManager.MyLevel.CheckForSolidCollision(MovementRectangle) == null)
                        {
                            if (MoveAmount > 0)
                                MyFacing = Facing.Right;
                            else
                                MyFacing = Facing.Left;

                            Position.X += MoveAmount;
                            AnimationFrame += Math.Abs (MoveAmount/80);

                            while (AnimationFrame > 1)
                                AnimationFrame -= 1;

                            ChangePlayerSprite();
                        }
                        else
                        {
                            MovementRectangle.Y = (int)(Position.Y - Math.Abs(MoveAmount));
                            if (GameManager.MyLevel.CheckForSolidCollision(MovementRectangle) == null)
                                Position += new Vector2(MoveAmount, -Math.Abs(MoveAmount));
                            else
                            {
                                MovementRectangle.Y = (int)(Position.Y + Math.Abs(MoveAmount));
                                if (GameManager.MyLevel.CheckForSolidCollision(MovementRectangle) == null)
                                    Position += new Vector2(MoveAmount, Math.Abs(MoveAmount));

                            }
                        }

                    }
                }

                if (JumpTimer > 0 || Jumps < MaxJumps)
                    if (PadState.IsButtonDown(Jump) && PreviousPadState.IsButtonUp(Jump))
                    {
                        if (JumpTimer > 0)
                            Jumps = 1;
                        else
                            Jumps++;

                        Speed.Y = -JumpSpeed;
                    }
            }
        }


        private BasicObject UpdateGravity(GameTime gameTime)
        {
            if (!IsDashing)
            {
                PushTime -= 1f * (float)gameTime.ElapsedGameTime.Milliseconds / (1000f / 60f);
                JumpTimer -= 1f * (float)gameTime.ElapsedGameTime.Milliseconds / (1000f / 60f);

                BasicObject Other = GameManager.MyLevel.CheckForSolidCollision(GravityRectangle);

                if (Other == null)
                    Speed += Gravity * gameTime.ElapsedGameTime.Milliseconds;
                else
                {
                    JumpTimer = 60;
                    Jumps = 1;
                }

                List<BasicObject> Victims = new List<BasicObject>();

                Vector2 ToPosition = Position + ((Speed * gameTime.ElapsedGameTime.Milliseconds));
                int Reps = 1;
                if (GameManager.MyLevel.CheckForSolidCollision(ToPosition, Size) != null)
                    Reps = (int)Vector2.Distance(Vector2.Zero, Speed * gameTime.ElapsedGameTime.Milliseconds);

                for (int i = 1; i < Reps+1; i++)
                {
                    ToPosition = Position + ((Speed * gameTime.ElapsedGameTime.Milliseconds) / Reps);

                    BasicObject Other2 = GameManager.MyLevel.CheckForSolidCollision(ToPosition, Size);

                    if (Other2 == null)
                        Position = ToPosition;
                    else
                    {
                        if (PushTime > 0)
                        {
                            if (!Victims.Contains(Other2))
                            {
                                Other2.TakeDamage((Math.Abs(Speed.X) + Math.Abs(Speed.Y)) * 150, null, Speed);
                                Victims.Add(Other2);
                                Speed *= 0.85f;
                            }
                            if (Other2.Damage <= Other2.Life)
                                Speed = Other2.Bounce(MyRectangle, Speed, (int)Math.Max(Speed.X, Speed.Y) * gameTime.ElapsedGameTime.Milliseconds / Reps);


                            Position = ToPosition;
                        }
                        else
                            Speed = Vector2.Zero;
                    }
                }

                if (Position.Y > GameManager.MyLevel.MyRectangle.Y + GameManager.MyLevel.MyRectangle.Height + 500)
                    Die();

                ChangePosition();




                return Other;
            }
            else
                return null;
        }

        public override void TakeDamage(float Damage, BasicObject Damager, Vector2 Direction)
        {
            if (Inv <= 0)
                if (Damager.Team != Team || Damager.FriendlyFire)
                    if (!IsDashing)
                    {

                        PushTime = Math.Max(PushTime,
                            Damage * (this.Damage + 750) / 500 * Damager.Push
                            );


                        Speed = ((Vector2.Normalize(Vector2.Normalize(Direction) + new Vector2(0, -0.35f))) *
                            Damage * (this.Damage + 750) / 18000 * Damager.Push) * 0.5f;

                        base.TakeDamage(Damage, Damager, Vector2.Zero);
                    }

        }

        public override void Draw()
        {
            int AddSize = 32;
            Rectangle DrawRect = new Rectangle((int)Position.X - AddSize / 2, (int)Position.Y - AddSize / 2, (int)Size.X + AddSize, (int)Size.Y + AddSize);

            Vector2 DrawDire = Direction;
            if (MyFacing == Facing.Right)
                DrawDire = -DrawDire;

            float Rot = (float)Math.Atan2(DrawDire.Y, DrawDire.X) - (float)Math.PI;

            int HeightOffset = 16;

            if (Visible)
            {
                Game1.spriteBatch.Draw(ArmSprite, Position + Size / 2 - new Vector2(0, HeightOffset), null, Color.White, Rot, new Vector2(ArmSprite.Width / 2, ArmSprite.Height / 2 - HeightOffset), 1, SpriteEffects.None, 0);
                Game1.spriteBatch.Draw(LegSprite, MyRectangle, CreateFromFrame((int)(AnimationFrame * 11)), Color.White);
                Game1.spriteBatch.Draw(TorsoSprite, DrawRect, CreateFromFrame((int)(AnimationFrame * 18)), Color.White);


                AddSize = 80;
                DrawRect = new Rectangle((int)Position.X - AddSize / 2 + 64, (int)Position.Y - AddSize / 2 + 64, (int)Size.X + AddSize, (int)Size.Y + AddSize);

                if (GunInvisibleTime < 0)
                {
                    Game1.spriteBatch.Draw(GunSprite, Position + Size / 2 - new Vector2(0, HeightOffset), null, Color.White, Rot, new Vector2(GunSprite.Width / 2, GunSprite.Height / 2 - HeightOffset), 1, SpriteEffects.None, 0);

                    AddSize = 400;
                    DrawRect = new Rectangle((int)Position.X - AddSize / 2 + 64, (int)Position.Y - AddSize / 2 + 64, (int)Size.X + AddSize, (int)Size.Y + AddSize);

                    Rot = (float)Math.Atan2(-Direction.X, Direction.Y) - (float)Math.PI;
                    if (PushTime < 1)
                        Game1.spriteBatch.Draw(Laser, Position + Size / 2 - new Vector2(0, HeightOffset + 3), null, Color.White, Rot, new Vector2(Laser.Width / 2, Laser.Height / 2), 1, SpriteEffects.None, 0);
                }
            }

            if (GunMenu != null)
                GunMenu.Draw();
            //Game1.spriteBatch.DrawString(MenuManager.Font, AutoAimSuccess.ToString(), this.Position, Color.White);
            base.Draw();
        }

        public Rectangle CreateFromFrame(int Frame)
        {
            return new Rectangle(Frame * 80, 0, 80, 80);

        }

        public override void ChangePosition()
        {
            base.ChangePosition();
            GravityRectangle.X = MyRectangle.X;
            GravityRectangle.Y = MyRectangle.Y+(int)Speed.Y+1;
            MovementRectangle.Y = MyRectangle.Y;
        }

        public override void ChangeSize(Vector2 NewSize)
        {
            base.ChangeSize(NewSize);

            GravityRectangle.Width = MyRectangle.Width;
            GravityRectangle.Height = MyRectangle.Height;

            MovementRectangle.Width = MyRectangle.Width;
            MovementRectangle.Height = MyRectangle.Height;
        }

        public void PlaceOnSpawn()
        {
            WeaponSelectTime = 0;

            ShieldPoints = 1;
            Alive = true;
            Speed = Vector2.Zero;

            Damage = 0;
            LastDamager = null;
            PushTime = 0;
            Speed = Vector2.Zero;

            if(IsRespawning)
                GameManager.MyLevel.RevivePlayer(this);

            IsRespawning = false;
            RespawnTime = 0;

            foreach(BasicObject Object in GameManager.MyLevel.ObjectList)
                if (Object.GetType().Equals(typeof(PlayerSpawn)))
                {
                    PlayerSpawn Spawn = Object as PlayerSpawn;
                    if (PlayerNumb==(int)Spawn.playerIndex)
                    {
                        ChangePosition(Spawn.Position);
                        break;
                    }
                }

            GunMenu = (WeaponMenu)new WeaponMenu(Position,this).Create();
            GunMenu.Active = true;
            GunMenu.Alive = true;
            GunMenu.Ready = true;

            ChangePlayerSprite();
        }

        public Player CreateAndSpawn()
        {
            Create(Vector2.Zero,Vector2.Zero);
            PlaceOnSpawn();
            return this;
        }

        public override void Die()
        {
            Alive = false;
            IsRespawning = true;
            GameManager.MyLevel.KillPlayer(this);
        }
    }
}
