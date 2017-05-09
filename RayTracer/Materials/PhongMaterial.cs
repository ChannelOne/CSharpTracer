using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using MathNet.Numerics.LinearAlgebra;

namespace RayTracer.Materials
{
    public sealed class PhongMaterial : IMaterial
    {
        public static Vector<float> LightDir = Vector<float>.Build.DenseOfArray(new float[] { 1, 1, 1}).Normalize(2);
        public static Color LightColor = Colors.White;
        // private Vector<float> lightColorVec = Vector<float>.Build.DenseOfArray(new float[] { (float)LightColor.R / 255, (float)LightColor.G / 255, (float)LightColor.B / 255});
        private Vector<float> lightColorVec = Vector<float>.Build.DenseOfArray(new float[] { 1f, 1f, 1f});

        private Color diffuse;
        private Color specular;
        private Vector<float> diffuseVec;
        private Vector<float> specularVec;

        private Vector<float> ColorToVec(Color color)
        {
            return Vector<float>.Build.DenseOfArray(new float[]
            {
                (float)color.R / 255,
                (float)color.G / 255,
                (float)color.B / 255
            });
        }

        public Color Diffuse
        {
            get { return diffuse; }
            set
            {
                diffuse = value;
                diffuseVec = ColorToVec(diffuse);
            }
        }

        public Color Specular
        {
            get { return specular; }
            set
            {
                specular = value;
                specularVec = ColorToVec(specular);
            }
        }

        public float Shininess { get; set; }

        public PhongMaterial(Color diffuse, Color specular, float shiness)
        {
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shiness;
        }

        private Vector<float> Product(Vector<float> a , Vector<float> b)
        {
            return Vector<float>.Build.DenseOfArray(new float[] {
                a[0] * b[0],
                a[1] * b[1],
                a[2] * b[2],
            });
        }

        private Color VectorToColor(Vector<float> vec)
        {
            return Color.FromRgb(
                (byte)Math.Min(vec[0] * 255, 255),
                (byte)Math.Min(vec[1] * 255, 255),
                (byte)Math.Min(vec[2] * 255, 255)
                );
        }

        public Color Sample(Ray ray, Vector<float> position, Vector<float> normal)
        {
            var nl = normal.DotProduct(LightDir);
            var H = (LightDir - ray.Direction).Normalize(2);
            var nh = normal.DotProduct(H);

            var diffuseTerm = diffuseVec * Math.Max(nl, 0);
            var specularTerm = specularVec * (float)(Math.Pow(Math.Max(nh, 0), Shininess));
            var vec = Product(lightColorVec, (diffuseTerm + specularTerm));
            return VectorToColor(vec);
        }
    }
}
