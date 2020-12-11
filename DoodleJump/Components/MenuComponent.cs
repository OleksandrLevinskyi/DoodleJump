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
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont standardItemFont, chosenItemFont;
        private List<string> menuItems;
        private int selectedIdx;
        private Vector2 position;
        private Color standardItemColor = Color.Black;
        private Color chosenItemColor = Color.Red;
        private KeyboardState oldState;

        public int SelectedIdx { get => selectedIdx; set => selectedIdx = value; }
        public Vector2 Position { get => position; set => position = value; }

        public MenuComponent(Game game, SpriteBatch spriteBatch,
            SpriteFont standardItemFont, SpriteFont chosenItemFont,
            string[] menuItems) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.standardItemFont = standardItemFont;
            this.chosenItemFont = chosenItemFont;

            this.menuItems = menuItems.ToList<string>();
            position = new Vector2(Shared.Stage.X / 2, Shared.Stage.Y / 2);
        }

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
    }
}
