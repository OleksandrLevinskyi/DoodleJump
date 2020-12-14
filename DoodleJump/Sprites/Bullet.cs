/*
 * Bullet.cs
 * Bullet sprite
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
    /// bullet sprite
    /// </summary>
    public class Bullet : Sprite
    {
        private const int BULLET_YSPEED = -30; // bullet speed

        /// <summary>
        /// sprite constructor, initialized necessary values
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="texture">sprite background</param>
        public Bullet(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game, spriteBatch, texture)
        {
            position = new Vector2(-2 * texture.Width, -texture.Height);
            speed = new Vector2(0, BULLET_YSPEED);
        }

        /// <summary>
        /// updates bullet properties to make it move/disappear
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (position.Y <= 0)
            {
                position = new Vector2(-2 * texture.Width, -texture.Height);
            }
            else
            {
                position += speed;
            }

            base.Update(gameTime);
        }
    }
}
