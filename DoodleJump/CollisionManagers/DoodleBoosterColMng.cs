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
    public class DoodleBoosterColMng:DoodleColMng
    {
        private const int BOOSTER_COUNT = 240;
        private const int ROCKET_SPEED = 400;
        private int counter = 0;
        private bool isCounterEnabled = true;
        private Booster booster;
        public Animation BoosterAnimation { get; set; }

        public DoodleBoosterColMng(Game game, Doodle doodle, SoundEffect hitSound, Booster booster) : base(game, doodle, hitSound)
        {
            this.doodle = doodle;
            this.booster = booster;
            this.hitSound = hitSound;
        }


        public override void Update(GameTime gameTime)
        {
            if(booster.Type == BoosterType.Spring)
            {
                Rectangle doodleFeetBoundary = doodle.GetFeetBound();
                Rectangle boosterBoundary = booster.GetBound();

                if (doodleFeetBoundary.Intersects(boosterBoundary))
                {
                    doodle.SpringBoost();
                    BoosterAnimation.Position = booster.Position;
                    //Animation.Position = new Vector2(booster.Position.X,);
                    BoosterAnimation.Start();
                    hitSound.Play();
                }
            }
            //else if(booster.Type == BoosterType.Rocket)
            //{
            //    Rectangle doodleBoundary = doodle.GetBound();
            //    Rectangle boosterBoundary = booster.GetBound();

            //    if (doodleBoundary.Intersects(boosterBoundary))
            //    {
            //        hitSound.Play();
            //        doodle.Gravity = 0;
            //        doodle.JumpSpeed = ROCKET_SPEED;
            //        counter = 0;
            //    }
            //    else if(counter < BOOSTER_COUNT)
            //    {
            //        counter++;
            //    }
            //    counter++;
            //}

            base.Update(gameTime);
        }
    }
}
