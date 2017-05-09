using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

using RayTracer.Materials;

namespace RayTracer.Shape
{
    public sealed class Sphere : Intersectable, IGeometry
    {
        public IMaterial Material { get; set; }
        public Vector<float> Center { get; set; }

        private float sqrRadius;
        private float radius;

        public float Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                sqrRadius = radius * radius;
            }
        }

        public float SqrRadius
        {
            get { return sqrRadius; }
        }

        public IntersectResult Intersect(Ray ray)
        {
            var v = ray.Origin - Center;
            var a0 = v.DotProduct(v) - sqrRadius;
            var dv = ray.Direction.DotProduct(v);

            if (dv <= 0)
            {
                var discr = dv * dv - a0;
                if (discr >= 0)
                {
                    var distance = -dv - (float)Math.Sqrt(discr);
                    var position = ray.GetPoint(distance);
                    return new IntersectResult()
                    {
                        Geometry = this,
                        Distance = distance,
                        Position = position,
                        Normal = (position - Center).Normalize(2.0)
                    };
                }
            }
            return IntersectResult.NullResult;
        }
    }
}
