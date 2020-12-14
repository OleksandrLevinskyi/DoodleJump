/*
 * DoodleAnimatedPlatfromColMng.cs
 * Doodle & Animated Platform collision manager
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DoodleJump.CollisionManagers
{
    /// <summary>
    /// tracks collision of Doodle & Animated Platform
    /// </summary>
    public class DoodleAnimatedPlatfromColMng : DoodlePlatformColMng
    {
        private Animation animation; // animation to play when a doodle hits a platform

        /// <summary>
        /// constructor to create a collision manager
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="doodle">doodle sprite</param>
        /// <param name="platform">animated platform sprite</param>
        /// <param name="hitSound">sound when hit</param>
        /// <param name="animation">animation to play when hit</param>
        public DoodleAnimatedPlatfromColMng(Game game, Doodle doodle,
            Platform platform, SoundEffect hitSound, Animation animation) : base(game, doodle, platform, hitSound)
        {
            this.animation = animation;
        }

        /// <summary>
        /// checks for collision between Doodle & Animated Platform
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            Rectangle platformBoundary = platform.GetBound();

            if (doodleFeetBoundary.Intersects(platformBoundary) && doodle.IsFalling)
            {
                hitSound.Play();
                animation.Position = platform.Position;
                animation.Start();
                if (platform.Type == PlatfromType.Disappearing)
                {
                    base.Update(gameTime);
                }
                platform.Position = new Vector2(0, Shared.Stage.Y); // put a platfrom outside the screen to be re-generated
            }
        }
    }
}
