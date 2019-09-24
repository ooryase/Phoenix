using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.Enemy
{
    class BigDog : VirtualCharacter
    {
        private Vector3 shake;

        protected override void Awake()
        {
            Scale = 1f;
        }

        protected override void NameSet()
        {
            effectName = "NoobEnemy";
            textureName = "inu";
        }
        public BigDog(Battle battle, Renderer renderer, Vector3 startPos) : base(battle, renderer)
        {
            position = startPos;
            physics = new MovePhysics(0.25f, 0.0025f, 0.92f);
            hp = 30f;

            shake = Vector3.Zero;

            effect.Parameters["Value"].SetValue(1f);
            effect.Parameters["Red"].SetValue(1f);
        }


        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate();
        }

        private void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        private void StateUpdate()
        {
            if (dead)
            {
                if (time > 2000)
                {
                    effect.Parameters["Red"].SetValue(0f);

                    effect.Parameters["Value"].SetValue(1f - (time - 2000) / 4000f);

                    shake.X = (time % 160 < 80) ? 0.05f : -0.05f;
                    shake.Y = -(time - 2000) / 4000f / 2f;

                    if (time > 7900)
                    {
                        delete = true;
                        parent.GameClear();
                    }
                }
            }

        }

        public override void MoveUpdate(int deltaTime)
        {
            physics.velocity *= physics.myu;

            physics.Gravity(deltaTime);

            position += physics.velocity * physics.speed * deltaTime;

            FieldMove();
        }


        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            if (dead)
                return;

            hp -= 5f;

            physics.velocity = (position - virtualObject.Position) / 12f;
            physics.velocity.Y += 0.2f;

            if (hp < 0f)
            {
                dead = true;
                time = 0;
                parent.BattleState = Battle.State.EVENT;
            }
        }

        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }

        public override void DrawUpdate()
        {
            vertices[0].TextureCoordinate.X = 1f * (time / 200 % 4 + (float)Direct);
            vertices[1].TextureCoordinate.X = 1f * (time / 200 % 4 + 1f - (float)Direct);
            vertices[2].TextureCoordinate.X = 1f * (time / 200 % 4 + 1f - (float)Direct);
            vertices[3].TextureCoordinate.X = 1f * (time / 200 % 4 + (float)Direct);

            VerticesSet(Billboard.PITCH_ONLY);

            for(int i = 0; i < vertices.Count(); i++)
            {
                vertices[i].Position += shake;
            }

            effect.Parameters["View"].SetValue(parent.View);
        }

        public override void Draw(Renderer renderer)
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

            if(dead && (time < 100 || (1000 < time && time < 1100)))
                renderer.DrawTexture(Vector2.Zero,new Rectangle(0,0,1280,960),1f);
            else if(dead && time > 7000)
                renderer.DrawTexture(Vector2.Zero, new Rectangle(0, 0, 1280, 960), (time - 7000) / 1000f);
        }

        public override RectangleF GetCollisionRectangle()
        {
            RectangleF rf = new RectangleF(
                position.X - 0.8f * Scale,
                position.Y + 0.8f * Scale,
                1.6f * Scale,
                1.8f * Scale
                );

            return rf;
        }
    }
}
