/*
 * BasicString.cs
 * Tool for drawing strings
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

namespace DoodleJump.Components
{
    /// <summary>
    /// tool that helps drawing strings
    /// </summary>
    public class BasicString : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        private SpriteFont font;
        private Color color;
        private Vector2 position;

        private string message;

        public Vector2 Position { get => position; set => position = value; } // manages position of the string
        public string Message { get => message; set => message = value; } // manages message of the string

        /// <summary>
        /// constructor to create a tool
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="font">font type</param>
        /// <param name="message">message</param>
        /// <param name="color">color of the message</param>
        public BasicString(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            string message,
            Color color) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.position = Vector2.Zero;
            this.message = message;
            this.color = color;
        }

        /// <summary>
        /// draws the string based on values provided
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, message, position, color);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
