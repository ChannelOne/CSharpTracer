using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RayTracer
{
    public class RayTracer
    {
        public static Color[] RenderDepth(int width, int height, Shape.Intersectable scene, PerspectiveCamera camera, int maxDepth)
        {
            Color[] colors = new Color[width * height];
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
                        colors[i++] = Color.FromArgb(255,
                            (byte)((result.Normal[0] + 1) * 128),
                            (byte)((result.Normal[1] + 1) * 128),
                            (byte)((result.Normal[2] + 1) * 128));
                    } else
                    {
                        colors[i++] = Color.FromRgb(0, 0, 0);
                    }
                }
            }
            return colors;
        }

        public static Color[] RenderMaterial(int width, int height, Shape.Intersectable scene, PerspectiveCamera camera, int maxDepth)
        {
            Color[] colors = new Color[width * height];
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
                        var color = result.Geometry.Material.Sample(ray, result.Position, result.Normal);
                        colors[i++] = color;
                    } else
                    {
                        colors[i++] = Colors.Black;
                    }
                }
            }
            return colors;
        }

    }
}
