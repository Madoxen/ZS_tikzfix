using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TikzFix.Utils
{
    static class VectorUtils
    {
        public static Vector Rotate(Vector v, double angle)
        {
            Vector result = new Vector(0, 0);

            result.X = v.X * Math.Cos(angle) - v.Y * Math.Sin(angle);
            result.Y = v.X * Math.Sin(angle) + v.Y * Math.Cos(angle);

            return result;
        }


    }
}
