using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shoot.Battle.BattleMain;
using Shoot.Battle.Player;
using Shoot.OutPuts;
using Shoot.Battle.EnemyBullet;

namespace Shoot.Battle.Enemy
{
    class NoobContinuousFire : VirtualEnemy
    {
        private float speed;

        private int headTime;
        private int middleTime;

        private int shootTime;
        private int shootInterval;
        private uint shootCount;
        private uint enableShootCount;

        public NoobContinuousFire(BattleMain.Battle battle, VirtualPlayer virtualPlayer, int indexNum, float x) : base(battle, virtualPlayer, indexNum)
        {
            hp = 65000;
            radius = 32f;

            position = new Vector2(x, BattleWindow.Up - 50f);
            speed = 6f;

            headTime = 600;
            middleTime = 5000;

            shootTime = 1600;
            shootInterval = 80;
            shootCount = 0;
            enableShootCount = 10;

            isBulletToItem = false;
        }
        protected override void StateUpdate()
        {
        }

        protected override void MoveUpdate(int deltaTime)
        {
            if(time < headTime)
            {
                position.Y += speed;
                speed -= (float)deltaTime / headTime * 6f;
            }
            else if(time > middleTime)
            {
                speed -= (float)deltaTime / headTime * 6f;
                position.Y += speed;
            }

        }

        protected override void Shoot()
        {
            if(time > shootTime &&
                shootCount < enableShootCount && (time - shootTime) / shootInterval >=  shootCount)
            {
                Vector2 pPos = player.GetPosition();
                Vector2 bulletDirection = new Vector2(pPos.X - position.X, pPos.Y - position.Y);
                bulletDirection.Normalize();
                parent.enemyBullets.Add(new StraightBullet(position, bulletDirection, index, 0.17f + shootCount * 0.09f,VirtualEnemyBullet.SetColor.PURPLE));
                shootCount++;
            }
        }
        protected override void ItemPop()
        {
            parent.items.Add(new Item.Item(position,Item.Item.ItemState.SCORE));
            parent.items.Add(new Item.Item(position, Item.Item.ItemState.SMALL_SCORE));
            parent.items.Add(new Item.Item(position, Item.Item.ItemState.SMALL_SCORE));
            parent.items.Add(new Item.Item(position, Item.Item.ItemState.SMALL_SCORE));

        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("e2",new Vector2(position.X - radius, position.Y - radius));
        }

    }
}
