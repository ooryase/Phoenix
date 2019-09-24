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
        public Dragon(Battle battle, Renderer renderer, Player.Player player, Vector3 position, EnemyName enemyName) : base(battle, renderer, player, position, enemyName)
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
            if (Math.Abs(player.Position.X - position.X) < 4f &&
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
            TextureCoordinateSet(time / 200 % 5, 0f);
            VerticesSet(Billboard.PITCH_ONLY);
            effect.Parameters["View"].SetValue(parent.View);
        }

        /// <summary>
        /// テクスチャコーディネイトの設定
        /// 連結画像によるアニメーション処理を行う
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void TextureCoordinateSet(float x, float y) //アニメーション関係
        {
            vertices[0].TextureCoordinate.X = 0.2f * (x + (float)Direct);
            vertices[1].TextureCoordinate.X = 0.2f * (x + 1f - (float)Direct);
            vertices[2].TextureCoordinate.X = 0.2f * (x + 1f - (float)Direct);
            vertices[3].TextureCoordinate.X = 0.2f * (x + (float)Direct);

            vertices[0].TextureCoordinate.Y = 1f * (y);
            vertices[1].TextureCoordinate.Y = 1f * (y + 1f);
            vertices[2].TextureCoordinate.Y = 1f * (y);
            vertices[3].TextureCoordinate.Y = 1f * (y + 1f);
        }
    }
}
