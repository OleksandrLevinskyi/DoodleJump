/*
 * Sprite.cs
 * Abstract class for all sprites
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

namespace DoodleJump.Sprites
{
    /// <summary>
    /// abstract class for all sprites
    /// </summary>
    public abstract class Sprite : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 speed;

        public Vector2 Position { get => position; set => position = value; } // position of the sprite
        public Texture2D Texture { get => texture; set => texture = value; } // texture of the sprite
        public Vector2 Speed { get => speed; set => speed = value; } // speed of the sprite

        /// <summary>
        /// sprite constructor, sets the texture
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="texture">sprite background</param>
        public Sprite(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
        }

        /// <summary>
        /// draws a sprite
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// gets a rectangular bound of the sprite
        /// </summary>
        /// <returns>rectangular bound</returns>
        public virtual Rectangle GetBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
