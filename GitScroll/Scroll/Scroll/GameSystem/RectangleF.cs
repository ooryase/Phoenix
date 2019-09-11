using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.GameSystem
{
    public struct RectangleF
    {
        public float Left;
        public float Top;
        public float Width;
        public float Height;

        public float Right { get => Left + Width; }
        public float Bottom { get => Top - Height; }

        public Vector2 Center { get => new Vector2(Left + Width / 2f, Top - Height / 2f); }

        public RectangleF(float left, float top, float width, float height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public bool Collision(RectangleF rectangleF)
        {
            if (Left > rectangleF.Right || Right < rectangleF.Left)
                return false;

            if (Bottom > rectangleF.Top || Top < rectangleF.Bottom)
                return false;

            return true;
        }
    }

    
}
