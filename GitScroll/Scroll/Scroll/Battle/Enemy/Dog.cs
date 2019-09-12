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
    class Dog : VirtualCharacter
    {

        protected override void Awake()
        {
            scale = 0.2f;
        }

        protected override void NameSet()
        {
            effectName = "NoobEnemy";
            textureName = "Dog";
        }
        public Dog(Battle battle, Renderer renderer,Vector3 startPos) : base(battle,renderer)
        {
            position = startPos;
            physics = new MovePhysics(0.25f, true, 0.0025f, 0.92f);
            hp = 30f;

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
            if(dead)
            {
                effect.Parameters["Value"].SetValue(1f - time / 1000f);
                if (time > 1000)
                    delete = true;
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

            if (virtualObject.Tag == TagName.FIELD)
            {
                if (virtualObject.Position.Y > 0)
                    physics.isGraund = true;

                position += virtualObject.Position;
                return;
            }

            if (dead)
                return;

            hp -= 20f;

            physics.velocity = (position - virtualObject.Position) / 6f;
            physics.velocity.Y += 0.2f;

            if (hp < 0f)
            {
                dead = true;
                time = 0;
                effect.Parameters["Red"].SetValue(0f);
            }
        }

        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }

        public override void DrawUpdate()
        {
            vertices[0].TextureCoordinate.X = 0.25f * (time / 200 % 4 + (float)Direct);
            vertices[1].TextureCoordinate.X = 0.25f * (time / 200 % 4 + 1f - (float)Direct);
            vertices[2].TextureCoordinate.X = 0.25f * (time / 200 % 4 + 1f - (float)Direct);
            vertices[3].TextureCoordinate.X = 0.25f * (time / 200 % 4 + (float)Direct);

            VerticesSet(Billboard.PITCH_ONLY);

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
        }

        public override RectangleF GetCollisionRectangle()
        {
            RectangleF rf = new RectangleF(
                position.X - 0.8f * scale,
                position.Y + 0.8f * scale,
                1.6f * scale,
                1.8f * scale
                );

            return rf;
        }
    }
}
