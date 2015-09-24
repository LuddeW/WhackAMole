using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;


namespace WhackAMole
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D BackgroundPic;
        Texture2D Background;
        Texture2D Front;
        Texture2D RabbitSprite;
        Texture2D ClockSprite;
        Texture2D EndScreen;
        Texture2D ClockSheet;
        Texture2D RabbitDead;
        Texture2D Chainsaw;
        Texture2D Crosshair;
        Texture2D Shotgun;
        Texture2D ShotgunFire;
        RabbitClass[] Rabbits = new RabbitClass[3];
        SpriteFont Font;
        Vector2 MousePos;
        SoundEffect ShotgunSound;
        public Vector2 TempPos;
        public float X0 = 43;
        public float X1 = 450 / 2 - 65;
        public float X2 = 275;
        public float StopY;
        public int NewVelocity;
        bool PrevMState;
        int Score = 0;
        float PlayTime;
        int TimePlayed;
        int iX;
        int iY;
        int xPos;
        int yPos;
        int RabbitCount = 0;
        int MaxRabbitCount = 25;
        public bool Dead = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 450;
            graphics.PreferredBackBufferHeight = 450;
            graphics.ApplyChanges();
            IsMouseVisible = false;
            

            Content.RootDirectory = "Content";
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            LoadPictures();
            LoadRabbits();
            LoadFonts();
            LoadSounds();


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            PlayTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            TimePlayed = (int)PlayTime;
            Spawn(PlayTime);
          
            Rabbits[0].Movement();
            Rabbits[1].Movement();
            Rabbits[2].Movement();
            CheckRabbitHit();
           
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            DrawPictures();
            DrawWeapons();
            DrawFonts();


            spriteBatch.End();
            base.Draw(gameTime);
        }
        // Load the pictures
        protected void LoadPictures()
        {
            Background = Content.Load<Texture2D>(@"rabbit_background");
            Front = Content.Load<Texture2D>(@"rabbit_front");
            RabbitSprite = Content.Load<Texture2D>(@"rabbit_ok");
            ClockSprite = Content.Load<Texture2D>(@"clock_0");
            EndScreen = Content.Load<Texture2D>(@"end_screen");
            ClockSheet = Content.Load<Texture2D>(@"Clock_sheet");
            RabbitDead = Content.Load<Texture2D>(@"rabbit_dead");
            Chainsaw = Content.Load<Texture2D>(@"chainsaw");
            Crosshair = Content.Load<Texture2D>(@"crosshair");
            Shotgun = Content.Load<Texture2D>(@"shotgun");
            ShotgunFire = Content.Load<Texture2D>(@"shotgun_fire");
            BackgroundPic = Content.Load<Texture2D>(@"backgroundpic");

        }
        // Draw the sprites
        protected void DrawPictures()
        {
            spriteBatch.Draw(BackgroundPic, new Rectangle(0, 0, BackgroundPic.Width, BackgroundPic.Height), Color.White);
            spriteBatch.Draw(Background, new Rectangle(0, 0, Background.Width, Background.Height), Color.White);
            Vector2 CenterClock = new Vector2(Window.ClientBounds.Width / 2 - ClockSprite.Width / 2, 10);
            spriteBatch.Draw(ClockSprite, CenterClock, Color.White);
            ClockPos();
            spriteBatch.Draw(Chainsaw, new Vector2(Window.ClientBounds.Width / 2, 145 - Chainsaw.Height / 2), Color.White);
            for (int i = 0; i < 3; i++)
            {
                if (Rabbits[i].Dead == false)
                {

                    spriteBatch.Draw(RabbitSprite, Rabbits[i].RabbitPos, Color.White);
                }
                else
                {
                    spriteBatch.Draw(RabbitDead, Rabbits[i].RabbitPos, Color.White);
                }
            }


            spriteBatch.Draw(Front, new Vector2(0, Window.ClientBounds.Height - Front.Height), Color.White);
            
            spriteBatch.Draw(Crosshair, new Vector2(MousePos.X - Crosshair.Width / 2, MousePos.Y - Crosshair.Height / 2), Color.White);
            End();

        }
        // Load the rabbits
        protected void LoadRabbits()
        {

            TempPos.Y = Window.ClientBounds.Height;

            float[] XPositions = new float[3];
            XPositions[0] = X0;
            XPositions[1] = X1;
            XPositions[2] = X2;
            for (int i = 0; i < 3; i++)
            {


                
                TempPos.X = XPositions[i];
                Rabbits[i] = new RabbitClass(TempPos, StopY, Front.Height, RabbitSprite.Width, RabbitSprite.Height);

            }

        }
        // Load the fonts
        protected void LoadFonts()
        {
            Font = Content.Load<SpriteFont>(@"NewSpriteFont");
        }
        // Draw the fonts
        protected void DrawFonts()
        {
            string score = "Score:" + Score;
            string restart = "Press 'R' to restart";
            if (GameEnded())
            {

                spriteBatch.DrawString(Font, score, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 + 25), Color.White);
                spriteBatch.DrawString(Font, restart, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 + 75), Color.White);
            }
            else
            {
                
                spriteBatch.DrawString(Font, score, new Vector2 (0, 0), Color.White);
            }

        }
        // Load sounds
        protected void LoadSounds()
        {
            ShotgunSound = Content.Load<SoundEffect>(@"shotgun_sound");
        }
        // Check if a rabbit is hitted and play the shotgun sound
        protected void CheckRabbitHit()
        {
            MousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!PrevMState)
                {
                    RabbitHit();
                    ShotgunSound.Play();
                }

            }
            PreviousMouseState();

        }
        // Check previous mousestate
        protected void PreviousMouseState()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                PrevMState = true;
            }
            else
            {
                PrevMState = false;
            }
        }
        // Check if the rabbit is hit and give score
        protected void RabbitHit()
        {
            for (int i = 0; i < 3; i++)
            {
                if (Rabbits[i].Dead == false)
                {


                    if (Rabbits[i].Hitbox.Contains(MousePos) && MousePos.Y < Window.ClientBounds.Height - Front.Height)
                    {
                        Score = Score + 10;
                        Rabbits[i].Dead = true;

                    }
                }
            }
        }
        // Drawing the endscreen and reset rabbit positions
        protected void End()
        {
            if (GameEnded())
            {
                spriteBatch.Draw(EndScreen, new Rectangle(0, 0, Background.Width, Background.Height), Color.White);
                Restart();
                for (int i = 0; i < 3; i++)
                {
                    Rabbits[i].RabbitPos.Y = 450;
                    Rabbits[i].Velocity = 0;
                }

            }
        }
        // Draw the clock face
        protected void ClockPos()
        {

            iY = TimePlayed / 10;
            iX = TimePlayed % 10;
            xPos = 122 * iX;
            yPos = 122 * iY;

            spriteBatch.Draw(ClockSheet, new Rectangle(Window.ClientBounds.Width / 2 - 122 / 2, 12, 122, 122),
            new Rectangle(xPos, yPos, 122, 122), Color.White);



        }
        // Spawn rabbits
        protected void Spawn(float time)
        {
            Random Rnd = new Random();

            if (RabbitCount < MaxRabbitCount)
            {
                for (int i = 0; i < 3; i++)
                {
                    // Give rabbits a spawntime
                    if (Rabbits[i].SpawnTime == 0 && Rabbits[i].Dead && Rabbits[i].Velocity == 0)
                    {

                        float RndSpawn = 1.0f * Rnd.Next(1, 100 - (int)time) / 10;

                        Rabbits[i].SpawnTime = time + RndSpawn;
                    }
                    // Spawn rabbits
                    if (Rabbits[i].SpawnTime > 0 && time > Rabbits[i].SpawnTime)
                    {
                        Rabbits[i].Dead = false;
                        Rabbits[i].Velocity = CalcVelocity();
                        Rabbits[i].SpawnTime = 0;
                        Rabbits[i].Direction = -1;
                        RabbitCount++;
                    }
                }
            }
        }
        // Calculate the velocity
        protected int CalcVelocity()

        {
            Random Rnd = new Random();
            return (int)(1.0f * Score / 50) + Rnd.Next(2, 4);
        }
        // Check if all rabbit's ded
        protected Boolean AllRabbitsDead()
        {
            Boolean result = true;
            for (int i = 0; i < 3; i++)
            {
                if (!Rabbits[i].Dead)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        // Check if game's ended
        protected Boolean GameEnded()
        {
            return TimePlayed >= 60 || (RabbitCount >= MaxRabbitCount && AllRabbitsDead());
        }
        // Draw the weapons and rotate it against the mouse
        protected void DrawWeapons()
        {

            spriteBatch.Draw(Chainsaw, new Vector2(Window.ClientBounds.Width / 2, 145 - Chainsaw.Height / 2), Color.White);

            Vector2 ShotgunPos = new Vector2(Window.ClientBounds.Width / 2, 430);
            float DirectionX = MousePos.X - (ShotgunPos.X - Shotgun.Width / 2);
            float DirectionY = MousePos.Y - (ShotgunPos.Y - Shotgun.Width / 2);
            float Rotation = (float)Math.Atan2(DirectionY, DirectionX);
            Vector2 Origin = new Vector2(Shotgun.Width / 2, Shotgun.Height / 2);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                
                spriteBatch.Draw(ShotgunFire, ShotgunPos, null, Color.White, Rotation + ((float)Math.PI * 0.37f), Origin, 1, SpriteEffects.None, 0);
                
                    
                
            }
            else
            {
                spriteBatch.Draw(Shotgun, ShotgunPos, null, Color.White, Rotation + ((float)Math.PI * 0.37f), Origin, 1, SpriteEffects.None, 0);
            }
            
            
            End();
        }
        // Using this for a restart
        public void Restart()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                Score = 0;
                PlayTime = 0;
                RabbitCount = 0;
                for (int i = 0; i < 3; i++)
                {
                    Rabbits[i].RabbitPos.Y = 450;
                }
                
            }
        }

    } 
}
