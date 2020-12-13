using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleJump.Scenes;
using DoodleJump.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DoodleJump.CollisionManagers
{
    public class DoodleMonsterColMng : DoodleColMng
    {
        private const int SOS_AREA = 500;
        private Monster monster;
        private SoundEffect defeatSound;
        private Song closeSound;
        public bool IsSongPlaying { get; set; }
        public DoodleMonsterColMng(Game game, Doodle doodle, SoundEffect hitSound, SoundEffect defeatSound, Song closeSound, Monster monster) : base(game, doodle, hitSound)
        {
            this.monster = monster;
            this.closeSound = closeSound;
            this.defeatSound = defeatSound;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle doodleBoundary = doodle.GetBound();
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            doodleBoundary = new Rectangle(doodleBoundary.X, doodleBoundary.Y, doodleBoundary.Width, doodleBoundary.Height - doodleFeetBoundary.Height);
            Rectangle monsterBoundary = monster.GetBound();

            if ((doodleBoundary.Top - SOS_AREA <= monsterBoundary.Bottom || doodleBoundary.Bottom + SOS_AREA <= monsterBoundary.Top) && monster.Status == MonsterStatus.None)
            {
                if (!IsSongPlaying)
                {
                    MediaPlayer.Play(closeSound);
                    MediaPlayer.IsRepeating = true;
                    IsSongPlaying = true;
                }
            }

            if (monster.Status == MonsterStatus.None)
            {
                // monster defeated
                if (doodleFeetBoundary.Intersects(monsterBoundary) && doodle.IsFalling)
                {
                    PauseSong();

                    doodle.IsJumping = false;
                    monster.Status = MonsterStatus.Defeated;
                    defeatSound.Play();
                }
                // monster won
                else if (doodleBoundary.Intersects(monsterBoundary))
                {
                    PauseSong();

                    doodle.IsJumping = true;
                    doodle.DoodleColor = Color.Pink;
                    doodle.Speed = new Vector2(doodle.Speed.X, 0);
                    monster.Status = MonsterStatus.Won;
                    hitSound.Play();
                }
            }

            base.Update(gameTime);
        }

        public void PauseSong()
        {
            MediaPlayer.Pause();
            IsSongPlaying = false;
        }
    }
}
