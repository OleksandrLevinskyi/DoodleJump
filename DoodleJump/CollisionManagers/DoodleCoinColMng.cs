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
    public class DoodleCoinColMng:DoodleColMng
    {
        protected Platform platform;

        public Platform Platform { get => platform; }

        public DoodleCoinColMng(Game game, Doodle doodle, Platform platform, SoundEffect hitSound) : base(game, doodle, hitSound)
        {
            this.doodle = doodle;
            this.platform = platform;
            this.hitSound = hitSound;
        }
    }
}
