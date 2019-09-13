using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scroll.Battle.Field.Block;

namespace Scroll.Battle.Field
{
    class BlockCollision
    {
        public Vector3 CheckCollision(Block block, VirtualObject virtualObject)
        {
            switch (block.BName)
            {
                case BlockName.LEFT_UP:
                    return LEFT_UPCollision(block, virtualObject);
                case BlockName.UP:
                    return UPCollision(block, virtualObject);
                case BlockName.RIGHT_UP:
                    RIGHT_UPCollision(block, virtualObject);
                    return RIGHTCollision(block, virtualObject);
                case BlockName.LEFT:
                    LEFTCollision(block, virtualObject);
                    return LEFTCollision(block, virtualObject);
                case BlockName.CENTER:
                    CENTERCollision();
                    break;
                case BlockName.RIGHT:
                    RIGHTCollision(block, virtualObject);
                    return RIGHTCollision(block, virtualObject);
                case BlockName.BOTTOM_LEFT:
                    BOTTOM_LEFTCollision(block, virtualObject);
                    return BOTTOM_LEFTCollision(block, virtualObject);
                case BlockName.BOTTOM:
                    BOTTOMCollision(block, virtualObject);
                    return BOTTOMCollision(block, virtualObject);
                case BlockName.BOTTOM_RIGHT:
                    BOTTOM_RIGHTCollision(block, virtualObject);
                    break;
                default:
                    return Vector3.Zero;
            }
            return Vector3.Zero;
        }
        private Vector3 LEFT_UPCollision(Block block, VirtualObject virtualObject)
        {
            if(block.Position.X + 1 * block.Scale < virtualObject.Position.X)
            {
                return Vector3.Zero;
            }
            if(block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                return Vector3.Zero;
            }

            var b = new Vector3(block.Position.X + 1 * block.Scale, block.Position.Y - 1 * block.Scale, 0);
            var d = Vector3.DistanceSquared(virtualObject.Position, b);
            var r = Math.Pow(virtualObject.Scale + block.Scale * 2f, 2f);
            if (d > r)
            {
                return Vector3.Zero;
            }
            return Vector3.Normalize(virtualObject.Position - b) * (float)Math.Sqrt(r - d);
        }
        private Vector3 UPCollision(Block block, VirtualObject virtualObject)
        {
            if (block.Position.X - 1 * block.Scale > virtualObject.Position.X ||
                block.Position.X + 1 * block.Scale < virtualObject.Position.X)
            {
                 return Vector3.Zero;
            }
            if(block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                 return Vector3.Zero;
            }
            if(block.Position.Y + 1 * block.Scale < virtualObject.Position.Y - virtualObject.Scale)
            {
                return Vector3.Zero;
            }
            var l = new Vector3(0, block.Position.Y + 1 * block.Scale - (virtualObject.Position.Y - virtualObject.Scale), 0);
            return l;

        }
        private Vector3 RIGHT_UPCollision(Block block, VirtualObject virtualObject)//まだテスト中
        {
            if (block.Position.X - 1 * block.Scale > virtualObject.Position.X)
            {
                return Vector3.Zero;
            }
            if (block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                return Vector3.Zero;
            }

            var b = new Vector3(block.Position.X - 1 * block.Scale, block.Position.Y - 1 * block.Scale, 0);
            var d = Vector3.DistanceSquared(virtualObject.Position, b);
            var r = Math.Pow(virtualObject.Scale + block.Scale * 2f, 2f);
            if (d > r)
            {
                return Vector3.Zero;
            }
            return Vector3.Normalize(virtualObject.Position - b) * (float)Math.Sqrt(r - d);
        }
        private Vector3 LEFTCollision(Block block, VirtualObject virtualObject)
        {
            if(block.Position.X - 1 * block.Scale < virtualObject.Position.X)
            {
                return Vector3.Zero;
            }
            if(block.Position.Y + 1 * block.Scale < virtualObject.Position.Y ||
                block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                return Vector3.Zero;
            }
            if(block.Position.X - 1 * block.Scale > virtualObject.Position.X + virtualObject.Scale)
            {
                return Vector3.Zero;
            }
            var l = new Vector3(block.Position.X - block.Scale - (virtualObject.Position.X + virtualObject.Scale), 0, 0);
            return l;
        }
        private Vector3 CENTERCollision()
        {
            return Vector3.Zero;
        }
        private Vector3 RIGHTCollision(Block block, VirtualObject virtualObject)
        {
            if (block.Position.X + 1 * block.Scale > virtualObject.Position.X)
            {
                return Vector3.Zero;
            }
            if (block.Position.Y + 1 * block.Scale < virtualObject.Position.Y ||
                block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                return Vector3.Zero;
            }
            if (block.Position.X + 1 * block.Scale < virtualObject.Position.X - virtualObject.Scale)
            {
                return Vector3.Zero;
            }
            var l = new Vector3(block.Position.X + block.Scale - (virtualObject.Position.X - virtualObject.Scale), 0, 0);
            return l;
        }
        private Vector3 BOTTOM_LEFTCollision(Block block, VirtualObject virtualObject)
        {
            return Vector3.Zero;
        }
        private Vector3 BOTTOMCollision(Block block, VirtualObject virtualObject)
        {
            if (block.Position.X - 1 * block.Scale > virtualObject.Position.X ||
                block.Position.X + 1 * block.Scale < virtualObject.Position.X)
            {
                return Vector3.Zero;
            }
            if (block.Position.Y - 1 * block.Scale < virtualObject.Position.Y)
            {
                return Vector3.Zero;
            }
            if (block.Position.Y - 1 * block.Scale > virtualObject.Position.Y + virtualObject.Scale)
            {
                return Vector3.Zero;
            }
            var l = new Vector3(0, block.Position.Y - 1 * block.Scale - (virtualObject.Position.Y + virtualObject.Scale), 0);
            return l;
        }
        private Vector3 BOTTOM_RIGHTCollision(Block block, VirtualObject virtualObject)
        {
            return Vector3.Zero;
        }
    }
}
