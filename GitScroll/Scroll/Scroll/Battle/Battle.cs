﻿using Microsoft.Xna.Framework;
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
        private List<Field.Block> noneColliBlocks;//当たり判定を取らないブロック
        private float blockSize;

        private Field.BlockCollision blockCollision;

        private List<VirtualObject> objects;
        private List<BattleEffect.VirtualEffect> battleEffects;

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

        RasterizerState depthBiasEnabledField;
        RasterizerState depthBiasEnabledBlock;

        public Matrix Projection { get => projection; set => projection = value; }
        public Matrix View { get => view; set => view = value; }
        public Vector3 CameraLookPos { get => cameraLookPos; private set => cameraLookPos = value; }
        internal State BattleState { get => state; set => state = value; }
        public float CameraLengthDefault { get => cameraLengthDefault; private set => cameraLengthDefault = value; }

        public Battle(GameMain gameMain,Renderer renderer,Sound sound)
            : base(gameMain,renderer,sound)
        {
            timeSpeed = TimeSpeed.NORMAL;
            time = 0;

            halfTimeTimer = 0;
            slowTimeTimer = 0;
            verySlowTimeTimer = 0;

            rotateManager = new RotateManager();
            rotateManager.cameraRotate = Vector3.Zero;

            state = State.NORMAL;

            CameraLengthDefault = 5f;
            cameraLength = CameraLengthDefault;
            Projection = CreateProjection();
            CameraLookPos = Vector3.Zero;
            View = CreateCameraView();

            player = new Player.Player(this,renderer,sound);
            enemies = new List<Enemy.VirtualEnemy>();
            //effectSystems = new List<EffectSystem.VirtualEffectSystem>();
            arts = new List<Arts.Fire>();
            fields = new List<Field.Field>();
            fields.Add(new Field.Field(this, renderer,sound, Field.Field.SetType.BACK));
            fields.Add(new Field.Field(this, renderer,sound, Field.Field.SetType.FRONT));
            blocks = new List<Field.Block>();
            noneColliBlocks = new List<Block>();
            objects = new List<VirtualObject>();
            battleEffects = new List<BattleEffect.VirtualEffect>();
            MapSet();
            player.EnemyCount = enemies.Count;

            depthBiasEnabledField = new RasterizerState
            {
                DepthBias = 0.2f,
                CullMode = CullMode.None
            };
            depthBiasEnabledBlock = new RasterizerState
            {
                DepthBias = 0.1f,
                CullMode = CullMode.None
            };

            sound.PlayBGM("bgm");

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
            blocksData = csvReader.GetIntMatrix("phoenix.csv");

            for(int y = 0;y < blocksData.GetLength(0);y++)
            {
                for (int x = 0; x < blocksData.GetLength(1); x++)
                {
                    if (blocksData[y, x] == -1)
                        continue;
                    if(blocksData[y, x] == 5)
                        noneColliBlocks.Add(new Field.Block(this, renderer, sound, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Field.Block.BlockName)blocksData[y, x]));
                    else if (blocksData[y,x] >= 0 &&
                       blocksData[y,x] <= 11)
                        blocks.Add(new Field.Block(this,renderer,sound,new Vector3(x * blockSize + blockSize /2f,y * blockSize + blockSize / 2f,0)
                            , (Field.Block.BlockName)blocksData[y,x]));
                    else if (blocksData[y, x] == 12)
                        enemies.Add(new Enemy.Wata(this, renderer,sound,player, new Vector3(x * blockSize +blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if (blocksData[y, x] == 13)
                        enemies.Add(new Enemy.Wolf(this, renderer,sound,player, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if (blocksData[y, x] == 14)
                        enemies.Add(new Enemy.Dragon(this, renderer,sound,player, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)
                            , (Enemy.VirtualEnemy.EnemyName)blocksData[y, x]));
                    else if(blocksData[y, x] == 15)
                        objects.Add(new GoalBlock(this, renderer,sound, new Vector3(x * blockSize + blockSize / 2f, y * blockSize + blockSize / 2f, 0)));
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
            battleEffects.ForEach(be => be.StartUpdate(deltaTime));

            //InputYawPitchRoll();

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
            battleEffects.ForEach(be => be.MoveUpdate(deltaTime));
        }

        /// <summary>
        /// 各Objectが当たった場合は各ObjectのOnCollisionEnterが呼ばれる
        /// </summary>
        private void CollisionUpdate()
        {

            foreach(var e in enemies)
            {
                var l = (player.StateProperty == Player.Player.State.RESPAWNN) ?
                    player.Scale * cameraLength * 0.54f : player.Scale;
                l += e.Scale;
                if(Vector3.DistanceSquared(player.Position,e.Position) < l * l)
                    e.OnCollisionEnter(player);
            }

            foreach (var o in objects)
            {
                var l = player.Scale + o.Scale;

                if (Vector3.DistanceSquared(player.Position, o.Position) < l * l)
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
            battleEffects.RemoveAll(be => be.Delete);
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
            noneColliBlocks.ForEach(b => b.DrawUpdate());
            objects.ForEach(o => o.DrawUpdate());
            battleEffects.ForEach(be => be.DrawUpdate());
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
            cameraDistinationLength = CameraLengthDefault * ratio;
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
            arts.Add(new Arts.Fire(this, renderer,sound, position, direction,enemies));
        }

        internal void AddBattleEffect(BattleEffect.VirtualEffect virtualEffect)
        {
            battleEffects.Add(virtualEffect);
        }

        public override void Draw(Output.Renderer renderer)
        {
            renderer.Begin();

            parent.GraphicsDevice.RasterizerState = depthBiasEnabledField;
            fields.ForEach(f => f.Draw(renderer));

            parent.GraphicsDevice.RasterizerState = depthBiasEnabledBlock;
            foreach (var b in blocks)
            {
                if (IsArea(b.Position))
                    b.Draw(renderer);
            }
            foreach (var b in noneColliBlocks)
            {
                if (IsArea(b.Position))
                    b.Draw(renderer);
            }
            //            blocks.ForEach(b => b.Draw(renderer));
            //            noneColliBlocks.ForEach(b => b.Draw(renderer));

            parent.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            List<VirtualObject> drawObjects = new List<VirtualObject>();
            drawObjects.Add(player);
            foreach(var e in enemies)
            {
                if (IsArea(e.Position))
                    drawObjects.Add(e);
            }
            //drawObjects.AddRange(enemies);
            drawObjects.AddRange(arts);
            drawObjects.AddRange(objects);
            drawObjects.AddRange(battleEffects);

            var obj = drawObjects.OrderBy(o => Vector3.DistanceSquared(GetCameraPos(),o.Position));
            foreach(var o in obj)
            {
                o.Draw(renderer);
            }

            player.DrawParam(renderer);

            renderer.End();
        }


        internal void GameClear()
        {
            isClose = true;
            parent.AddSceneReservation(new Title.Title(parent, renderer,sound));
        }

        internal void GameClearSet()
        {
            state = State.CLEAR;
        }

        public Vector3 GetCameraPos()
        {
            return CameraLookPos + rotateManager.YawPitchRoll(Vector3.UnitZ) * cameraLength;

        }

        internal Renderer GetRenderer()
        {
            return renderer;
        }

        private bool IsArea(Vector3 position)
        {
            if (Math.Abs(cameraLookPos.X - position.X) > 4f * cameraLength / cameraLengthDefault ||
                Math.Abs(cameraLookPos.Y - position.Y) > 3f * cameraLength / cameraLengthDefault)
                return false;

            return true;
        }

    }

}
