using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Materials
{
    public interface IMaterial
    {
        float Reflectiveness { get; }
        Color Sample(Ray ray, Vector<float> position, Vector<float> normal);
    }
}
