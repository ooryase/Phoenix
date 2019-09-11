using Microsoft.Xna.Framework;
using Shoot.OutPuts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.EnemyBullet
{
    class SpinToCurve : VirtualEnemyBullet
    {
        private Vector2 basePos;
        private double radian;

        private int startTime;
        private int setTime;
        private float setRadius;

        private double plusMinus;

        public SpinToCurve(Vector2 parentPos, Vector2 startDirect, int indexNum, float spd, SetColor color, double rad, double setPlusMinus)
            : base(parentPos, startDirect, indexNum, spd, color)
        {
            basePos = parentPos;
            radius = 16f;

            radian = rad;
            plusMinus = setPlusMinus;
            startTime = 1600;
            setTime = 2000;
            setRadius = 200f;

            speed = baseSpeed;

            SetColor32(color);

        }
        protected override void StateUpdate(int deltaTime, Vector2 pPos)
        {
            if(time < setTime)
                radian += deltaTime * 0.0015 * plusMinus;
            else
                radian += deltaTime * 0.0003 * plusMinus;

            if (time > 20000 && BattleMain.BattleWindow.IsGrayAreaOut(position))
                Dead();
        }

        protected override void MoveUpdate(int deltaTime, Vector2 pPos)
        {
            if (time < startTime)
            {
                position.X = basePos.X + (float)Math.Cos(radian) * (time / (float)startTime) * setRadius;
                position.Y = basePos.Y - (float)Math.Sin(radian) * (time / (float)startTime) * setRadius;
            }
            else if(time < setTime)
            {
                position.X = basePos.X + (float)Math.Cos(radian) * setRadius;
                position.Y = basePos.Y - (float)Math.Sin(radian) * setRadius;
            }
            else
            {
                position.X -= (float)Math.Cos(radian) * speed * deltaTime;
                position.Y -= (float)Math.Sin(radian) * speed * deltaTime;
            }
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("32tex", new Vector2(position.X - radius, position.Y - radius), drawRect, Color.White);
        }



    }
}
