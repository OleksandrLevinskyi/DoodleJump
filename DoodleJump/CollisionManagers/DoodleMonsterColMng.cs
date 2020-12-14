/*
 * DoodleMonsterColMng.cs
 * Doodle & Monster collision manager
 * 
 * Revision History
 *          Oleksandr Levinskyi, 2020.12.06: Created & Imlemented
 * Oleksandr Levinskyi, 2020.12.13: Revised & Completed
*/

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
    /// <summary>
    /// tracks collision of Doodle & Monster
    /// </summary>
    public class DoodleMonsterColMng : DoodleColMng
    {
        private const int SOS_AREA = 500;
        private Monster monster;
        private SoundEffect defeatSound;
        private Song closeSound;
        public bool IsSongPlaying { get; set; } // indicated if closeSound is playing

        /// <summary>
        /// constructor to create a collision manager
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="doodle">doodle sprite</param>
        /// <param name="hitSound">sound when hit</param>
        /// <param name="defeatSound">sound when defeated</param>
        /// <param name="closeSound">sound when monster is in SOS_AREA</param>
        /// <param name="monster">monster sprite</param>
        public DoodleMonsterColMng(Game game, Doodle doodle, SoundEffect hitSound, SoundEffect defeatSound, Song closeSound, Monster monster) : base(game, doodle, hitSound)
        {
            this.monster = monster;
            this.closeSound = closeSound;
            this.defeatSound = defeatSound;
        }

        /// <summary>
        /// checks for collision between Doodle & Monster
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle doodleBoundary = doodle.GetBound();
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            Rectangle monsterBoundary = monster.GetBound();

            // remove feet area from doodle boundary
            doodleBoundary = new Rectangle(doodleBoundary.X, doodleBoundary.Y, doodleBoundary.Width, doodleBoundary.Height - doodleFeetBoundary.Height);

            // if monster is in sos-area (close to the doodle)
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

        /// <summary>
        /// pauses the closeSound of a monster
        /// </summary>
        public void PauseSong()
        {
            MediaPlayer.Pause();
            IsSongPlaying = false;
        }
    }
}
