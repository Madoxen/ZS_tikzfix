using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Styling;

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


        public static void SetStyle(this Shape s, TikzStyle style)
        {
            s.Stroke = new SolidColorBrush(style.StrokeColor.GetColor());
            s.Fill = new SolidColorBrush(style.FillColor.GetColor());
            s.StrokeThickness = style.LineWidth.GetLineWidth();
            s.StrokeDashArray = style.LineType.GetDashArray();
            s.StrokeStartLineCap = style.LineEnding.GetLineCaps()[0];
            s.StrokeEndLineCap = style.LineEnding.GetLineCaps()[1];
            s.StrokeDashCap = style.LineEnding.GetLineCaps()[2];
        }
    }
}
