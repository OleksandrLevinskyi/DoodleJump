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
using Microsoft.Xna.Framework.Media;

namespace DoodleJump.Scenes
{
    public class ActionScene : GameScene
    {
        private int counter = 0;
        private int platformGap = 70;
        private int topIdx;

        private bool isGameOver = false;
        private bool isSongPlaying = false;

        private const int MOVEMENT = 5;
        private const int FALL = 40;
        private const int PLATFORM_COUNT = 15;
        private const int INIT_START_POINT = 20;
        private const int GAME_OVER_DURATION = 60;
        private const int MAX_PLATFORM_GAP = 70;

        private Song gameOverSong;
        private Doodle doodle;
        private static Random randomNumGen = new Random();

        private List<Platform> platforms;
        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            platforms = new List<Platform>();
            topIdx = PLATFORM_COUNT - 1;
            gameOverSong = game.Content.Load<Song>("Sounds/game_over");

            // to remove glitching on fall
            MediaPlayer.Play(gameOverSong);
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Pause();

            Texture2D doodleTexture = game.Content.Load<Texture2D>("Images/doodle");
            doodle = new Doodle(game, spriteBatch, doodleTexture);
            this.Components.Add(doodle);

            Texture2D platformTexture = game.Content.Load<Texture2D>("Images/platform");
            SoundEffect doodlePlatformHitSound = game.Content.Load<SoundEffect>("Sounds/jump");
            for (int i = 0; i < PLATFORM_COUNT; i++)
            {
                platformGap = randomNumGen.Next(platformTexture.Height, MAX_PLATFORM_GAP);
                float positionY = Shared.Stage.Y - INIT_START_POINT - platformGap * i;
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
            if (!isGameOver)
            {
                MovePlatforms();

                Console.WriteLine(doodle.Score);

                if (doodle.GetLowerBound() >= Shared.Stage.Y)
                {
                    isGameOver = true;
                }
            }
            else
            {
                if (!isSongPlaying)
                {
                    MediaPlayer.Resume();
                    isSongPlaying = true;
                }

                if (counter <= GAME_OVER_DURATION)
                {
                    counter++;
                    FallDoodle();
                }
                else if (doodle.Position.Y >= Shared.Stage.Y)
                {
                    doodle.Dispose();
                }
            }

            base.Update(gameTime);
        }

        private void MovePlatforms()
        {
            if (doodle.Position.Y <= Shared.Stage.Y / 2)
            {
                doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y + MOVEMENT);
                doodle.Score += MOVEMENT;

                for (int i = 0; i < platforms.Count; i++)
                {
                    platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y + MOVEMENT);
                    // if platform is out of the screen
                    if (platforms[i].Position.Y >= Shared.Stage.Y)
                    {
                        platforms[i].UpdatePosition(platforms[topIdx].Position.Y - platformGap);
                        topIdx = i;
                    }
                }
            }
        }


        private void FallDoodle()
        {
            doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y - FALL);
            for (int i = 0; i < platforms.Count; i++)
            {
                platformGap = randomNumGen.Next(platforms[i].Texture.Height, MAX_PLATFORM_GAP);
                platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y - FALL);
            }
        }

        private void AddScore()
        {
            this.
        }
    }
}
