using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle
{


    internal abstract class VirtualCharacter : VirtualObject
    {
        protected float hp;
        protected bool dead;

        protected MovePhysics physics;

        public VirtualCharacter(Battle battle, Renderer renderer,Sound sound) : base(battle, renderer,sound)
        {
        }

        public abstract void MoveUpdate(int deltaTime);

        
        public abstract GameSystem.RectangleF GetCollisionRectangle();


        /// <summary>
        /// 使わないから気にしないで
        /// </summary>
        /// <param name="arts"></param>
        protected  void OnArtsCollisionEnter(Arts.Fire arts)
        {
            hp -= arts.Damage;
        }

        /// <summary>
        /// フィールドの範囲外に出た場合内側に戻す
        /// 今回は呼ばない気がする
        /// </summary>
        protected void FieldMove()
        {

            /*var rf = GetCollisionRectangle();

            if (physics.isGraund = rf.Bottom < BattleWindow.Down)
            {
                position.Y += BattleWindow.Down - rf.Bottom;
            }

            if (rf.Left < BattleWindow.Left)
                position.X += BattleWindow.Left - rf.Left;

            if (rf.Right > BattleWindow.Right)
                position.X += BattleWindow.Right - rf.Right;
                */
        }

        public override void EndUpdate()
        {
            dead = (hp <= 0);
        }
    }
}
