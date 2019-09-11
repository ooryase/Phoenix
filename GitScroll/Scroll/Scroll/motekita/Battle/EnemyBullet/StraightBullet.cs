using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shoot.OutPuts;

namespace Shoot.Battle.EnemyBullet
{
    class StraightBullet : VirtualEnemyBullet
    {
        private int startTime;
        private int startReduceTime;
        private float startReduceSpeed;
        private bool startReduce;


        public StraightBullet(Vector2 parentPos, Vector2 startDirect, int indexNum, float spd, SetColor color,bool startSpeedReduce = false) : base(parentPos, startDirect, indexNum, spd, color)
        {
            startReduce = startSpeedReduce;

            radius = 16f;

            direction = startDirection;
            startSpeed = 0.32f;
            speed = baseSpeed;
            startReduceSpeed = startSpeed;

            startTime = 150;
            startReduceTime = 75;

            SetColor32(color);


        }

        protected override void StateUpdate(int deltaTime, Vector2 pPos)
        {
        }

        protected override void MoveUpdate(int deltaTime, Vector2 pPos)
        {
            if (startReduce)
            {
                if (time < startTime)
                {
                    position += direction * startSpeed * deltaTime;
                }
                else if (time < startTime + startReduceTime)
                {
                    startReduceSpeed -= (startSpeed - speed) * (deltaTime / startReduceTime);
                    position += direction * startReduceSpeed * deltaTime;
                }
                else
                {
                    position += direction * speed * deltaTime;
                }
            }
            else
            {
                position += direction * speed * deltaTime;
            }
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("32tex", new Vector2(position.X - radius, position.Y - radius), drawRect, Color.White);
        }

    }
}
