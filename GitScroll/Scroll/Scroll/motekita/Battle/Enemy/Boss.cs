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
    class Boss : VirtualEnemy
    {
        private enum Phase
        {
            POP = 1000,
            ATTACK1,
            ATTACK2,
            SUMMON_WING,
            DEAD = 5000

        }
        private Phase phase;

        private uint shootCount;

        private double shootDirection;

        private uint attack1enableShootCount;
        private readonly int attack1ShootInterval;

        private uint attack2enableShootCount;
        private readonly int attack2ShootInterval;

        private enum ShootState
        {
            CHARGE,
            SHOOT,
            RELOAD,
        }
        private ShootState shootState;

        private Rectangle wingDrawRect;

        public Boss(BattleMain.Battle battle, VirtualPlayer virtualPlayer, int indexNum) : base(battle, virtualPlayer, indexNum)
        {
            radius = 64f;

            phase = Phase.POP;

            hp = 20000000;

            shootCount = 0;

            shootDirection = 1.0;

            attack1enableShootCount = 2;
            attack1ShootInterval = 125;

            attack2enableShootCount = 20;
            attack2ShootInterval = 250;

            shootState = ShootState.RELOAD;

            isBulletToItem = true;

            wingDrawRect = new Rectangle(0, 0, 128, 128);
        }


        protected override void ItemPop()
        {
            throw new NotImplementedException();
        }

        protected override void StateUpdate()
        {
            switch (phase)
            {
                case Phase.POP:
                    PopState();
                    break;
                case Phase.ATTACK1:
                    Attack1State();
                    break;
                case Phase.ATTACK2:
                    Attack2State();
                    break;
                case Phase.SUMMON_WING:
                    break;
                case Phase.DEAD:
                    DeadState();
                    break;
            }

        }

        private void PopState()
        {
            if (time > (int)Phase.POP)
                phase = Phase.ATTACK1;
        }

        private void Attack1State()
        {
            if(hp < 17500000)
            {
                phase = Phase.ATTACK2;
                parent.ConvertEnemyBulletToItem(index);
                shootCount = 0;
                time = 0;
                shootState = ShootState.RELOAD;

                parent.items.Add(new Item.Item(position, Item.Item.ItemState.LIFE));
                parent.items.Add(new Item.Item(position, Item.Item.ItemState.BOMB));
                parent.items.Add(new Item.Item(position, Item.Item.ItemState.BOMB));
                parent.items.Add(new Item.Item(position, Item.Item.ItemState.POWER));
                parent.items.Add(new Item.Item(position, Item.Item.ItemState.POWER));
                for(int i = 0; i < 20;i++)
                {
                    parent.items.Add(new Item.Item(position, Item.Item.ItemState.SCORE));

                }
            }
        }

        private void Attack2State()
        {
            if (hp < 14200000)
            {
                //phase = Phase.SUMMON_WING;
                phase = Phase.DEAD;
                parent.ConvertEnemyBulletToItem(index);
                shootCount = 0;
                time = 0;
                shootState = ShootState.RELOAD;
                for (int i = 0; i < 30; i++)
                {
                    parent.items.Add(new Item.Item(position, Item.Item.ItemState.SCORE));

                }
            }
        }

        private void DeadState()
        {
            if(time > (int)Phase.DEAD)
            {
                parent.BattleEnd(player.GetScore());
            }
        }

        protected override void MoveUpdate(int deltaTime)
        {
            switch (phase)
            {
                case Phase.POP:
                    PopMove(deltaTime);
                    break;
                case Phase.ATTACK1:
                    break;
                case Phase.ATTACK2:
                    break;
                case Phase.SUMMON_WING:
                    break;
            }

        }



        private void PopMove(int deltaTime)
        {
            double r = (1000 - time) * 0.001 * MathHelper.PiOver2 + MathHelper.PiOver2;
            position.X = (float)Math.Cos(r) * 350f + BattleWindow.Center.X; 
            position.Y = (float)Math.Sin(-r) * 350f + BattleWindow.Center.Y + 50f;
        }

        protected override void Shoot()
        {
            switch (phase)
            {
                case Phase.POP:
                    break;
                case Phase.ATTACK1:
                    Attack1Shoot();
                    break;
                case Phase.ATTACK2:
                    Attack2Shoot();
                    break;
                case Phase.SUMMON_WING:
                    break;
                case Phase.DEAD:
                    DeadShoot();
                    break;
            }
        }

        private void Attack1Shoot()
        {
            var t = time % 7000;

            if (t < 2000)
            {
                shootState = ShootState.RELOAD;
                shootCount = 0;
            }
            else if (t < 4000)
            {
                if(shootState == ShootState.RELOAD)
                {
                    parent.effectSystems.Add(new EffectSystem.SpellCharge(position,EffectSystem.Particle.State.CONVERGENCE));
                    shootState = ShootState.CHARGE;
                }
            }
            else if(4000 < t)
            {
                shootState = ShootState.SHOOT;
                if(shootCount < attack1enableShootCount && (t - 4000) / attack1ShootInterval >= shootCount)
                {
                    for (int s = 0; s < 7; s++)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                parent.enemyBullets.Add(
                                    new SpinToStraight(position, Vector2.Zero, index, 0.1f + i * 0.07f, (VirtualEnemyBullet.SetColor)s,
                                    s / 7.0 * MathHelper.TwoPi, shootDirection, (SpinToStraight.Status)j));
                            }
                        }
                    }
                    shootCount++;
                    shootDirection *= -1.0;
                }

            }
        }

        private void Attack2Shoot()
        {
            var t = time % 8000;

            if (t < 2000)
            {
                shootState = ShootState.RELOAD;
                shootCount = 0;
            }
            else if (t < 4000)
            {
                if (shootState == ShootState.RELOAD)
                {
                    parent.effectSystems.Add(new EffectSystem.SpellCharge(position, EffectSystem.Particle.State.CONVERGENCE));
                    shootState = ShootState.CHARGE;
                }
            }
            else if (4000 < t)
            {
                if (shootCount < attack2enableShootCount && (t - 4000) / attack2ShootInterval >= shootCount)
                {
                    for (int i = 0; i < 17; i++)
                    {
                        parent.enemyBullets.Add(
                            new SpinToCurve(position, Vector2.Zero, index, 0.15f, (VirtualEnemyBullet.SetColor)(shootCount % 7), (i / 17.0) * MathHelper.TwoPi + ((time + 10000) / 350.0), shootDirection));
                    }
                    shootCount++;
                    shootDirection *= -1.0;
                }

            }
        }

        private void DeadShoot()
        {
            if (shootState == ShootState.RELOAD)
            {
                parent.effectSystems.Add(new EffectSystem.SpellCharge(position, EffectSystem.Particle.State.DIVERGENCE));
                shootState = ShootState.CHARGE;
            }

        }



        public override void Draw(Renderer renderer)
        {
            if (phase != Phase.DEAD)
            {
                float y = position.Y - radius + 10f * (float)Math.Sin(time / 400.0);
                renderer.DrawTexture("maho2", new Vector2(position.X, y + radius), 1.2f, Color.Yellow * 0.5f, time / 500f, new Vector2(radius, radius));
                renderer.DrawTexture("boss", new Vector2(position.X - radius, y));

            }
            else
            {
                float y = position.Y - radius + 10f * (float)Math.Sin(time / 400.0);
                float alpha = ((float)Phase.DEAD - time) / (float)Phase.DEAD;
                renderer.DrawTexture("maho2", new Vector2(position.X, y + radius), 1.2f, Color.Black * 0.5f * alpha, time / 500f, new Vector2(radius, radius));
                renderer.DrawTexture("boss", new Vector2(position.X - radius, y),  alpha);

            }

            /*if(phase >= Phase.SUMMON_WING)
            {
                wingDrawRect.X = time / 100 % 4 * 128;

                if (phase == Phase.SUMMON_WING && time < 400)
                {
                    wingDrawRect.Y = 0;
                }
                else
                {
                    wingDrawRect.Y = 128;
                }

                renderer.DrawTexture("wing", new Vector2(position.X - radius + 64f, y), wingDrawRect, Color.White);
            }*/

        }



    }
}
