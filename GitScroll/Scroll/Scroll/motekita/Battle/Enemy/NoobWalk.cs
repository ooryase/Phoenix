using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoot.Battle.EnemyBullet;

namespace Shoot.Battle.Enemy
{
    class NoobWalk : Enemy.VirtualEnemy
    {
        private Vector2 startPosition;
        private readonly double direction;
        private readonly float circleRadius;
        private double radian;
        private double speed;
        private readonly float height;

        private readonly int shootTime;
        private uint shootCount;

        private Item.Item.ItemState popItem;

        public enum ActivePattern
        {
            RIGHT = 1,
            LEFT = -1
        }


        public NoobWalk(BattleMain.Battle battle, Player.VirtualPlayer virtualPlayer, int indexNum, ActivePattern activePattern, Item.Item.ItemState itemState) : base(battle, virtualPlayer, indexNum)
        {
            radius = 32f;

            speed = 0.0003;

            shootTime = 2000;
            shootCount = 0;

            hp = 20000;

            popItem = itemState;

            isBulletToItem = false;

            height = 2f;
            circleRadius = 250.3f;
            direction = (double)activePattern;

            if (activePattern == ActivePattern.LEFT)
            {
                startPosition = new Vector2(BattleMain.BattleWindow.Center.X, BattleMain.BattleWindow.Up - 50f);
                radian = Math.PI;
            }
            else if (activePattern == ActivePattern.RIGHT)
            {
                startPosition = new Vector2(BattleMain.BattleWindow.Center.X, BattleMain.BattleWindow.Up - 50f);
                radian = 0.0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void StateUpdate()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void MoveUpdate(int deltaTime)
        {
            radian += deltaTime * speed * direction;

            position.X = startPosition.X + (float)Math.Cos(radian) * circleRadius;
            position.Y = startPosition.Y + (float)Math.Sin(radian) * circleRadius * height;

        }

        protected override void Shoot()
        {
            if(shootCount == 0 && time > shootTime)
            {
                Vector2 pPos = player.GetPosition();
                Vector2 bulletDirection = new Vector2(pPos.X - position.X, pPos.Y - position.Y);
                bulletDirection.Normalize();
                parent.enemyBullets.Add(new StraightBullet(position, bulletDirection, index, 0.17f,VirtualEnemyBullet.SetColor.GREEN,true));

                shootCount++;
            }
        }

        protected override void ItemPop()
            {
                parent.items.Add(new Item.Item(position,popItem));
            }

        public override void Draw(OutPuts.Renderer renderer)
            {
                renderer.DrawTexture("e1",new Vector2(position.X - radius, position.Y - radius));
            }

    }
}
