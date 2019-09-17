using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.Output;

namespace Scroll.Battle.Enemy
{
    class Wata : VirtualEnemy
    {
        public Vector3 basePosition;
        public Wata(Battle battle, Renderer renderer, Vector3 position, EnemyName enemyName) : base(battle, renderer, position, enemyName)
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
            textureName = "wata";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            position.Y = basePosition.Y + (float)Math.Sin(time / 1000.0);
        }

        protected override void NormalStateUpdate(int deltaTime)
        {

        }
    }
}
