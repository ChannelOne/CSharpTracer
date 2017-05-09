using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Shape
{
    public sealed class IntersectResult
    {
        public static IntersectResult NullResult = new IntersectResult();

        public IGeometry Geometry { get; set; } = null;
        public float Distance { get; set; } = float.MaxValue;
        public Vector<float> Position { get; set; } = Vector<float>.Build.Dense(3);
        public Vector<float> Normal { get; set; } = Vector<float>.Build.Dense(3);
    }
}
