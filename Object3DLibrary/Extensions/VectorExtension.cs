using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Object3DLibrary.Extensions
{
    internal static class VectorExtension
    {
        public static bool IsBetween(this Vector2 vector, Vector2 min, Vector2 max)
        {
            return vector.X >= min.X && vector.X <= max.X &&
                   vector.Y >= min.Y && vector.Y <= max.Y;
        }
    }
}
