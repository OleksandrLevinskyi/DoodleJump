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
        private const int NUM_PADDING = 50;
        private const int NAME_PADDING = 100;
        private const int SCORE_PADDING = 350;
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
                Color color = Color.Black;

                if (i == 0) color = Color.DarkOrange;
                else if (i == 1) color = Color.Gray;
                else if (i == 2) color = Color.Brown;

                BasicString numStr = new BasicString(game, spriteBatch, standardItemFont, $"{i + 1}. ", color);
                numStr.Position = new Vector2(NUM_PADDING, INIT_TOP_POSITION + LINE_HEIGHT * i);
                this.Components.Add(numStr);

                BasicString nameStr = new BasicString(game, spriteBatch, standardItemFont, $"{players[i].Name}", color);
                nameStr.Position = new Vector2(NAME_PADDING, INIT_TOP_POSITION + LINE_HEIGHT * i);
                this.Components.Add(nameStr);

                BasicString scoreStr = new BasicString(game, spriteBatch, standardItemFont, $"{players[i].Score}", color);
                scoreStr.Position = new Vector2(SCORE_PADDING, INIT_TOP_POSITION + LINE_HEIGHT * i);
                this.Components.Add(scoreStr);
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
    }
}
