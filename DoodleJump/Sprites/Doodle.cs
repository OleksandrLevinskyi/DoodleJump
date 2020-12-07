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

        private int jumpSpeed = 1000;
        private bool inJump = false;
        public bool InJump { get => inJump; set => inJump = value; }
        public int JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }

        public Doodle(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game, spriteBatch, texture)
        {
            position = new Vector2((Shared.Stage.X + texture.Width) / 2, Shared.Stage.Y - texture.Height);
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

            if (!inJump)
            {
                speed.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                inJump = true;
            }

            if (inJump)
            {
                speed.Y += GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                speed.Y = 0;
            }

            position.Y += speed.Y;

            //inJump = position.Y<=700;

            base.Update(gameTime);
        }
    }
}
