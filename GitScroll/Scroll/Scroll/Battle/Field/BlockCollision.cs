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
        public Vector3? CheckCollision(Block block, VirtualObject virtualObject)
        {
            switch (block.BName)
            {
                case BlockName.LEFT_UP:
                    LEFT_UPCollision(block, virtualObject);
                    break;
                case BlockName.UP:
                    UPCollision(block, virtualObject);
                    break;
                case BlockName.RIGHT_UP:
                    RIGHT_UPCollision();
                    break;
                case BlockName.LEFT:
                    LEFTCollision();
                    break;
                case BlockName.CENTER:
                    CENTERCollision();
                    break;
                case BlockName.RIGHT:
                    RIGHTCollision();
                    break;
                case BlockName.BOTTOM_LEFT:
                    BOTTOM_LEFTCollision();
                    break;
                case BlockName.BOTTOM:
                    BOTTOMCollision();
                    break;
                case BlockName.BOTTOM_RIGHT:
                    BOTTOM_LEFTCollision();
                    break;
                default:
                    return null;
            }
            return null;
        }
        private Vector3? LEFT_UPCollision(Block block, VirtualObject virtualObject)
        {
            if(block.Position.X + 1 * block.Scale < virtualObject.Position.X)
            {
                return null;
            }
            if(block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                return null;
            }

            var b = new Vector3(block.Position.X + 1 * block.Scale, block.Position.Y - 1 * block.Scale, 0);
            var d = Vector3.DistanceSquared(virtualObject.Position, b);
            if(d > Math.Pow(virtualObject.Scale + block.Scale * 2f, 2f))
            {
                return null;
            }
            return Vector3.Normalize(virtualObject.Position - b) * d;
        }
        private Vector3? UPCollision(Block block, VirtualObject virtualObject)
        {
            if (block.Position.X - 1 * block.Scale > virtualObject.Position.X ||
                block.Position.X + 1 * block.Scale < virtualObject.Position.X)
            {
                 return null;
            }
            if(block.Position.Y - 1 * block.Scale > virtualObject.Position.Y)
            {
                 return null;
            }
            if(block.Position.Y + 1 * block.Scale < virtualObject.Position.Y - virtualObject.Scale)
            {
                return null;
            }
            return new Vector3(0, block.Position.Y + 1 * block.Scale - virtualObject.Position.Y - virtualObject.Scale, 0);

        }
        private Vector3? RIGHT_UPCollision()
        {
            return null;
        }
        private Vector3? LEFTCollision()
        {
            return null;
        }
        private Vector3? CENTERCollision()
        {
            return null;
        }
        private Vector3? RIGHTCollision()
        {
            return null;
        }
        private Vector3? BOTTOM_LEFTCollision()
        {
            return null;
        }
        private Vector3? BOTTOMCollision()
        {
            return null;
        }
        private Vector3? BOTTOM_RIGHTCollision()
        {
            return null;
        }
    }
}
