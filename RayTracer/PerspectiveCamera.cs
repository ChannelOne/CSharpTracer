using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer
{
    public static class MyVectorExtension
    {
        public static Vector<float> CrossProduct(this Vector<float> u, Vector<float> v)
        {
            return Vector<float>.Build.DenseOfArray(new float[]
                {
                    -u[2] * v[1] + u[1] * v[2],
                    u[2] * v[0] - u[0] * v[2],
                    -u[1] * v[0] + u[0] * v[1],
                });
        }
    }

    public sealed class PerspectiveCamera
    {
        private float fov;
        private float fovScale;
        private Vector<float> front;
        private Vector<float> up;
        private Vector<float> right;

        public Vector<float> Eye { get; set; }

        public Vector<float> Front
        {
            get { return front; }
            set
            {
                front = value;
                if (up != null)
                    right = front.CrossProduct(up);
            }
        }

        public Vector<float> Up
        {
            get { return up; }
            set
            {
                up = value;
                if (front != null)
                    right = front.CrossProduct(up);
            }
        }

        public float Fov
        {
            get { return fov; }
            set
            {
                fov = value;
                fovScale = (float)Math.Tan(fov * 0.5 * Math.PI / 180) * 2f;
            }
        }

        public Ray GenerateRay(float x, float y)
        {
            var r = right.Multiply((x - 0.5f) * fovScale);
            var u = up.Multiply((y - 0.5f) * fovScale);
            return new Ray()
            {
                Origin = Eye.Clone(),
                Direction = (front + r + u).Normalize(2),
            };
        }

    }
}
