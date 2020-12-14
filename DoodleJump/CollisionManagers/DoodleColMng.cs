/*
 * DoodleColMng.cs
 * abstract class used for collision managers interacting with Doodle
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
    /// tracks collision of Doodle & other sprite
    /// </summary>
    public abstract class DoodleColMng : GameComponent
    {
        protected Doodle doodle;
        protected SoundEffect hitSound;

        /// <summary>
        /// constructor to create a collision manager, cannot be instantiated directly
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="doodle">doodle sprite</param>
        /// <param name="hitSound">sound when hit</param>
        public DoodleColMng(Game game, Doodle doodle, SoundEffect hitSound) : base(game)
        {
            this.doodle = doodle;
            this.hitSound = hitSound;
        }
    }
}
