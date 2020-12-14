/*
 * InputManager.cs
 * Tool for inputting values
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
    /// tool that helps fetching input from the user
    /// </summary>
    public class InputManager : DrawableGameComponent
    {
        private const int MAX_NAME_LENGTH = 10;
        private const int ASCII_MIN = 97;
        private const int ASCII_MAX = 122;

        private string message = "";
        private bool disableUpdate = true;

        private BasicString basicString;
        private KeyboardState oldState;

        public string Message { get => message; set => message = value; } // message of the tool
        public bool DisableUpdate { get => disableUpdate; set => disableUpdate = value; } // mode of the tool (none/edit)
        public Vector2 Position { get; set; } // position of the tool

        /// <summary>
        /// constructor to create a tool
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        /// <param name="font">font type</param>
        /// <param name="color">color of the message</param>
        public InputManager(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Color color) : base(game)
        {
            basicString = new BasicString(game, spriteBatch, font, message, color);
        }

        /// <summary>
        /// draws the typed message
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            basicString.Draw(gameTime);

            base.Draw(gameTime);
        }

        /// <summary>
        /// fetches input from the user (lowercase letters allowed only, up to 10 chars) if in edit mode
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (!DisableUpdate)
            {
                // fetch the user input
                KeyboardState ks = Keyboard.GetState();

                Keys? key = ks.GetPressedKeys().FirstOrDefault();

                if (key != null && ks.IsKeyDown((Keys)key) && !oldState.IsKeyDown((Keys)key))
                {
                    string keyValue = key.ToString().ToLower();
                    if (message.Length < MAX_NAME_LENGTH && keyValue.Length == 1 &&
                        (int)keyValue[0] >= ASCII_MIN && (int)keyValue[0] <= ASCII_MAX &&
                            Char.IsLetter(keyValue[0]))
                    {
                        message += keyValue;
                    }
                    else if (key == Keys.Back && message.Length > 0)
                    {
                        message = message.Substring(0, message.Length - 1);
                    }
                }
                oldState = ks;
            }
            basicString.Message = message;
            basicString.Position = Position;

            base.Update(gameTime);
        }
    }
}
