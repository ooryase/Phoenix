using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.EffectSystem
{
    class Particle
    {
        private Vector2 startPosition;
        private Vector2 endPosition;
        private Vector2 position;

        private int time;
        private readonly int endTime;

        private bool isDead;

        private float drawRadius;

        private float alpha;
        private float scale;

        public enum State
        {
            CONVERGENCE,
            DIVERGENCE
        }
        private State state;

        public Particle(Vector2 sPos, Vector2 ePos,State setState)
        {
            startPosition = sPos;
            endPosition = ePos;
            state = setState;
            endTime = 960;
            time = (int)state * endTime;
            
            isDead = false;

            drawRadius = 64f;
        }


        public void StartUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);
        }

        public void MainUpdate(int deltaTime)
        {
            StateUpdate();
            MoveUpdate(deltaTime);
        }

        private void TimeUpdate(int deltaTime)
        {
            if (state == State.CONVERGENCE)
                time += deltaTime;
            else if (state == State.DIVERGENCE)
                time -= deltaTime;
        }
        
        private void StateUpdate()
        {
            if (time < 0 || time > endTime)
                isDead = true;
        }

        private void MoveUpdate(int deltaTime)
        {
            var p = (float)Math.Pow((endTime - time) / (double)endTime, 2);

            scale = p;
            position = (1f - p) * startPosition + p * endPosition;
        }

        public void Draw(OutPuts.Renderer renderer, string tex)
        {
            if (time < 192)
            {
                alpha = time / 192f;
            }
            else
            {
                alpha = 1f;
            }

            /*if(time > 808)
            {
                scale = (endTime - time) / 192f;
            }
            else
            {
                scale = 1f;
            }*/

            var v = new Vector2(position.X - drawRadius * scale, position.Y - drawRadius * scale);
            var r = new Rectangle((int)(position.X), (int)(position.Y), (int)(64f * scale), (int)(64f * scale));

            if(state == State.DIVERGENCE)
            { 
}

            renderer.DrawTexture(tex, v, scale, Color.White * alpha);
            //renderer.DrawTexture(tex, v, alpha);


        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}
