using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
