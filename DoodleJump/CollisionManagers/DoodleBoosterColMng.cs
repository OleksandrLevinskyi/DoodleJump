/*
 * DoodleBoosterColMng.cs
 * Doodle & Booster collision manager
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
    /// tracks collision of Doodle & Booster
    /// </summary>
    public class DoodleBoosterColMng : DoodleColMng
    {
        private Booster booster;
        public Animation BoosterAnimation { get; set; } // animation to play when doodle & booster collide

        /// <summary>
        /// constructor to create a collision manager
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="doodle">doodle sprite</param>
        /// <param name="hitSound">sound when hit</param>
        /// <param name="booster">booster sprite</param>
        public DoodleBoosterColMng(Game game, Doodle doodle, SoundEffect hitSound, Booster booster) : base(game, doodle, hitSound)
        {
            this.booster = booster;
        }

        /// <summary>
        /// checks for collision between Doodle & Booster
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            if (booster.Type == BoosterType.Spring)
            {
                Rectangle doodleFeetBoundary = doodle.GetFeetBound();
                Rectangle boosterBoundary = booster.GetBound();

                if (doodleFeetBoundary.Intersects(boosterBoundary) && doodle.IsFalling)
                {
                    doodle.IsJumping = true;
                    doodle.SpringBoost();
                    BoosterAnimation.Position = new Vector2(booster.MasterPlatform.Position.X + (booster.MasterPlatform.Texture.Width - BoosterAnimation.Texture.Width / BoosterAnimation.Cols) / 2,
                                                                booster.MasterPlatform.Position.Y - BoosterAnimation.Texture.Height / BoosterAnimation.Rows);
                    BoosterAnimation.Start();
                    hitSound.Play();
                }
            }

            base.Update(gameTime);
        }
    }
}
