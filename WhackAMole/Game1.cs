using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Texture2D Background;
        Texture2D Front;
        Texture2D RabbitSprite;
        Texture2D ClockSprite;
        Texture2D EndScreen;
        Texture2D ClockSheet;
        Texture2D RabbitDead;
        RabbitClass[] Rabbits = new RabbitClass[3];
        SpriteFont Font;
        Vector2 MousePos;
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
            IsMouseVisible = true;


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

            ScoreCount();

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

            DrawFonts();


            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void LoadPictures()
        {
            Background = Content.Load<Texture2D>(@"rabbit_background");
            Front = Content.Load<Texture2D>(@"rabbit_front");
            RabbitSprite = Content.Load<Texture2D>(@"rabbit_ok");
            ClockSprite = Content.Load<Texture2D>(@"clock_0");
            EndScreen = Content.Load<Texture2D>(@"end_screen");
            ClockSheet = Content.Load<Texture2D>(@"Clock_sheet");
            RabbitDead = Content.Load<Texture2D>(@"rabbit_dead");

        }
        protected void DrawPictures()
        {

            spriteBatch.Draw(Background, new Rectangle(0, 0, Background.Width, Background.Height), Color.White);
            Vector2 CenterClock = new Vector2(Window.ClientBounds.Width / 2 - ClockSprite.Width / 2, 10);
            spriteBatch.Draw(ClockSprite, CenterClock, Color.White);
            SheetPos();
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
            End();

        }
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

        protected void LoadFonts()
        {
            Font = Content.Load<SpriteFont>(@"NewSpriteFont");
        }

        protected void DrawFonts()
        {
            string score = "Score:" + Score;
            if (GameEnded())
            {

                spriteBatch.DrawString(Font, score, new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 + 25), Color.White);
            }
            else
            {
                Vector2 scorelen = Font.MeasureString(score);
                spriteBatch.DrawString(Font, score, new Vector2(Window.ClientBounds.Width / 2 - scorelen.X / 2, Window.ClientBounds.Height - (Front.Height - 50)), Color.White);
            }

        }

        protected void ScoreCount()
        {
            MousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!PrevMState)
                {
                    RabbitHit();
                }

            }
            PreviousMouseState();

        }

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

        protected void End()
        {
            if (GameEnded())
            {
                spriteBatch.Draw(EndScreen, new Rectangle(0, 0, Background.Width, Background.Height), Color.White);

                for (int i = 0; i < 3; i++)
                {
                    Rabbits[i].RabbitPos.Y = 450;
                    Rabbits[i].Velocity = 0;
                }

            }
        }

        protected void SheetPos()
        {

            iY = TimePlayed / 10;
            iX = TimePlayed % 10;
            xPos = 122 * iX;
            yPos = 122 * iY;

            spriteBatch.Draw(ClockSheet, new Rectangle(Window.ClientBounds.Width / 2 - 122 / 2, 12, 122, 122),
            new Rectangle(xPos, yPos, 122, 122), Color.White);



        }
        protected void Spawn(float time)
        {
            Random Rnd = new Random();

            if (RabbitCount < MaxRabbitCount)
            {
                for (int i = 0; i < 3; i++)
                {

                    if (Rabbits[i].SpawnTime == 0 && Rabbits[i].Dead && Rabbits[i].Velocity == 0)
                    {

                        float RndSpawn = 1.0f * Rnd.Next(30, 100 - (int)time) / 100;

                        Rabbits[i].SpawnTime = time + RndSpawn;
                    }
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
        protected int CalcVelocity()

        {
            Random Rnd = new Random();
            return (int)(1.0f * Score / 50) + Rnd.Next(2, 4);
        }
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
        protected Boolean GameEnded()
        {
            return TimePlayed >= 60 || (RabbitCount >= MaxRabbitCount && AllRabbitsDead());
        }


    } 
}
