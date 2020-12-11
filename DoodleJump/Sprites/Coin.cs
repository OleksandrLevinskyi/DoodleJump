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
    public class Coin : Sprite
    {
        public Coin(Game game, SpriteBatch spriteBatch, Texture2D texture, Vector2 position) : base(game, spriteBatch, texture)
        {
            this.position = position;
        }
    }
}
