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

        public static string MapShapeWithTool(this Shape shape)
        {
            if (shape is Line)
                return "LineTool";
            else if (shape is Ellipse)
                return "EllipseTool";
            else if (shape is Rectangle)
                return "RectangleTool";

            throw new Exception("Unsuported shape type. Currently suppoerted: [Line, Ellipse, Rectangle]");
        }
    }
}
