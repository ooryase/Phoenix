using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scroll.GameSystem;
using Scroll.Output;
using Scroll.Battle.Field;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Scroll.Battle
{
    class Battle : Scene
    {
        private Player.Player player;

        private List<Enemy.VirtualEnemy> enemies;
        private List<Arts.Fire> arts;
        //internal List<EffectSystem.VirtualEffectSystem> effectSystems;

        private List<Field.Field> fields;

        private int[,] blocksData;
        private List<Field.Block> blocks;
        private float blockSize;

        private Field.BlockCollision blockCollision;

        private List<VirtualObject> objects;

        private Matrix projection;

        private Vector3 cameraLookPos;
        private float cameraLength;
        private float cameraDistinationLength;
        private float cameraLastLength;
        private int cameraMoveTime;
        private int cameraMoveCont;
        private float cameraLengthDefault;

        public RotateManager rotateManager;
        //private Vector3 yawPitchRoll;

        private Matrix view;


        internal enum State
        {
            NORMAL,
            PAUSE,
            EVENT,
            CLEAR
        }
        private State state;

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

        public Matrix Projection { get => projection; set => projection = value; }
        public Matrix View { get => view; set => view = value; }
        public Vector3 CameraLookPos { get => cameraLookPos; private set => cameraLookPos = value; }
        internal State BattleState { get => state; set => state = value; }

        public Battle(GameMain gameMain,Renderer renderer)
            : base(gameMain,renderer)
        {
            timeSpeed = TimeSpeed.NORMAL;
            time = 0;

            halfTimeTimer = 0;
            slowTimeTimer = 0;
            verySlowTimeTimer = 0;

            rotateManager = new RotateManager();
            rotateManager.cameraRotate = Vector3.Zero;

            state = State.NORMAL;

            cameraLengthDefault = 5f;
            cameraLength = cameraLengthDefault;
            Projection = CreateProjection();
            CameraLookPos = Vector3.Zero;
            View = CreateCameraView();

            player = new Player.Player(this,renderer);
            enemies = new List<Enemy.VirtualEnemy>();
            //effectSystems = new List<EffectSystem.VirtualEffectSystem>();
            arts = new List<Arts.Fire>();
            fields = new List<Field.Field>();
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.BACK));
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.FRONT));
            blocks = new List<Field.Block>();
            objects = new List<VirtualObject>();
            MapSet();
        }

        private Matrix CreateProjection()
        {
            var projection = Matrix.CreatePerspectiveFieldOfView
            (
                MathHelper.ToRadians(45),   //視野の角度。ここでは45°
                renderer.GraphicsDevice.Viewport.AspectRatio,//画面のアスペクト比(=横/縦)
                1,      //カメラからこれより近い物体は画面に映らない
                100     //カメラからこれより遠い物体は画面に映らない
            );

            return projection;
        }

        private Matrix CreateCameraView()
        {
            var view = Matrix.CreateLookAt
                (
                CameraLookPos + rotateManager.YawPitchRoll(Vector3.UnitZ) * cameraLength,
                CameraLookPos,
                Vector3.UnitY
                );
            return view;
        }

        private void MapSet()
        {
            blockSize = 0.5f;

            CSVReader csvReader = new CSVReader();
            blocksData = csvReader.GetIntMatrix("EnemyMax2.csv");

            for(int y = 0;y < blocksData.GetLength(0);y++)
            {
                for (int x = 0; x < blocksData.GetLength(1); x++)
                {
                    if (blocksData[y, x] == -1)
                        continue;
                    if(blocksData[y,x] >= 0 &&
                       blocksData[y,x] <= 11)
                        blocks.Add(new Field.Block(this,renderer,new Vector3(x * blockSize + blockSize /2f,y * blockSize + blockSize / 2f,0)
                            , (Field.Block.BlockName)blocksData[y,x]));
                    else if (blocksData[y, x] == 12)
                        enemies.Add(new Enemy.Wata(this, renderer,player, new Vector3(x * blockSize +blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if (blocksData[y, x] == 13)
                        enemies.Add(new Enemy.Wolf(this, renderer,player, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if (blocksData[y, x] == 14)
                        enemies.Add(new Enemy.Dragon(this, renderer,player, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if(blocksData[y, x] == 15)
                        objects.Add(new GoalBlock(this, renderer, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)));
                }

            }

            blockCollision = new Field.BlockCollision();
        }

        /// <summary>
        /// バトルシーンでのUpdateは処理内容に応じて
        /// StartUpdate
        /// MoveUpdate
        /// CollisionUpdate
        /// EndUpdate
        /// DrawUpdate
        /// の順に分けて実行する
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            StartUpdate(gameTime);
            MoveUpdate();
            CollisionUpdate();
            EndUpdate();
            DrawUpdate();
        }
        /// <summary>
        /// 時間経過の処理
        /// 入力処理
        /// ステータス処理
        /// 基本的な処理はここで行う
        /// </summary>
        /// <param name="gameTime"></param>
        private void StartUpdate(GameTime gameTime)
        {
            TimeUpdate(gameTime);

            player.StartUpdate(deltaTime);
            //PlayerTargetSet();
            enemies.ForEach(e => e.StartUpdate(deltaTime));
            arts.ForEach(a => a.StartUpdate(deltaTime));
            //effectSystems.ForEach(es => es.startUpdate(deltaTime));
            objects.ForEach(o => o.StartUpdate(deltaTime));

            InputYawPitchRoll();

        }

        /// <summary>
        /// コントローラ入力に応じてカメラ角度を変更する
        /// </summary>
        private void InputYawPitchRoll()
        {
            rotateManager.cameraRotate.Y -= InputContllorer.StickRightX() / 100f;
            rotateManager.cameraRotate.X += InputContllorer.StickRightY() / 100f;

            rotateManager.cameraRotate.X = (rotateManager.cameraRotate.X > BattleWindow.CameraYawRight) ? BattleWindow.CameraYawRight : rotateManager.cameraRotate.X;
            rotateManager.cameraRotate.X = (rotateManager.cameraRotate.X < BattleWindow.CameraYawLeft) ? BattleWindow.CameraYawLeft : rotateManager.cameraRotate.X;

            rotateManager.cameraRotate.Y = (rotateManager.cameraRotate.Y > BattleWindow.CameraPitchDown) ? BattleWindow.CameraPitchDown : rotateManager.cameraRotate.Y;
            rotateManager.cameraRotate.Y = (rotateManager.cameraRotate.Y < BattleWindow.CameraPitchUp) ? BattleWindow.CameraPitchUp : rotateManager.cameraRotate.Y;

        }


        /// <summary>
        /// 各ObjectのMoveUpdateはここで行う
        /// </summary>
        private void MoveUpdate()
        {
            player.MoveUpdate(deltaTime);
            enemies.ForEach(e => e.MoveUpdate(deltaTime));
            arts.ForEach(a => a.MoveUpdate(deltaTime, player.Position));
        }

        /// <summary>
        /// 各Objectが当たった場合は各ObjectのOnCollisionEnterが呼ばれる
        /// </summary>
        private void CollisionUpdate()
        {

            foreach(var e in enemies)
            {
                if(Vector3.DistanceSquared(player.Position,e.Position) < (float)Math.Pow( player.Scale + e.Scale,2.0))
                    e.OnCollisionEnter(player);
            }

            foreach (var o in objects)
            {
                if (Vector3.DistanceSquared(player.Position, o.Position) < (float)Math.Pow(player.Scale + o.Scale, 2.0))
                    player.OnCollisionEnter(o);
            }

            foreach (var b in blocks)
            {
                var c = blockCollision.CheckCollision(b, player);
                if (c != Vector3.Zero) 
                    player.OnCollisionBlock(c,
                        b.BName == Block.BlockName.RIGHT_UP ||
                        b.BName == Block.BlockName.LEFT_UP ||
                        b.BName == Block.BlockName.UP);

                foreach (var e in enemies)
                {
                    var cc = blockCollision.CheckCollision(b, e);
                    if (cc != Vector3.Zero)
                        e.OnCollisionBlock(cc,
                            b.BName == Block.BlockName.RIGHT_UP ||
                            b.BName == Block.BlockName.LEFT_UP ||
                            b.BName == Block.BlockName.UP);
                }
            }
        }


        /// <summary>
        /// Objectの消去
        /// 最後に行うべき特殊処理はここで行う
        /// </summary>
        private void EndUpdate()
        {
            //player.EndUpdate();
            //enemies.ForEach(e => e.EndUpdate());
            enemies.RemoveAll(e => e.Delete);
            //effectSystems.ForEach(es => es.EndUpdate());
            //effectSystems.RemoveAll(es => es.IsClose());
            arts.RemoveAll(a => a.Delete);

            objects.RemoveAll(o => o.Delete);
        }


        /// <summary>
        /// 頂点位置の変更
        /// シェーダーパラメータの設定はここで行う
        /// </summary>
        private void DrawUpdate()
        {
            CameraUpdate();

            fields.ForEach(f => f.DrawUpdate());
            player.DrawUpdate();
            enemies.ForEach(e => e.DrawUpdate());
            arts.ForEach(a => a.DrawUpdate());
            blocks.ForEach(b => b.DrawUpdate());
            objects.ForEach(o => o.DrawUpdate());
        }

        /// <summary>
        /// カメラ座標の更新
        /// </summary>
        private void CameraUpdate()
        {
            CameraLengthMove();

            if(state != State.CLEAR)
                cameraLookPos = player.Position;

            View = CreateCameraView();
        }

        private void CameraLengthMove()
        {
            if (cameraMoveTime == 0)
                return;

            cameraMoveCont += deltaTime;

            var rr = 1f - cameraMoveCont / (float)cameraMoveTime;
            cameraLength = cameraDistinationLength * (1f - rr * rr) + cameraLastLength * rr * rr;

            if (cameraMoveCont >= cameraMoveTime)
                cameraMoveTime = 0;
        }

        public void CameraLengthSet(float ratio ,int time)
        {
            cameraLastLength = cameraLength;
            cameraDistinationLength = cameraLengthDefault * ratio;
            cameraMoveTime = time;
            cameraMoveCont = 0;
        }

        /// <summary>
        /// 時間経過の更新
        /// timeSpeedに応じて加算量を調整は調整される
        /// </summary>
        /// <param name="gameTime"></param>
        private void TimeUpdate(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds / (int)timeSpeed; ;

            time += deltaTime;
            TimeSpeedUpdate(gameTime);
        }

        /// <summary>
        /// 各TimeSpeedTimerを更新する
        /// タイマ―が0を切るとスピードが戻る
        /// </summary>
        /// <param name="gameTime"></param>
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

        /// <summary>
        /// より遅いスピードが優先される
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="t"></param>
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
            //parent.AddSceneReservation(new Result.Result(parent, score));
        }

        internal void PlayerArts(Vector3 position,VirtualObject.Direction direction)
        {
            arts.Add(new Arts.Fire(this, renderer, position, direction,enemies));
        }

        public override void Draw(Output.Renderer renderer)
        {
            fields.ForEach(f => f.Draw(renderer));

            blocks.ForEach(b => b.Draw(renderer));

            List<VirtualObject> drawObjects = new List<VirtualObject>();
            drawObjects.Add(player);
            drawObjects.AddRange(enemies);
            drawObjects.AddRange(arts);
            drawObjects.AddRange(objects);

            if (rotateManager.cameraRotate.Y > 0)
            {
                var obj = drawObjects.OrderBy(o => o.Position.X);
                foreach(var o in obj)
                {
                    o.Draw(renderer);
                }
            }
            else
            {
                var obj = drawObjects.OrderByDescending(o => o.Position.X);
                foreach (var o in obj)
                {
                    o.Draw(renderer);
                }
            }

            player.DrawParam(renderer);
        }


        internal void GameClear()
        {
            isClose = true;
            parent.AddSceneReservation(new Title.Title(parent, renderer));
        }

        internal void GameClearSet()
        {
            state = State.CLEAR;
        }

    }

}
