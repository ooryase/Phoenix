using Microsoft.Xna.Framework;
using Scroll.GameSystem;
using Scroll.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.Battle.Enemy
{
    class Dragon : VirtualEnemy
    {
        public Vector3 basePosition;
        public Dragon(Battle battle, Renderer renderer,Sound sound, Player.Player player, Vector3 position, EnemyName enemyName)
            : base(battle, renderer,sound, player,position, enemyName)
        {
            hp = 1f; //HP兼攻撃ゲージ
            basePosition = position;
            this.position = basePosition;
            this.enemyName = enemyName;
            StateSet(State.NORMAL);

            physics = new MovePhysics(0.015f, null, null);
            effect.Parameters["Value"].SetValue(1f);
            effect.Parameters["Red"].SetValue(1f);
        }
        protected override void Awake()
        {
            scale = 0.6f;
        }

        protected override void NameSet()
        {
            textureName = "Wyvern";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            physics.Inertia(deltaTime);

            if (state == State.NORMAL)
                physics.velocity.Y = (float)Math.Sin(time / 100.0) * 0.2f;
            else if (state == State.PERCEPTION)
            {
                var v3 = player.Position - position;
                v3.Normalize();
                physics.velocity = v3 * 0.03f;
            }

            physics.Gravity(deltaTime);

            position += physics.velocity * physics.speed * deltaTime;
            FieldMove();
        }
        protected override void NormalStateUpdate(int deltaTime)
        {
            if(Math.Abs(player.Position.X - position.X) < 4f &&
                Math.Abs(player.Position.Y - position.Y) < 4f)
            {
                StateSet(VirtualEnemy.State.PERCEPTION);
            }
        }
        protected override void PerceptionStateUpdate(int deltaTime)
        {
            
        }

        public override void DrawUpdate()
        {
            VerticesSet(Billboard.PITCH_ONLY);

            effect.Parameters["View"].SetValue(parent.View);
        }
    }
}
