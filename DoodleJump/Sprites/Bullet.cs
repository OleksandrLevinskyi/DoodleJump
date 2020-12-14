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
    public class Bullet : Sprite
    {
        private const int BULLET_YSPEED = -30;
        public Bullet(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game, spriteBatch, texture)
        {
            position = new Vector2(-2*texture.Width, -texture.Height);
            speed = new Vector2(0, BULLET_YSPEED);
        }

        public override void Update(GameTime gameTime)
        {
            if (position.Y <= 0)
            {
                position = new Vector2(-2*texture.Width, -texture.Height);
            }
            else
            {
                position += speed;
            }

            base.Update(gameTime);
        }
    }
}
