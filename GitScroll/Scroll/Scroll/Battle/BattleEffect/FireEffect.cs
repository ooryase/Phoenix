using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.BattleEffect
{

    class FireEffect : VirtualEffect
    {
        private float sheaderTop;
        private float sheaderBottom;
        private Vector3 sheaderRBG;

        private double rotate;

        public FireEffect(Battle battle, Renderer renderer,Sound sound, VirtualObject baseObj, Vector3 position ,double rotate) 
            : base(battle, renderer,sound,baseObj,position)
        {
            this.rotate = rotate;

            sheaderTop = 1f;
            sheaderBottom = 1f;
            sheaderRBG = new Vector3(1.0f,0,0);

            effect.Parameters["Top"].SetValue(sheaderTop);
            effect.Parameters["Bottom"].SetValue(sheaderBottom);
            effect.Parameters["RBG"].SetValue(sheaderRBG);

        }
        protected override void Awake()
        {
            Scale = 0.6f;
        }

        protected override void NameSet()
        {
            effectName = "AttackFire";
            textureName = "FireEffect";
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
            if (time < 400)
                sheaderTop -= deltaTime * 0.00375f;
            else
                sheaderTop = -0.5f;

            if(time > 200)
                sheaderBottom -= deltaTime * 0.005f;
            if (time >= 500)
                delete = true;
        }

        public override void MoveUpdate(int deltaTime)
        {
            position = baseObj.Position;
        }

        public override void OnCollisionEnter(VirtualObject virtualObject)
        {
            throw new NotImplementedException();
        }
        public override void EndUpdate()
        {
        }

        public override void DrawUpdate()
        {

            for (int i = 0; i < vertices.Count(); i++)
            {
                vertices[i].Position = position +
                    parent.rotateManager.NewRoll(
                        parent.rotateManager.Pitch(baseVertexPosition[i]), rotate);
            }


            effect.Parameters["View"].SetValue(parent.View);
            effect.Parameters["Top"].SetValue(sheaderTop);
            effect.Parameters["Bottom"].SetValue(sheaderBottom);
        }
    }
}
