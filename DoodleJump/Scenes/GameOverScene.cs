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
        private const int LINE_HEIGHT = 50;
        private const int COEFFICIENT = 4;

        private string[] menuItems = { "Play Again", "Main Menu" };

        private MenuComponent menu;
        private InputManager input;
        private bool isArranged = false;
        private KeyboardState oldState;

        private const string DEFAULT_NAME = "doodle";

        private BasicString scoreString;
        private BasicString highScoreString;
        private BasicString nameString;
        private Texture2D editModeTexture;
        private Texture2D mainModeTexture;
        public bool IsEditNameMode { get; set; } = false;

        public int Score { get; set; }
        public int HighScore { get; set; }
        public string Name { get; set; } = DEFAULT_NAME;
        public MenuComponent Menu { get => menu; set => menu = value; }

        public GameOverScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            mainModeTexture = game.Content.Load<Texture2D>("Images/gameover_scene");
            editModeTexture = game.Content.Load<Texture2D>("Images/gameover_scene_editmode");
            this.texture = mainModeTexture;

            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            scoreString = new BasicString(game, spriteBatch, standardItemFont, "", Color.Black);
            scoreString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, TITLE_POSITION);
            this.Components.Add(scoreString);

            highScoreString = new BasicString(game, spriteBatch, standardItemFont, "", Color.Black);
            highScoreString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, TITLE_POSITION);
            this.Components.Add(highScoreString);

            nameString = new BasicString(game, spriteBatch, standardItemFont, "your name: ", Color.Black);
            nameString.Position = new Vector2(Shared.Stage.X / COEFFICIENT, TITLE_POSITION);
            this.Components.Add(nameString);

            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            menu.Position = GetMiddlePosition(chosenItemFont, menuItems[0], menu.Position.Y);
            this.Components.Add(menu);

            input = new InputManager(game, spriteBatch, standardItemFont, Color.Black);
            input.Message = Name;
            input.Position = new Vector2(nameString.Position.X + standardItemFont.MeasureString(nameString.Message).X, nameString.Position.Y);
            this.Components.Add(input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();
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
            if (ks.IsKeyDown(Keys.LeftControl) && ks.IsKeyDown(Keys.Space) && !IsEditNameMode)
            {
                this.texture = editModeTexture;
                menu.Hide();

                input.Message = "";
                input.DisableUpdate = false;
                IsEditNameMode = true;
            }

            oldState = ks;

            scoreString.Message = $"your score: {Score}";
            highScoreString.Message = $"highest score: {HighScore}";

            if (!isArranged)
            {
                ArrangeStrings();
                input.Position = new Vector2(input.Position.X, nameString.Position.Y);
                isArranged = true;
            }

            base.Update(gameTime);
        }

        private Vector2 GetMiddlePosition(SpriteFont font, string input, float yCoord = TITLE_POSITION)
        {
            return new Vector2((Shared.Stage.X - font.MeasureString(input).X) / 2, yCoord);
        }

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
