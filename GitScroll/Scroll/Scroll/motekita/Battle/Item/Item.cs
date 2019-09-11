using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.Item
{
    class Item
    {
        protected Vector2 position;
        protected float radius;
        protected Vector2 direction;
        protected int time;
        protected int popTime;
        protected float speed;

        protected bool vacuum;
        protected float vacuumSpeed;

        protected bool isDead;

        protected Rectangle drawRect;

        public enum ItemState
        {
            SMALL_SCORE,
            SCORE,
            POWER,
            LIFE,
            BOMB,
        }
        protected ItemState itemState;
        protected int stateNumbers;

        public Item(Vector2 popPos, ItemState state, bool isVacuum = false)
        {
            position = popPos;
            itemState = state;
            vacuum = isVacuum;

            radius = 16f;
            direction = GameSystem.StaticRandom.GetRandomDirection();
            time = 0;

            speed = 0.19f;
            popTime = 200;
            vacuumSpeed = 0.94f;

            isDead = false;

            switch (itemState)
            {
                case ItemState.SMALL_SCORE:
                    stateNumbers = 150;
                    drawRect = new Rectangle(96, 32, 32, 32);
                    break;
                case ItemState.SCORE:
                    stateNumbers = 5000;
                    drawRect = new Rectangle(0, 64, 32, 32);
                    break;
                case ItemState.POWER:
                    stateNumbers = 5;
                    drawRect = new Rectangle(32, 64, 32, 32);
                    break;
                case ItemState.LIFE:
                    stateNumbers = 1;
                    drawRect = new Rectangle(96, 64, 32, 32);
                    break;
                case ItemState.BOMB:
                    stateNumbers = 1;
                    drawRect = new Rectangle(64, 64, 32, 32);
                    break;
            }
        }

        public void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
        }
        public void MainUpdate(int deltaTime, Vector2 pPos)
        {
            MoveUpdate(deltaTime, pPos);
            CheckPosition();
        }
        public void CollisionUpdate(Player.VirtualPlayer player)
        {
            if (DistanceSquared(player.GetPosition()) < PowTwoRadius(player.GetRadius() + radius))
            {
                player.AddItem(itemState,stateNumbers);
                Dead();
            }
        }
        public void EndUpdate(bool playerVacuum)
        {
            if (playerVacuum)
            {
                vacuum = playerVacuum;
            }
        }

        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        protected void MoveUpdate(int deltaTime, Vector2 pPos)
        {
            if(time < popTime)
            {
                position += direction * speed * deltaTime;
            }
            else
            {
                if(vacuum)
                {
                    direction = new Vector2(pPos.X - position.X, pPos.Y - position.Y);
                    direction.Normalize();
                    position += direction * vacuumSpeed * deltaTime;
                }
                else
                {
                    direction += Vector2.UnitY * 0.1f;
                    direction.Normalize();
                    position += direction * speed * deltaTime;
                }
            }
        }

        protected void CheckPosition()
        {
            if (BattleMain.BattleWindow.IsAreaOut(position))
            {
                isDead = true;
            }
        }

        public void Draw(OutPuts.Renderer renderer)
        {
            //            renderer.DrawTexture(position);
            renderer.DrawTexture("32tex", new Vector2(position.X - radius, position.Y - radius), drawRect, Color.White);
        }

        public float DistanceSquared(Vector2 pPps)
        {
            return Vector2.DistanceSquared(position, pPps);
        }

        public float PowTwoRadius(float pR)
        {
            return (float)Math.Pow(radius + pR, 2);
        }

        public void Vacuum()
        {
            vacuum = true;
        }

        public void Dead()
        {
            isDead = true;
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}
