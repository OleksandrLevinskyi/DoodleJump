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
        private int platformGap = 70;
        private bool isGameOver = false;
        private const int PLATFORM_MOVEMENT = 3;
        private const int INIT_PLATFORM_COUNT = 15;
        private int platformCount;
        private int topIdx;

        private List<Platform> platforms;
        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            platforms = new List<Platform>();
            platformCount = INIT_PLATFORM_COUNT;
            topIdx = platformCount - 1;

            Texture2D doodleTexture = game.Content.Load<Texture2D>("Images/doodle");
            doodle = new Doodle(game, spriteBatch, doodleTexture);
            this.Components.Add(doodle);

            Texture2D platformTexture = game.Content.Load<Texture2D>("Images/platform");
            SoundEffect doodlePlatformHitSound = game.Content.Load<SoundEffect>("Sounds/click");
            for (int i = 0; i < platformCount; i++)
            {
                float positionY = Shared.Stage.Y - 20 - platformGap * i;
                Platform platform = new Platform(game, spriteBatch, platformTexture, positionY);
                this.Components.Add(platform);

                DoodlePlatformColMng doodlePlatformColMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
                this.Components.Add(doodlePlatformColMng);

                platforms.Add(platform);
            }

            // put doodle on the first platform
            doodle.Position = new Vector2(platforms[0].Position.X + platforms[0].Texture.Width / 2 - doodle.Texture.Width / 2,
                platforms[0].Position.Y - doodle.Texture.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            MovePlatforms();

            if (doodle.GetLowerBound() >= Shared.Stage.Y)
            {
                GameOver();
            }

            base.Update(gameTime);
        }

        private void MovePlatforms()
        {
            if (doodle.Position.Y <= Shared.Stage.Y / 2)
            {
                for (int i = 0; i < platforms.Count; i++)
                {
                    platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y + PLATFORM_MOVEMENT);
                    // if platform is out of the screen
                    if (platforms[i].Position.Y >= Shared.Stage.Y)
                    {
                        platforms[i].UpdatePosition(platforms[topIdx].Position.Y - platformGap);
                        topIdx = i;
                    }
                }
            }
        }

        private void GameOver()
        {
            Console.WriteLine("game over");
            isGameOver = true;
            //this.Components.Remove(doodle);
        }
    }
}
