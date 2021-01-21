using System;
using System.Windows;

namespace TikzFix.Utils
{
    internal static class VectorUtils
    {
        public static Vector Rotate(Vector v, double angle)
        {
            Vector result = new Vector(0, 0)
            {
                X = v.X * Math.Cos(angle) - v.Y * Math.Sin(angle),
                Y = v.X * Math.Sin(angle) + v.Y * Math.Cos(angle)
            };

            return result;
        }


    }
}
