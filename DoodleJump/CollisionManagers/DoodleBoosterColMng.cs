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
    public class DoodleBoosterColMng : DoodleColMng
    {
        private const int BOOSTER_COUNT = 240;
        private const int ROCKET_SPEED = 400;
        private int counter = 0;
        private bool isCounterEnabled = true;
        private Booster booster;
        public Animation BoosterAnimation { get; set; }

        public DoodleBoosterColMng(Game game, Doodle doodle, SoundEffect hitSound, Booster booster) : base(game, doodle, hitSound)
        {
            this.booster = booster;
        }


        public override void Update(GameTime gameTime)
        {
            if (Activated)
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
}
