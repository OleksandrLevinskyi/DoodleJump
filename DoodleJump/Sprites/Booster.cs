using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoodleJump.Sprites
{
    public enum BoosterType
    {
        Spring,
        Propeller,
        Rocket
    }

    public class Booster : Sprite
    {
        public Booster(Game game, SpriteBatch spriteBatch, Texture2D texture) : base(game, spriteBatch, texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
