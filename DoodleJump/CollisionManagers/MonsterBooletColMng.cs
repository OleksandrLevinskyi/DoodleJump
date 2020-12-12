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
    public class MonsterBooletColMng : GameComponent
    {
        private Monster monster;
        private Bullet bullet;
        private SoundEffect hitSound;
        public MonsterBooletColMng(Game game, Monster monster, SoundEffect hitSound, Bullet bullet) : base(game)
        {
            this.monster = monster;
            this.bullet = bullet;
            this.hitSound = hitSound;
        }

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
