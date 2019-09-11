using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shoot.OutPuts;

namespace Shoot.Battle.EnemyBullet
{
    class SpinToStraight : VirtualEnemyBullet
    {
        private Vector2 basePos;
        private double radian;

        private int setTime;
        private float setRadius;

        private double plusMinus;
        private bool shoot;

        
        public enum Status
        {
            STRAIGHT = 0,
            RIGHT = -1,
            LEFT = 1,
        }
        private Status status;


        public SpinToStraight(Vector2 parentPos, Vector2 startDirect, int indexNum, float spd, SetColor color, double rad ,double setPlusMinus,Status setStates)
            : base(parentPos, startDirect, indexNum, spd, color)
        {
            basePos = parentPos;
            radius = 16f;

            radian = rad;
            plusMinus = setPlusMinus;
            status = setStates;
            setTime = 2000;
            setRadius = 100f;

            shoot = false;

            speed = baseSpeed;
            
            SetColor32(color);

        }
        protected override void StateUpdate(int deltaTime, Vector2 pPos)
        {
            if (!shoot && time > setTime &&
                Math.Cos(radian) * plusMinus < -0.98 )
            {
                shoot = true;

                double r = Math.Atan2(pPos.Y - position.Y, pPos.X - position.X) + (double)status * 0.5;
                direction = new Vector2((float)Math.Cos(r), (float)Math.Sin(r));
            }

            radian += deltaTime * 0.0015 * plusMinus;
        }

        protected override void MoveUpdate(int deltaTime, Vector2 pPos)
        {
            if (shoot)
            {
                position += direction * speed * deltaTime;
            }
            else
            {
                if (time < setTime)
                {
                    position.X = basePos.X + (float)Math.Cos(radian) * (time / (float)setTime) * setRadius;
                    position.Y = basePos.Y - (float)Math.Sin(radian) * (time / (float)setTime) * setRadius;
                }
                else
                {
                    position.X = basePos.X + (float)Math.Cos(radian) * setRadius;
                    position.Y = basePos.Y - (float)Math.Sin(radian) * setRadius;
                }
            }
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("32tex", new Vector2(position.X - radius, position.Y - radius), drawRect, Color.White);
        }


    }
}
