using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.Output;

namespace Scroll.Battle.Enemy
{
    class Wolf : VirtualEnemy
    {
        public Vector3 basePosition;
        public Wolf(Battle battle, Renderer renderer, Player.Player player, Vector3 position, EnemyName enemyName) : base(battle, renderer, player, position, enemyName)
        {
            hp = 1f; //HP兼攻撃ゲージ
            basePosition = position;
            this.position = basePosition;
            this.enemyName = enemyName;
            StateSet(State.NORMAL);

            physics = new MovePhysics(0.015f, 0.005f, 0.999f);
            effect.Parameters["Value"].SetValue(1f);
            effect.Parameters["Red"].SetValue(1f);
        }
        protected override void Awake()
        {
            scale = 0.25f;
        }

        protected override void NameSet()
        {
            textureName = "flog";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            physics.Inertia(deltaTime);

            if (state == State.NORMAL)
            {
                if(time > 2000)
                {
                    physics.velocity.Y = 1f;
                    time = 0;
                }
            }

            physics.Gravity(deltaTime);

            position += physics.velocity * physics.speed * deltaTime;
            FieldMove();
            physics.isGraund = false;
        }
        protected override void NormalStateUpdate(int deltaTime)
        {

        }

        protected override void PerceptionStateUpdate(int deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void DrawUpdate()
        {
            VerticesSet(Billboard.PITCH_ONLY);

            effect.Parameters["View"].SetValue(parent.View);
        }
    }
}
