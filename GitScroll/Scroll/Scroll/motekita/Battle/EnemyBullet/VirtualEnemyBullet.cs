using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.EnemyBullet
{
    abstract class VirtualEnemyBullet
    {
        protected Vector2 position;
        protected float radius;
        protected Vector2 startDirection;
        protected Vector2 direction;
        protected int index;
        protected float startSpeed;
        protected float baseSpeed;
        protected float speed;
        protected int time;

        protected bool isDead;

        protected Rectangle drawRect;

        public enum SetColor
        {
            GREEN,
            YELLOW,
            RIGHT_BLUE,
            BLUE,
            PURPLE,
            RED,
            ORANGE
        }


        public VirtualEnemyBullet(Vector2 parentPos, Vector2 startDirect, int indexNum, float spd, SetColor color)
        {
            position = parentPos;
            startDirection = startDirect;
            index = indexNum;
            baseSpeed = spd;

            time = 0;

            isDead = false;
        }

        protected void SetColor32(SetColor color)
        {
            drawRect = new Rectangle( (int)color % 4 * 32, (int)color / 4 * 32, 32, 32);
        }

        public void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
        }

        public void MainUpdate(int deltaTime, Vector2 pPos)
        {
            StateUpdate(deltaTime, pPos);
            MoveUpdate(deltaTime, pPos);
            CheckPosition();
        }

        public void CollisionUpdate()
        {

        }

        public void EndUpdate()
        {

        }

        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        protected abstract void StateUpdate(int deltaTime, Vector2 pPos);

        protected abstract void MoveUpdate(int deltaTime, Vector2 pPos);

        protected void CheckPosition()
        {
            if (BattleMain.BattleWindow.IsAreaOut(position))
            {
                isDead = true;
            }
        }


        public abstract void Draw(OutPuts.Renderer renderer);

        public float DistanceSquared(Vector2 pPps)
        {
            return Vector2.DistanceSquared(position, pPps);
        }

        public Vector2 GetPosition()
        {
            return position;
        }
        public int GetIndex()
        {
            return index;
        }

        public float PowTwoRadius(float pR)
        {
            return (float)Math.Pow(radius + pR, 2);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void Dead()
        {
            isDead = true;
        }
    }
}
