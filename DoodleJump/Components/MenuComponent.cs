/*
 * MenuComponent.cs
 * Tool for creating Menus
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

namespace DoodleJump.Components
{
    /// <summary>
    /// tool that helps creating interactive menus
    /// </summary>
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont standardItemFont, chosenItemFont;
        private Vector2 position;
        private KeyboardState oldState;

        private Color standardItemColor = Color.Black;
        private Color chosenItemColor = Color.Red;

        private List<string> menuItems;

        private int selectedIdx;

        public int SelectedIdx { get => selectedIdx; set => selectedIdx = value; } // currently selected item's index in the menu 
        public Vector2 Position { get => position; set => position = value; } // position of the tool

        /// <summary>
        /// constructor to create a tool
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="standardItemFont"></param>
        /// <param name="chosenItemFont"></param>
        /// <param name="menuItems"></param>
        public MenuComponent(Game game, SpriteBatch spriteBatch,
            SpriteFont standardItemFont, SpriteFont chosenItemFont,
            string[] menuItems) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.standardItemFont = standardItemFont;
            this.chosenItemFont = chosenItemFont;

            this.menuItems = menuItems.ToList<string>();
            position = new Vector2(Shared.Stage.X / 2, Shared.Stage.Y / 2);

            this.Show();
        }

        /// <summary>
        /// draws a menu with the currently selected item highlighted
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPosition = position;

            spriteBatch.Begin();
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (selectedIdx == i)
                {
                    spriteBatch.DrawString(chosenItemFont, menuItems[i], tempPosition, chosenItemColor);
                    tempPosition.Y += chosenItemFont.LineSpacing;
                }
                else
                {
                    spriteBatch.DrawString(standardItemFont, menuItems[i], tempPosition, standardItemColor);
                    tempPosition.Y += standardItemFont.LineSpacing;
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// updates the selected index based on the user input
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
            {
                selectedIdx++;
                if (selectedIdx == menuItems.Count)
                {
                    selectedIdx = 0;
                }
            }

            if (ks.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
            {
                selectedIdx--;
                if (selectedIdx == -1)
                {
                    selectedIdx = menuItems.Count - 1;
                }
            }

            oldState = ks;

            base.Update(gameTime);
        }

        /// <summary>
        /// hides & disables the menu
        /// </summary>
        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }

        /// <summary>
        /// shows & enables the menu
        /// </summary>
        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }
    }
}
