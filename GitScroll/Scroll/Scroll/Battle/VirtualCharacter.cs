using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.Output;

namespace Scroll.Battle
{


    internal abstract class VirtualCharacter : VirtualObject
    {
        protected float hp;
        protected float speed;
        protected float gSpeed;

        protected bool gravity;
        protected bool dead;
        protected bool isGraund;

        protected Vector3 velocity;

        public VirtualCharacter(Battle battle, Renderer renderer) : base(battle, renderer)
        {
            gravity = true;
            velocity = Vector3.Zero;
        }

        public abstract void MoveUpdate(int deltaTime);

        
        public abstract GameSystem.RectangleF GetCollisionRectangle();

        protected void GravityUpdate(int deltaTime)
        {
            if (!gravity || isGraund)
                return;

            velocity.Y -= gSpeed * deltaTime;

        }

        protected  void OnArtsCollisionEnter(Arts.Fire arts)
        {
            hp -= arts.Damage;
        }

        protected void FieldMove()
        {

            var rf = GetCollisionRectangle();

            if (isGraund = rf.Bottom < BattleWindow.Down)
            {
                position.Y += BattleWindow.Down - rf.Bottom;
            }

            if (rf.Left < BattleWindow.Left)
                position.X += BattleWindow.Left - rf.Left;

            if (rf.Right > BattleWindow.Right)
                position.X += BattleWindow.Right - rf.Right;

        }

        public override void EndUpdate()
        {
            dead = (hp <= 0);
        }
    }
}
