using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Scenes
{
    public class StartScene : GameScene
    {
        private string[] menuItems = { "Start Game", "Help", "High Score", "Store", "Quit" };
        private MenuComponent menu;
        public MenuComponent Menu { get => menu; set => menu = value; }

        public StartScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            this.Components.Add(menu);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
