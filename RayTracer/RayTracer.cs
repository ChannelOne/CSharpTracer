using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    public class RayTracer
    {
        public static byte[] RenderDepth(int width, int height, Shape.Intersectable scene, PerspectiveCamera camera, int maxDepth)
        {
            // byte[] pixels = new byte[width * height * 4];
            byte[] pixels = new byte[width * height];
            int i = 0;
            for (var y = 0; y < height; ++y)
            {
                float sy = 1 - y * 1f / height;
                for (var x = 0; x < width; ++x)
                {
                    float sx = x * 1f / width;
                    var ray = camera.GenerateRay(sx, sy);
                    var result = scene.Intersect(ray);
                    if (result.Geometry != null)
                    {
                        var depth = 255f - (float)Math.Min((result.Distance / maxDepth) * 255, 255);
                        pixels[i++] = (byte)depth;
                        /*
                        pixels[i++] = (byte)depth;
                        pixels[i++] = (byte)depth;
                        pixels[i++] = 255;
                        */
                    } else
                    {
                        pixels[i++] = 0;
                    }
                }
            }
            return pixels;
        }
    }
}
