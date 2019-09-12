using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scroll.GameSystem;
using Microsoft.Xna.Framework.Graphics;
using Scroll.Output;

namespace Scroll.Battle
{
    /// <summary>
    /// バトルシーンで扱うオブジェクトは必ず継承すること
    /// </summary>
    abstract class VirtualObject
    {
        protected Vector3 position;
        protected float scale;
        public enum Direction
        {
            RIGHT,
            LEFT,
        }
        protected Direction direct;


        protected Effect effect;
        /// <summary>
        /// 最終的に描画される頂点座標
        /// </summary>
        protected VertexPositionTexture[] vertices; 
        /// <summary>
        /// 回転処理を行う前の頂点座標
        /// </summary>
        protected Vector3[] baseVertexPosition;
        protected int[] indices;

        protected Battle parent;
        protected String effectName;
        protected String textureName;

        /// <summary>
        /// deleteがtrueのオブジェクトはEndUpdateで消去される
        /// </summary>
        protected bool delete;

        protected int time;

        protected enum Billboard
        {
            BILLBOARD,
            PITCH_ONLY,
            NONE
        }

        public enum TagName
        {
            PLAYER,         //プレイヤー
            PLAYER_ARTS,    //プレイヤーの技(ダメージ与えるのはこっち)
            ENEMY,          //エネミー
            ITEM,           //回収型アイテム
            OBJECT,         //干渉できるフィールドオブジェクト
            FIELD           //足場及び景観
        }
        protected TagName tag;

        public Vector3 Position { get => position; protected set => position = value; }
        public bool Delete { get => delete; protected set => delete = value; }
        public TagName Tag { get => tag; set => tag = value; }
        public Direction Direct { get => direct; protected set => direct = value; }

        public VirtualObject(Battle battle, Renderer renderer)
        {
            parent = battle;
            time = 0;
            delete = false;

            Awake();
            //ここから下が描画設定
            NameSet();
            SetBaseVertices();

            effect = CreateEffect(parent.Projection,parent.View,renderer);
            vertices = CreateVertices();
            indices = CreateIndices();
        }

        /// <summary>
        /// 継承先のコンストラクタよりも先に呼ばれる
        /// </summary>
        protected abstract void Awake();

        /// <summary>
        /// ここでeffectNameとtextureNameの初期化を行うこと
        /// </summary>
        protected abstract void NameSet();

        protected Effect CreateEffect(Matrix projection, Matrix view, Renderer renderer)
        {
            var effect = renderer.Effects[effectName].Clone();

            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);
            effect.Parameters["MyTexture"].SetValue(renderer.Textures[textureName]);

            return effect;
        }

        protected virtual void SetBaseVertices()
        {
            baseVertexPosition = new[]
            {
                new Vector3(1f,1f, 0) * scale,
                new Vector3(-1f,-1f, 0) * scale,
                new Vector3(-1f, 1f, 0) * scale,
                new Vector3(1f, -1f, 0) * scale
            };
        }

        protected virtual VertexPositionTexture[] CreateVertices()
        {
            var vertices = new[]
            {
                new VertexPositionTexture(baseVertexPosition[0], new Vector2(1, 0)),
                new VertexPositionTexture(baseVertexPosition[1], new Vector2(0, 1)),
                new VertexPositionTexture(baseVertexPosition[2], new Vector2(0, 0)),
                new VertexPositionTexture(baseVertexPosition[3], new Vector2(1, 1)),

            };
            return vertices;
        }

        protected virtual int[] CreateIndices()
        {
            var indices = new[]
            {
                0,1,2,
                0,3,1
            };

            return indices;
        }

        public abstract void StartUpdate(int deltaTime);

        public abstract void OnCollisionEnter(VirtualObject virtualObject);


        public abstract void EndUpdate();

        public abstract void DrawUpdate();
        /// <summary>
        /// カメラ角度に応じて頂点座標を回転する
        /// billboardの値がNONEの場合は回転しない
        /// </summary>
        /// <param name="billboard"></param>
        protected void VerticesSet(Billboard billboard)
        {
            for (int i = 0; i < vertices.Count(); i++)
            {
                switch (billboard)
                {
                    case Billboard.BILLBOARD:
                        vertices[i].Position = position + parent.YawPitchRoll(baseVertexPosition[i]);
                        break;
                    case Billboard.PITCH_ONLY:
                        vertices[i].Position = position + parent.Pitch(baseVertexPosition[i]);
                        break;
                    case Billboard.NONE:
                        vertices[i].Position = position + baseVertexPosition[i];
                        break;
                }

            }
        }

        public abstract void Draw(Output.Renderer renderer);

    }
}
