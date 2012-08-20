using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Invaders
{
    enum GameState
    {
        Menu1,
        Resume,
        Pause,
        GameOver,
        RoundDelay,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class InvadersGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameState gameState;
        SpriteFont spriteFont;
        UI UI;
        
        // Input
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        MouseState currentMouseState;
        MouseState previousMouseState;
        KeyboardDelayableInput keyboardDInput;

        // Game objects
        Player player;
        List<Bullet> playerBullets;
        List<Invader> invaders;
        InvadersSquad invadersSquad;
        List<Bullet> invadersBullets;

        //
        int round;
        double roundDelayStart;

        public InvadersGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 570;
            graphics.ApplyChanges();

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
            gameState = GameState.Menu1;
            UI = new UI();

            keyboardDInput = new KeyboardDelayableInput(200);

            ScreenSize.Viewport = GraphicsDevice.Viewport;

            playerBullets = new List<Bullet>();
            player = new Player(playerBullets);

            invadersBullets = new List<Bullet>();

            invaders = new List<Invader>();
            invadersSquad = new InvadersSquad();

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

            // Load font
            spriteFont = Content.Load<SpriteFont>("font");

            // Load player content
            Texture2D playerTexture = Content.Load<Texture2D>("player");
            Animation playerAnimation = new Animation();
            playerAnimation.Initialize(playerTexture, 
                Vector2.Zero, 44, 30, 1f);

            Texture2D bulletTexture = Content.Load<Texture2D>("bullet");

            Animation playerBullet = new Animation();
            playerBullet.Initialize(bulletTexture,
                Vector2.Zero, 5, 8, 1f);

            SoundEffect playerBulletSound = Content.Load<SoundEffect>("sound\\bullet");

            player.Initialize(playerAnimation, Vector2.Zero, playerBullet,
                playerBulletSound);

            // Load invaders animations
            Animation[] invadersAnimations = new Animation[InvadersSquad.SquadHeight];
            for (int i = 0; i < InvadersSquad.SquadHeight; i++)
            {
                invadersAnimations[i] = new Animation();
                invadersAnimations[i].Initialize(
                    Content.Load<Texture2D>("invader" + i),
                    Vector2.Zero,
                    30, 30, 1f);
            }

            invadersSquad.Initialize(invaders, invadersAnimations, invadersBullets,
                playerBullet, playerBulletSound);

            UI.Initialize(spriteFont, playerTexture);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void ResetGame()
        {
            player.ResetPosition();

            player.Animation.Visible = true;
            player.Health = 3;
            player.Score = 0;
            player.ShootDelay = 500;
            playerBullets.Clear();
            invadersBullets.Clear();

            round = 0;
            invadersSquad.NewRound(round);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Input - Keyboard
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            keyboardDInput.Update(currentKeyboardState);

            if (!IsActive)
            {
                if (gameState == GameState.Resume)
                    gameState = GameState.Pause;
                IsMouseVisible = true;
                return;
            }
            else if (gameState == GameState.Menu1 ||
                gameState == GameState.GameOver)
            {
                //if (currentKeyboardState.IsKeyDown(Keys.Enter) &&
                //    !previousKeyboardState.IsKeyDown(Keys.Enter))
                if (keyboardDInput.KeyCheck(Keys.Enter, gameTime))
                {
                    // Start or Restart game
                    gameState = GameState.Resume;
                    ResetGame();
                    resetMousePosition();
                    // and update game logic
                    IsMouseVisible = false;
                }
                else
                {
                    IsMouseVisible = true;
                    return;
                }
            }
            else
            {
                //if (currentKeyboardState.IsKeyUp(Keys.P) &&
                //    previousKeyboardState.IsKeyDown(Keys.P))
                if (keyboardDInput.KeyCheck(Keys.P, gameTime) ||
                    keyboardDInput.KeyCheck(Keys.Escape, gameTime))
                {
                    if (gameState == GameState.Resume)
                    {
                        gameState = GameState.Pause;
                        IsMouseVisible = true;
                        return;
                    }
                    else
                    {
                        gameState = GameState.Resume;
                        resetMousePosition();
                        // update game logic
                        //IsMouseVisible = false;
                    }
                }
                else if (gameState == GameState.Pause)
                {
                    return;
                }
                else if (gameState == GameState.RoundDelay)
                {
                    if (gameTime.TotalGameTime.TotalMilliseconds - roundDelayStart < 1200)
                    {
                        resetMousePosition();
                        return;
                    }
                    gameState = GameState.Resume;
                }
                IsMouseVisible = false;
            }

            //
            // UPDATE GAME LOGIC
            //

            // Input - Mouse
            currentMouseState = Mouse.GetState();
            resetMousePosition();
            previousMouseState = Mouse.GetState();

            //
            player.Update(gameTime, currentMouseState, previousMouseState,
                currentKeyboardState, previousKeyboardState);
            //UpdatePlayer(gameTime);
            UpdateBullets(gameTime);
            invadersSquad.Update(gameTime);

            Collisions.BetweenPlayerAndInvaders(player, invaders);
            Collisions.BetweenInvadersAndPlayersBullets(player, invaders, playerBullets);
            Collisions.BetweenPlayerAndInvadersBullets(player, invadersBullets);
            player.Score += 15 * Collisions.BetweenBullets(playerBullets, invadersBullets);

            UI.Update(gameTime, player.Score, player.Health);

            if (invadersSquad.IsCrossedBottom)
            {
                gameState = GameState.GameOver;
            }
            if (player.Health <= 0)
            {
                player.Animation.Visible = false;
                // Explosion?
                gameState = GameState.GameOver;
            }
            else if (invaders.Count == 0)
            {
                round++;
                player.ShootDelay -= 10;
                if (player.ShootDelay < 350)
                    player.ShootDelay = 350;
                player.ResetPosition();
                player.Update(gameTime);
                invadersSquad.NewRound(round);
                playerBullets.Clear();
                invadersBullets.Clear();
                roundDelayStart = gameTime.TotalGameTime.TotalMilliseconds;
                gameState = GameState.RoundDelay;
            }

            base.Update(gameTime);
        }

        //void UpdatePlayer(GameTime gameTime)
        //{
        //    // Mouse move
        //    player.Position.X += currentMouseState.X - previousMouseState.X;
        //    player.Position.Y += currentMouseState.Y - previousMouseState.Y;

        //    // Keyboard move
        //    if (currentKeyboardState.IsKeyDown(Keys.Up) 
        //        || currentKeyboardState.IsKeyDown(Keys.W))
        //    {
        //        player.Position.Y -= player.MoveSpeed;
        //    }
        //    if (currentKeyboardState.IsKeyDown(Keys.Down) 
        //        || currentKeyboardState.IsKeyDown(Keys.S))
        //    {
        //        player.Position.Y += player.MoveSpeed;
        //    }
        //    if (currentKeyboardState.IsKeyDown(Keys.Left) 
        //        || currentKeyboardState.IsKeyDown(Keys.A))
        //    {
        //        player.Position.X -= player.MoveSpeed;
        //    }
        //    if (currentKeyboardState.IsKeyDown(Keys.Right) 
        //        || currentKeyboardState.IsKeyDown(Keys.D))
        //    {
        //        player.Position.X += player.MoveSpeed;
        //    }

        //    // Fire
        //    if (currentKeyboardState.IsKeyDown(Keys.Space)
        //        || currentMouseState.LeftButton == ButtonState.Pressed)
        //    {
        //        player.TryShoot(gameTime, playerBullets);
        //    }

        //    player.Position.X = MathHelper.Clamp(player.Position.X,
        //        0, GraphicsDevice.Viewport.Width - player.Width);
        //    player.Position.Y = MathHelper.Clamp(player.Position.Y,
        //        0, GraphicsDevice.Viewport.Height - player.Height);

        //    player.Update(gameTime);
        //}

        void UpdateBullets(GameTime gameTime)
        {
            for (int i = playerBullets.Count - 1; i >= 0; i--)
            {
                playerBullets[i].Update(gameTime);

                if (!playerBullets[i].Active)
                    playerBullets.RemoveAt(i);
            }

            for (int i = invadersBullets.Count - 1; i >= 0; i--)
            {
                invadersBullets[i].Update(gameTime);

                if (!invadersBullets[i].Active)
                    invadersBullets.RemoveAt(i);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (gameState == GameState.Menu1)
            {
                // Draw Menu1
                UI.DrawAtCenter(spriteBatch, Vector2.Zero, "Invaders", 0.4f, Color.White);
                UI.DrawAtCenter(spriteBatch, new Vector2(0, 30), "For start game press 'Enter'.", 0.2f, Color.White);
            }
            else
            {
                // Draw everything
                invadersSquad.Draw(spriteBatch);

                player.Draw(spriteBatch);

                foreach (Bullet bullet in invadersBullets)
                    bullet.Draw(spriteBatch);
                foreach (Bullet bullet in playerBullets)
                    bullet.Draw(spriteBatch);

                UI.Draw(spriteBatch);

                // Special states
                if (gameState == GameState.GameOver)
                {
                    // Draw GameOver state
                    UI.DrawAtCenter(spriteBatch, Vector2.Zero, "GAME OVER", 0.6f, Color.Red);
                }
                else if (gameState == GameState.Pause)
                {
                    // Draw Pause state
                    UI.DrawAtCenter(spriteBatch, Vector2.Zero, "Pause", 0.4f, Color.White);
                }
                else if (gameState == GameState.RoundDelay)
                {
                    UI.DrawAtCenter(spriteBatch, Vector2.Zero, "Round " + (round + 1), 0.4f, Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void resetMousePosition()
        {
            Mouse.SetPosition(
                Window.ClientBounds.Center.X - Window.ClientBounds.X,
                Window.ClientBounds.Center.Y - Window.ClientBounds.Y);
        }
    }
}
