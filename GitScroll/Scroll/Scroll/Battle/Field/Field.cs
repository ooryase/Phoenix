using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.Output;

namespace Scroll.Battle.Field
{
    class Field : VirtualObject
    {
        /*public Field(GraphicsDevice graphicsDevice, Matrix projection,Matrix view, Texture2D texture)
        {
            effect = CreateEffect(graphicsDevice,projection,view,texture);
            vertices = CreateVertices();
            indices = CreateIndices();
        }*/
        public enum SetType
        {
            FLOAR,
            WORLD,
            FRONT,
            BACK,
        }

        private SetType setType;


        public Field(Battle battle, Renderer renderer, SetType setType) : base(battle, renderer)
        {
            this.setType = setType;
            position = Vector3.Zero;

            if (setType == SetType.BACK)
                scale = 12f;

            SetBaseVertices();
            SetTextureCoordinate();
            VerticesSet(Billboard.NONE);
            effect.Parameters["Value"].SetValue(0f);

        }

        /*private BasicEffect CreateEffect(GraphicsDevice graphicsDevice, Matrix projection, Matrix view, Texture2D texture)
        {
            var effect = new BasicEffect(graphicsDevice)
            {
                TextureEnabled = true,
                View = view,
                Projection = projection
            };
            effect.Texture = texture;

            return effect;
        }*/

        /*private VertexPositionTexture[] CreateVertices()
        {
            var vertices = new[]
            {
                new VertexPositionTexture(new Vector3(2f, 0, -1f), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-2f, -0.2f, 1f), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(-2f, 0, -1f), new Vector2(0, 0)),

                new VertexPositionTexture(new Vector3(2f, -0.2f, 1f), new Vector2(1, 1)),

            };
            return vertices;
        }

        private int[] CreateIndices()
        {
            var indices = new[]
            {
                0,1,2,
                0,3,1
            };

            return indices;
        }*/


        /*public void Draw(GraphicsDevice graphicsDevice)
        {
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    vertices, 0, vertices.Length,
                    indices, 0, indices.Length / 3);
                //                graphicsDevice.
            }
        }*/

        protected override void Awake()
        {
            scale = 8f;
        }


        protected override void SetBaseVertices()
        {
            switch (setType)
            {
                case SetType.FLOAR:
                    SetFloar();
                    break;
                case SetType.WORLD:
                    break;
                case SetType.FRONT:
                    SetFront();
                    break;
                case SetType.BACK:
                    SetBack();
                    break;
            }

        }

        private void SetFloar()
        {
            baseVertexPosition = new[]
            {
                new Vector3(0.5f,0, -0.25f) * scale,
                new Vector3(-0.5f, 0,0.25f) * scale,
                new Vector3(-0.5f, 0, -0.25f) * scale,
                new Vector3(0.5f, 0, 0.25f) * scale,

                new Vector3(0.5f,0, -0.25f) * scale,
                new Vector3(-0.5f, 0,0.25f) * scale,
                new Vector3(-0.5f, 0, -0.25f) * scale,
                new Vector3(0.5f, 0, 0.25f) * scale
            };
        }

        private void SetFront()
        {
            baseVertexPosition = new[]
            {
                new Vector3(1.5f,1.5f, 0f) * scale,
                new Vector3(0f,-0.1f, 0f) * scale,
                new Vector3(0f, 1.5f, 0f) * scale,
                new Vector3(1.5f, -0.1f, 0f) * scale,

                new Vector3(3.0f,1.5f, 0f) * scale,
                new Vector3(1.5f,-0.1f, 0f) * scale,
                new Vector3(1.5f, 1.5f, 0f) * scale,
                new Vector3(3.0f, -0.1f, 0f) * scale
            };
        }

        private void SetBack()
        {
            baseVertexPosition = new[]
            {
                new Vector3(1.5f,0.8f, -0.15f) * scale,
                new Vector3(0f,-0.1f, -0.15f) * scale,
                new Vector3(0f, 0.8f, -0.15f) * scale,
                new Vector3(1.5f, -0.1f, -0.15f) * scale,

                new Vector3(3.0f,0.8f, -0.15f) * scale,
                new Vector3(1.5f,-0.1f, -0.15f) * scale,
                new Vector3(1.5f, 0.8f, -0.15f) * scale,
                new Vector3(3.0f, -0.1f, -0.15f) * scale

            };

        }


        private void SetTextureCoordinate()
        {
            float i;

            switch (setType)
            {
                case SetType.FLOAR: i = 1f; break;
                case SetType.WORLD: i = 0;  break;
                case SetType.FRONT: i = 0f; break;
                case SetType.BACK:  i = 1f; break;
                default:            i = 1;  break;
            }
            vertices[0].TextureCoordinate.Y = 0.5f * (i);
            vertices[1].TextureCoordinate.Y = 0.5f * (i + 1f);
            vertices[2].TextureCoordinate.Y = 0.5f * (i);
            vertices[3].TextureCoordinate.Y = 0.5f * (i + 1f);

            vertices[4].TextureCoordinate.Y = 0.5f * (i);
            vertices[5].TextureCoordinate.Y = 0.5f * (i + 1f);
            vertices[6].TextureCoordinate.Y = 0.5f * (i);
            vertices[7].TextureCoordinate.Y = 0.5f * (i + 1f);
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

                4,5,6,
                4,7,5,
            };

            return indices;
        }



        protected override void NameSet()
        {
            effectName = "BackGraund";
            textureName = "Back";
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
            System.Console.WriteLine(parent.CameraLookPos.X);

            baseVertexPosition[0].X = (1.5f + 3.0f * (int)((parent.CameraLookPos.X + 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[1].X = (0.0f + 3.0f * (int)((parent.CameraLookPos.X + 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[2].X = (0.0f + 3.0f * (int)((parent.CameraLookPos.X + 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[3].X = (1.5f + 3.0f * (int)((parent.CameraLookPos.X + 0.75f * scale) / (3 * scale))) * scale;

            baseVertexPosition[4].X = (3.0f + 3.0f * (int)((parent.CameraLookPos.X - 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[5].X = (1.5f + 3.0f * (int)((parent.CameraLookPos.X - 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[6].X = (1.5f + 3.0f * (int)((parent.CameraLookPos.X - 0.75f * scale) / (3 * scale))) * scale;
            baseVertexPosition[7].X = (3.0f + 3.0f * (int)((parent.CameraLookPos.X - 0.75f * scale) / (3 * scale))) * scale;
            //            new Vector3(1.5f, 1.5f, 0f) * scale,
            //                new Vector3(0f, -0.1f, 0f) * scale,
            //                new Vector3(0f, 1.5f, 0f) * scale,
            //                new Vector3(1.5f, -0.1f, 0f) * scale,
            //
            //                new Vector3(3.0f, 1.5f, 0f) * scale,
            //                new Vector3(1.5f, -0.1f, 0f) * scale,
            //                new Vector3(1.5f, 1.5f, 0f) * scale,
            //                new Vector3(3.0f, -0.1f, 0f) * scale
            //

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

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            throw new NotImplementedException();
        }
    }
}
