using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System.Windows.Threading;

namespace Preview
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Task.Run(() =>
            {
                imageField.Dispatcher.Invoke(() =>
                {
                    var image = RenderImage();
                    imageField.Source = image;
                });
            });
        }

        private byte[] ColorToBytes(Color[] colors)
        {
            var pixels = new byte[colors.Length * 4];
            int i = 0;
            foreach(Color color in colors)
            {
                pixels[i++] = color.B;
                pixels[i++] = color.G;
                pixels[i++] = color.R;
                pixels[i++] = color.A;
            }
            return pixels;
        }

        private BitmapSource RenderImage()
        {
            int width = 500;
            int height = 500;
            var sphere1 = new RayTracer.Shape.Sphere()
            {
                Center = Vector<float>.Build.DenseOfArray(new float[] { -10f, 10f, -10f }),
                Radius = 10f,
                Material = new RayTracer.Materials.PhongMaterial(Colors.Red, Colors.White, 16),
            };

            var sphere2 = new RayTracer.Shape.Sphere()
            {
                Center = Vector<float>.Build.DenseOfArray(new float[] { 10, 10f, -10f }),
                Radius = 10f,
                Material = new RayTracer.Materials.PhongMaterial(Colors.Green, Colors.White, 16),
            };

            var plane = new RayTracer.Shape.Plane()
            {
                Normal = Vector<float>.Build.DenseOfArray(new[] { 0f, 1f, 0f }),
                Distance = 0f,
                Material = new RayTracer.Materials.CheckerMaterial(0.6f),
            };

            var union = new RayTracer.Shape.IntersectableUnion();
            union.Add(sphere1);
            union.Add(sphere2);
            union.Add(plane);
            Color[] colors = RayTracer.RayTracer.RenderMaterial(width, height,
                union,
                new RayTracer.PerspectiveCamera()
                {
                    Eye=Vector<float>.Build.DenseOfArray(new float[] { 0f, 5f, 15f }),
                    Front=Vector<float>.Build.DenseOfArray(new float[] { 0f, 0f, -1f}),
                    Up=Vector<float>.Build.DenseOfArray(new float[] { 0f, 1f, 0f}),
                    Fov=90,
                },
                20);
            byte[] pixels = ColorToBytes(colors);
            BitmapPalette myPelette = BitmapPalettes.Halftone256;
            BitmapSource image = BitmapSource.Create(
                    width,
                    height,
                    96,
                    96,
                    PixelFormats.Pbgra32,
                    myPelette,
                    pixels,
                    width * 4
                );

            return image;
        }
    }
}
