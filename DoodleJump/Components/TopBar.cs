using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Components.ActionScene
{
    public class TopBar : DrawableGameComponent
    {
        private const int TEXT_PADDING = 5;

        private SpriteBatch spriteBatch;
        private Texture2D topbarTexture;
        private SpriteFont font;
        private BasicString scoreString;

        private int score = 0;
        private float yCoord = 0;

        public int Score { get => score; set => score = value; }

        public TopBar(Game game, SpriteBatch spriteBatch,
            Texture2D topbarTexture, SpriteFont font) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.topbarTexture = topbarTexture;
            this.font = font;

            yCoord = (topbarTexture.Height - font.LineSpacing) / 2;
            scoreString = new BasicString(game, spriteBatch, font, score.ToString(), Color.Black);
            scoreString.Position = new Vector2(TEXT_PADDING, yCoord);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(topbarTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            scoreString.Draw(gameTime);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            scoreString.Message = score.ToString();
            //coinCountString.Message = coinCount.ToString();

            //coinCountString.Position = new Vector2(Shared.Stage.X - font.MeasureString(coinCount.ToString()).X - TEXT_PADDING, yCoord);

            base.Update(gameTime);
        }
    }
}
