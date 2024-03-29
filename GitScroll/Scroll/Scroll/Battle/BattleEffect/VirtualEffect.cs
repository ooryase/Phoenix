﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Scroll.GameSystem;
using Scroll.Output;

namespace Scroll.Battle.BattleEffect
{
    abstract class VirtualEffect : VirtualObject
    {
        protected VirtualObject baseObj;

        public VirtualEffect(Battle battle, Renderer renderer,Sound sound, VirtualObject baseObj, Vector3 position) 
            : base(battle, renderer,sound)
        {
            this.baseObj = baseObj;
            this.position = position;
        }

        abstract protected override void Awake();

        abstract protected override void NameSet();

        abstract public override void StartUpdate(int deltaTime);

        abstract public void MoveUpdate(int deltaTime);

        abstract public override void OnCollisionEnter(VirtualObject virtualObject);

        abstract public override void EndUpdate();

        abstract public override void DrawUpdate();
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

    }
}
