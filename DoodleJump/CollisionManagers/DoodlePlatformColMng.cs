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
            Rectangle doodleBoundary = doodle.GetBound();
            Rectangle platformBoundary = platform.GetBound();

            doodleBoundary = new Rectangle(doodleBoundary.X, 
                doodleBoundary.Y + doodleBoundary.Height - platformBoundary.Height, 
                doodleBoundary.Width, 
                platformBoundary.Height);

            if (doodleBoundary.Intersects(platformBoundary) && doodle.IsFalling)
            {
                hitSound.Play();
                doodle.IsJumping = !doodle.IsJumping;
            }

            base.Update(gameTime);
        }
    }
}
