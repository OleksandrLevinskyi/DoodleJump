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
    public abstract class Sprite : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 speed;

        public Vector2 Position { get => position; set => position = value; }
        public Texture2D Texture { get => texture; set => texture = value; }

        public Sprite(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
        }

        public virtual Rectangle GetBound()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
