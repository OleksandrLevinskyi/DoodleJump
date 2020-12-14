/*
 * ParallaxBackground.cs
 * Parallax Background
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
using DoodleJump.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump
{
    /// <summary>
    /// parallax background
    /// </summary>
    public class ParallaxBackground : Sprite
    {
        private Vector2 positionLeft;

        /// <summary>
        /// sprite constructor, sets the texture
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="texture">sprite background</param>
        /// <param name="speed">layer speed</param>
        public ParallaxBackground(Game game,
            SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 speed) : base(game, spriteBatch, texture)
        {
            this.speed = speed;

            this.position = Vector2.Zero;
            this.positionLeft = new Vector2(position.X + texture.Width, position.Y);
        }

        /// <summary>
        /// draws a sprite
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(texture, positionLeft, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// updates a sprite to make it movable
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            position -= speed;
            positionLeft -= speed;

            if (position.X < -texture.Width)
            {
                position.X = positionLeft.X + texture.Width;
            }
            if (positionLeft.X < -texture.Width)
            {
                positionLeft.X = position.X + texture.Width;
            }

            base.Update(gameTime);
        }
    }
}
