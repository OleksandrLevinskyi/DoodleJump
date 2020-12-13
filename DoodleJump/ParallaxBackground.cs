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
    public class ParallaxBackground : Sprite
    {
        private Vector2 positionLeft;

        public ParallaxBackground(Game game,
            SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 speed) : base(game,spriteBatch,texture)
        {
            this.speed = speed;

            this.position = Vector2.Zero;
            this.positionLeft = new Vector2(position.X + texture.Width, position.Y);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.Draw(texture, positionLeft, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            position -= speed;
            positionLeft -= speed;

            //if(position2.X <= 0)
            //{
            //    position1.X = position2.X + texture.Width;
            //}
            //if(position1.X <= 0)
            //{
            //    position2.X = position1.X + texture.Width;
            //}

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
