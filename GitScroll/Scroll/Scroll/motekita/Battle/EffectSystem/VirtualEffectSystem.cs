using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.EffectSystem
{
    abstract class VirtualEffectSystem
    {
        protected Vector2 basePosision;
        protected int time;

        protected bool isClose;


        protected List<Particle> particles;

        public VirtualEffectSystem(Vector2 setPosition)
        {
            basePosision = setPosition;
            time = 0;
            particles = new List<Particle>();

            isClose = false;
        }

        public void startUpdate(int deltaTime)
        {
            TimeUpdate(deltaTime);

            particles.ForEach(p => p.StartUpdate(deltaTime));
        }

        public void MainUpdate(int deltaTime)
        {
            StateUpdate();
            MoveUpdate(deltaTime);

            particles.ForEach(p => p.MainUpdate(deltaTime));

        }

        public void EndUpdate()
        {
            particles.RemoveAll(p => p.IsDead());
        }


        protected void TimeUpdate(int deltaTime)
        {
            time += deltaTime;
        }

        protected abstract void StateUpdate();

        protected abstract void MoveUpdate(int deltaTime);

        public abstract void Draw(OutPuts.Renderer renderer);        

        public bool IsClose()
        {
            return isClose;
        }
    }
}
