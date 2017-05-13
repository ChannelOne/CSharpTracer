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

        public async static Task<Color[]> RenderReflection(int width, int height, Shape.Intersectable scene, PerspectiveCamera camera, int maxReflect)
        {
            Color[] colors = new Color[width * height];
            int i = 0;

            int half = height / 2;
            await Task.Run(() =>
            {
                for (var y = 0; y < half; ++y)
                {
                    float sy = 1 - y * 1f / height;
                    for (var x = 0; x < width; ++x)
                    {
                        float sx = x * 1f / width;
                        var ray = camera.GenerateRay(sx, sy);
                        var color = RayTraceRecursive(scene, ray, maxReflect);
                        colors[i++] = color;
                    }
                }
            });
            await Task.Run(() => {
                int j = half * width;
                for (var y = half + 1; y < height; ++y)
                {
                    float sy = 1 - y * 1f / height;
                    for (var x = 0; x < width; ++x)
                    {
                        float sx = x * 1f / width;
                        var ray = camera.GenerateRay(sx, sy);
                        var color = RayTraceRecursive(scene, ray, maxReflect);
                        colors[j++] = color;
                    }
                }
            });
            return colors;
        }

        public static Color RayTraceRecursive(Shape.Intersectable scene, Ray ray, int maxReflect)
        {
            var result = scene.Intersect(ray);

            if (result.Geometry != null)
            {
                float reflectiveness = result.Geometry.Material.Reflectiveness;
                var color = result.Geometry.Material.Sample(ray, result.Position, result.Normal);
                color = color * (1 - reflectiveness);

                if (reflectiveness > 0 && maxReflect > 0)
                {
                    var r = result.Normal * (-2 * result.Normal.DotProduct(ray.Direction)) + ray.Direction;
                    ray = new Ray()
                    {
                        Origin = result.Position,
                        Direction = r,
                    };
                    var reflectedColor = RayTraceRecursive(scene, ray, maxReflect - 1);
                    color += reflectedColor * reflectiveness;
                }

                return color;
            }
            else
                return Colors.Black;
        }

    }
}
