/*
 * MonsterBooletColMng.cs
 * Monster & Bullet collision manager
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
using Microsoft.Xna.Framework.Media;

namespace DoodleJump.CollisionManagers
{
    /// <summary>
    /// tracks collision of Monster & Bullet
    /// </summary>
    public class MonsterBulletColMng : GameComponent
    {
        private Monster monster;
        private Bullet bullet;
        private SoundEffect hitSound;

        /// <summary>
        /// constructor to create a collision manager
        /// </summary>
        /// <param name="game">game</param>
        /// <param name="monster">monster sprite</param>
        /// <param name="hitSound">sound when hit</param>
        /// <param name="bullet">bullet sprite</param>
        public MonsterBulletColMng(Game game, Monster monster, SoundEffect hitSound, Bullet bullet) : base(game)
        {
            this.monster = monster;
            this.bullet = bullet;
            this.hitSound = hitSound;
        }

        /// <summary>
        /// checks for collision between Monster & Boolet
        /// </summary>
        /// <param name="gameTime">provides a snapshot of timing values</param>
        public override void Update(GameTime gameTime)
        {
            Rectangle monsterBoundary = monster.GetBound();
            Rectangle bulletBoundary = bullet.GetBound();

            if (monsterBoundary.Intersects(bulletBoundary))
            {
                monster.Status = MonsterStatus.Shooted;
                bullet.Position = new Vector2(0, -bullet.Texture.Height);
                hitSound.Play();
            }

            base.Update(gameTime);
        }
    }
}
