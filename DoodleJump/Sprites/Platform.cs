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
    public enum PlatfromType
    {
        Original,
        MovableHor,
        MovableVer,
        Wooden,
        Disappearing
    }

    public class Platform : Sprite
    {
        private PlatfromType type;
        private static Random randomNumGen = new Random();

        public PlatfromType Type { get => type; set => type = value; }

        public Platform(Game game, SpriteBatch spriteBatch, Texture2D texture, float positionY, PlatfromType type = PlatfromType.Original) : base(game, spriteBatch, texture)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);

            this.type = type;
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
            base.Update(gameTime);
        }

        public void UpdatePosition(float positionY)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);
        }
    }
}
