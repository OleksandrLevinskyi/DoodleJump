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
        private static Random randomNumGen = new Random();
        private const int MOVE_RANGE = 40;
        private float topBound = 0;
        private float bottomBound = 0;
        private PlatfromType type;

        public float TopBound { get => topBound; set => topBound = value; }
        public float BottomBound { get => bottomBound; set => bottomBound = value; }
        public PlatfromType Type { get => type; set => type = value; }

        public Platform(Game game, SpriteBatch spriteBatch, Texture2D texture, float positionY, PlatfromType type = PlatfromType.Original) : base(game, spriteBatch, texture)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);

            topBound = position.Y - MOVE_RANGE;
            bottomBound = position.Y + MOVE_RANGE;

            this.type = type;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.type == PlatfromType.MovableHor)
            {
                if (position.X <= 0 || position.X + texture.Width >= Shared.Stage.X)
                {
                    speed = -speed;
                }
                position += speed;
            }
            else if (this.type == PlatfromType.MovableVer)
            {
                if (position.Y <= topBound || position.Y >= bottomBound)
                {
                    speed = -speed;
                }
                position += speed;
            }

            base.Update(gameTime);
        }

        public void UpdatePosition(float positionY)
        {
            this.position.Y = positionY;
            this.position.X = randomNumGen.Next(0, (int)Shared.Stage.X - texture.Width);

            topBound = position.Y - MOVE_RANGE;
            bottomBound = position.Y + MOVE_RANGE;
        }

        public void UpdateBounds(int movement)
        {
            topBound += movement;
            bottomBound += movement;
        }
    }
}
