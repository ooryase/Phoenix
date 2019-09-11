using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.Field
{
    class Block : VirtualObject
    {
        public Block(Battle battle, Renderer renderer,Vector3 position) : base(battle, renderer)
        {
            this.position = position;
        }




        protected override void Awake()
        {
            scale = 0.25f;
        }

        protected override void NameSet()
        {
            textureName = "Block";
            effectName = "Block";
        }

        protected override void SetBaseVertices()
        {
            baseVertexPosition = new[]
            {
                new Vector3(1f,1f, 1f) * scale,
                new Vector3(-1f,-1f, 1f) * scale,
                new Vector3(-1f, 1f, 1f) * scale,
                new Vector3(1f, -1f, 1f) * scale,

                new Vector3(-1f,-1f, -1f) * scale,
                new Vector3(1f,1f, -1f) * scale,
                new Vector3(-1f, 1f, -1f) * scale,
                new Vector3(1f, -1f, -1f) * scale
            };
        }


        protected override VertexPositionTexture[] CreateVertices()
        {
            var vertices = new[]
            {
                new VertexPositionTexture(baseVertexPosition[0], new Vector2(1, 0)),
                new VertexPositionTexture(baseVertexPosition[1], new Vector2(0, 1)),
                new VertexPositionTexture(baseVertexPosition[2], new Vector2(0, 0)),
                new VertexPositionTexture(baseVertexPosition[3], new Vector2(1, 1)),

                new VertexPositionTexture(baseVertexPosition[4], new Vector2(1, 0)),
                new VertexPositionTexture(baseVertexPosition[5], new Vector2(0, 1)),
                new VertexPositionTexture(baseVertexPosition[6], new Vector2(0, 0)),
                new VertexPositionTexture(baseVertexPosition[7], new Vector2(1, 1)),
            };
            return vertices;
        }

        protected override int[] CreateIndices()
        {
            var indices = new[]
            {
                0,1,2,
                0,3,1,
                0,5,3,
                3,5,7,
                2,4,6,
                2,1,4,
                1,3,7,
                1,7,4,
                0,2,6,
                0,6,5

            };

            return indices;
        }
        public override void StartUpdate(int deltaTime)
        {
            throw new NotImplementedException();
        }
        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }
        public override void DrawUpdate()
        {
            VerticesSet(Billboard.NONE);

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

        public RectangleF GetCollisionRectangle()
        {
            RectangleF rf = new RectangleF(
            position.X - 1f * scale,
            position.Y + 1f * scale,
            2f * scale,
            2f * scale
            );

            return rf;

        }

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            throw new NotImplementedException();
        }
    }
}
