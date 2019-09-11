using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Shoot.Battle.EffectSystem
{
    class SpellCharge : VirtualEffectSystem
    {
        private int particleCount;
        private int enableCriate;
        private int particleInterval;

        private float particleRadius;

        private Particle.State pState;

        public SpellCharge(Vector2 setPosition , Particle.State state) : base(setPosition)
        {
            particleCount = 0;
            enableCriate = 60;
            particleInterval = 30;

            particleRadius = 200f;

            pState = state;
        }

        protected override void MoveUpdate(int deltaTime)
        {
        }

        protected override void StateUpdate()
        {
            if (time > 4000)
                isClose = true;

            if(particleCount < enableCriate && time / particleInterval >= particleCount)
            {
                if(pState == Particle.State.DIVERGENCE)
                {

                }

                var d = time / 4.51;
                var p = basePosision + new Vector2((float)Math.Cos(d), (float)Math.Sin(d)) * particleRadius;
                particles.Add(new Particle(basePosision, p, pState));
                particleCount++;
            }
        }

        public override void Draw(OutPuts.Renderer renderer)
        {
            particles.ForEach(p => p.Draw(renderer, "maho2"));
        }
    }
}
