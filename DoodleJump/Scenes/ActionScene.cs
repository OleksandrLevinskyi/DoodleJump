/*
 * ActionScene.cs
 * Scene for playing the game
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
using DoodleJump.Sprites;
using DoodleJump.CollisionManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using DoodleJump.Components;

namespace DoodleJump.Scenes
{
    /// <summary>
    /// scene for playing the game
    /// </summary>
    public class ActionScene : GameScene
    {
        private const int MOVEMENT = 5; // when doodle moves up
        private const int FALL = 40; // to adjust fall speed
        private const int PLATFORM_COUNT = 20; // number of platforms to be created
        private const int INIT_START_POINT = 30; // start point of the first generated platform
        private const int GAME_OVER_DURATION = 60; // delay before showing game over scene
        private const int GAP_DIFF = 10; // initial gap difference between platfroms
        private const int INIT_PLATFORM_GAP = 60; // initial maximum gap between platforms
        private const int SPRITE_COUNT = 5; // number of bullets
        private const int MAX_CHANGE_BOUND = 20000; // bound where platform gap geometric progression stops to ensure playable game
        private const int PLATFORM_CHANGE_BOUND = 5000; // bound where platform types get switched
        private const int SPRING_ANIM_ROWS = 2; // rows in spring animation
        private const int SPRING_ANIM_COLS = 5; // columns in spring animation
        private const int WOOD_ANIM_ROWS = 2; // rows in wooden platform animation
        private const int WOOD_ANIM_COLS = 3; // columns in wooden platform animation
        private const int DIS_ANIM_DIM = 4; // rows & columns in disappearing platform animation
        private const int ANIMATION_DELAY = 5; // animation delay
        private const int SPEED = 1; // speed of the moving platforms & parallax
        private const int MONSTER_GAP = 3000; // height difference to generate a new monster
        private const float PARALLAX_DIFF = 1.5f; // parallax speed difference
        private const double COEFFICIENT = 1.000035; // coefficient used in platform gap geometric progression

        private int bulletIdx = 0;
        private int counter = 0;
        private int platformGap = 70;
        private int topIdx = 0; // index of the top-most platform in the platfroms list
        private float score = 0;
        private float oldPosition = 0; // old Y position of the doodle
        private double maxPlatfromGap = 0;

        private bool isGameOver = false;
        private bool isSongPlaying = false;
        private bool isStarted = false; // indicate if the doodle started jumping

        private KeyboardState oldState;
        private Game game;
        private Doodle doodle;
        private Booster booster;
        private TopBar topbar;
        private Monster monster;

        private DoodleBoosterColMng doodleBoosterColMng;
        private DoodleMonsterColMng doodleMonsterColMng;
        private List<DoodlePlatformColMng> doodlePlatformColMngs;
        private List<MonsterBulletColMng> monsterBooletColMngs;

        private List<Platform> platforms;
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
        private Texture2D monsterTexture;

        private static Random randomNumGen = new Random();

        public bool ShowGameOver { get; set; } = false; // whether to show game over screen
        public float Score { get => score; } // getter for a score

        /// <summary>
        /// scene constructor, loads & initializes all the necessary components
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="spriteBatch">spriteBatch for drawing</param>
        public ActionScene(Game game, SpriteBatch spriteBatch) : base(game, spriteBatch)
        {
            // set the background
            this.texture = game.Content.Load<Texture2D>("Images/background");

            // create parallax
            Texture2D cloudsBottomTexture = game.Content.Load<Texture2D>("Images/clouds_bottom");
            Vector2 speedBottom = new Vector2(SPEED, 0);
            ParallaxBackground layerBottom = new ParallaxBackground(game, spriteBatch, cloudsBottomTexture, speedBottom);
            this.Components.Add(layerBottom);

            Texture2D cloudsTopTexture = game.Content.Load<Texture2D>("Images/clouds_top");
            Vector2 speedTop = new Vector2(SPEED * PARALLAX_DIFF, 0);
            ParallaxBackground layerTop = new ParallaxBackground(game, spriteBatch, cloudsTopTexture, speedTop);
            this.Components.Add(layerTop);

            // initialize values
            this.game = game;
            platforms = new List<Platform>();
            doodlePlatformColMngs = new List<DoodlePlatformColMng>();
            monsterBooletColMngs = new List<MonsterBulletColMng>();
            bullets = new List<Bullet>();

            // load necessary content
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

            // create a doodle
            Texture2D doodleTextureRight = game.Content.Load<Texture2D>("Images/doodle_right");
            Texture2D doodleTextureLeft = game.Content.Load<Texture2D>("Images/doodle_left");
            Texture2D doodleTextureUp = game.Content.Load<Texture2D>("Images/doodle_up");
            doodle = new Doodle(game, spriteBatch, doodleTextureRight, doodleTextureLeft, doodleTextureUp);

            // generate platforms & collision managers
            for (int i = 0; i < PLATFORM_COUNT; i++)
            {
                // platforms
                platformGap = randomNumGen.Next(INIT_PLATFORM_GAP - GAP_DIFF, INIT_PLATFORM_GAP);
                float positionY = i == 0 ? Shared.Stage.Y - INIT_START_POINT : platforms[topIdx].Position.Y - platformGap;
                Platform platform = new Platform(game, spriteBatch, ordinaryPlatformTexture, positionY);
                topIdx = i;
                this.Components.Add(platform);

                // collision managers
                DoodlePlatformColMng doodlePlatformColMng = new DoodlePlatformColMng(game, doodle, platform, doodlePlatformHitSound);
                this.Components.Add(doodlePlatformColMng);

                // add to their appropriate lists
                platforms.Add(platform);
                doodlePlatformColMngs.Add(doodlePlatformColMng);
            }

            // monster
            monsterTexture = game.Content.Load<Texture2D>("Images/monster");
            monster = new Monster(game, spriteBatch, monsterTexture, new Vector2(Shared.Stage.X / 2, MONSTER_GAP));
            this.Components.Add(monster);

            SoundEffect monsterHitSound = game.Content.Load<SoundEffect>("Sounds/monster_crash"); // if doodle hits the monster
            SoundEffect monsterDefeatSound = game.Content.Load<SoundEffect>("Sounds/monster_defeat"); // if doodle jumps on the monster
            Song monsterCloseSound = game.Content.Load<Song>("Sounds/monster_close"); // if monster is nearby
            doodleMonsterColMng = new DoodleMonsterColMng(game, doodle, monsterHitSound, monsterDefeatSound, monsterCloseSound, monster);
            this.Components.Add(doodleMonsterColMng);

            // bullets
            Texture2D bulletTexture = game.Content.Load<Texture2D>("Images/bullet");
            // generate bullets
            for (int i = 0; i < SPRITE_COUNT; i++)
            {
                Bullet bullet = new Bullet(game, spriteBatch, bulletTexture);
                bullets.Add(bullet);
                this.Components.Add(bullet);

                MonsterBulletColMng monsterBooletColMng = new MonsterBulletColMng(game, monster, monsterDefeatSound, bullet);
                monsterBooletColMngs.Add(monsterBooletColMng);
                this.Components.Add(monsterBooletColMng);
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

            // put doodle on the first platform
            doodle.Position = new Vector2(platforms[0].Position.X + platforms[0].Texture.Width / 2 - doodle.Texture.Width / 2,
                platforms[0].Position.Y - doodle.Texture.Height);

            // add doodle one of the last to make it placed on the front
            this.Components.Add(doodle);

            // topbar
            SpriteFont standardItemFont = game.Content.Load<SpriteFont>("Fonts/standardItemFont");
            Texture2D topbarTexture = game.Content.Load<Texture2D>("Images/topbar");
            topbar = new TopBar(game, spriteBatch, topbarTexture, standardItemFont);
            this.Components.Add(topbar);
        }

        /// <summary>
        /// manages the scene and its components
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            // if game is not over
            if (!isGameOver)
            {
                // if game is started
                if (!isStarted)
                {
                    oldPosition = doodle.Position.Y;
                    isStarted = true;
                }

                // logic to calculate the score
                float diff = oldPosition - doodle.Position.Y;

                if (diff > 0)
                {
                    score += diff;
                    oldPosition = doodle.Position.Y;

                    topbar.Score = (int)score;
                }

                // shoot bullets
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

                // adjust sprites' positions
                MoveSprites();

                // if doodle got hit by the monster
                if (monster.Status == MonsterStatus.Won)
                {
                    DisableAllColMngs();
                }

                // if doodle has fallen
                if (doodle.GetLowerBound() >= Shared.Stage.Y)
                {
                    isGameOver = true;
                    DisableAllColMngs();
                }
            }
            else // if game is over
            {
                // play the sound once
                if (!isSongPlaying)
                {
                    gameOverSound.Play();
                    isSongPlaying = true;
                }

                // make doodle fall & disappear
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

        /// <summary>
        /// adjusts position of all sprites if the doodle goes over the middle of the screen
        /// </summary>
        private void MoveSprites()
        {
            if (doodle.Position.Y <= Shared.Stage.Y / 2)
            {
                doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y + MOVEMENT);
                woodPlatAnim.Position = new Vector2(woodPlatAnim.Position.X, woodPlatAnim.Position.Y + MOVEMENT);
                disapPlatAnim.Position = new Vector2(disapPlatAnim.Position.X, disapPlatAnim.Position.Y + MOVEMENT);
                springAnim.Position = new Vector2(springAnim.Position.X, springAnim.Position.Y + MOVEMENT);
                monster.Position = new Vector2(monster.Position.X, monster.Position.Y + MOVEMENT);
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
                // if platform is out of the screen, renew it
                if (platforms[i].Position.Y + platforms[i].Texture.Height >= Shared.Stage.Y)
                {
                    PlatfromType oldType = platforms[i].Type;
                    platforms[i] = GetNewPlatfrom(platforms[i]);
                    doodlePlatformColMngs[i] = GetNewColMng(platforms[i], i, oldType);
                    topIdx = i;
                }
            }
            // if monster is out of the screen, renew it
            if (monster.Position.Y + monster.Texture.Height >= Shared.Stage.Y || monster.Status == MonsterStatus.Shooted)
            {
                monster.Position = new Vector2(monster.Position.X, monster.Position.Y - MONSTER_GAP);
                monster.Status = MonsterStatus.None;
                doodleMonsterColMng.PauseSong();
            }
        }

        /// <summary>
        /// makes the doodle and other sprites fall
        /// </summary>
        public void FallDoodle()
        {
            // sprites & animations
            doodle.Position = new Vector2(doodle.Position.X, doodle.Position.Y - FALL);
            monster.Position = new Vector2(monster.Position.X, monster.Position.Y - FALL);
            woodPlatAnim.Position = new Vector2(woodPlatAnim.Position.X, woodPlatAnim.Position.Y - FALL);
            disapPlatAnim.Position = new Vector2(disapPlatAnim.Position.X, disapPlatAnim.Position.Y - FALL);
            springAnim.Position = new Vector2(springAnim.Position.X, springAnim.Position.Y - FALL);

            // platforms
            for (int i = 0; i < platforms.Count; i++)
            {
                platforms[i].Position = new Vector2(platforms[i].Position.X, platforms[i].Position.Y - FALL);
            }
        }

        /// <summary>
        /// renews the platforms
        /// </summary>
        /// <param name="currPlatform">platform to renew</param>
        /// <returns>renewed platform</returns>
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

            // check for boosters
            if (booster.MasterPlatform == currPlatform)
            {
                booster.IsUsed = false;
                booster.MasterPlatform = null;
            }

            // adjust platform appearence
            switch (currPlatform.Type)
            {
                case PlatfromType.Original:
                    currPlatform.Texture = ordinaryPlatformTexture;
                    if (!booster.IsUsed)
                    {
                        booster.MasterPlatform = currPlatform;
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

        /// <summary>
        /// renewes the collision managers
        /// </summary>
        /// <param name="platform">platform</param>
        /// <param name="idx">index of the platform in its list</param>
        /// <param name="oldType">old type of the platform</param>
        /// <returns>new collision manager</returns>
        private DoodlePlatformColMng GetNewColMng(Platform platform, int idx, PlatfromType oldType)
        {
            DoodlePlatformColMng colMng = doodlePlatformColMngs[idx];
            // renew collision manager only if platform type has been changed
            if (oldType != platform.Type)
            {
                this.Components.Remove(doodlePlatformColMngs[idx]);
                // base collision manager on a platform type
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

        /// <summary>
        /// gets a new type for platform based on the score
        /// </summary>
        /// <returns>platform type</returns>
        private PlatfromType GetNewPlatformType()
        {
            int typeIdx;
            List<PlatfromType> possiblePlatfromTypes = new List<PlatfromType>();

            // all heights - allow original
            possiblePlatfromTypes.Add(PlatfromType.Original);

            // up to 10k in score & the previous one is not wooden - allow breaking
            if (MAX_CHANGE_BOUND / 2 >= score && platforms[topIdx].Type != PlatfromType.Wooden)
            {
                possiblePlatfromTypes.Add(PlatfromType.Wooden);
            }

            // if between 0-5k OR 10k+ - allow movable horizontally
            if (score <= PLATFORM_CHANGE_BOUND || score >= MAX_CHANGE_BOUND / 2)
            {
                possiblePlatfromTypes.Add(PlatfromType.MovableHor);
            }

            // if between 5-10k OR 15k+ - allow movable vertically
            if ((score <= MAX_CHANGE_BOUND / 2 && score >= PLATFORM_CHANGE_BOUND) || score >= MAX_CHANGE_BOUND / 2 + PLATFORM_CHANGE_BOUND)
            {
                possiblePlatfromTypes.Add(PlatfromType.MovableVer);
            }

            // if between 10-15k OR 20k+ - allow disappearing
            if ((score >= MAX_CHANGE_BOUND / 2 && score <= MAX_CHANGE_BOUND / 2 + PLATFORM_CHANGE_BOUND) || score >= MAX_CHANGE_BOUND)
            {
                possiblePlatfromTypes.Add(PlatfromType.Disappearing);
            }

            typeIdx = randomNumGen.Next(0, possiblePlatfromTypes.Count);

            return possiblePlatfromTypes[typeIdx];
        }

        /// <summary>
        /// disables all the collision managers & bullets of the game
        /// </summary>
        private void DisableAllColMngs()
        {
            // booster
            doodleBoosterColMng.Enabled = false;

            // monster
            doodleMonsterColMng.Enabled = false;
            doodleMonsterColMng.PauseSong(); // stop the monster sound

            // platforms
            foreach (DoodlePlatformColMng colMng in doodlePlatformColMngs)
            {
                colMng.Enabled = false;
            }

            // boolets
            foreach (MonsterBulletColMng colMng in monsterBooletColMngs)
            {
                colMng.Enabled = false;
            }

            // disable boolets
            foreach (Bullet bullet in bullets)
            {
                bullet.Enabled = false;
            }
        }
    }
}
