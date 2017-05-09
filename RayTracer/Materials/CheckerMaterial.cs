using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Materials
{
    public sealed class CheckerMaterial : IMaterial
    {

        public CheckerMaterial(float scale)
        {
            Scale = scale;
        }

        public float Scale { get; set; }

        public Color Sample(Ray ray, Vector<float> position, Vector<float> normal)
        {
            var tmp = Math.Abs(Math.Floor(position[0] * 0.1) + Math.Floor(position[2] * Scale)) % 2;
            return tmp < 1 ? Colors.Black : Colors.White;
        }
    }
}
