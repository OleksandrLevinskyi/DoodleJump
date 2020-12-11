using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump
{
    public abstract class GameScene : DrawableGameComponent
    {
        private List<GameComponent> components;
        protected SpriteBatch spriteBatch;

        public List<GameComponent> Components { get => components; set => components = value; }

        protected GameScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            components = new List<GameComponent>();
            this.spriteBatch = spriteBatch;
            Hide();
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent drawGameComp = null;
            foreach (GameComponent gameComp in components)
            {
                if(gameComp is DrawableGameComponent)
                {
                    drawGameComp = (DrawableGameComponent)gameComp;
                    if (drawGameComp.Enabled)
                    {
                        drawGameComp.Draw(gameTime);
                    }
                }
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent gameComp in components)
            {
                if (gameComp.Enabled)
                {
                    gameComp.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }
    }
}
