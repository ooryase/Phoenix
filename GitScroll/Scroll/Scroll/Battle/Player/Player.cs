using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scroll.Battle;
using Scroll.GameSystem;
using Microsoft.Xna.Framework.Graphics;
using Scroll.Output;

namespace Scroll.Battle.Player
{
    class Player : VirtualCharacter
    {
        private Vector3 maxSpeed;
        /// <summary>
        /// ホーミング弾とかで使ってた
        /// </summary>
        //protected Vector2 targetPos;

        public enum State
        {
            NORMAL,
            RESPAWNN = 650, //復活後バーンってなる状態
            DASH = 200,
            ATTACK = 300,
            DEAD = 1000,
            FALL
        }
        protected State state;
        /// <summary>
        /// 現在移動しているかどうか
        /// 移動アニメーションに切り替える際参照する
        /// </summary>
        private bool move;

        /// <summary>
        /// 無敵
        /// </summary>
        protected bool invincible;
        /// <summary>
        /// 無敵残り時間
        /// </summary>
        protected int invincibleTime;

        /// <summary>
        /// ダッシュの方向取得変数
        /// </summary>
        private Vector3 dashDirection;
        private Vector3 attackMove;

        private float haiGage = 1000; //灰のゲージ

        float Value;



        protected override void Awake()
        {
            Scale = 0.5f; //画像描画のサイズ
        }
        protected override void NameSet()
        {
            effectName = "Player";
            textureName = "fenikkusu";
        }

        public Player(Battle battle, Renderer renderer) : base(battle, renderer)
        {
            tag = TagName.PLAYER;
            hp = 1000f; //HP兼攻撃ゲージ
            position = Vector3.UnitX;
            StateSet(State.NORMAL);
            move = false;

            physics = new MovePhysics(0.015f, null, null);
            maxSpeed = new Vector3(0.45f, 0.7f, 0);

            invincible = false;
            invincibleTime = 0;
            Value = 1f;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                     バトルクラスから呼ばれるよ！（1番目）
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate(deltaTime);

        }
        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;

        }

        protected void StateUpdate(int deltaTime)
        {
            InvincibleUpdate(deltaTime);
            hp -= 1.5f;
            switch (state)
            {
                case State.NORMAL:
                    NormalStateUpdate(deltaTime);
                    Value = 0.1f;
                    break;
                case State.DASH:
                    DashUpdate(deltaTime);
                    break;
                case State.ATTACK:
                    AttackStateUpdate(deltaTime);
                    break;
                case State.FALL:
                    FallUpdate(deltaTime);
                    break;
                case State.DEAD:
                    DeadUpdate(deltaTime);
                    break;
                case State.RESPAWNN:
                    RespawnUpdate(deltaTime);
                    break;
            }
        }

        protected void InvincibleUpdate(int deltaTime)
        {
            if (invincible)
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

        protected void NormalStateUpdate(int deltaTime)
        {
            if (parent.BattleState != Battle.State.NORMAL)
                return;

            var a = new Vector3(InputContllorer.StickLeftX(), InputContllorer.StickLeftY(), 0f);
            if (InputContllorer.IsPush(Buttons.A) && a != Vector3.Zero)
            {
                StateSet(State.ATTACK);
                parent.CameraLengthSet(1.5f, 700); //引数はどれくらい引くかと何秒かけて引くか
                parent.PlayerArts(position, Direct);
                attackMove = a;
                attackMove.Normalize();

            }

            var d = new Vector3(InputContllorer.StickLeftX(), InputContllorer.StickLeftY(), 0f);

            if (InputContllorer.IsPush(Buttons.B) && d != Vector3.Zero)
            {
                parent.CameraLengthSet(1.5f, 700);
                StateSet(State.DASH);
                dashDirection = d;
                dashDirection.Normalize();
            }

            if (hp <= 0)
            {
                StateSet(State.FALL);
            }
        }

        private void AttackStateUpdate(int deltaTime)
        {
            if (time > (int)State.ATTACK)
            {
                parent.CameraLengthSet(1f, 500); //引数はどれくらい引くかと何秒かけて引くか
                StateSet(State.NORMAL);
            }
            if (time > (int)State.ATTACK)
                StateSet(State.FALL);

            hp -= 3.5f;
        }

        private void DashUpdate(int deltaTime)
        {
            if (time > (int)State.DASH)
            {
                if (hp <= 0)
                    StateSet(State.FALL);
                else
                {
                    parent.CameraLengthSet(1f, 650);
                    StateSet(State.NORMAL);
                }
            }
        }

        private void FallUpdate(int deltaTime)
        {
            if (physics.isGraund == true)
            {
                parent.CameraLengthSet(0.6f, 960);
                StateSet(State.DEAD);
            }
        }

        private void DeadUpdate(int deltaTime)
        {
            if (time > (int)State.DEAD)
            {
                parent.CameraLengthSet(5f, 650);
                StateSet(State.RESPAWNN);
            }
        }

        private void RespawnUpdate(int deltaTime)
        {
            hp = 1000;
            if (time > (int)State.RESPAWNN)
            {
                parent.CameraLengthSet(1f, 500);
                StateSet(State.NORMAL);
            }
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                     バトルクラスから呼ばれるよ！（2番目）
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void MoveUpdate(int deltaTime)
        {
            physics.Inertia(deltaTime);

            MoveInputUpdate(deltaTime);

            MoveDashUpdate();

            AttackUpdate();


            FallUpdate();

            DeadUpdate();

            RespawnUpdate();

            // physics.Gravity(deltaTime);

            position += physics.velocity * physics.speed * deltaTime;
            FieldMove();
        }
        private void MoveInputUpdate(int deltaTime)
        {
            if (parent.BattleState != Battle.State.NORMAL)
            {
                // move = false;
                return;
            }
            if (InputContllorer.IsPress(Keys.Left))
            {
                LeftMove(physics.speed * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Right))
            {
                RightMove(physics.speed * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Up))
            {
                UpMove(physics.speed * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Down))
            {
                DownMove(physics.speed * deltaTime);
            }

            var l = InputContllorer.StickLeftX();
            if (move = l != 0f)
                SideMove(l * physics.speed * deltaTime);

            var a = InputContllorer.StickLeftY();
            if (move = a != 0f)
                UpDownMove(a * physics.speed * deltaTime);

            //if (physics.isGraund && InputContllorer.IsPush(Buttons.A))
            //    UpMove(physics.speed * deltaTime * 10f);
        }

        private void MoveDashUpdate()
        {
            if (State.DASH != state)
                return;
            physics.velocity = dashDirection;
        }

        private void AttackUpdate()
        {
            if (State.ATTACK != state)
                return;
            physics.velocity = attackMove;
        }

        private void FallUpdate()
        {
            if (State.FALL != state)
                return;

            physics.velocity = new Vector3(0f, -0.53f, 0f);
        }

        private void DeadUpdate()
        {
            if (State.DEAD != state)
                return;

            physics.velocity = new Vector3(0f, 0f, 0f);
        }

        private void RespawnUpdate()
        {
            if (State.RESPAWNN != state)
                return;

            physics.velocity = new Vector3(0f, 0.665f, 0);
        }

        private void RightMove(float l)
        {
            move = true;
            physics.velocity.X += l;
            physics.velocity.X = (physics.velocity.X > maxSpeed.X) ? maxSpeed.X : physics.velocity.X;
            Direct = Direction.RIGHT;
        }
        private void LeftMove(float l)
        {
            move = true;
            physics.velocity.X -= l;
            physics.velocity.X = (physics.velocity.X < -maxSpeed.X) ? -maxSpeed.X : physics.velocity.X;
            Direct = Direction.LEFT;
        }
        private void UpMove(float l)
        {
            move = true;
            physics.velocity.Y = l;
            physics.velocity.Y = (physics.velocity.Y > maxSpeed.Y) ? maxSpeed.Y : physics.velocity.Y;
        }
        private void DownMove(float l)
        {
            move = true;
            physics.velocity.Y -= l;
            physics.velocity.Y = (physics.velocity.Y < -maxSpeed.Y) ? -maxSpeed.Y : physics.velocity.Y;
        }

        private void SideMove(float l)
        {
            move = true;
            physics.velocity.X += l;
            physics.velocity.X = (physics.velocity.X < -maxSpeed.X) ? -maxSpeed.X : physics.velocity.X;
            physics.velocity.X = (physics.velocity.X > maxSpeed.X) ? maxSpeed.X : physics.velocity.X;
            Direct = (l > 0f) ? Direction.RIGHT : Direction.LEFT;

        }

        private void UpDownMove(float l)
        {
            move = true;
            physics.velocity.Y += l;
            physics.velocity.Y = (physics.velocity.Y < -maxSpeed.Y) ? -maxSpeed.Y : physics.velocity.Y;
            physics.velocity.Y = (physics.velocity.Y > maxSpeed.Y) ? maxSpeed.Y : physics.velocity.Y;
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                     バトルクラスから呼ばれるよ！（呼ばれるとしたら3番目）
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="virtualObject"></param>
        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            if (virtualObject.Tag == TagName.FIELD)
            {
                if (virtualObject.Position.Y > 0)
                    physics.isGraund = true;

                position += virtualObject.Position;
            }

            if(virtualObject.Tag == TagName.CLEAR_BLOCK)
            {
                parent.GameClear();
            }
        }

        public void OnCollisionBlock(Vector3 vector3, bool isGround)
        {
            physics.isGraund = isGround;
            position += vector3;
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///                                     バトルクラスから呼ばれるよ！（4番目）
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="virtualObject"></param>
        public override void EndUpdate()
        {

        }

        /// <summary>
        /// Stateの切り替えと同時にtimeを0にする
        /// </summary>
        /// <param name="s"></param>
        protected void StateSet(State s)
        {
            state = s;
            time = 0;
        }

        public override void DrawUpdate()
        {
            if (state == State.NORMAL)
            {
                //if (move)
                //{
                //    //1,2,2,3,4,4の順
                //    var i = time / 70 % 6;
                //    i -= (time % 420 >= 140) ? 1 : 0;
                //    i -= (time % 420 >= 350) ? 1 : 0;

                //    TextureCoordinateSet(i, 1f);
                //}
                //else
                {
                    TextureCoordinateSet(0f, 0f);
                }
            }

            else if (state == State.RESPAWNN)
            {
                TextureCoordinateSet(2f, 0f); //time割る数の増加で細かく
            }

            else if (state == State.DEAD)
            {
                TextureCoordinateSet(1f, 0f);
            }

            else if (state == State.ATTACK)
            {
                TextureCoordinateSet(3f, 0f);
            }

            VerticesSet(Billboard.PITCH_ONLY);
            effect.Parameters["View"].SetValue(parent.View);
            effect.Parameters["Value"].SetValue(Value);
        }

        /// <summary>
        /// テクスチャコーディネイトの設定
        /// 連結画像によるアニメーション処理を行う
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void TextureCoordinateSet(float x, float y) //アニメーション関係
        {
            vertices[0].TextureCoordinate.X = 0.25f * (x + (float)Direct);
            vertices[1].TextureCoordinate.X = 0.25f * (x + 1f - (float)Direct);
            vertices[2].TextureCoordinate.X = 0.25f * (x + 1f - (float)Direct);
            vertices[3].TextureCoordinate.X = 0.25f * (x + (float)Direct);

            vertices[0].TextureCoordinate.Y = 1f * (y);
            vertices[1].TextureCoordinate.Y = 1f * (y + 1f);
            vertices[2].TextureCoordinate.Y = 1f * (y);
            vertices[3].TextureCoordinate.Y = 1f * (y + 1f);
        }

        public override void Draw(Output.Renderer renderer)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                renderer.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    vertices, 0, vertices.Length,
                    indices, 0, indices.Length / 3);
                //                graphicsDevice.
            }

        }

        /// <summary>
        /// ステータスパラメータの表示
        /// </summary>
        /// <param name="renderer"></param>
        public void DrawParam(Output.Renderer renderer)
        {
            renderer.DrawTexture(Vector2.Zero, new Rectangle(0, 0, (int)hp, 50), 1.0f);
            renderer.DrawTexture(new Vector2(0,50), new Rectangle(0, 0, (int)haiGage,50),1.0f);
        }

        /// <summary>
        /// 四角の当たり判定を返す
        /// </summary>
        /// <returns></returns>
        public override RectangleF GetCollisionRectangle()
        {
            RectangleF rf = new RectangleF(
                position.X - 0.8f * Scale,
                position.Y + 0.6f * Scale,
                1.6f * Scale,
                1.6f * Scale
                );

            return rf;
        }

        //public void AddItem(Item.Item.ItemState itemState ,int stateNumbers)
        //{
        //    switch (itemState)
        //    {
        //        case Item.Item.ItemState.SMALL_SCORE:
        //            score += stateNumbers;
        //            break;
        //        case Item.Item.ItemState.SCORE:
        //            score += stateNumbers;
        //            break;
        //        case Item.Item.ItemState.POWER:
        //            power += stateNumbers;
        //            break;
        //        case Item.Item.ItemState.LIFE:
        //            life += stateNumbers;
        //            break;
        //        case Item.Item.ItemState.BOMB:
        //            bomb += stateNumbers;
        //            break;
        //    }
        //}
        public void AddAsh(float ash)
        {
            haiGage += ash;
        }




    }
}
