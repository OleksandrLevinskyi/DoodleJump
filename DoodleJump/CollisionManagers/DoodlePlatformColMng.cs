/*
 * DoodlePlatformColMng.cs
 * Doodle & Platform collision manager
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
using DoodleJump.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.CollisionManagers
{
    /// <summary>
    /// tracks collision of Doodle & Platform
    /// </summary>
    public class DoodlePlatformColMng : DoodleColMng
    {
        protected Platform platform;

        public Platform Platform { get => platform; }

        /// <summary>
        /// constructor to create a collision manager
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="doodle">doodle sprite</param>
        /// <param name="platform">platform sprite</param>
        /// <param name="hitSound">sound when hit</param>
        public DoodlePlatformColMng(Game game, Doodle doodle, Platform platform, SoundEffect hitSound) : base(game, doodle, hitSound)
        {
            this.doodle = doodle;
            this.platform = platform;
            this.hitSound = hitSound;
        }

        /// <summary>
        /// checks for collision between Doodle & Platform
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            Rectangle platformBoundary = platform.GetBound();

            if (doodleFeetBoundary.Intersects(platformBoundary) && doodle.IsFalling)
            {
                hitSound.Play();
                doodle.IsJumping = false;
            }

            base.Update(gameTime);
        }
    }
}
