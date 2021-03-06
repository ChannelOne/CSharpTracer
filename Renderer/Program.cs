﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathNet.Numerics.LinearAlgebra;

namespace Renderer
{
    class Program
    {
        private static byte[] ColorToBytes(Color[] colors)
        {
            var pixels = new byte[colors.Length * 4];
            int i = 0;
            foreach(Color color in colors)
            {
                pixels[i++] = color.R;
                pixels[i++] = color.G;
                pixels[i++] = color.B;
                pixels[i++] = color.A;
            }
            return pixels;
        }

        static void Main(string[] args)
        {
            int width = 500;
            int height = 500;
            Color[] colors = RayTracer.RayTracer.RenderDepth(width, height,
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
            byte[] pixels = ColorToBytes(colors);
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
            FileStream stream = new FileStream("new.png", FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
            stream.Close();
        }
    }
}
