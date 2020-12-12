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
    public abstract class DoodleColMng : GameComponent
    {
        protected Doodle doodle;
        protected SoundEffect hitSound;
        public bool Activated { get; set; } = true;
        public DoodleColMng(Game game, Doodle doodle, SoundEffect hitSound) : base(game)
        {
            this.doodle = doodle;
            this.hitSound = hitSound;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
