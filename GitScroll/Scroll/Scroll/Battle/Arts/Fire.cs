using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.Output;

namespace Scroll.Battle.Arts
{
    class Fire : VirtualObject
    {
        //private bool 座標基準の設定
        public enum BasePosition
        {
            PARENT,
            WORLD,
            CAMERA
        }

        private BasePosition basePosition;

        //private bool 発生保障
        private bool Independence;

        //private class 対応するプレイヤーの技

        protected float damage;


        protected Dictionary<VirtualObject, bool> hitEnemies;

        public float Damage { get => damage; protected set => damage = value; }

        public Fire(Battle battle, Renderer renderer,Vector3 pos,Direction direction,List<Enemy.VirtualEnemy> enemiesList) : base(battle, renderer)
        {
            Tag = TagName.PLAYER_ARTS;
            var d = (Direct == Direction.LEFT) ? -0.05f : 0.05f;

            Direct = direction;
            position = new Vector3(pos.X + d, pos.Y, pos.Z);

            damage = 2f;
            basePosition = BasePosition.PARENT;

            hitEnemies = new Dictionary<VirtualObject, bool>();
            foreach(var e in enemiesList)
            {
                hitEnemies.Add(e, false);
            }
        }
        protected override void Awake()
        {
            Scale = 0.3f;
        }

        protected override void NameSet()
        {
            textureName = "f";
            effectName = "Arts";
        }
        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate();
        }

        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }


        private void StateUpdate()
        {
            if (time >= 700)
                delete = true;
        }

        public void MoveUpdate(int deltaTime,Vector3 bPos)
        {
            if(basePosition == BasePosition.PARENT)
                position = bPos;
        }



        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }



        public override void DrawUpdate()
        {
            vertices[0].TextureCoordinate.X = 0.1f * (time / 70 + 1f - (float)Direct);
            vertices[1].TextureCoordinate.X = 0.1f * (time / 70 + (float)Direct);
            vertices[2].TextureCoordinate.X = 0.1f * (time / 70 + (float)Direct);
            vertices[3].TextureCoordinate.X = 0.1f * (time / 70 + 1f - (float)Direct);

            vertices[0].TextureCoordinate.Y = 1f;
            vertices[1].TextureCoordinate.Y = 0f;
            vertices[2].TextureCoordinate.Y = 1f;
            vertices[3].TextureCoordinate.Y = 0f;


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

        public bool IsHit(VirtualCharacter vc)
        {
            if(hitEnemies.ContainsKey(vc))
            {
                if(hitEnemies[vc])
                {
                    return false;
                }
                else
                {
                    hitEnemies[vc] = true;
                    return true;
                }
            }
            else
            {
                hitEnemies.Add(vc, true);
                return true;
            }
        }

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            throw new NotImplementedException();
        }
    }
}
