using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer.Shape
{
    public sealed class IntersectableUnion : List<Intersectable>, Intersectable
    {

        public IntersectResult Intersect(Ray ray)
        {
            var minDistance = float.MaxValue;
            var minResult = IntersectResult.NullResult;
            foreach (var geo in this)
            {
                var result = geo.Intersect(ray);
                if (result.Geometry != null && result.Distance < minDistance)
                {
                    minDistance = result.Distance;
                    minResult = result;
                }
            }
            return minResult;
        }

    }
}
