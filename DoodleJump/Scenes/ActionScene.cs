﻿using System;
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
        private const int SPRITE_COUNT = 5;
        private const double COEFFICIENT = 1.000035;
        private const int MAX_CHANGE_BOUND = 20000;
        private const int SPRING_ANIM_ROWS = 2;
        private const int SPRING_ANIM_COLS = 5;
        private const int WOOD_ANIM_ROWS = 2;
        private const int WOOD_ANIM_COLS = 3;
        private const int DIS_ANIM_DIM = 4;
        private const int ANIMATION_DELAY = 5;
        private const int SPEED = 1;

        private double maxPlatfromGap = 0;

        private int bulletIdx = 0;
        private KeyboardState oldState;
        private Game game;
        private Doodle doodle;
        private Booster booster;
        private TopBar topbar;
        private static Random randomNumGen = new Random();

        private DoodleBoosterColMng doodleBoosterColMng;
        private List<Platform> platforms;
        private List<DoodlePlatformColMng> doodlePlatformColMngs;
        private List<Bullet> bullets;

        private SoundEffect gameOverSound;
        private SoundEffect bulletSound;
        private SoundEffect doodlePlatformHitSound;
        private SoundEffect doodleWoodenPlatformHitSound;
        private SoundEffect doodleDisappearingPlatformHitSound;

        private SoundEffect springSound;

        private Animation springAnim;
        private Animation woodPlatAnim;
        private Animation disapPlatAnim;

        private Texture2D ordinaryPlatformTexture;
        private Texture2D movingHorPlatformTexture;
        private Texture2D woodenPlatformTexture;
        private Texture2D movingVerPlatformTexture;
        private Texture2D disappearingPlatformTexture;

        private Texture2D springTexture;
        private Texture2D rocketTexture;
        private Texture2D doodleRocketTexture;

        private Texture2D monsterTexture;

        public bool ShowGameOver { get; set; }

        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            this.game = game;
            platforms = new List<Platform>();
            doodlePlatformColMngs = new List<DoodlePlatformColMng>();
            bullets = new List<Bullet>();

            gameOverSound = game.Content.Load<SoundEffect>("Sounds/game_over");
            bulletSound = game.Content.Load<SoundEffect>("Sounds/bullet");
            doodlePlatformHitSound = game.Content.Load<SoundEffect>("Sounds/jump");
            doodleWoodenPlatformHitSound = game.Content.Load<SoundEffect>("Sounds/wood_platform_break");
            doodleDisappearingPlatformHitSound = game.Content.Load<SoundEffect>("Sounds/disappearing_platform");

            ordinaryPlatformTexture = game.Content.Load<Texture2D>("Images/platform");
            movingHorPlatformTexture = game.Content.Load<Texture2D>("Images/movingHor_platform");
            woodenPlatformTexture = game.Content.Load<Texture2D>("Images/wooden_platform");
            movingVerPlatformTexture = game.Content.Load<Texture2D>("Images/movingVer_platform");
            disappearingPlatformTexture = game.Content.Load<Texture2D>("Images/disappearing_platform");

            monsterTexture = game.Content.Load<Texture2D>("Images/monster");

            rocketTexture = game.Content.Load<Texture2D>("Images/rocket");
            doodleRocketTexture = game.Content.Load<Texture2D>("Images/doodle_rocket");

            // create a doodle
            Texture2D doodleTextureRight = game.Content.Load<Texture2D>("Images/doodle_right");
            Texture2D doodleTextureLeft = game.Content.Load<Texture2D>("Images/doodle_left");
            Texture2D doodleTextureUp = game.Content.Load<Texture2D>("Images/doodle_up");
            doodle = new Doodle(game, spriteBatch, doodleTextureRight, doodleTextureLeft, doodleTextureUp);

            // generate platforms & collision managers
            for (int i = 0; i < PLATFORM_COUNT; i++)
            {
                platformGap = randomNumGen.Next(INIT_PLATFORM_GAP - GAP_DIFF, INIT_PLATFORM_GAP);
                float positionY = i == 0 ? Shared.Stage.Y - INIT_START_POINT : platforms[topIdx].Position.Y - platformGap;
                Platform platform = new Platform(game, spriteBatch, ordinaryPlatformTexture, positionY);
                topIdx = i;
                this.Components.Add(platform);

                DoodlePlatformColMng doodlePlatformColMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
                this.Components.Add(doodlePlatformColMng);

                platforms.Add(platform);
                doodlePlatformColMngs.Add(doodlePlatformColMng);
            }

            Texture2D bulletTexture = game.Content.Load<Texture2D>("Images/bullet");
            // generate bullets
            for (int i = 0; i < SPRITE_COUNT; i++)
            {
                Bullet bullet = new Bullet(game, spriteBatch, bulletTexture);
                bullets.Add(bullet);
                this.Components.Add(bullet);
            }

            // spring booster
            springTexture = game.Content.Load<Texture2D>("Images/spring");
            booster = new Booster(game, spriteBatch, springTexture, BoosterType.Spring);
            this.Components.Add(booster);

            Texture2D springSpriteSheet = game.Content.Load<Texture2D>("Images/spring_spritesheet");
            springAnim = new Animation(game, spriteBatch, springSpriteSheet, SPRING_ANIM_ROWS, SPRING_ANIM_COLS, ANIMATION_DELAY);
            this.Components.Add(springAnim);

            springSound = game.Content.Load<SoundEffect>("Sounds/spring");
            doodleBoosterColMng = new DoodleBoosterColMng(game, doodle, springSound, booster);
            doodleBoosterColMng.BoosterAnimation = springAnim;
            this.Components.Add(doodleBoosterColMng);

            // animations
            Texture2D woodenPlatformSpriteSheet = game.Content.Load<Texture2D>("Images/wooden_platfrom_spritesheet");
            woodPlatAnim = new Animation(game, spriteBatch, woodenPlatformSpriteSheet, WOOD_ANIM_ROWS, WOOD_ANIM_COLS, ANIMATION_DELAY);
            this.Components.Add(woodPlatAnim);

            Texture2D disappearingPlatformSpriteSheet = game.Content.Load<Texture2D>("Images/disappearing_platform_spritesheet");
            disapPlatAnim = new Animation(game, spriteBatch, disappearingPlatformSpriteSheet, DIS_ANIM_DIM, DIS_ANIM_DIM, ANIMATION_DELAY);
            this.Components.Add(disapPlatAnim);

            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            Texture2D topbarTexture = game.Content.Load<Texture2D>("Images/topbar");
            topbar = new TopBar(game, spriteBatch, topbarTexture, standardItemFont);
            this.Components.Add(topbar);

            // put doodle on the first platform
            doodle.Position = new Vector2(platforms[0].Position.X + platforms[0].Texture.Width / 2 - doodle.Texture.Width / 2,
                platforms[0].Position.Y - doodle.Texture.Height);

            // add doodle the last one to make it front-most on the screen
            this.Components.Add(doodle);
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

                    bulletSound.Play();
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
                    gameOverSound.Play();
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
                woodPlatAnim.Position = new Vector2(woodPlatAnim.Position.X, woodPlatAnim.Position.Y + MOVEMENT);
                disapPlatAnim.Position = new Vector2(disapPlatAnim.Position.X, disapPlatAnim.Position.Y + MOVEMENT);
                oldPosition += MOVEMENT;

                for (int i = 0; i < platforms.Count; i++)
                {
                    if (platforms[i].Type == PlatfromType.MovableVer)
                    {
                        platforms[i].UpdateBounds(MOVEMENT);
                    }
                    platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y + MOVEMENT);
                }
            }
            for (int i = 0; i < platforms.Count; i++)
            {
                // if platform is out of the screen
                if (platforms[i].Position.Y + platforms[i].Texture.Height >= Shared.Stage.Y)
                {
                    PlatfromType oldType = platforms[i].Type;
                    platforms[i] = GetNewPlatfrom(platforms[i]);
                    doodlePlatformColMngs[i] = GetNewColMng(platforms[i], i, oldType);
                    topIdx = i;
                }
            }
        }

        private void FallDoodle()
        {
            doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y - FALL);
            woodPlatAnim.Position = new Vector2(woodPlatAnim.Position.X, woodPlatAnim.Position.Y - FALL);
            disapPlatAnim.Position = new Vector2(disapPlatAnim.Position.X, disapPlatAnim.Position.Y - FALL);
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y - FALL);
            }
        }

        private Platform GetNewPlatfrom(Platform currPlatform)
        {
            // adjust platfrom gaps
            if (score <= MAX_CHANGE_BOUND)
            {
                maxPlatfromGap = INIT_PLATFORM_GAP * Math.Pow(COEFFICIENT, score - 1);
            }
            platformGap = randomNumGen.Next((int)maxPlatfromGap - GAP_DIFF, (int)maxPlatfromGap);

            currPlatform.UpdatePosition(platforms[topIdx].Position.Y - platformGap);

            // adjust platform type
            currPlatform.Type = GetNewPlatformType();
            currPlatform.Speed = Vector2.Zero;

            // check boosters
            if(booster.MasterPlatform == currPlatform)
            {
                booster.IsUsed = false;
                booster.MasterPlatform = null;
            }

            switch (currPlatform.Type)
            {
                case PlatfromType.Original:
                    currPlatform.Texture = ordinaryPlatformTexture;
                    if (!booster.IsUsed)
                    {
                        booster.MasterPlatform = currPlatform;//////////////////////////////////////////////
                        booster.IsUsed = true;
                    }
                    break;
                case PlatfromType.MovableHor:
                    currPlatform.Texture = movingHorPlatformTexture;
                    currPlatform.Speed = new Vector2(SPEED, 0);
                    break;
                case PlatfromType.MovableVer:
                    currPlatform.Texture = movingVerPlatformTexture;
                    currPlatform.Speed = new Vector2(0, SPEED);
                    break;
                case PlatfromType.Wooden:
                    currPlatform.Texture = woodenPlatformTexture;
                    break;
                case PlatfromType.Disappearing:
                    currPlatform.Texture = disappearingPlatformTexture;
                    break;
                default:
                    break;
            }

            return currPlatform;
        }

        private DoodlePlatformColMng GetNewColMng(Platform platform, int idx, PlatfromType oldType)
        {
            DoodlePlatformColMng colMng = doodlePlatformColMngs[idx];
            if (oldType != platform.Type)
            {
                this.Components.Remove(doodlePlatformColMngs[idx]);
                switch (platform.Type)
                {
                    case PlatfromType.Wooden:
                        colMng = new DoodleAnimatedPlatfromColMng(game, doodle, platform, doodleWoodenPlatformHitSound, woodPlatAnim);
                        break;
                    case PlatfromType.Disappearing:
                        colMng = new DoodleAnimatedPlatfromColMng(game, doodle, platform, doodleDisappearingPlatformHitSound, disapPlatAnim);
                        break;
                    default:
                        colMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
                        break;
                }
                this.Components.Add(colMng);
            }
            return colMng;
        }

        private PlatfromType GetNewPlatformType()
        {
            int typeIdx;
            List<PlatfromType> possiblePlatfromTypes = new List<PlatfromType>();

            // all heights - allow original
            possiblePlatfromTypes.Add(PlatfromType.Original);
            possiblePlatfromTypes.Add(PlatfromType.MovableHor);

            // up to 10k in score & the previous one is not wooden - allow breaking
            if (MAX_CHANGE_BOUND / 2 >= score && platforms[topIdx].Type != PlatfromType.Wooden)
            {
                possiblePlatfromTypes.Add(PlatfromType.Wooden);
            }

            // if above 10k - allow movable & disappearing
            if (score >= MAX_CHANGE_BOUND / 2)
            {
                possiblePlatfromTypes.Add(PlatfromType.MovableVer);
                possiblePlatfromTypes.Add(PlatfromType.Disappearing);
            }

            typeIdx = randomNumGen.Next(0, possiblePlatfromTypes.Count);

            return possiblePlatfromTypes[typeIdx];
        }
    }
}
