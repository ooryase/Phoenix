using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Shoot.OutPuts;

namespace Shoot.Battle.PlayerBullet
{
    class TestHoming : VirtualPlayerBullet
    {
        public TestHoming(Vector2 pPos, Vector2 ePos, int pPower) : base(pPos, ePos, pPower)
        {
            radius = 16f;
            power = pPower * 9;
            speed = 0.95f;

            position = pPos;
            direction = -Vector2.UnitY;
        }


        protected override void MoveUpdate(int deltaTime, Vector2 targetPos)
        {
            if (time > 140)
            {
                var d = new Vector2(targetPos.X - position.X, targetPos.Y - position.Y);
                d.Normalize();

                direction += d * 0.49f; ;
                direction.Normalize();
            }

            position += direction * speed * deltaTime;
        }

        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture("tama",new Vector2(position.X - radius, position.Y - radius));
        }

    }
}
