/*
 * GameScene.cs
 * Abstract class for all game scenes
 * 
 * Revision History
 *          Oleksandr Levinskyi, 2020.12.06: Created & Imlemented
 *          Oleksandr Levinskyi, 2020.12.13: Revised & Completed
 */

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
    /// <summary>
    /// abstract class with included scenes' common features, behaves similar to Game class
    /// </summary>
    public abstract class GameScene : DrawableGameComponent
    {
        protected Texture2D texture;
        protected SpriteBatch spriteBatch;

        private List<GameComponent> components;

        public List<GameComponent> Components { get => components; set => components = value; } // the list of components to be drawn

        /// <summary>
        /// scene constructor, hides the scene by default
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        protected GameScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            components = new List<GameComponent>();
            this.spriteBatch = spriteBatch;
            Hide();
        }

        /// <summary>
        /// draws the scene components if they are drawable
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.End();

            DrawableGameComponent drawGameComp = null;
            foreach (GameComponent gameComp in components)
            {
                if (gameComp is DrawableGameComponent)
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

        /// <summary>
        /// executes the Update function of each enabled game component
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
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

        /// <summary>
        /// hides & disables the scene
        /// </summary>
        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        /// <summary>
        /// shows & enables the scene
        /// </summary>
        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }
    }
}
