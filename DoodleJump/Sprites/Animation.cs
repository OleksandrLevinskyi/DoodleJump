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
    public class Animation : Sprite
    {
        private Vector2 dimension; // size of the frame
        private List<Rectangle> frames;
        private int frameIdx = -1;
        private int delay; // time between frame update
        private int delayCounter = 0; // helper variable to manage delay

        private int rows = 0;
        private int cols = 0;

        public Animation(Game game, SpriteBatch spriteBatch,
            Texture2D texture, int rows, int cols, int delay) : base(game, spriteBatch, texture)
        {
            this.rows = rows;
            this.cols = cols;
            this.delay = delay;

            this.position = Vector2.Zero;

            dimension = new Vector2(texture.Width / cols, texture.Height / rows);

            this.Hide(); // hide

            CreateFrames(); // create frames
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (frameIdx >= 0)
            {
                spriteBatch.Draw(texture, position, frames[frameIdx], Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIdx++;
                if (frameIdx > rows * cols - 1)
                {
                    frameIdx = -1;
                    Hide();
                }
                delayCounter = 0;
            }

            base.Update(gameTime);
        }

        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int x = j * (int)dimension.X;
                    int y = i * (int)dimension.Y;
                    Rectangle r = new Rectangle(x, y, (int)dimension.X, (int)dimension.Y);
                    frames.Add(r);
                }
            }
        }

        public void Start()
        {
            this.Visible = true;
            this.Enabled = true;
            frameIdx = -1;
        }

        public void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }
    }
}
