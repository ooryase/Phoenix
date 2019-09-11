using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.Enemy
{
    abstract class VirtualEnemy
    {
        protected BattleMain.Battle parent;
        protected Player.VirtualPlayer player;
        protected int index;

        protected Vector2 position;
        protected float radius;

        protected int time;

        protected int hp;

        public enum State
        {
            NORMAL,
            STEALTH,
            AREA_OUT,
            DEATH
        }
        protected State state;

        protected bool isDead;

        protected bool isBulletToItem; 

        public VirtualEnemy(BattleMain.Battle battle, Player.VirtualPlayer virtualPlayer,int indexNum)
        {
            index = indexNum;
            parent = battle;
            player = virtualPlayer;
            state = State.NORMAL;
            time = 0;
        }




        public void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
        }
        public void MainUpdate(int deltaTime)
        {
            StateUpdate();
            MoveUpdate(deltaTime);
            CheckPosition();
            
        }
        public void CollisionUpdate()
        {
        }
        public void EndUpdate()
        {
            Shoot();
        }


        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        abstract protected void StateUpdate();


        abstract protected void MoveUpdate(int deltaTime);

        protected void CheckPosition()
        {
            if (BattleMain.BattleWindow.IsAreaOut(position))
            {
                state = State.AREA_OUT;
                isDead = true;
            }
        }

        abstract protected void Shoot();

        abstract public void Draw(OutPuts.Renderer renderer);


        public Vector2 GetPosition()
        {
            return position;
        }

        public float DistanceSquared(Vector2 pPps)
        {
            return Vector2.DistanceSquared(position, pPps);
        }

        public float PowTwoRadius(float pR)
        {
            return (float)Math.Pow(radius + pR, 2);
        }

        public bool IsDead()
        {
            return isDead;
        }

        public bool IsNotInvincible()
        {
            return state != State.STEALTH;
        }

        public void Damege(int power)
        {
            hp -= power;
            if (hp <= 0)
            {
                ItemPop();
                if (isBulletToItem)
                    parent.ConvertEnemyBulletToItem(index);

                state = State.DEATH;
                isDead = true;
            }
        }

        protected abstract void ItemPop();

        public void Dead()
        {
            if (isDead)
                return;

            ItemPop();

            if (isBulletToItem)
                parent.ConvertEnemyBulletToItem(index);

            state = State.DEATH;
            isDead = true;
        }
    }
}
