using System;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.ToolImpl
{
    internal class LineTool : ITool
    {
        private int x1, y1;
        private DrawingShape current;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                x1 = canvasEventArgs.X;
                y1 = canvasEventArgs.Y;

                var line = new Line
                {
                    Stroke = new SolidColorBrush(style.StrokeColor.GetColor()),
                    X1 = x1,
                    X2 = x1,
                    Y1 = y1,
                    Y2 = y1,
                    StrokeThickness = style.LineWidth.GetLineWidth()
                };

                current = new DrawingShape(new TikzLine(line, style), ShapeState.START);

            }
            else
            {
                if (current.TikzShape.Shape is not Line l)
                {
                    throw new Exception("Shape-Tool type mismatch, tool type: LineTool, expected shape type Line");
                }

                l.X2 = canvasEventArgs.X;
                l.Y2 = canvasEventArgs.Y;

                if (canvasEventArgs.MouseState == MouseState.UP)
                {
                    current.ShapeState = ShapeState.FINISHED;
                }
                else if (canvasEventArgs.MouseState == MouseState.MOVE)
                {
                    current.ShapeState = ShapeState.DRAWING;
                }
            }
            return current;
        }
    }
}
