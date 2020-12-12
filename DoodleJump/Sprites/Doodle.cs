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
    public class Doodle : Sprite
    {
        private const int DOODLE_XSPEED = 5;
        private const int GRAVITY = 30;
        private const int NOSE_WIDTH = 25;
        private const int FEET_WIDTH = 38;
        private const int FEET_HEIGHT = 12;
        private const int FEET_GAP = 7;
        private const int DELAY = 10;
        public const int INIT_JUMPSPEED = 800;

        private const int SPRING_BOOST_JUMPSPEED = 1200;
        private const int SPRING_BOOST_DELAY = 60;

        private enum MovementDirection
        {
            Left,
            Right,
            Up
        }

        private int jumpSpeed = INIT_JUMPSPEED;
        private bool isJumping = false;
        private bool isFalling = false;

        private KeyboardState oldState;
        private Texture2D textureRight;
        private Texture2D textureLeft;
        private Texture2D textureUp;
        private int count = 0;
        private bool isCount = false;
        private int boostCount = 0;
        private bool isBoostCount = false;

        private MovementDirection movementDirection = MovementDirection.Right;

        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }
        public bool IsFalling { get => isFalling; set => isFalling = value; }

        public Doodle(Game game, SpriteBatch spriteBatch,
            Texture2D textureRight, Texture2D textureLeft, Texture2D textureUp) : base(game, spriteBatch, textureRight)
        {
            position = Vector2.Zero; // default
            speed = new Vector2(DOODLE_XSPEED, 0);

            this.textureRight = textureRight;
            this.textureLeft = textureLeft;
            this.textureUp = textureUp;
        }

        public override void Update(GameTime gameTime)
        {
            if (isCount)
            {
                count++;
            }

            // manage keyboard arrow clicks
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= speed.X;
                if (position.X + texture.Width / 2 <= 0)
                {
                    position.X = Shared.Stage.X - texture.Width;
                }
                this.texture = textureLeft;
                movementDirection = MovementDirection.Left;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
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
                this.texture = textureUp;
                movementDirection = MovementDirection.Up;
                count = 0;
                isCount = true;
            }
            else if (count >= DELAY)
            {
                this.texture = textureRight;
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

            // counters for boosters
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

        public void SpringBoost()
        {
            this.JumpSpeed = SPRING_BOOST_JUMPSPEED;
            isBoostCount = true;
        }

        public float GetLowerBound()
        {
            float lowerBound = position.Y + texture.Height;
            return lowerBound;
        }

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
