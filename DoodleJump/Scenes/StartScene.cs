/*
 * StartScene.cs
 * First scene in the game
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
using DoodleJump.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DoodleJump.Scenes
{
    /// <summary>
    /// scene for game navigation
    /// </summary>
    public class StartScene : GameScene
    {
        private string[] menuItems = { "Start Game", "Help", "High Score", "About", "Quit" };

        private Song backgroundSong;
        private MenuComponent menu;

        public MenuComponent Menu { get => menu; set => menu = value; } // menu on the scene

        /// <summary>
        /// scene constructor, launches the background song
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        public StartScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            SpriteFont chosenItemFont = game.Content.Load<SpriteFont>("Fonts/chosenItemFont");

            backgroundSong = game.Content.Load<Song>("Sounds/background_music");
            MediaPlayer.Play(backgroundSong);
            MediaPlayer.IsRepeating = true;

            // load the background texture
            this.texture = game.Content.Load<Texture2D>("Images/start_scene");

            // add menu
            menu = new MenuComponent(game, spriteBatch, standardItemFont, chosenItemFont, menuItems);
            this.Components.Add(menu);
        }

        /// <summary>
        /// hides & disables the scene, pauses the background song
        /// </summary>
        public override void Hide()
        {
            MediaPlayer.Pause();
            base.Hide();
        }

        /// <summary>
        /// shows & enables the scene, starts playing the background song
        /// </summary>
        public override void Show()
        {
            MediaPlayer.Play(backgroundSong);
            base.Show();
        }
    }
}
