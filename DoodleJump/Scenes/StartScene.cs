using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DoodleJump.Scenes
{
    public class StartScene : GameScene
    {
        private string[] menuItems = { "Start Game", "Help", "High Score", "About", "Quit" };
        Song backgroundSong;
        private MenuComponent menu;
        public MenuComponent Menu { get => menu; set => menu = value; }

        public StartScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            backgroundSong = game.Content.Load<Song>("Sounds/background_music");
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;

            this.texture = game.Content.Load<Texture2D>("Images/start_scene");

            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            this.Components.Add(menu);
        }

        public override void Hide()
        {
            MediaPlayer.Pause();
            base.Hide();
        }

        public override void Show()
        {
            MediaPlayer.Play(backgroundSong);
            base.Show();
        }
    }
}
