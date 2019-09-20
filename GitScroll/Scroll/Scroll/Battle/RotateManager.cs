using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scroll.Battle
{
    struct RotateManager
    {
        public Vector3 cameraRotate;

        internal Vector3 YawPitchRoll(Vector3 vector3)
        {
            return Roll(Pitch(Yaw(vector3)));
        }

        internal Vector3 NewYawPitchRoll(Vector3 vector3 , Vector3 rotate)
        {
            return NewRoll(NewPitch(NewYaw(vector3,(double)rotate.X), (double)rotate.Y), (double)rotate.Z);
        }

        /// <summary>
        /// Yawの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Yaw(Vector3 vector3)
        {
            var s = (float)Math.Sin(cameraRotate.X);
            var c = (float)Math.Cos(cameraRotate.X);

            var x = vector3.X;
            var y = vector3.Y * c - vector3.Z * s;
            var z = vector3.Y * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Pitchの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Pitch(Vector3 vector3)
        {
            var s = (float)Math.Sin(cameraRotate.Y);
            var c = (float)Math.Cos(cameraRotate.Y);

            var x = vector3.X * c + vector3.Z * s;
            var y = vector3.Y;
            var z = -vector3.X * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rollの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 Roll(Vector3 vector3)
        {
            var s = (float)Math.Sin(cameraRotate.Z);
            var c = (float)Math.Cos(cameraRotate.Z);

            var x = vector3.X * c - vector3.Y * s;
            var y = vector3.X * s + vector3.Y * c;
            var z = vector3.Z;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Yawの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 NewYaw(Vector3 vector3,double rotate)
        {
            var s = (float)Math.Sin(rotate);
            var c = (float)Math.Cos(rotate);

            var x = vector3.X;
            var y = vector3.Y * c - vector3.Z * s;
            var z = vector3.Y * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Pitchの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 NewPitch(Vector3 vector3, double rotate)
        {
            var s = (float)Math.Sin(rotate);
            var c = (float)Math.Cos(rotate);

            var x = vector3.X * c + vector3.Z * s;
            var y = vector3.Y;
            var z = -vector3.X * s + vector3.Z * c;

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Rollの回転処理
        /// </summary>
        /// <param name="vector3"></param>
        /// <returns></returns>
        internal Vector3 NewRoll(Vector3 vector3,double rotate)
        {
            var s = (float)Math.Sin(rotate);
            var c = (float)Math.Cos(rotate);

            var x = vector3.X * c - vector3.Y * s;
            var y = vector3.X * s + vector3.Y * c;
            var z = vector3.Z;

            return new Vector3(x, y, z);
        }
    }
}
