using System;
using System.Windows;
using System.Windows.Shapes;

namespace TikzFix.Utils
{
    static class ShapeUtils
    {
        public static Thickness GetMargin(int left, int top)
        {
            return new Thickness(left, top, 0, 0);
        }
    }
}
