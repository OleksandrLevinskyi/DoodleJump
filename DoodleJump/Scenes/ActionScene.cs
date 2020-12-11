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
using DoodleJump.Components;
using DoodleJump.Components.ActionScene;

namespace DoodleJump.Scenes
{
    public class ActionScene : GameScene
    {
        private float score = 0;
        private float oldPosition = 0; // old Y position of the doodle
        private int counter = 0;
        private int platformGap = 70;
        private int topIdx = 0; // index of the top-most platform in the platfroms list

        private bool isGameOver = false;
        private bool isSongPlaying = false;
        private bool isStarted = false; // indicate if the doodle started jumping

        private const int MOVEMENT = 5;
        private const int FALL = 40;
        private const int PLATFORM_COUNT = 20;
        private const int INIT_START_POINT = 20;
        private const int GAME_OVER_DURATION = 60;
        private const int GAP_DIFF = 10;
        private const int INIT_PLATFORM_GAP = 60; // initial maximum gap between platforms
        private const int BULLET_COUNT = 5;
        private const double COEFFICIENT = 1.000025;
        private const int MAX_CHANGE_BOUND = 1000;

        private double maxPlatfromGap = 0;

        private Song gameOverSong;
        private Song bulletSound;
        private Doodle doodle;
        private TopBar topbar;
        private static Random randomNumGen = new Random();

        private List<Platform> platforms;
        private List<Bullet> bullets;
        private int bulletIdx = 0;
        private KeyboardState oldState;

        public bool ShowGameOver { get; set; }

        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            platforms = new List<Platform>();
            bullets = new List<Bullet>();
            gameOverSong = game.Content.Load<Song>("Sounds/game_over");
            bulletSound = game.Content.Load<Song>("Sounds/bullet");

            // to remove glitching on fall
            //MediaPlayer.Play(gameOverSong);
            //MediaPlayer.IsRepeating = false;
            //MediaPlayer.Pause();

            // create a doodle
            Texture2D doodleTextureRight = game.Content.Load<Texture2D>("Images/doodle_right");
            Texture2D doodleTextureLeft = game.Content.Load<Texture2D>("Images/doodle_left");
            Texture2D doodleTextureUp = game.Content.Load<Texture2D>("Images/doodle_up");
            Texture2D bulletTexture = game.Content.Load<Texture2D>("Images/bullet");
            doodle = new Doodle(game, spriteBatch, bulletTexture, doodleTextureRight, doodleTextureLeft, doodleTextureUp);

            // generate platforms & collision managers
            Texture2D platformTexture = game.Content.Load<Texture2D>("Images/platform");
            SoundEffect doodlePlatformHitSound = game.Content.Load<SoundEffect>("Sounds/jump");
            for (int i = 0; i < PLATFORM_COUNT; i++)
            {
                platformGap = randomNumGen.Next(INIT_PLATFORM_GAP - GAP_DIFF, INIT_PLATFORM_GAP);
                float positionY = i == 0 ? Shared.Stage.Y - INIT_START_POINT : platforms[topIdx].Position.Y - platformGap;
                Platform platform = new Platform(game, spriteBatch, platformTexture, positionY);
                topIdx = i;
                this.Components.Add(platform);

                DoodlePlatformColMng doodlePlatformColMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
                this.Components.Add(doodlePlatformColMng);

                platforms.Add(platform);
            }

            // put doodle on the first platform
            doodle.Position = new Vector2(platforms[0].Position.X + platforms[0].Texture.Width / 2 - doodle.Texture.Width / 2,
                platforms[0].Position.Y - doodle.Texture.Height);

            // add doodle the last one to make it front-most on the screen
            this.Components.Add(doodle);

            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            Texture2D topbarTexture = game.Content.Load<Texture2D>("Images/topbar");
            topbar = new TopBar(game, spriteBatch, topbarTexture, standardItemFont);
            this.Components.Add(topbar);

            for (int i = 0; i < BULLET_COUNT; i++)
            {
                Bullet bullet = new Bullet(game, spriteBatch, bulletTexture);
                bullets.Add(bullet);
                this.Components.Add(bullet);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!isGameOver)
            {
                if (!isStarted)
                {
                    oldPosition = doodle.Position.Y;
                    isStarted = true;
                }

                float diff = oldPosition - doodle.Position.Y;

                if (diff > 0)
                {
                    score += diff;
                    oldPosition = doodle.Position.Y;

                    topbar.Score = score;
                }

                KeyboardState ks = Keyboard.GetState();
                if (oldState.IsKeyUp(Keys.Space) && ks.IsKeyDown(Keys.Space))
                {
                    // manage bullets
                    bullets[bulletIdx].Position = new Vector2(doodle.Position.X + doodle.Texture.Width / 2, doodle.Position.Y);
                    bulletIdx++;
                    if (bulletIdx == bullets.Count)
                    {
                        bulletIdx = 0;
                    }

                    MediaPlayer.Play(bulletSound);
                }
                oldState = ks;

                MovePlatforms();

                if (doodle.GetLowerBound() >= Shared.Stage.Y)
                {
                    isGameOver = true;
                }
            }
            else
            {
                if (!isSongPlaying)
                {
                    MediaPlayer.Stop();
                    MediaPlayer.Play(gameOverSong);
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
                    ShowGameOver = true;
                }
            }

            base.Update(gameTime);
        }

        private void MovePlatforms()
        {
            if (doodle.Position.Y <= Shared.Stage.Y / 2)
            {
                doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y + MOVEMENT);
                oldPosition += MOVEMENT;

                for (int i = 0; i < platforms.Count; i++)
                {
                    platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y + MOVEMENT);
                    // if platform is out of the screen
                    if (platforms[i].Position.Y >= Shared.Stage.Y)
                    {
                        platforms[i] = GetNewPlatfrom(platforms[i]);
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
                platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y - FALL);
            }
        }

        private Platform GetNewPlatfrom(Platform currPlatform)
        {
            if (score <= MAX_CHANGE_BOUND)
            {
                maxPlatfromGap = INIT_PLATFORM_GAP * Math.Pow(COEFFICIENT, score - 1);
            }
            platformGap = randomNumGen.Next((int)maxPlatfromGap - GAP_DIFF, (int)maxPlatfromGap);
            currPlatform.UpdatePosition(platforms[topIdx].Position.Y - platformGap);
            Console.WriteLine(maxPlatfromGap);

            return currPlatform;
        }
    }
}
