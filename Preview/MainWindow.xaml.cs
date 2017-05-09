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

        private BitmapSource RenderImage()
        {
            int width = 500;
            int height = 500;
            byte[] pixels = RayTracer.RayTracer.RenderDepth(width, height,
                new RayTracer.Shape.Sphere()
                {
                    Center=Vector<float>.Build.DenseOfArray(new float[] { 0f, 10f, -10f }),
                    Radius=10f,
                },
                new RayTracer.PerspectiveCamera()
                {
                    Eye=Vector<float>.Build.DenseOfArray(new float[] { 0f, 10f, 10f }),
                    Front=Vector<float>.Build.DenseOfArray(new float[] { 0f, 0f, -1f}),
                    Up=Vector<float>.Build.DenseOfArray(new float[] { 0f, 1f, 0f}),
                    Fov=90,
                },
                20);
            BitmapPalette myPelette = BitmapPalettes.Gray256;
            BitmapSource image = BitmapSource.Create(
                    width,
                    height,
                    96,
                    96,
                    PixelFormats.Indexed8,
                    myPelette,
                    pixels,
                    width
                );

            return image;
        }
    }
}
