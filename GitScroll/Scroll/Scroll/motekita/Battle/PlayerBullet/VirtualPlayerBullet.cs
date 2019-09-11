using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.PlayerBullet
{
    abstract class VirtualPlayerBullet
    {
        protected Vector2 position;
        protected float radius;
        protected float speed;
        protected Vector2 direction;

        protected int time;

        protected int power;

        protected bool isDead;

        public VirtualPlayerBullet(Vector2 pPos, Vector2 ePos, int pPower)
        {
            time = 0;
            position = pPos;
            direction = new Vector2(ePos.X - pPos.X,ePos.Y - ePos.Y);
            direction.Normalize();
            isDead = false;
        }

        public void MainUpdate(int deltaTime, Vector2 targetPos)
        {
            TimeUpdate(deltaTime);
            MoveUpdate(deltaTime, targetPos);
            CheckPosition();
        }

        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        abstract protected void MoveUpdate(int deltaTime, Vector2 targetPos);

        protected void CheckPosition()
        {
            if(BattleMain.BattleWindow.IsAreaOut(position))
            {
                isDead = true;
            }
        }

        public void CollisionUpdate(List<Enemy.VirtualEnemy> enemies)
        {
            var collisionEnemies = enemies.Where(e => e.IsNotInvincible() && e.DistanceSquared(position) < e.PowTwoRadius(radius));
            if (collisionEnemies.Count() > 0)
            {
                isDead = true;
                foreach (var ce in collisionEnemies)
                {
                    ce.Damege(power);
                }
            }

        }

        abstract public void Draw(OutPuts.Renderer renderer);


        public bool IsDead()
        {
            return isDead;
        }
    }
}
