using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.Player
{
    internal class TestPlayer : VirtualPlayer
    {
        public TestPlayer(BattleMain.Battle battle) : base(battle)
        {
            speed = 0.28f;
            shiftSpeed = 0.18f;

            shootInterval = 50;
            bombShootInterval = 100;

            drawRect = new Rectangle(0, 0, 64, 64);
            drawRadius = 32f;
        }


        protected override void ShootBomb()
        {
            shootCount++;

            bullets.Add(new PlayerBullet.BombShoot(position, position, power));
        }

        protected override void ShootBullet()
        {
            shootCount++;

            if (power <= 100)
            {
                bullets.Add(new PlayerBullet.TestPlayerBullet(position,position,power));
            }
            else if(power <= 200)
            {
                bullets.Add(new PlayerBullet.TestPlayerBullet(position, position, power));
                bullets.Add(new PlayerBullet.TestHoming(position, position, power));

            }
            else if (power <= 300)
            {
                bullets.Add(new PlayerBullet.TestPlayerBullet(position, position, power));
                bullets.Add(new PlayerBullet.TestHoming(position, position, power));
            }
            else if (power <= 400)
            {

            }
        }

        public override void Draw(OutPuts.Renderer renderer)
        {
            bullets.ForEach(b => b.Draw(renderer));


            Color color = Color.White;
            drawRect.X = time / 200 % 3 * 64;

            if (invincible && (state == State.MOVE || state == State.REBIRTH) && time % 160 < 80)
                color = Color.Blue;


            renderer.DrawTexture("player", new Vector2(position.X - drawRadius, position.Y - drawRadius), drawRect,color);


        }

    }
}
