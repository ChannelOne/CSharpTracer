using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;
using RayTracer.Materials;

namespace RayTracer.Shape
{
    public class Plane : Intersectable, IGeometry
    {
        private Vector<float> normal;
        private Nullable<float> distance;
        private Vector<float> position;

        public Vector<float> Normal
        {
            get { return normal; }
            set
            {
                normal = value;
                if (distance != null)
                    computePosition();
            }
        }
        public float Distance
        {
            get { return distance == null ? float.MaxValue : distance.Value; }
            set
            {
                distance = new Nullable<float>(value);
                if (distance != null)
                    computePosition();
            }
        }

        public IMaterial Material { get; set; }

        private void computePosition()
        {
            position = normal * distance.Value;
        }

        public IntersectResult Intersect(Ray ray)
        {
            var a = ray.Direction.DotProduct(Normal);
            if (a >= 0)
                return IntersectResult.NullResult;

            var b = Normal.DotProduct(ray.Origin - position);

            float _distance = -b / a;
            return new IntersectResult()
            {
                Geometry = this,
                Distance = _distance,
                Position = ray.GetPoint(_distance),
                Normal = normal,
            };
        }
    }
}
