﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.Scenes
{
    public class AboutScene : GameScene
    {
        public AboutScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.texture = game.Content.Load<Texture2D>("Images/about_scene");
        }
    }
}