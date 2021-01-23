using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Styling;

namespace TikzFix.Utils
{
    internal static class ShapeUtils
    {
        public static Thickness GetMargin(double left, double top)
        {
            return new Thickness(left, top, 0, 0);
        }

        public static Thickness GetMarginEqual(Point first, Point second)
        {
            return GetMargin(
                    first.X - Math.Abs(first.X - second.X),
                    first.Y - Math.Abs(first.Y - second.Y)
                );
        }

        public static Thickness GetMarginLower(Point first, Point second)
        {
            return GetMargin(
                    Math.Min(first.X, second.X),
                    Math.Min(first.Y, second.Y)
                );
        }


        public static void SetStyle(this Shape s, TikzStyle style)
        {
            if (style == null)
                return;

            s.Stroke = new SolidColorBrush(style.StrokeColor);
            s.Fill = new SolidColorBrush(style.FillColor);
            s.StrokeThickness = style.LineWidth.GetLineWidth();
            s.StrokeDashArray = style.LineType.GetDashArray();
            s.StrokeStartLineCap = style.LineEnding.GetLineCaps()[0];
            s.StrokeEndLineCap = style.LineEnding.GetLineCaps()[1];
            s.StrokeDashCap = style.LineEnding.GetLineCaps()[2];
        }
    }
}
