using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.Enemy
{
    class Wata : VirtualEnemy
    {
        public float minScale = 0.3f;
        public float maxScale = 0.4f;
        public int scaleChenger;
        public Vector3 basePosition;
        public Wata(Battle battle, Renderer renderer,Sound sound, Player.Player player, Vector3 position, EnemyName enemyName)
            : base(battle, renderer,sound, player, position, enemyName)
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
            physics.Inertia(deltaTime);

            scaleChenger = scaleTime / 16;
            if (state == State.NORMAL)
                physics.velocity.Y = (float)Math.Sin(time / 1000.0) * 0.03f;

            if (state == State.NORMAL)
                position.Y = basePosition.Y + (float)Math.Sin(time / 1000.0);

            if (scaleChenger < 11)
                ScaleUp(deltaTime);

            if (scaleChenger < 21 &&
                scaleChenger > 10)
                ScaleDown(deltaTime);

            if (scaleChenger >= 21)
                scaleTime = 0;

            physics.Gravity(deltaTime);

            position += physics.velocity * physics.speed * deltaTime;
            FieldMove();
        }

        public void ScaleUp(int deltaTime)//scaleを増やすメソッド
        {
            scale += 0.0015f;//ここ追加
            SetBaseVertices();
            vertices = CreateVertices();
        }
        public void ScaleDown(int deltaTime)//scaleを減らすメソッド
        {
            scale -= 0.0015f;
            SetBaseVertices();
            vertices = CreateVertices();
        }
        protected override void Awake()
        {
            scale = maxScale;
            //もとのscaleは0.25ｆ
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
