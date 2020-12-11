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
    public class DoodlePlatformColMng : GameComponent
    {
        private Doodle doodle;
        private Platform platform;
        private SoundEffect hitSound;

        public DoodlePlatformColMng(Game game,Doodle doodle,Platform platform,SoundEffect hitSound) : base(game)
        {
            this.doodle = doodle;
            this.platform = platform;
            this.hitSound = hitSound;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            Rectangle platformBoundary = platform.GetBound();

            if (doodleFeetBoundary.Intersects(platformBoundary) && doodle.IsFalling)
            {
                hitSound.Play();
                doodle.IsJumping = !doodle.IsJumping;
            }

            base.Update(gameTime);
        }
    }
}
