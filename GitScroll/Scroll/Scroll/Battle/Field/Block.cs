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
        public enum BlockName
        {
            LEFT_UP,
            UP,
            RIGHT_UP,

            LEFT = 4,
            CENTER,
            RIGHT,

            BOTTOM_LEFT = 8,
            BOTTOM,
            BOTTOM_RIGHT,
        }
        private BlockName blockName;

        public BlockName BName { get => blockName;private set => blockName = value; }

        public Block(Battle battle, Renderer renderer,Sound sound,Vector3 position, BlockName blockName)
            : base(battle, renderer,sound)
        {
            this.position = position;
            this.BName = blockName;
            Coordinate();

            var c = new Vector3(0.6f - position.Y / 85f,0.1f + position.Y / 55f, 0.1f + position.Y / 130f) ;
            effect.Parameters["RBG"].SetValue(c);
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
        private void Coordinate()
        {
            switch (BName)
            {
                case BlockName.LEFT_UP:
                    vertices[0].TextureCoordinate = new Vector2(0.25f, 0);
                    vertices[1].TextureCoordinate = new Vector2(0, 0.25f);
                    vertices[2].TextureCoordinate = new Vector2(0, 0);
                    vertices[3].TextureCoordinate = new Vector2(0.25f, 0.25f);
                    break;
                case BlockName.UP:
                    vertices[0].TextureCoordinate = new Vector2(0.5f, 0);
                    vertices[1].TextureCoordinate = new Vector2(0.25f, 0.25f);
                    vertices[2].TextureCoordinate = new Vector2(0.25f, 0);
                    vertices[3].TextureCoordinate = new Vector2(0.5f, 0.25f);
                    break;
                case BlockName.RIGHT_UP:
                    vertices[0].TextureCoordinate = new Vector2(0.75f, 0);
                    vertices[1].TextureCoordinate = new Vector2(0.5f, 0.25f);
                    vertices[2].TextureCoordinate = new Vector2(0.5f, 0);
                    vertices[3].TextureCoordinate = new Vector2(0.75f, 0.25f);
                    break;
                case BlockName.LEFT:
                    vertices[0].TextureCoordinate = new Vector2(0.25f, 0.25f);
                    vertices[1].TextureCoordinate = new Vector2(0, 0.5f);
                    vertices[2].TextureCoordinate = new Vector2(0, 0.25f);
                    vertices[3].TextureCoordinate = new Vector2(0.25f, 0.5f);
                    break;
                case BlockName.CENTER:
                    vertices[0].TextureCoordinate = new Vector2(0.5f, 0.25f);
                    vertices[1].TextureCoordinate = new Vector2(0.25f, 0.5f);
                    vertices[2].TextureCoordinate = new Vector2(0.25f, 0.25f);
                    vertices[3].TextureCoordinate = new Vector2(0.5f, 0.5f);
                    break;
                case BlockName.RIGHT:
                    vertices[0].TextureCoordinate = new Vector2(0.75f, 0.25f);
                    vertices[1].TextureCoordinate = new Vector2(0.5f, 0.5f);
                    vertices[2].TextureCoordinate = new Vector2(0.5f, 0.25f);
                    vertices[3].TextureCoordinate = new Vector2(0.75f, 0.5f);
                    break;
                case BlockName.BOTTOM_LEFT:
                    vertices[0].TextureCoordinate = new Vector2(0.25f, 0.5f);
                    vertices[1].TextureCoordinate = new Vector2(0, 0.75f);
                    vertices[2].TextureCoordinate = new Vector2(0, 0.5f);
                    vertices[3].TextureCoordinate = new Vector2(0.25f, 0.75f);
                    break;
                case BlockName.BOTTOM:
                    vertices[0].TextureCoordinate = new Vector2(0.5f, 0.5f);
                    vertices[1].TextureCoordinate = new Vector2(0.25f, 0.75f);
                    vertices[2].TextureCoordinate = new Vector2(0.25f, 0.5f);
                    vertices[3].TextureCoordinate = new Vector2(0.5f, 0.75f);
                    break;
                case BlockName.BOTTOM_RIGHT:
                    vertices[0].TextureCoordinate = new Vector2(0.75f, 0.5f);
                    vertices[1].TextureCoordinate = new Vector2(0.5f, 0.75f);
                    vertices[2].TextureCoordinate = new Vector2(0.5f, 0.5f);
                    vertices[3].TextureCoordinate = new Vector2(0.75f, 0.75f);
                    break;
            }
            
        }
        public override void StartUpdate(int deltaTime)
        {
            throw new NotImplementedException();
        }
        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }
        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            switch (BName)
            {
                case BlockName.LEFT_UP:
                    LEFT_UPCollision(virtualObject);
                    break;
                case BlockName.UP:
                    UPCollision(virtualObject);
                    break;
                case BlockName.RIGHT_UP:
                    RIGHT_UPCollision(virtualObject);
                    break;
                case BlockName.LEFT:
                    LEFTCollision(virtualObject);
                    break;
                case BlockName.CENTER:
                    CENTERCollision(virtualObject);
                    break;
                case BlockName.RIGHT:
                    RIGHTCollision(virtualObject);
                    break;
                case BlockName.BOTTOM_LEFT:
                    BOTTOM_LEFTCollision(virtualObject);
                    break;
                case BlockName.BOTTOM:
                    BOTTOMCollision(virtualObject);
                    break;
                case BlockName.BOTTOM_RIGHT:
                    BOTTOM_LEFTCollision(virtualObject);
                    break;
            }
        }
        private void LEFT_UPCollision(VirtualObject virtualObject)
        {

        }
        private void UPCollision(VirtualObject virtualObject)
        {

        }
        private void RIGHT_UPCollision(VirtualObject virtualObject)
        {

        }
        private void LEFTCollision(VirtualObject virtualObject)
        {

        }
        private void CENTERCollision(VirtualObject virtualObject)
        {

        }
        private void RIGHTCollision(VirtualObject virtualObject)
        {

        }
        private void BOTTOM_LEFTCollision(VirtualObject virtualObject)
        {

        }
        private void BOTTOMCollision(VirtualObject virtualObject)
        {

        }
        private void BOTTOM_RIGHTCollision(VirtualObject virtualObject)
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
