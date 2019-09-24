using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scroll.Output;

namespace Scroll.GameSystem
{
    abstract class Scene
    {
        protected GameMain parent;
        protected Renderer renderer;
        protected bool isClose;

        protected Sound sound;

        public Scene(GameMain gameMain, Renderer renderer,Sound sound)
        {
            parent = gameMain;
            this.renderer = renderer;
            isClose = false;
            this.sound = sound;

        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(Output.Renderer renderer);

        public bool IsClose()
        {
            return isClose;
        }
    }

}
