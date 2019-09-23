using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.Output;

namespace Scroll.Battle.BattleEffect
{
    class ReBarthEffect : VirtualEffect
    {
        private float length;
        private float sheaderTop;
        private float sheaderBottom;
        private Vector3 sheaderRBG;


        public ReBarthEffect(Battle battle, Renderer renderer, VirtualObject baseObj, Vector3 position,float length) : base(battle, renderer, baseObj, position)
        {
            this.length = length;

            scale = length;
            SetBaseVertices();
            vertices = CreateVertices();

            sheaderTop = 0f;
            sheaderBottom = 0f;
            sheaderRBG = new Vector3(1.0f, 0, 0);

            effect.Parameters["Top"].SetValue(sheaderTop);
            effect.Parameters["Bottom"].SetValue(sheaderBottom);
            effect.Parameters["RBG"].SetValue(sheaderRBG);
        }

        protected override void Awake()
        {
            scale = 1f;
        }

        protected override void NameSet()
        {
            effectName = "ReBarthFire";
            textureName = "ReBarth";
        }


        public override void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
            StateUpdate(deltaTime);
        }

        private void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        private void StateUpdate(int deltaTime)
        {
            if (time < 800)
                sheaderTop += deltaTime * 0.001875f;
            else
                sheaderTop = 1.5f;

            if (time > 400)
                sheaderBottom += deltaTime * 0.0025f;
            if (time >= 1000)
                delete = true;
        }
        public override void DrawUpdate()
        {
            VerticesSet(Billboard.BILLBOARD);

            effect.Parameters["View"].SetValue(parent.View);
            effect.Parameters["Top"].SetValue(sheaderTop * sheaderTop);
            effect.Parameters["Bottom"].SetValue(sheaderBottom * sheaderBottom);
        }

        public override void EndUpdate()
        {
            throw new NotImplementedException();
        }

        public override void MoveUpdate(int deltaTime)
        {
            position = baseObj.Position;
        }

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            throw new NotImplementedException();
        }

    }
}
