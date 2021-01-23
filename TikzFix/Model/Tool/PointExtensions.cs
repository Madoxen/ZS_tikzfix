using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TikzFix.Model.Tool
{
    static class PointExtensions
    {
        public static Point CreateFromPoint(Point p)
        {
            return new Point(p.X, p.Y);
        }
    }
}
