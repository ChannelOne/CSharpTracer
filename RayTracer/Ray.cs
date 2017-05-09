using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

namespace RayTracer
{
    public sealed class Ray
    {
        private Vector<float> origin;
        private Vector<float> direction;

        public Vector<float> Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public Vector<float> Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public Vector<float> GetPoint(float t)
        {
            return origin + direction * t;
        }

    }
}
