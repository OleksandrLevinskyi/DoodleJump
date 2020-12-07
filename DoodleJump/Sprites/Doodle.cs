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
        private const int NOSE_SIZE = 20;

        private int jumpSpeed = 800;
        private bool isJumping = false;
        private bool isFalling = false;
        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }
        public bool IsFalling { get => isFalling; set => isFalling = value; }

        public Doodle(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game, spriteBatch, texture)
        {
            position = Vector2.Zero; // default
            speed = new Vector2(DOODLE_XSPEED, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // manage keyboard arrow clicks
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
            {
                position.X -= speed.X;
                if (position.X + texture.Width/2 <= 0)
                {
                    position.X = Shared.Stage.X - texture.Width;
                }
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                position.X += speed.X;
                if(position.X + texture.Width/2 >= Shared.Stage.X)
                {
                    position.X = 0;
                }
            }

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
                if(speed.Y >= 0) {
                    isFalling = true;
                }
            }
            else
            {
                speed.Y = 0;
            }

            position.Y += speed.Y;

            //isJumping = position.Y<=700;

            base.Update(gameTime);
        }

        public float GetLowerBound()
        {
            float lowerBound = position.Y + texture.Height;
            return lowerBound;
        }
        public override Rectangle GetBound()
        {
            return new Rectangle((int)position.X - NOSE_SIZE, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
