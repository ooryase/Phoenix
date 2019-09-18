using Microsoft.Xna.Framework;
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
        public Dragon(Battle battle, Renderer renderer, Vector3 position, EnemyName enemyName) : base(battle, renderer, position, enemyName)
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
            scale = 0.25f;
        }

        protected override void NameSet()
        {
            textureName = "doragon";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            if(state == State.NORMAL)
            position.Y = basePosition.Y + (float)Math.Sin(time / 1000.0);
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
