using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.Enemy
{
     internal abstract class VirtualEnemy : VirtualCharacter
    {
        public int scaleTime;
        public enum EnemyName
        {
            wata = 12,
            wolf = 13,
            dragon = 14,
        }
        public EnemyName enemyName;
        public enum State
        {
            NORMAL,
            DEAD,
            ATTACK,
        }
        protected State state;
        public VirtualEnemy(Battle battle, Renderer renderer, Vector3 position, EnemyName enemyName) : base(battle, renderer)
        {
            this.position = position;
            this.enemyName = enemyName;
            //Coordinate();

            state = State.NORMAL;
        }
        
        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate(deltaTime);

        }
        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
            scaleTime += deltaTime;
        }
        public override abstract void MoveUpdate(int deltaTime);
   
        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            if(virtualObject.Tag == TagName.PLAYER &&
                state != State.DEAD)
            {
                dead = true;
                StateSet(State.DEAD);

                Player.Player p = (Player.Player)virtualObject;
                p.AddAsh(10f);
            }


        }
        protected void StateUpdate(int deltaTime)
        {
            switch (state)
            {
                case State.NORMAL:
                    NormalStateUpdate(deltaTime);
                    break;
                case State.DEAD:
                    DeadStateUpdate(deltaTime);
                    break;
                case State.ATTACK:
                    AttackStateUpdate(deltaTime);
                    break;
            }
        }
        protected abstract void NormalStateUpdate(int deltaTime);
        protected void DeadStateUpdate(int deltaTime)
        {
            if (time > 960)
                delete = true;
        }
        protected virtual void AttackStateUpdate(int deltaTime) { }
        public override void Draw(Renderer renderer)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                renderer.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    vertices, 0, vertices.Length,
                    indices, 0, indices.Length / 3);
                //                graphicsDevice.
            }
        }

        public override RectangleF GetCollisionRectangle()
        {
            return new RectangleF(0, 0, 0, 0);
        }
        protected void StateSet(State s)
        {
            state = s;
            time = 0;
        }
    }
}
