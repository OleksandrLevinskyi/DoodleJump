﻿/*
 * GameOverScene.cs
 * Scene displayed after the game finishes
 * 
 * Revision History
 *          Oleksandr Levinskyi, 2020.12.06: Created & Imlemented
 *          Oleksandr Levinskyi, 2020.12.13: Revised & Completed
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Scenes
{
    /// <summary>
    /// scene displayed after the game finishes
    /// </summary>
    public class GameOverScene : GameScene
    {
        private const int INIT_TOP_POSITION = 200;
        private const int LINE_HEIGHT = 50;
        private const int COEFFICIENT = 4;

        private const string DEFAULT_NAME = "doodle";

        private string[] menuItems = { "Play Again", "Main Menu" };

        private MenuComponent menu;
        private InputManager input;
        private KeyboardState oldState;

        private BasicString scoreString;
        private BasicString highScoreString;
        private BasicString nameString;
        private Texture2D editModeTexture;
        private Texture2D mainModeTexture;

        private bool isArranged = false;

        public MenuComponent Menu { get => menu; set => menu = value; } // game over menu
        public string Name { get; set; } = DEFAULT_NAME; // name of the user, can be edited with input
        public bool IsEditNameMode { get; set; } = false; // mode of the scene (none/edit)
        public int Score { get; set; } // score of the game played
        public int HighScore { get; set; } // highest game score

        /// <summary>
        /// scene constructor, loads & initializes all the necessary components
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        public GameOverScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            mainModeTexture = game.Content.Load<Texture2D>("Images/gameover_scene");
            editModeTexture = game.Content.Load<Texture2D>("Images/gameover_scene_editmode");
            this.texture = mainModeTexture;

            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            scoreString = new BasicString(game, spriteBatch, standardItemFont, "", Color.Black);
            scoreString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, INIT_TOP_POSITION);
            this.Components.Add(scoreString);

            highScoreString = new BasicString(game, spriteBatch, standardItemFont, "", Color.Black);
            highScoreString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, INIT_TOP_POSITION);
            this.Components.Add(highScoreString);

            nameString = new BasicString(game, spriteBatch, standardItemFont, "your name: ", Color.Black);
            nameString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, INIT_TOP_POSITION);
            this.Components.Add(nameString);

            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            menu.Position = GetMiddlePosition(chosenItemFont, menuItems[0], menu.Position.Y + 2 * LINE_HEIGHT);
            this.Components.Add(menu);

            input = new InputManager(game, spriteBatch, standardItemFont, Color.Black);
            input.Message = Name;
            input.Position = new Vector2(nameString.Position.X + standardItemFont.MeasureString(nameString.Message).X, nameString.Position.Y);
            this.Components.Add(input);
        }

        /// <summary>
        /// fetches the user input to switch between modes, can change user name
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            // save a new entered name
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.S) && IsEditNameMode)
            {
                if (input.Message.Length == 0)
                {
                    input.Message = Name;
                }
                else
                {
                    Name = input.Message;
                }
                this.texture = mainModeTexture;

                menu.Show();

                input.DisableUpdate = true;
                IsEditNameMode = false;
            }
            // switch to the edit mode
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.Space) && !IsEditNameMode)
            {
                this.texture = editModeTexture;
                menu.Hide();

                input.Message = "";
                input.DisableUpdate = false;
                IsEditNameMode = true;
            }

            oldState = ks;

            // display messages
            scoreString.Message = $"your score: {Score}";
            highScoreString.Message = $"highest score: {HighScore}";

            // adjust strings' positions
            if (!isArranged)
            {
                ArrangeStrings();
                input.Position = new Vector2(input.Position.X, nameString.Position.Y);
                isArranged = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// gets the middle position of the string
        /// </summary>
        /// <param name="font">font</param>
        /// <param name="input">input text</param>
        /// <param name="yCoord">Y coordinate</param>
        /// <returns>middle position</returns>
        private Vector2 GetMiddlePosition(SpriteFont font, string input, float yCoord = INIT_TOP_POSITION)
        {
            return new Vector2((Shared.Stage.X - font.MeasureString(input).X) / 2, yCoord);
        }

        /// <summary>
        /// adds padding to strings
        /// </summary>
        private void ArrangeStrings()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is BasicString)
                {
                    BasicString str = (BasicString)Components[i];
                    str.Position = new Vector2(str.Position.X, str.Position.Y + (i * LINE_HEIGHT));
                }
            }
        }
    }
}
