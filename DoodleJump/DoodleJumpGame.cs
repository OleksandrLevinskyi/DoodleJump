using DoodleJump.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DoodleJump
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class DoodleJumpGame : Game
    {
        private const int SCREEN_WIDTH = 500;
        private const int SCREEN_HEIGHT = 800;
        private const int MAX_CAPACITY = 10;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // scene references
        private StartScene startScene;
        private ActionScene actionScene;
        private HelpScene helpScene;
        private HighScoreScene highScoreScene;
        private AboutScene aboutScene;
        private GameOverScene gameOverScene;

        private KeyboardState oldState;

        private List<Player> highScorePlayers;
        private StreamReader reader;
        private StreamWriter writer;
        private string fileName = "scores.txt";

        private enum MainMenu
        {
            Start,
            Help,
            HighScore,
            About,
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
            highScorePlayers = new List<Player>();

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

            aboutScene = new AboutScene(this, spriteBatch);
            this.Components.Add(aboutScene);

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
                        RebootHighScoreScene();
                        highScoreScene.Show();
                    }
                    else if (selectedIdx == (int)MainMenu.About)
                    {
                        aboutScene.Show();
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
                    actionScene.Enabled = false;
                    gameOverScene.Score = (int)actionScene.Score;
                    highScorePlayers = ReadScores(); // read scores

                    // if there are players saved and the current score is lower compared to the highest one,
                    // set highscore to this max one;
                    // otherwise set to the current score
                    if (highScorePlayers.Any() && gameOverScene.Score < highScorePlayers[0].Score)
                    {
                        gameOverScene.HighScore = highScorePlayers[0].Score;
                    }
                    else
                    {
                        gameOverScene.HighScore = (int)actionScene.Score;
                    }
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
            if (highScoreScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    highScoreScene.Hide();
                    startScene.Show();
                }
            }
            if (aboutScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    aboutScene.Hide();
                    startScene.Show();
                }
            }
            if (gameOverScene.Enabled && !gameOverScene.IsEditNameMode)
            {
                if (oldState.IsKeyDown(Keys.Enter) && ks.IsKeyUp(Keys.Enter))
                {
                    selectedIdx = gameOverScene.Menu.SelectedIdx;

                    gameOverScene.Hide();

                    Player player = new Player(gameOverScene.Name, gameOverScene.Score);
                    UpdateScores(player); // update the 'scores' file

                    if (selectedIdx == (int)GameOverMenu.PlayAgain)
                    {
                        actionScene.Hide();
                        RebootActionScene();
                        actionScene.Show();
                    }
                    else if (selectedIdx == (int)GameOverMenu.MainMenu)
                    {
                        gameOverScene.Menu.SelectedIdx = 0;
                        actionScene.Hide();
                        //RebootGameOverScene();
                        RebootActionScene();
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

            this.Components.Remove(gameOverScene);
            this.Components.Add(gameOverScene);
        }

        private void RebootHighScoreScene()
        {
            this.Components.Remove(highScoreScene);
            highScoreScene = new HighScoreScene(this, spriteBatch);
            this.Components.Add(highScoreScene);
        }

        public List<Player> ReadScores()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    highScorePlayers = new List<Player>();
                    using (reader = new StreamReader(fileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            string record = reader.ReadLine();
                            string[] recordData = record.Split(new char[] { '\t' });
                            Player player = new Player(recordData[0], Convert.ToInt32(recordData[1]));
                            highScorePlayers.Add(player);
                        }
                    }
                }

                return highScorePlayers.OrderByDescending(r => r.Score).ToList(); // sort by score
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }
            return highScorePlayers;
        }

        private void UpdateScores(Player player)
        {
            bool isChanged = false;
            try
            {
                // update the list
                if (highScorePlayers.Count < MAX_CAPACITY)
                {
                    highScorePlayers.Add(player);
                    isChanged = true;
                }
                else if (highScorePlayers[highScorePlayers.Count - 1].Score < gameOverScene.Score)
                {
                    highScorePlayers[highScorePlayers.Count - 1] = player;
                    isChanged = true;
                }

                // update the file
                if (isChanged)
                {
                    highScorePlayers = highScorePlayers.OrderByDescending(r => r.Score).ToList(); // sort by score

                    using (writer = new StreamWriter(fileName, append: false))
                    {
                        foreach (Player p in highScorePlayers)
                        {
                            writer.WriteLine(p.ToString());
                        }
                    }

                    highScorePlayers = new List<Player>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
