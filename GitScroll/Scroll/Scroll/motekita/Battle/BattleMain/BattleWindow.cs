using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot.Battle.BattleMain
{
    static class BattleWindow
    {
        public static readonly int Left = 100;
        public static readonly int Up = 50;
        public static readonly int Right = 740;
        public static readonly int Down = 910;

        public static readonly int DeleteLeft = -400;
        public static readonly int DeleteUp = -450;
        public static readonly int DeleteRight = 1240;
        public static readonly int DeleteDown = 1410;

        public static readonly int ItemVacuumUp = 350;

        public static readonly Vector2 Center = new Vector2(420f,480f);

        public static readonly Vector2 DrawParam = new Vector2(800f, 300f);


        public static bool IsAreaOut(Vector2 pos)
        {
            if (pos.X < DeleteLeft ||
                pos.X > DeleteRight ||
                pos.Y < DeleteUp ||
                pos.Y > DeleteDown)
            {
                return true;
            }
            else
                return false;
        }

        public static bool IsGrayAreaOut(Vector2 pos)
        {
            if (pos.X < DeleteLeft + 400f ||
                pos.X > DeleteRight - 400f ||
                pos.Y < DeleteUp + 400f ||
                pos.Y > DeleteDown - 400f)
            {
                return true;
            }
            else
                return false;
        }

    }
}
