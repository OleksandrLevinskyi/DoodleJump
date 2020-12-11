using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Components
{
    public class BasicString : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        private SpriteFont font;
        private Color color;
        private Vector2 position;
        private string message;

        public Vector2 Position { get => position; set => position = value; }
        public string Message { get => message; set => message = value; }

        public BasicString(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            string message,
            Color color) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.font = font;
            this.position = Vector2.Zero;
            this.message = message;
            this.color = color;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, message, position, color);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
