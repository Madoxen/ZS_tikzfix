using System;
using System.Windows;

namespace TikzFix.Utils
{
    static class ShapeUtils
    {
        public static Thickness GetMargin(int left, int top)
        {
            return new Thickness(left, top, 0, 0);
        }

        public static Thickness GetMarginEqual(Model.Tool.Point first, Model.Tool.Point second)
        {
            return GetMargin(
                    first.X - Math.Abs(first.X - second.X),
                    first.Y - Math.Abs(first.Y - second.Y)
                );
        }

        public static Thickness GetMarginLower(Model.Tool.Point first, Model.Tool.Point second)
        {
            return GetMargin(
                    Math.Min(first.X, second.X),
                    Math.Min(first.Y, second.Y)
                );
        }
    }
}
