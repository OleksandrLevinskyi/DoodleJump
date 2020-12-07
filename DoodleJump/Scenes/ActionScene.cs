using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Sprites;
using DoodleJump.CollisionManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace DoodleJump.Scenes
{
    public class ActionScene : GameScene
    {
        private Doodle doodle;
        private Platform platform;
        private DoodlePlatformColMng doodlePlatformColMng;
        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            Texture2D doodleTexture = game.Content.Load<Texture2D>("Images/doodle");
            doodle = new Doodle(game, spriteBatch, doodleTexture);
            this.Components.Add(doodle);

            Texture2D platformTexture = game.Content.Load<Texture2D>("Images/platform");
            platform = new Platform(game, spriteBatch, platformTexture);
            this.Components.Add(platform);

            SoundEffect doodlePlatformHitSound = game.Content.Load<SoundEffect>("Sounds/click");
            doodlePlatformColMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
            this.Components.Add(doodlePlatformColMng);
        }
    }
}
