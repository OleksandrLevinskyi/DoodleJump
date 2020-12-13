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
    public class HighScoreScene : GameScene
    {
        private const int INIT_TOP_POSITION = 94;
        private const int LEFT_PADDING = 30;
        private const int LINE_HEIGHT = 44;
        private const int MAX_CAPACITY = 10;
        public List<Player> players { get; set; }
        public HighScoreScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.texture = game.Content.Load<Texture2D>("Images/highscore_scene");

            DoodleJumpGame djGame = (DoodleJumpGame)game;
            this.players = djGame.ReadScores();

            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            int count = players.Count > MAX_CAPACITY ? MAX_CAPACITY : players.Count;
            for (int i = 0; i < count; i++)
            {
                string text = $"{i + 1}  -  {players[i].Name}  -  {players[i].Score}";
                BasicString playerStr = new BasicString(game, spriteBatch, standardItemFont, text, Color.Black);
                playerStr.Position = GetMiddlePosition(standardItemFont, text, INIT_TOP_POSITION + LINE_HEIGHT * i);
                this.Components.Add(playerStr);
            }
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private Vector2 GetMiddlePosition(SpriteFont font, string input, float yCoord = INIT_TOP_POSITION)
        {
            return new Vector2((Shared.Stage.X - font.MeasureString(input).X) / 2, yCoord);
        }
    }
}
