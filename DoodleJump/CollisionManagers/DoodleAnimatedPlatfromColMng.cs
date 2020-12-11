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
    public class DoodleAnimatedPlatfromColMng : DoodlePlatformColMng
    {
        private Animation animation;
        public DoodleAnimatedPlatfromColMng(Game game, Doodle doodle, 
            Platform platform, SoundEffect hitSound, Animation animation) : base(game, doodle, platform, hitSound)
        {
            this.animation = animation;
        }

        public override void Update(GameTime gameTime)
        {
            Rectangle doodleFeetBoundary = doodle.GetFeetBound();
            Rectangle platformBoundary = platform.GetBound();

            if (doodleFeetBoundary.Intersects(platformBoundary) && doodle.IsFalling)
            {
                hitSound.Play();
                animation.Position = platform.Position;
                platform.Position = new Vector2(0, Shared.Stage.Y); // put a platfrom outside the screen to be re-generated
                animation.Start();
            }
        }
    }
}
