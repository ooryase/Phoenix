using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoot.Battle.EnemyBullet;

namespace Shoot.Battle.Enemy
{
    class TestEnemy : VirtualEnemy
    {
        private Vector2 direction;
        private float speed;
        private int startTime;
        private int returnTime;

        private float circleRadius;

        private int enableShootCount;
        private int shootCount;
        private int enableShootInterbal;

        private Item.Item.ItemState popItem;

        public TestEnemy(BattleMain.Battle battle, Player.VirtualPlayer virtualPlayer, int indexNum, Vector2 startPosision, Item.Item.ItemState itemState) : base(battle, virtualPlayer,indexNum)
        {

            position = startPosision;
            radius = 32f;

            speed = 0.14f;
            startTime = 650;
            returnTime = 5600;
            circleRadius = 3.3f;

            popItem = itemState;

            hp = 130000;
            enableShootCount = 0;
            enableShootInterbal = 500;

            isBulletToItem = true;

            if(startPosision.X < BattleMain.BattleWindow.Center.X)
            {
                direction = new Vector2(0.5f, 0.5f);
            }
            else
            {
                direction = new Vector2(-0.5f, 0.5f);
            }
        }
        protected override void StateUpdate()
        {
        }

        protected override void MoveUpdate(int deltaTime)
        {
            if (time < startTime)
            {
                position.X += direction.X * circleRadius * speed * deltaTime;
                position.Y += direction.Y * speed * deltaTime;
            }
            else
            {
                position.X += direction.X * (float)Math.Cos((time - startTime) / 450.0) * circleRadius * speed * deltaTime;
                position.Y += direction.Y * speed * deltaTime;
            }

        }

        protected override void Shoot()
        {
            if (time < startTime || time > returnTime)
                return;

            enableShootCount = (time - startTime) / enableShootInterbal;
        
            if (shootCount < enableShootCount)
            {
                shootCount++;

                double bulletNumbers = 10;
                for (double i = 0.0; i < bulletNumbers; i++)
                {
                    Vector2 bulletDirection = new Vector2(
                        (float)Math.Cos(shootCount + i / bulletNumbers * MathHelper.TwoPi),
                        (float)Math.Sin(shootCount + i / bulletNumbers * MathHelper.TwoPi));
                    parent.enemyBullets.Add(new StraightBullet(position,bulletDirection, index, 0.37f, VirtualEnemyBullet.SetColor.BLUE));
                }
            }
        }

        protected override void ItemPop()
        {
            parent.items.Add(new Item.Item(position,Item.Item.ItemState.SCORE));
            parent.items.Add(new Item.Item(position,Item.Item.ItemState.POWER));
            parent.items.Add(new Item.Item(position, popItem));


        }

        public override void Draw(OutPuts.Renderer renderer)
        {
            renderer.DrawTexture("e3",new Vector2(position.X - radius,position.Y - radius));
        }

    }
}
