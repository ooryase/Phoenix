using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scroll.GameSystem;
using Scroll.Output;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Scroll.Battle
{
    class Battle : Scene
    {
        private Player.Player player;

        private List<VirtualCharacter> enemies;
        private List<Arts.Fire> arts;
        //internal List<EffectSystem.VirtualEffectSystem> effectSystems;

        private List<Field.Field> fields;

        private int[,] blocksData;
        private List<Field.Block> blocks;
        private float blockSize;

        private Matrix projection;

        private Vector3 cameraPos;
        private Vector3 cameraLookPos;
        private float cameraLength;

        private Vector3 yawPitchRoll;

        private Matrix view;


        internal enum State
        {
            NORMAL,
            PAUSE,
            EVENT
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
        public Vector3 CameraPos { get => cameraPos; private set => cameraPos = value; }
        public Vector3 CameraLookPos { get => cameraLookPos; private set => cameraLookPos = value; }
        public float CameraLength { get => cameraLength; private set => cameraLength = value; }
        internal State BattleState { get => state; set => state = value; }

        public Battle(GameMain gameMain,Renderer renderer)
            : base(gameMain,renderer)
        {
            timeSpeed = TimeSpeed.NORMAL;
            time = 0;

            halfTimeTimer = 0;
            slowTimeTimer = 0;
            verySlowTimeTimer = 0;

            yawPitchRoll = Vector3.Zero;

            state = State.NORMAL;

            CameraLength = 5f;
            Projection = CreateProjection();
            CameraLookPos = new Vector3(0,0.8f,0);
            CameraPos = CameraLookPos + Vector3.UnitZ * CameraLength;
            View = CreateView();

            player = new Player.Player(this,renderer);
            enemies = new List<VirtualCharacter>();
            enemies.Add(new Enemy.Dog(this,renderer,new Vector3(6.7f,6f,0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(1.7f, 4f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(3.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(4.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(6.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(8.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(10.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(13.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(15.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(17.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(20.7f, 8f, 0)));
            enemies.Add(new Enemy.Dog(this, renderer, new Vector3(21.7f, 8f, 0)));
            enemies.Add(new Enemy.BigDog(this, renderer, new Vector3(35f, 0f, 0)));
            //effectSystems = new List<EffectSystem.VirtualEffectSystem>();
            arts = new List<Arts.Fire>();
            fields = new List<Field.Field>();
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.FLOAR));
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.RIGHT));
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.LEFT));
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.BACK));
            fields.Add(new Field.Field(this, renderer, Field.Field.SetType.FRONT));
            blocks = new List<Field.Block>();
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

        private Matrix CreateView()
        {
            var view = Matrix.CreateLookAt
                (
                CameraLookPos + YawPitchRoll(Vector3.UnitZ) * CameraLength,
                CameraLookPos,
                Vector3.UnitY
                );
            return view;
        }

        private void MapSet()
        {
            blockSize = 0.5f;

            CSVReader csvReader = new CSVReader();
            blocksData = csvReader.GetIntMatrix("Map2.csv");

            for(int y = 0;y < blocksData.GetLength(0);y++)
            {
                for (int x = 0; x < blocksData.GetLength(1); x++)
                {
                    if(blocksData[y,x] == 1)
                        blocks.Add(new Field.Block(this,renderer,new Vector3(x * blockSize + blockSize /2f,y * blockSize + blockSize / 2f,0)));
                }

            }
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

            InputYawPitchRoll();

        }

        /// <summary>
        /// コントローラ入力に応じてカメラ角度を変更する
        /// </summary>
        private void InputYawPitchRoll()
        {
            yawPitchRoll.Y -= InputContllorer.StickRightX() / 100f;
            yawPitchRoll.X += InputContllorer.StickRightY() / 100f;

            yawPitchRoll.X = (yawPitchRoll.X > BattleWindow.CameraYawRight) ? BattleWindow.CameraYawRight : yawPitchRoll.X;
            yawPitchRoll.X = (yawPitchRoll.X < BattleWindow.CameraYawLeft) ? BattleWindow.CameraYawLeft : yawPitchRoll.X;

            yawPitchRoll.Y = (yawPitchRoll.Y > BattleWindow.CameraPitchDown) ? BattleWindow.CameraPitchDown : yawPitchRoll.Y;
            yawPitchRoll.Y = (yawPitchRoll.Y < BattleWindow.CameraPitchUp) ? BattleWindow.CameraPitchUp : yawPitchRoll.Y;

        }

        /// <summary>
        /// Yaw,Pitch,Rollの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 YawPitchRoll(Vector3 vector3)
        {
            return Roll(Pitch(Yaw(vector3)));
        }
        /// <summary>
        /// Yawの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Yaw(Vector3 vector3)
        {
            var s = (float)Math.Sin(yawPitchRoll.X);
            var c = (float)Math.Cos(yawPitchRoll.X);

            var x = vector3.X;
            var y = vector3.Y * c - vector3.Z * s;
            var z = vector3.Y * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Pitchの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Pitch(Vector3 vector3)
        {
            var s = (float)Math.Sin(yawPitchRoll.Y);
            var c = (float)Math.Cos(yawPitchRoll.Y);

            var x = vector3.X * c + vector3.Z * s;
            var y = vector3.Y;
            var z = -vector3.X * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rollの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Roll(Vector3 vector3)
        {
            var s = (float)Math.Sin(yawPitchRoll.Z);
            var c = (float)Math.Cos(yawPitchRoll.Z);

            var x = vector3.X * c - vector3.Y * s;
            var y = vector3.X * s + vector3.Y * c;
            var z = vector3.Z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 各ObjectのMoveUpdateはここで行う
        /// </summary>
        private void MoveUpdate()
        {
            player.MoveUpdate(deltaTime);
            enemies.ForEach(e => e.MoveUpdate(deltaTime));
            arts.ForEach(a => a.)
        }

        /// <summary>
        /// 各Objectが当たった場合は各ObjectのOnCollisionEnterが呼ばれる
        /// </summary>
        private void CollisionUpdate()
        {
            foreach(var e in enemies)
            {
                if(player.GetCollisionRectangle().Collision(e.GetCollisionRectangle()))
                {
                    e.OnCollisionEnter(player);
                }
            }

            //BlockCollision(player);
            //enemies.ForEach(e => BlockCollision(e));
        }

        /// <summary>
        /// 消去予定　現状バグてるよ
        /// </summary>
        /// <param name="vc"></param>
        /*private void BlockCollision(VirtualCharacter vc)
        {
            RectangleF prf = vc.GetCollisionRectangle();

            int left = (int)(prf.Left / blockSize);
            int right = (int)(prf.Right / blockSize);
            int top = (int)(prf.Top / blockSize);
            int bottom = (int)(prf.Bottom / blockSize);

            var leftTop = blocksData[top,left];
            var leftBottom = blocksData[bottom,left];
            var rightTop = blocksData[top,right];
            var rightBottom = blocksData[ bottom, right];

            //parent.Window.Title = "(l,r)" + left + "," + right + "   (t,b)" + top + "," + bottom + " :::: lt lb rt rb" + leftTop + leftBottom + rightTop + rightBottom;


            float l = blockSize - prf.Left % blockSize;
            float r = prf.Right % blockSize;
            float t = prf.Top % blockSize;
            float b = blockSize - prf.Bottom % blockSize;

            if(leftTop > 0)
            {
                if(rightTop > 0)
                {
                    if(leftBottom > 0)
                        vc.OnCollisionEnter(new Vector3(l, -t, 0), VirtualObject.TagName.FIELD);
                    else if(rightBottom > 0)
                        vc.OnCollisionEnter(new Vector3(-r, -t, 0), VirtualObject.TagName.FIELD);
                    else
                        vc.OnCollisionEnter(new Vector3(0, -t, 0), VirtualObject.TagName.FIELD);
                }
                else
                {
                    if (leftBottom > 0)
                    {
                        if (rightBottom > 0)
                            vc.OnCollisionEnter(new Vector3(l, b, 0), VirtualObject.TagName.FIELD);
                        else
                            vc.OnCollisionEnter(new Vector3(l, 0, 0), VirtualObject.TagName.FIELD);
                    }
                    else if(l > t)
                        vc.OnCollisionEnter(new Vector3(0, -t, 0), VirtualObject.TagName.FIELD);
                    else
                        vc.OnCollisionEnter(new Vector3(l, 0, 0), VirtualObject.TagName.FIELD);

                }
            }
            else
            {
                if(rightTop > 0)
                {
                    if(rightBottom > 0)
                    {
                        if(leftBottom > 0)
                            vc.OnCollisionEnter(new Vector3(-r, b, 0), VirtualObject.TagName.FIELD);
                        else
                            vc.OnCollisionEnter(new Vector3(-r, 0, 0), VirtualObject.TagName.FIELD);
                    }
                    else if(r > t)
                        vc.OnCollisionEnter(new Vector3(0, -t, 0), VirtualObject.TagName.FIELD);
                    else
                        vc.OnCollisionEnter(new Vector3(-r, 0, 0), VirtualObject.TagName.FIELD);

                }
                else
                {
                    if (leftBottom > 0)
                    {
                        if (rightBottom > 0)
                            vc.OnCollisionEnter(new Vector3(0, b, 0), VirtualObject.TagName.FIELD);
                        else if(l > b)
                            vc.OnCollisionEnter(new Vector3(0, b, 0), VirtualObject.TagName.FIELD);
                        else
                            vc.OnCollisionEnter(new Vector3(l, 0, 0), VirtualObject.TagName.FIELD);

                    }
                    else if(rightBottom > 0)
                    {
                        if(r > b)
                            vc.OnCollisionEnter(new Vector3(0, b, 0), VirtualObject.TagName.FIELD);
                        else
                            vc.OnCollisionEnter(new Vector3(-r, 0, 0), VirtualObject.TagName.FIELD);
                    }
                }
            }

        }*/

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

        }

        /// <summary>
        /// カメラ座標の更新
        /// </summary>
        private void CameraUpdate()
        {
            //CameraPos = CameraLookPos + YawPitchRoll(Vector3.UnitZ) * CameraLength;

            cameraLookPos = player.Position;
            cameraLookPos.Y += 0.8f;

            View = CreateView();
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

            if (yawPitchRoll.Y > 0)
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
        }


        internal void GameClear()
        {
            isClose = true;
            parent.AddSceneReservation(new Title.Title(parent, renderer));
        }
    }

}
