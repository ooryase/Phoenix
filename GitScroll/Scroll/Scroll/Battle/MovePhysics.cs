using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.Battle
{
    class MovePhysics
    {
        public Vector3 velocity;

        /// <summary>
        /// velocityに付与する基本量
        /// </summary>
        public float speed;
        /// <summary>
        /// 重力加速度
        /// </summary>
        public float gAcceleration;
        /// <summary>
        /// 摩擦係数
        /// </summary>
        public float myu;

        /// <summary>
        /// 接地判定
        /// </summary>
        public bool isGraund;

        public bool gravity;
        /// <summary>
        /// 摩擦
        /// </summary>
        protected bool inertia;


        /// <summary>
        /// 重力、摩擦が有効の場合は効果量を
        /// 無効の場合はVector3.Zeroを引数にする
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="isGraund"></param>
        /// <param name="gravity"></param>
        /// <param name="inertia"></param>
        public MovePhysics(float speed,float? gravity, float? inertia)
        {
            velocity = Vector3.Zero;
            this.speed = speed;
            isGraund = false;
            if (this.gravity = gravity != null)
                gAcceleration = (float)gravity;
            if (this.inertia = inertia!= null)
                myu = (float)inertia;
        }


        public void Gravity(int deltaTime)
        {
            if (!gravity || isGraund)
                return;

            velocity.Y -= gAcceleration * deltaTime;
        }

        public void Inertia(int deltaTime)
        {
            if (!inertia)
                velocity = Vector3.Zero;

            velocity *= myu * deltaTime; 
        }

    }
}
