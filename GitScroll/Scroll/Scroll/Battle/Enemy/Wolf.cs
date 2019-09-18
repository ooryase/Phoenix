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
        public bool jump;
        public Wolf(Battle battle, Renderer renderer, Vector3 position, EnemyName enemyName) : base(battle, renderer, position, enemyName)
        {
            hp = 1f; //HP兼攻撃ゲージ
            basePosition = position;
            this.position = basePosition;
            this.enemyName = enemyName;
            StateSet(State.NORMAL);

            physics = new MovePhysics(0.015f, 0.2f, null);
            effect.Parameters["Value"].SetValue(1f);
            effect.Parameters["Red"].SetValue(1f);
        }
        protected override void Awake()
        {
            scale = 0.25f;
        }

        protected override void NameSet()
        {
            textureName = "inu";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            physics.Inertia(deltaTime);
            physics.Gravity(deltaTime);
            position += physics.velocity * physics.speed * deltaTime;
            FieldMove();
        }
        protected override void NormalStateUpdate(int deltaTime)
        {

        }

        public override void DrawUpdate()
        {
            VerticesSet(Billboard.PITCH_ONLY);

            effect.Parameters["View"].SetValue(parent.View);
        }
    }
}
