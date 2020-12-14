/*
 * Platform.cs
 * Platform sprite
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
    /// platform types
    /// </summary>
    public enum PlatfromType
    {
        Original,
        MovableHor,
        MovableVer,
        Wooden,
        Disappearing
    }

    /// <summary>
    /// platform sprite
    /// </summary>
    public class Platform : Sprite
    {
        private const int MOVE_RANGE = 40;
        private float topBound = 0;
        private float bottomBound = 0;
        private PlatfromType type;

        public float TopBound { get => topBound; set => topBound = value; } // top bound for MovableVer
        public float BottomBound { get => bottomBound; set => bottomBound = value; } // bottom bound for MovableVer
        public PlatfromType Type { get => type; set => type = value; } // platform type

        private static Random randomNumGen = new Random();

        /// <summary>
        /// sprite constructor, generates random X coordinate of platform position
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="texture">sprite background</param>
        /// <param name="positionY">Y coordinate</param>
        /// <param name="type">type of the platform</param>
        public Platform(Game game, SpriteBatch spriteBatch, Texture2D texture, float positionY, PlatfromType type = PlatfromType.Original) : base(game, spriteBatch, texture)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);

            topBound = position.Y - MOVE_RANGE;
            bottomBound = position.Y + MOVE_RANGE;

            this.type = type;
        }

        /// <summary>
        /// updates the platform position if applicable
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (this.type == PlatfromType.MovableHor)
            {
                // move platform horizontally
                if (position.X <= 0 || position.X + texture.Width >= Shared.Stage.X)
                {
                    speed = -speed;
                }
                position += speed;
            }
            else if (this.type == PlatfromType.MovableVer)
            {
                // move platform vertically
                if (position.Y <= topBound || position.Y >= bottomBound)
                {
                    speed = -speed;
                }
                position += speed;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// updates position of the platform
        /// </summary>
        /// <param name="positionY">Y coordinate</param>
        public void UpdatePosition(float positionY)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);

            topBound = position.Y - MOVE_RANGE;
            bottomBound = position.Y + MOVE_RANGE;
        }

        /// <summary>
        /// updates bounds of the platform
        /// </summary>
        /// <param name="movement">value to update by</param>
        public void UpdateBounds(int movement)
        {
            topBound += movement;
            bottomBound += movement;
        }
    }
}
