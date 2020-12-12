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
    public enum MonsterStatus
    {
        None,
        Won,
        Defeated,
        Shooted
    }
    public class Monster : Sprite
    {
        private const float MIN_SCALE = .9f;
        private const float MAX_SCALE = 1f;
        private const float INIT_SCALE_CHANGE = .03f;

        private const float MIN_ROTATION = -.1f;
        private const float MAX_ROTATION = .1f;
        private const float INIT_ROTATION_CHANGE = .001f;
        private const int FALL_SPEED = 10;
        private const int SPEED = 1;

        private float scaleChange = INIT_SCALE_CHANGE;
        private float rotationChange = INIT_ROTATION_CHANGE;

        private Rectangle destRectangle;
        private Rectangle srcRectange;
        private Vector2 origin;
        public float Scale { get; set; } = 1.0f;
        public float Rotation { get; set; }
        public MonsterStatus Status { get; set; } = MonsterStatus.None;
        public Monster(Game game, SpriteBatch spriteBatch, Texture2D texture, Vector2 position) : base(game, spriteBatch, texture)
        {
            this.position = position;

            this.destRectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width * 2, texture.Height);
            this.srcRectange = new Rectangle(0, 0, texture.Width, texture.Height);
            this.origin = Vector2.Zero;

            this.speed = new Vector2(SPEED, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, position, srcRectange, Color.White, Rotation, origin, Scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if(position.X + texture.Width >= Shared.Stage.X || position.X <= 0)
            {
                speed = -speed;
            }
            position += speed;

            Rotation += rotationChange;
            Scale += scaleChange;
            if (Scale > MAX_SCALE || Scale < MIN_SCALE)
            {
                scaleChange = -scaleChange;
            }
            if (Rotation > MAX_ROTATION || Rotation < MIN_ROTATION)
            {
                rotationChange = -rotationChange;
            }

            if(Status == MonsterStatus.Defeated)
            {
                position.Y += FALL_SPEED;
            }

            base.Update(gameTime);
        }
    }
}
