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
        public float MinScale = 1.75f;
        public float MaxScale = 0.25f;
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
        //元々ここにAwake()
        protected override void NameSet()
        {
            textureName = "wata";
            effectName = "NoobEnemy";
        }
        public override void MoveUpdate(int deltaTime)
        {
            if (state == State.NORMAL)
            position.Y = basePosition.Y + (float)Math.Sin(time / 1000.0);
            
        }
        protected override void NormalStateUpdate(int deltaTime)
        {
            //スケールを変えてアニメーションさせたい。これはまだ出来てない状態。
            if (Scale > MaxScale)
                ScaleDown(deltaTime);
            if (Scale < MinScale)
                ScaleUp(deltaTime);

            Console.WriteLine(Scale);
        }
        public void ScaleUp(int deltaTime)
        {
            Scale += 0.03f;
        }
        public void ScaleDown(int deltaTime)
        {
            Scale -= 0.03f;
        }
        protected override void Awake()
        {
            Scale = 0.25f;
        }
        public override void DrawUpdate()
        {
            VerticesSet(Billboard.PITCH_ONLY);

            effect.Parameters["View"].SetValue(parent.View);

        }

    }
}
