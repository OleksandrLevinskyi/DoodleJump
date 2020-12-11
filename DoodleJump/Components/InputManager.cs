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
    public class InputManager : DrawableGameComponent
    {
        private string message = "";
        private BasicString basicString;
        private const int MAX_NAME_LENGTH = 10;
        private const int ASCII_MIN = 97;
        private const int ASCII_MAX = 122;
        private bool disableUpdate = true;

        private KeyboardState oldState;

        public string Message { get => message; set => message = value; }
        public bool DisableUpdate { get => disableUpdate; set => disableUpdate = value; }

        public InputManager(Game game,
            SpriteBatch spriteBatch,
            SpriteFont font,
            Vector2 position,
            Color color) : base(game)
        {
            basicString = new BasicString(game, spriteBatch, font, message, color);
            basicString.Position = position;
        }

        public override void Draw(GameTime gameTime)
        {
            basicString.Draw(gameTime);

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!DisableUpdate)
            {
                KeyboardState ks = Keyboard.GetState();

                Keys? key = ks.GetPressedKeys().FirstOrDefault();

                if (key != null && ks.IsKeyDown((Keys)key) && !oldState.IsKeyDown((Keys)key))
                {
                    string keyValue = key.ToString().ToLower();
                    if (message.Length <= MAX_NAME_LENGTH && keyValue.Length==1 &&
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

            base.Update(gameTime);
        }
    }
}
