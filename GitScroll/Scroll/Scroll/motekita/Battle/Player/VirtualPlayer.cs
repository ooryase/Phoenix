using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoot.GameSystem;

namespace Shoot.Battle.Player
{
    abstract class VirtualPlayer
    {
        protected Vector2 position;
        protected float radius;
        protected float speed;
        protected float shiftSpeed;
        protected Vector2 targetPos;

        protected int power;
        protected int life;
        protected int bomb;
        protected int score;

        protected int time;

        protected int shootInterval;
        protected int bombShootInterval;
        protected int shootCount;


        protected Rectangle drawRect;
        protected float drawRadius;


        public enum State
        {
            WAIT,
            MOVE = 1800,
            BOMB = 2000,
            PICHUN = 80,
            REBIRTH = 1000,
        }
        protected State state;
        protected bool shiftMove;
        protected bool invincible;
        protected int invincibleTime;


        protected List<PlayerBullet.VirtualPlayerBullet> bullets;

        private BattleMain.Battle parent;
        
        public VirtualPlayer(BattleMain.Battle battle)
        {
            parent = battle;
            position = new Vector2(BattleMain.BattleWindow.Center.X,BattleMain.BattleWindow.Down - 120);
            radius = 3f;
            StateSet(State.MOVE);
            power = 100;
            bullets = new List<PlayerBullet.VirtualPlayerBullet>();

            life = 3;
            bomb = 3;
            score = 0;

            invincible = false;
            invincibleTime = 0;
        }




        public void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
        }
        public void MainUpdate(int deltaTime)
        {
            StateUpdate(deltaTime);
            MoveUpdate(deltaTime);
            BulletUpdate();

            bullets.ForEach(b => b.MainUpdate(deltaTime, targetPos));

        }
        public void CollisionUpdate(List<Enemy.VirtualEnemy> enemies, List<EnemyBullet.VirtualEnemyBullet> enemyBullets)
        {
            PlayerCollision(enemies, enemyBullets);
            BulletCollision(enemies);
        }
        public void EndUpdate()
        {
            BulletDeleteUpdate();
        }

        protected void TimeUpdate(int deltaTime)
        {
            score += deltaTime / 2;
            time += deltaTime;
        }

        protected void StateUpdate(int deltaTime)
        {
            InvincibleUpdate(deltaTime);

            switch (state)
            {
                case State.WAIT:
                    break;
                case State.MOVE:
                    MoveStateUpdate();
                    break;
                case State.BOMB:
                    BombStateUpdate();
                    break;
                case State.PICHUN:
                    PichunStateUpdate();
                    break;
                case State.REBIRTH:
                    ReBirthUpdate();
                    break;
            }                 
        }

        protected void InvincibleUpdate(int deltaTime)
        {
            if(invincible)
            {
                invincibleTime -= deltaTime;
                if (invincibleTime < 0)
                    invincible = false;
            }
        }

        protected void Invincible(int time)
        {
            invincible = true;
            invincibleTime = time;
        }

        protected void MoveStateUpdate()
        {                

            shiftMove = InputContllorer.IsPress(Keys.LeftShift);

            if (InputContllorer.IsPush(Keys.X) && bomb > 0)
            {
                bomb--;
                Invincible((int)State.BOMB);
                StateSet(State.BOMB);
            }
        }

        protected void BombStateUpdate()
        {
            if (time > (int)State.BOMB)
            {
                parent.ConvertEnemyBulletToItem(true);
                StateSet(State.MOVE);
            }
        }
        protected void PichunStateUpdate()
        {
            if (state == State.PICHUN)
            {
                if (time > (int)State.PICHUN)
                {
                    Invincible((int)State.REBIRTH);
                    StateSet(State.REBIRTH);
                    life--;
                    bomb = 3;

                    if(life < 0)
                    {
                        parent.BattleEnd(score);
                    }
                    else
                    {
                        parent.ConvertEnemyBulletToItem(false);
                    }
                }
                if (InputContllorer.IsPush(Keys.X) && bomb > 1)
                {
                    Invincible((int)State.BOMB);
                    StateSet(State.BOMB);
                    bomb -= 2;
                }
            }

        }

        protected void ReBirthUpdate()
        {
            if (time > (int)State.REBIRTH)
            {
                Invincible((int)State.MOVE);
                StateSet(State.MOVE);
            }
        }


        protected void MoveUpdate(int deltaTime)
        {
            if (state == State.REBIRTH)
            {
                position.X = BattleMain.BattleWindow.Center.X;
                position.Y = BattleMain.BattleWindow.Down + radius * 2 - time / 10f;
                return;
            }

            if (state == State.WAIT || state == State.PICHUN)
                return;

            float s = (shiftMove) ? shiftSpeed : speed;

            drawRect.Y = 0;
            if (InputContllorer.IsPress(Keys.Left))
            {
                drawRect.Y = 64;
                LeftMove(s * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Right))
            {
                drawRect.Y = 128;
                RightMove(s * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Up))
                UpMove(s * deltaTime);

            if (InputContllorer.IsPress(Keys.Down))
                DownMove(s * deltaTime);


        }

        protected void RightMove(float l)
        {
            position.X += l;
            if(position.X > BattleMain.BattleWindow.Right - radius)
            {
                position.X = BattleMain.BattleWindow.Right - radius;
            }
        }
        protected void LeftMove(float l)
        {
            position.X -= l;
            if (position.X < BattleMain.BattleWindow.Left + radius)
            {
                position.X = BattleMain.BattleWindow.Left + radius;
            }
        }
        protected void UpMove(float l)
        {
            position.Y -= l;
            if (position.Y < BattleMain.BattleWindow.Up + radius)
            {
                position.Y = BattleMain.BattleWindow.Up + radius;
            }
        }
        protected void DownMove(float l)
        {
            position.Y += l;
            if (position.Y > BattleMain.BattleWindow.Down - radius)
            {
                position.Y = BattleMain.BattleWindow.Down - radius;
            }
        }


        protected void BulletUpdate()
        {
            if(state == State.BOMB)
            {
                if(time / bombShootInterval > shootCount)
                    ShootBomb();
            }
            else if(state == State.MOVE && InputContllorer.IsPress(Keys.Z))
            {
                if (time / shootInterval > shootCount)
                    ShootBullet();
            }
        }

        abstract protected void ShootBomb();

        abstract protected void ShootBullet();


        protected void PlayerCollision(List<Enemy.VirtualEnemy> enemies, List<EnemyBullet.VirtualEnemyBullet> enemyBullets)
        {
            if (IsInvincible())
            {
                return;
            }

            if (enemies.Any(e => e.DistanceSquared(position) < e.PowTwoRadius(radius)))
            {
                parent.SetTimeSpeed(BattleMain.Battle.TimeSpeed.VERYSLOW, 960);
                Invincible((int)State.PICHUN);
                StateSet(State.PICHUN);
                return;
            }

            var collisionEnemyBullets = enemyBullets.Where(eb => eb.DistanceSquared(position) < eb.PowTwoRadius(radius));

            if (collisionEnemyBullets.Count() > 0)
            {
                parent.SetTimeSpeed(BattleMain.Battle.TimeSpeed.VERYSLOW, 960);
                Invincible((int)State.PICHUN);
                StateSet(State.PICHUN);
                foreach (var ceb in collisionEnemyBullets)
                {
                    ceb.Dead();
                }
            }
        }

        protected void BulletCollision(List<Enemy.VirtualEnemy> enemies)
        {
            foreach(var pb in bullets)
            {
                pb.CollisionUpdate(enemies);
//                var collisionEnemies = enemies.Where(e => e.DistanceSquared(pb.Get) < e.PowTwoRadius(pb.radius));

            }


        }

        protected void BulletDeleteUpdate()
        {
            bullets.RemoveAll(b => b.IsDead());
        }

        protected void StateSet(State s)
        {
            state = s;
            time = 0;
            shootCount = 0;
        }

        abstract public void Draw(OutPuts.Renderer renderer);

        public void DrawParam(OutPuts.Renderer renderer)
        {
            var drawP = BattleMain.BattleWindow.DrawParam;
            var dPRight = new Vector2(drawP.X + 100f, drawP.Y);
            renderer.DrawFont("k8x12L", "SCORE",drawP);
            renderer.DrawFont("k8x12L", score.ToString(),  dPRight);

            drawP.Y += 50f;
            dPRight.Y += 50f;

            var r = new Rectangle(96, 64, 32, 32);

            renderer.DrawFont("k8x12L", "LIFE", drawP);
            for (int i = 0; i < life; i++)
            {
                renderer.DrawTexture("32tex", new Vector2(dPRight.X + i * 36f, dPRight.Y), r, Color.White);
            }

            drawP.Y += 50f;
            dPRight.Y += 50f;

            r.X -= 32;

            renderer.DrawFont("k8x12L", "MAGIC", drawP);
            for (int i = 0 ;i < bomb;i++)
            {
                renderer.DrawTexture("32tex",  new Vector2(dPRight.X + i * 36f ,dPRight.Y), r, Color.White);
            }

            drawP.Y += 50f;
            dPRight.Y += 50f;

            renderer.DrawFont("k8x12L", "POWER", drawP);
            renderer.DrawFont("k8x12L", power.ToString(), dPRight);

            if(state == State.PICHUN)
                renderer.DrawTexture("HitRed", new Vector2(BattleMain.BattleWindow.Left,BattleMain.BattleWindow.Up));

        }

        public void AddItem(Item.Item.ItemState itemState ,int stateNumbers)
        {
            switch (itemState)
            {
                case Item.Item.ItemState.SMALL_SCORE:
                    score += stateNumbers;
                    break;
                case Item.Item.ItemState.SCORE:
                    score += stateNumbers;
                    break;
                case Item.Item.ItemState.POWER:
                    power += stateNumbers;
                    break;
                case Item.Item.ItemState.LIFE:
                    life += stateNumbers;
                    break;
                case Item.Item.ItemState.BOMB:
                    bomb += stateNumbers;
                    break;
            }
        }

        public bool IsVacuumPos()
        {
            return position.Y < BattleMain.BattleWindow.ItemVacuumUp;
        }

        public bool IsInvincible()
        {
            return invincible;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetRadius()
        {
            return radius;
        }

        public int GetScore()
        {
            return score;
        }


        public void SetTargetPos(Vector2 tPos)
        {
            targetPos = tPos;
        }
    }
}
