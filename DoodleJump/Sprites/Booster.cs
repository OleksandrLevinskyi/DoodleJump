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
        Rocket
    }

    public class Booster : Sprite
    {
        private BoosterType type;
        private Platform masterPlatform;
        public BoosterType Type { get => type; set => type = value; }
        public Platform MasterPlatform { get => masterPlatform; set { masterPlatform = value; } }
        public bool IsUsed { get; set; }

        public Booster(Game game, SpriteBatch spriteBatch, Texture2D texture, BoosterType type) : base(game, spriteBatch, texture)
        {
            this.type = type;
        }

        public override void Update(GameTime gameTime)
        {
            if (masterPlatform != null)
            {
                this.position.Y = masterPlatform.Position.Y - texture.Height;
                this.position.X = masterPlatform.Position.X + (masterPlatform.Texture.Width - texture.Width) / 2;
            }
            else
            {
                position = new Vector2(-texture.Width, 0);
                IsUsed = false;
            }

            base.Update(gameTime);
        }
    }
}
