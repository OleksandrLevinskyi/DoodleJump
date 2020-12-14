/*
 * TopBar.cs
 * TopBar on ActionScene
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Components.ActionScene
{
    /// <summary>
    /// top bar with highscore on the left
    /// </summary>
    public class TopBar : DrawableGameComponent
    {
        private const int TEXT_PADDING = 5;

        private SpriteBatch spriteBatch;
        private Texture2D topbarTexture;
        private BasicString scoreString;
        private SpriteFont font;

        private int score = 0;
        private float yCoord = 0;

        public int Score { get => score; set => score = value; } // score to be displayed

        /// <summary>
        /// constructor to create a tool
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="topbarTexture">background picture of the top bar</param>
        /// <param name="font">font</param>
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

        /// <summary>
        /// draws the background & score
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(topbarTexture, Vector2.Zero, Color.White);
            spriteBatch.End();

            scoreString.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// updates the message on the tool
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            scoreString.Message = score.ToString();

            base.Update(gameTime);
        }
    }
}
