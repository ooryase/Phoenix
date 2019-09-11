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
        protected Vector2 targetPos;

        public enum State
        {
            NORMAL,
            ATTACK = 700,
        }
        protected State state;
        private bool move;

        protected bool invincible;
        protected int invincibleTime;



        protected override void Awake()
        {
            scale = 0.25f;
        }
        protected override void NameSet()
        {
            effectName = "Player";
            textureName = "Player";
        }

        public Player(Battle battle ,Renderer renderer) : base(battle,renderer)
        {
            position = Vector3.UnitX;
            StateSet(State.NORMAL);
            move = false;
            speed = 0.0085f;
            gSpeed = 0.0025f;
            maxSpeed = new Vector3(0.45f, 0.7f, 0);

            invincible = false;
            invincibleTime = 0;

        }



        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate(deltaTime);

        }

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            if(virtualObject.Tag == TagName.FIELD)
            {
                if (virtualObject.Position.Y > 0)
                    isGraund = true;

                position += virtualObject.Position;
            }
        }

        public override void EndUpdate()
        {

        }

        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        protected void StateUpdate(int deltaTime)
        {
            InvincibleUpdate(deltaTime);

            switch (state)
            {
                case State.NORMAL:
                    NormalStateUpdate(deltaTime);
                    break;
                case State.ATTACK:
                    AttackStateUpdate(deltaTime);
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

        protected void NormalStateUpdate(int deltaTime)
        {
            if (parent.BattleState != Battle.State.NORMAL)
                return;

            if(InputContllorer.IsPush(Keys.Z))
            {
                StateSet(State.ATTACK);
                parent.PlayerArts(position,Direct);
            }

        }

        private void AttackStateUpdate(int deltaTime)
        {
            if (time > (int)State.ATTACK)
                StateSet(State.NORMAL);
        }

        public override void MoveUpdate(int deltaTime)
        {

            velocity.X *= 0.92f;

            MoveInputUpdate(deltaTime);

            GravityUpdate(deltaTime);

            position += velocity * speed * deltaTime;
            FieldMove();
        }

        private void MoveInputUpdate(int deltaTime)
        {
            if (parent.BattleState != Battle.State.NORMAL)
            {
                move = false;
                return;
            }
            if (InputContllorer.IsPress(Keys.Left))
            {
                LeftMove(speed * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Right))
            {
                RightMove(speed * deltaTime);
            }
            if (InputContllorer.IsPress(Keys.Up) && isGraund)
                UpMove(speed * deltaTime * 10f);
            

            //if (InputContllorer.IsPress(Keys.Down))
            //    DownMove(speed * deltaTime);

            var l = InputContllorer.StickLeftX();
            if (move = l != 0f)
                SideMove(l * speed * deltaTime);
            if (isGraund && InputContllorer.IsPush(Buttons.A))
                UpMove(speed * deltaTime * 10f);
        }

        private void RightMove(float l)
        {
            move = true;
            velocity.X += l;
            velocity.X = (velocity.X > maxSpeed.X) ? maxSpeed.X : velocity.X;
            Direct = Direction.RIGHT;
        }
        private void LeftMove(float l)
        {
            move = true;
            velocity.X -= l;
            velocity.X = (velocity.X < -maxSpeed.X) ? -maxSpeed.X : velocity.X;
            Direct = Direction.LEFT;
        }
        private void UpMove(float l)
        {
            move = true;
            velocity.Y = l;
            velocity.Y = (velocity.Y > maxSpeed.Y) ? maxSpeed.Y : velocity.Y;
        }
        private void DownMove(float l)
        {
            move = true;
            velocity.Y -= l;
            velocity.Y = (velocity.Y < -maxSpeed.Y) ? -maxSpeed.Y : velocity.Y;
        }

        private void SideMove(float l)
        {
            move = true;
            velocity.X += l;
            velocity.X = (velocity.X < -maxSpeed.X) ? -maxSpeed.X : velocity.X;
            velocity.X = (velocity.X > maxSpeed.X) ? maxSpeed.X : velocity.X;
            Direct = (l > 0f) ? Direction.RIGHT : Direction.LEFT;

        }


        protected void StateSet(State s)
        {
            state = s;
            time = 0;
        }

        public override void DrawUpdate()
        {
            if (state == State.NORMAL)
            {
                if (move)
                {
                    //1,2,2,3,4,4の順
                    var i = time / 70 % 6;
                    i -= (time % 420 >= 140) ? 1 : 0;
                    i -= (time % 420 >= 350) ? 1 : 0;

                    TextureCoordinateSet(i, 1f);
                }
                else
                {
                    TextureCoordinateSet(time / 200 % 4, 0f);

                }
            }
            else if(state == State.ATTACK)
            {
                var i = (time >= 280) ? 3 : time / 70;

                TextureCoordinateSet(i, 2f);
            }


            VerticesSet(Billboard.PITCH_ONLY);

            effect.Parameters["View"].SetValue(parent.View);

        }

        private void TextureCoordinateSet(float x,float y)
        {
            vertices[0].TextureCoordinate.X = 0.25f * (x + (float)Direct);
            vertices[1].TextureCoordinate.X = 0.25f * (x + 1f - (float)Direct);
            vertices[2].TextureCoordinate.X = 0.25f * (x + 1f - (float)Direct);
            vertices[3].TextureCoordinate.X = 0.25f * (x + (float)Direct);

            vertices[0].TextureCoordinate.Y = 0.25f * (y);
            vertices[1].TextureCoordinate.Y = 0.25f * (y + 1f);
            vertices[2].TextureCoordinate.Y = 0.25f * (y);
            vertices[3].TextureCoordinate.Y = 0.25f * (y + 1f);
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

        public void DrawParam(Output.Renderer renderer)
        {
            //var drawP = BattleMain.BattleWindow.DrawParam;
            //var dPRight = new Vector2(drawP.X + 100f, drawP.Y);
            //renderer.DrawFont("k8x12L", "SCORE",drawP);
            //renderer.DrawFont("k8x12L", score.ToString(),  dPRight);

            //drawP.Y += 50f;
            //dPRight.Y += 50f;

            //var r = new Rectangle(96, 64, 32, 32);

            //renderer.DrawFont("k8x12L", "LIFE", drawP);
            //for (int i = 0; i < life; i++)
            //{
            //    renderer.DrawTexture("32tex", new Vector2(dPRight.X + i * 36f, dPRight.Y), r, Color.White);
            //}

            //drawP.Y += 50f;
            //dPRight.Y += 50f;

            //r.X -= 32;

            //renderer.DrawFont("k8x12L", "MAGIC", drawP);
            //for (int i = 0 ;i < bomb;i++)
            //{
            //    renderer.DrawTexture("32tex",  new Vector2(dPRight.X + i * 36f ,dPRight.Y), r, Color.White);
            //}

            //drawP.Y += 50f;
            //dPRight.Y += 50f;

            //renderer.DrawFont("k8x12L", "POWER", drawP);
            //renderer.DrawFont("k8x12L", power.ToString(), dPRight);

            //if(state == State.PICHUN)
            //    renderer.DrawTexture("HitRed", new Vector2(BattleMain.BattleWindow.Left,BattleMain.BattleWindow.Up));

        }

        public override RectangleF GetCollisionRectangle()
        {
            RectangleF rf = new RectangleF(
                position.X - 0.8f * scale,
                position.Y + 0.6f * scale,
                1.6f * scale,
                1.6f * scale
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





    }
}
