using DoodleJump.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DoodleJumpGame : Game
    {
        private const int SCREEN_WIDTH = 500;
        private const int SCREEN_HEIGHT = 800;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // scene references
        private StartScene startScene;
        private ActionScene actionScene;
        private HelpScene helpScene;
        private HighScoreScene highScoreScene;
        private StoreScene storeScene;
        private GameOverScene gameOverScene;

        private KeyboardState oldState;

        private enum MainMenu
        {
            Start,
            Help,
            HighScore,
            Store,
            Quit
        }
        private enum GameOverMenu
        {
            PlayAgain,
            MainMenu
        }
        public DoodleJumpGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
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

            // TODO: use this.Content to load your game content here
            Shared.Stage = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // instantiate all scenes
            startScene = new StartScene(this, spriteBatch);
            this.Components.Add(startScene);

            actionScene = new ActionScene(this, spriteBatch);
            this.Components.Add(actionScene);

            helpScene = new HelpScene(this, spriteBatch);
            this.Components.Add(helpScene);

            highScoreScene = new HighScoreScene(this, spriteBatch);
            this.Components.Add(highScoreScene);

            storeScene = new StoreScene(this, spriteBatch);
            this.Components.Add(storeScene);

            gameOverScene = new GameOverScene(this, spriteBatch);
            this.Components.Add(gameOverScene);

            // show start scene
            startScene.Show();
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            int selectedIdx = 0;
            KeyboardState ks = Keyboard.GetState();
            if (startScene.Enabled)
            {
                if (oldState.IsKeyDown(Keys.Enter) && ks.IsKeyUp(Keys.Enter))
                {
                    selectedIdx = startScene.Menu.SelectedIdx;
                    System.Console.WriteLine(selectedIdx);
                    startScene.Hide();
                    if (selectedIdx == (int)MainMenu.Start)
                    {
                        actionScene.Show();
                    }
                    else if (selectedIdx == (int)MainMenu.Help)
                    {
                        helpScene.Show();
                    }
                    else if (selectedIdx == (int)MainMenu.HighScore)
                    {
                        highScoreScene.Show();
                    }
                    else if (selectedIdx == (int)MainMenu.Store)
                    {
                        storeScene.Show();
                    }
                    else if (selectedIdx == (int)MainMenu.Quit)
                    {
                        Exit();
                    } 
                }
            }
            if (actionScene.Enabled)
            {
                if (actionScene.ShowGameOver)
                {
                    gameOverScene.Show();
                }
            }
            if (helpScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    helpScene.Hide();
                    startScene.Show();
                }
            }
            if (gameOverScene.Enabled)
            {
                if (oldState.IsKeyDown(Keys.Enter) && ks.IsKeyUp(Keys.Enter))
                {
                    selectedIdx = gameOverScene.Menu.SelectedIdx;
                    System.Console.WriteLine(selectedIdx);
                    gameOverScene.Hide();
                    if (selectedIdx == (int)GameOverMenu.PlayAgain)
                    {
                        actionScene.Hide();
                        RebootActionScene();
                        actionScene.Show();
                    }
                    else if (selectedIdx == (int)GameOverMenu.MainMenu)
                    {
                        actionScene.Hide();
                        RebootGameOverScene();
                        startScene.Show();
                    }
                }
            }
            oldState = ks;

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

            base.Draw(gameTime);
        }


        private void RebootActionScene()
        {
            this.Components.Remove(actionScene);
            actionScene = new ActionScene(this, spriteBatch);
            this.Components.Add(actionScene);
        }
        private void RebootGameOverScene()
        {
            this.Components.Remove(gameOverScene);
            gameOverScene = new GameOverScene(this, spriteBatch);
            this.Components.Add(gameOverScene);
        }
    }
}
