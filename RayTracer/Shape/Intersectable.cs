﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RayTracer.Materials;

namespace RayTracer.Shape
{
    public interface Intersectable
    {
        IntersectResult Intersect(Ray ray);
    }
}
