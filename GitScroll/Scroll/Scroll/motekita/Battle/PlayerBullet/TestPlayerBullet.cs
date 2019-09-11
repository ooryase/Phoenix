using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.PlayerBullet
{
    class TestPlayerBullet : VirtualPlayerBullet
    {
        public TestPlayerBullet(Vector2 pPos, Vector2 ePos, int pPower) : base(pPos, ePos, pPower)
        {
            radius = 16f;
            power = pPower * 12;
            speed = 1.3f;

            position = pPos;
            direction = -Vector2.UnitY;
        }


        protected override void MoveUpdate(int deltaTime , Vector2 targetPos)
        {
            position += direction * speed * deltaTime;
        }
        public override void Draw(OutPuts.Renderer renderer)
        {
            renderer.DrawTexture("tama", new Vector2(position.X - radius,position.Y - radius));
        }


    }
}
