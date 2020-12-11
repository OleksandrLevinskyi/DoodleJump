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
    public class GameOverScene : GameScene
    {
        private const int TITLE_POSITION = 200;
        private const int PADDING = 30;

        private string[] menuItems = { "Play Again", "Main Menu" };

        private MenuComponent menu;
        private InputManager input;
        private bool isEditNameMode = false;
        private bool isArranged = false;
        private KeyboardState oldState;

        private const string DEFAULT_NAME = "doodle";

        private BasicString scoreString;
        private BasicString highScoreString;
        private BasicString overallHighScoreString;
        private BasicString nameString;

        public string Score { get; set; }
        public string HighScore { get; set; }
        public string OverallHighScore { get; set; }
        public string Name { get; set; }
        public MenuComponent Menu { get => menu; set => menu = value; }

        public GameOverScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            BasicString titleString = new BasicString(game, spriteBatch, standardItemFont, "GAME OVER!", Color.Red);
            titleString.Position = GetMiddlePosition(standardItemFont, titleString.Message);
            this.Components.Add(titleString);

            scoreString = new BasicString(game, spriteBatch, standardItemFont, $"your score: {Score}", Color.Black);
            scoreString.Position = GetMiddlePosition(standardItemFont, scoreString.Message);
            this.Components.Add(scoreString);

            highScoreString = new BasicString(game, spriteBatch, standardItemFont, $"your high score: {HighScore}", Color.Black);
            highScoreString.Position = GetMiddlePosition(standardItemFont, highScoreString.Message);
            this.Components.Add(highScoreString);

            overallHighScoreString = new BasicString(game, spriteBatch, standardItemFont, $"overall high score: {OverallHighScore}", Color.Black);
            overallHighScoreString.Position = GetMiddlePosition(standardItemFont, overallHighScoreString.Message);
            this.Components.Add(overallHighScoreString);

            nameString = new BasicString(game, spriteBatch, standardItemFont, $"your name: {Name}", Color.Black);
            nameString.Position = GetMiddlePosition(standardItemFont, nameString.Message);
            this.Components.Add(nameString);

            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            this.Components.Add(menu);

            input = new InputManager(game, spriteBatch, standardItemFont, Vector2.Zero, Color.Black);
            this.Components.Add(input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.S) && isEditNameMode)
            {
                if (input.Message.Length == 0)
                {
                    input.Message = DEFAULT_NAME;
                }
                input.DisableUpdate = true;
                isEditNameMode = false;
            }
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.Space) && !isEditNameMode)
            {
                input.DisableUpdate = false;
                isEditNameMode = true;
            }

            oldState = ks;

            if (!isArranged)
            {
                scoreString.Message = $"your score: {Score}";
                highScoreString.Message = $"your high score: {HighScore}";
                overallHighScoreString.Message = $"overall high score: {OverallHighScore}";
                nameString.Message = $"your name: {Name}";

                ArrangeStrings();
                isArranged = true;
            }

            base.Update(gameTime);
        }

        private Vector2 GetMiddlePosition(SpriteFont font, string input)
        {
            return new Vector2((Shared.Stage.X - font.MeasureString(input).X) / 2, TITLE_POSITION);
        }

        private void ArrangeStrings()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is BasicString)
                {
                    BasicString str = (BasicString)Components[i];
                    str.Position = new Vector2(str.Position.X, str.Position.Y + (i * PADDING));
                }
            }
        }
    }
}
