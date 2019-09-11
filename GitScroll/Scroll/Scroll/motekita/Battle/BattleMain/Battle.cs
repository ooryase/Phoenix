using Microsoft.Xna.Framework;
using Shoot.Battle.Enemy;
using Shoot.Battle.EnemyBullet;
using Shoot.Battle.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.BattleMain
{
    class Battle : Scene
    {
        private Player.VirtualPlayer player;

        private List<Enemy.VirtualEnemy> enemies;
        internal List<EnemyBullet.VirtualEnemyBullet> enemyBullets;
        internal List<Item.Item> items;
        internal List<EffectSystem.VirtualEffectSystem> effectSystems;

        private Queue<EnemyPopTimeLine> enemyPopTimeLines;
        private int enemyPopTimer;
        private int enemyIndex;

        internal enum TimeSpeed
        {
            NORMAL = 1,
            HALF = 2,
            SLOW = 4,
            VERYSLOW = 8,
        }
        private TimeSpeed timeSpeed;
        private int halfTimeTimer;
        private int slowTimeTimer;
        private int verySlowTimeTimer;

        private int time;
        private int deltaTime;

        public Battle(GameMain gameMain) : base(gameMain)
        {
            timeSpeed = TimeSpeed.NORMAL;
            time = 0;

            enemyIndex = 0;
            enemyPopTimer = 0;

            halfTimeTimer = 0;
            slowTimeTimer = 0;
            verySlowTimeTimer = 0;

            player = new Player.TestPlayer(this);
            enemies = new List<VirtualEnemy>();
            enemyBullets = new List<VirtualEnemyBullet>();
            items = new List<Item.Item>();
            effectSystems = new List<EffectSystem.VirtualEffectSystem>();

            EnemyTimeLineSet();
        }

        public override void Update(GameTime gameTime)
        {
            StartUpdate(gameTime);
            MainUpdate();
            CollisionUpdate();
            EndUpdate();            
        }
        private void StartUpdate(GameTime gameTime)
        {
            TimeUpdate(gameTime);
            EnemyPop();

            player.StartUpdate(deltaTime);
            PlayerTargetSet();
            enemies.ForEach(e => e.StartUpdate(deltaTime));
            enemyBullets.ForEach(eb => eb.StartUpdate(deltaTime));
            items.ForEach(i => i.StartUpdate(deltaTime));
            effectSystems.ForEach(es => es.startUpdate(deltaTime));

        }
        private void MainUpdate()
        {
            player.MainUpdate(deltaTime);
            enemies.ForEach(e => e.MainUpdate(deltaTime));
            enemyBullets.ForEach(eb => eb.MainUpdate(deltaTime, player.GetPosition()));
            items.ForEach(i => i.MainUpdate(deltaTime , player.GetPosition()));
            effectSystems.ForEach(es => es.MainUpdate(deltaTime));
        }
        private void CollisionUpdate()
        {
            player.CollisionUpdate(enemies, enemyBullets);
            enemies.ForEach(e => e.CollisionUpdate());
            enemyBullets.ForEach(eb => eb.CollisionUpdate());
            items.ForEach(i => i.CollisionUpdate(player));

        }
        private void EndUpdate()
        {
            player.EndUpdate();
            enemies.ForEach(e => e.EndUpdate());
            enemyBullets.ForEach(eb => eb.EndUpdate());
            enemies.RemoveAll(e => e.IsDead());
            enemyBullets.RemoveAll(eb => eb.IsDead());
            items.ForEach(i => i.EndUpdate(player.IsVacuumPos()));
            items.RemoveAll(i => i.IsDead());
            effectSystems.ForEach(es => es.EndUpdate());
            effectSystems.RemoveAll(es => es.IsClose());
        }

        private void EnemyTimeLineSet()
        {
            enemyPopTimeLines = new Queue<EnemyPopTimeLine>();
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(null, 2000));

            for (int i = 0;i < 5;i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.LEFT, (Item.Item.ItemState)(1 + i % 2)), 400));
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.LEFT, Item.Item.ItemState.SCORE), 3000));

            for (int i = 0; i < 5; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.RIGHT, (Item.Item.ItemState)(1 + i % 2)), 400));
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.RIGHT, Item.Item.ItemState.SCORE), 3000));

            for (int i = 0; i < 5; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.LEFT, (Item.Item.ItemState)(1 + i % 2)), 0));
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.RIGHT, (Item.Item.ItemState)(1 + i % 2)), 400));
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.LEFT, Item.Item.ItemState.SCORE), 0));
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobWalk(this, player, enemyIndex, NoobWalk.ActivePattern.RIGHT, Item.Item.ItemState.SCORE), 6000));
            enemyIndex++;



            for (int i = 0; i < 5; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Left + 70f + 100f * i), 0));
                enemyIndex++;
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Left + 570f), 6000));
            enemyIndex++;

            for (int i = 0; i < 5; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Left + 70f + 100f * i), 0));
                enemyIndex++;
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Left + 570f), 6000));
            enemyIndex++;


            for (int i = 0;i < 2;i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new TestEnemy(this, player, enemyIndex, new Vector2(BattleWindow.Left - 50f, BattleWindow.Up), (Item.Item.ItemState)(1 + i * 3)), 0));
                enemyIndex++;
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new TestEnemy(this, player, enemyIndex, new Vector2(BattleWindow.Right + 50f, BattleWindow.Up),(Item.Item.ItemState)(1 + i * 3)), 7000));
                enemyIndex++;
            }
            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(null, 4000));

            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new TestEnemy(this, player, enemyIndex, new Vector2(BattleWindow.Right + 50f, BattleWindow.Up), Item.Item.ItemState.LIFE), 0));
            enemyIndex++;

            for (int i = 0; i < 6; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Left + 70f + 100f * i), 400));
                enemyIndex++;
            }

            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(null, 4000));

            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new TestEnemy(this, player, enemyIndex, new Vector2(BattleWindow.Left - 50f, BattleWindow.Up), Item.Item.ItemState.LIFE), 0));
            enemyIndex++;

            for (int i = 0; i < 6; i++)
            {
                enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new NoobContinuousFire(this, player, enemyIndex, BattleWindow.Right - 70f - 100f * i), 400));
                enemyIndex++;
            }

            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(null, 8000));



            enemyPopTimeLines.Enqueue(new EnemyPopTimeLine(new Boss(this, player, enemyIndex), 3000000));

        }

        private void TimeUpdate(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds / (int)timeSpeed; ;
            enemyPopTimer -= deltaTime;

            time += deltaTime;
            TimeSpeedUpdate(gameTime);
        }

        private void TimeSpeedUpdate(GameTime gameTime)
        {
            int t = gameTime.ElapsedGameTime.Milliseconds;

            switch (timeSpeed)
            {
                case TimeSpeed.VERYSLOW:
                    verySlowTimeTimer -= t;
                    slowTimeTimer -= t * (int)TimeSpeed.SLOW / (int)TimeSpeed.VERYSLOW;
                    halfTimeTimer -= t * (int)TimeSpeed.HALF / (int)TimeSpeed.VERYSLOW;
                    if (verySlowTimeTimer < 0)
                        timeSpeed = TimeSpeed.SLOW;
                    break;
                case TimeSpeed.SLOW:
                    slowTimeTimer -= t;
                    halfTimeTimer -= t * (int)TimeSpeed.HALF / (int)TimeSpeed.SLOW;
                    if (slowTimeTimer < 0)
                        timeSpeed = TimeSpeed.HALF;
                    break;
                case TimeSpeed.HALF:
                    halfTimeTimer -= t;
                    if (halfTimeTimer < 0)
                        timeSpeed = TimeSpeed.NORMAL;
                    break;
            }

        }

        private void EnemyPop()
        {            
            while(enemyPopTimer <= 0)
            {
                if (enemyPopTimeLines.Count == 0)
                    return;

                var e = enemyPopTimeLines.Dequeue();
                if(e.enemy != null)
                    enemies.Add(e.enemy);
                enemyPopTimer = e.nextTime;
            }
        }


        private void PlayerTargetSet()
        {
            var a = enemies.Count;

            if (enemies.Count == 0)
            {
                player.SetTargetPos(new Vector2((BattleWindow.Left + BattleWindow.Right) / 2, BattleWindow.DeleteUp));
            }
            else
            {
                Vector2 p = player.GetPosition();

                var minId = enemies.Select((val, i) => new { V = val, I = i }).Aggregate((min, working) => (min.V.DistanceSquared(p) > working.V.DistanceSquared(p)) ? working : min).I;

                player.SetTargetPos(enemies[minId].GetPosition());
            }
        }

        internal void ConvertEnemyBulletToItem(bool toItem)
        {
            foreach (var ceb in enemyBullets)
            {
                ceb.Dead();
                if(toItem)
                    items.Add(new Item.Item(ceb.GetPosition(), Item.Item.ItemState.SMALL_SCORE, true));
            }
        }

        internal void ConvertEnemyBulletToItem(int index)
        {
            var convertEnemyBullets = enemyBullets.Where(eb => eb.GetIndex() == index);

            foreach(var ceb in convertEnemyBullets)
            {
                ceb.Dead();
                items.Add(new Item.Item(ceb.GetPosition(),Item.Item.ItemState.SMALL_SCORE,true));
            }
        }

        internal void SetTimeSpeed(TimeSpeed ts,int t)
        {
            switch(ts)
            {
                case TimeSpeed.HALF:
                    timeSpeed = (ts > timeSpeed) ? ts : timeSpeed;
                    halfTimeTimer = (t > halfTimeTimer) ? t : halfTimeTimer;
                    break;
                case TimeSpeed.SLOW:
                    timeSpeed = (ts > timeSpeed) ? ts : timeSpeed;
                    slowTimeTimer = (t > slowTimeTimer) ? t : slowTimeTimer;
                    break;
                case TimeSpeed.VERYSLOW:
                    timeSpeed = (ts > timeSpeed) ? ts : timeSpeed;
                    verySlowTimeTimer = (t > verySlowTimeTimer) ? t : verySlowTimeTimer;
                    break;
            }
        }

        internal void BattleEnd(int score)
        {
            isClose = true;
            parent.AddSceneReservation(new Result.Result(parent, score));
        }

        public override void Draw(OutPuts.Renderer renderer)
        {
            DrawBack(renderer);

            player.Draw(renderer);
            enemies.ForEach(e => e.Draw(renderer));
            enemyBullets.ForEach(eb => eb.Draw(renderer));
            items.ForEach(i => i.Draw(renderer));
            effectSystems.ForEach(es => es.Draw(renderer));

            DrawFrame(renderer);
            player.DrawParam(renderer);

        }

        private void DrawBack(OutPuts.Renderer renderer)
        {
            var v = new Vector2(BattleWindow.Left, BattleWindow.Up);


            renderer.DrawTexture("ie", new Vector2(BattleWindow.Left,BattleWindow.Up - 860f + time / 16 % 860f));
            renderer.DrawTexture("ie", new Vector2(BattleWindow.Left, BattleWindow.Up + time / 16 % 860f));
            renderer.DrawTexture("smog", new Vector2(BattleWindow.Left, BattleWindow.Up + 860f - time / 8 % 860f));
            renderer.DrawTexture("smog", new Vector2(BattleWindow.Left, BattleWindow.Up - time / 8 % 860f));

        }

        private void DrawFrame(OutPuts.Renderer renderer)
        {
            renderer.DrawTexture("TestBattleFrame", Vector2.Zero);
        }
    }

}
