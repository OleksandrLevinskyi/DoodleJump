/*
 * Booster.cs
 * Booster sprite
 * 
 * Revision History
 *          Oleksandr Levinskyi, 2020.12.06: Created & Imlemented
 *          Oleksandr Levinskyi, 2020.12.13: Revised & Completed
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump.Sprites
{
    /// <summary>
    /// booster types, can be expanded later
    /// </summary>
    public enum BoosterType
    {
        Spring
    }

    /// <summary>
    /// booster sprite
    /// </summary>
    public class Booster : Sprite
    {
        private BoosterType type;
        private Platform masterPlatform;
        public BoosterType Type { get => type; set => type = value; } // booster type
        public Platform MasterPlatform { get => masterPlatform; set { masterPlatform = value; } } // master platform
        public bool IsUsed { get; set; } // whether booster is used

        /// <summary>
        /// sprite constructor, initialized necessary values
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="texture">sprite background</param>
        /// <param name="type">booster type</param>
        public Booster(Game game, SpriteBatch spriteBatch, Texture2D texture, BoosterType type) : base(game, spriteBatch, texture)
        {
            this.type = type;
        }

        /// <summary>
        /// updates booster properties to make it move/disappear
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (masterPlatform != null)
            {
                this.position.Y = masterPlatform.Position.Y - texture.Height;
                this.position.X = masterPlatform.Position.X + (masterPlatform.Texture.Width - texture.Width) / 2;
            }
            else
            {
                position = new Vector2(-texture.Width, 0);
                IsUsed = false;
            }

            base.Update(gameTime);
        }
    }
}
