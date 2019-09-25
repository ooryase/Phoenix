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
    class GoalBlock : VirtualObject
    {
        public GoalBlock(Battle battle, Renderer renderer, Sound sound, Vector3 position) : base(battle, renderer,sound)
        {
            this.position = position;
            tag = TagName.CLEAR_BLOCK;

            vertices[0].TextureCoordinate = new Vector2(1f, 0.75f);
            vertices[1].TextureCoordinate = new Vector2(0.75f, 1f);
            vertices[2].TextureCoordinate = new Vector2(0.75f, 0.75f);
            vertices[3].TextureCoordinate = new Vector2(1f, 1f);

            effect.Parameters["RBG"].SetValue(Vector3.One);

        }
        protected override void Awake()
        {
            Scale = 0.25f;
        }

        protected override void NameSet()
        {
            textureName = "Maptip";
            effectName = "Block";
        }
        public override void StartUpdate(int deltaTime)
        {
        }
        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }
        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
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
            position.X - 1f * Scale,
            position.Y + 1f * Scale,
            2f * Scale,
            2f * Scale
            );

            return rf;

        }


    }
}
