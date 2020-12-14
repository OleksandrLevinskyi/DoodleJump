/*
 * Doodle.cs
 * Doodle sprite
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
    /// doodle sprite
    /// </summary>
    public class Doodle : Sprite
    {
        private const int DOODLE_XSPEED = 5;
        private const int GRAVITY = 30;
        private const int NOSE_WIDTH = 25;
        private const int FEET_HEIGHT = 12;
        private const int FEET_GAP = 7;
        private const int DELAY = 10;
        public const int INIT_JUMPSPEED = 800;

        private const int SPRING_BOOST_JUMPSPEED = 1300;
        private const int SPRING_BOOST_DELAY = 20;

        // doodle possible movement direction
        private enum MovementDirection
        {
            Left,
            Right,
            Up
        }

        private KeyboardState oldState;
        private Texture2D textureRight;
        private Texture2D textureLeft;
        private Texture2D textureUp;

        private int count = 0;
        private int boostCount = 0;
        private int jumpSpeed = INIT_JUMPSPEED;

        private bool isCount = false;
        private bool isBoostCount = false;
        private bool isJumping = false;
        private bool isFalling = false;

        private MovementDirection movementDirection = MovementDirection.Right; // default right

        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; } // jump speed of the doodle
        public bool IsJumping { get => isJumping; set => isJumping = value; } // whether doodle is jumping
        public bool IsFalling { get => isFalling; set => isFalling = value; } // whether doodle is falling
        public Color DoodleColor { get; set; } = Color.White; // doodle color

        /// <summary>
        /// sprite constructor, initialized necessary values
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="textureRight">texture for right movement</param>
        /// <param name="textureLeft">texture for left movement</param>
        /// <param name="textureUp">texture for up movement</param>
        public Doodle(Game game, SpriteBatch spriteBatch,
            Texture2D textureRight, Texture2D textureLeft, Texture2D textureUp) : base(game, spriteBatch, textureRight)
        {
            position = Vector2.Zero; // default
            speed = new Vector2(DOODLE_XSPEED, 0);

            this.textureRight = textureRight;
            this.textureLeft = textureLeft;
            this.textureUp = textureUp;
        }

        /// <summary>
        /// draws a doodle
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, DoodleColor);
            spriteBatch.End();
        }

        /// <summary>
        /// updates doodle properties to adjust its behaviour
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (isCount)
            {
                count++;
            }

            // manage keyboard arrow clicks
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left) && movementDirection != MovementDirection.Up)
            {
                // move left
                position.X -= speed.X;
                if (position.X + texture.Width / 2 <= 0)
                {
                    position.X = Shared.Stage.X - texture.Width;
                }
                this.texture = textureLeft;
                movementDirection = MovementDirection.Left;
            }
            if (ks.IsKeyDown(Keys.Right) && movementDirection != MovementDirection.Up)
            {
                // move right
                position.X += speed.X;
                if (position.X + texture.Width / 2 >= Shared.Stage.X)
                {
                    position.X = 0;
                }
                this.texture = textureRight;
                movementDirection = MovementDirection.Right;
            }

            if (oldState.IsKeyUp(Keys.Space) && ks.IsKeyDown(Keys.Space))
            {
                // shoot
                this.texture = textureUp;
                this.position.Y -= textureUp.Height - textureRight.Height;
                movementDirection = MovementDirection.Up;
                count = 0;
                isCount = true;
            }
            else if (count >= DELAY)
            {
                // stop shot
                this.texture = textureRight;
                this.position.Y += textureUp.Height - textureRight.Height;
                movementDirection = MovementDirection.Right;
                count = 0;
                isCount = false;
            }

            oldState = ks;

            // manage jumping
            if (!isJumping)
            {
                speed.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                isJumping = true;
                isFalling = false;
            }

            if (isJumping)
            {
                speed.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (speed.Y >= 0)
                {
                    isFalling = true;
                }
            }
            else
            {
                speed.Y = 0;
            }

            position.Y += speed.Y;

            // counter for boosters
            if (isBoostCount)
            {
                boostCount++;
            }
            if (boostCount > SPRING_BOOST_DELAY)
            {
                this.JumpSpeed = INIT_JUMPSPEED;
                isBoostCount = false;
                boostCount = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// adjusts doodle behaviour if spring was hit
        /// </summary>
        public void SpringBoost()
        {
            this.JumpSpeed = SPRING_BOOST_JUMPSPEED;
            isBoostCount = true;
        }

        /// <summary>
        /// gets doodle lower bound
        /// </summary>
        /// <returns>doodle lower bound</returns>
        public float GetLowerBound()
        {
            float lowerBound = position.Y + texture.Height;
            return lowerBound;
        }

        /// <summary>
        /// gets doodle feet bound
        /// </summary>
        /// <returns>doodle feet bound</returns>
        public Rectangle GetFeetBound()
        {
            Rectangle feetBound = new Rectangle((int)position.X + FEET_GAP,
                        (int)position.Y + this.texture.Height - FEET_HEIGHT,
                        this.texture.Width - NOSE_WIDTH - FEET_GAP,
                        FEET_HEIGHT);

            switch (movementDirection)
            {
                case MovementDirection.Left:
                    feetBound.X = (int)position.X + NOSE_WIDTH;
                    feetBound.Width = this.texture.Width - NOSE_WIDTH - FEET_GAP;
                    break;
                case MovementDirection.Right:
                    break;
                case MovementDirection.Up:
                    feetBound.X = (int)position.X + FEET_GAP;
                    feetBound.Width = this.texture.Width - FEET_GAP;
                    break;
                default:
                    break;
            }

            return feetBound;
        }
    }
}
