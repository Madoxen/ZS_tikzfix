using System;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class LineTool : ITool
    {
        private Point firstPoint;
        private DrawingShape current;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                firstPoint = canvasEventArgs.Point;

                var line = new Line
                {
                    X1 = firstPoint.X,
                    X2 = firstPoint.X,
                    Y1 = firstPoint.Y,
                    Y2 = firstPoint.Y,
                };

                line.SetStyle(style);
                current = new DrawingShape(new TikzLine(line, style), ShapeState.START);
               
            }
            else
            {
                if (current.TikzShape.Shape is not Line l)
                {
                    throw new Exception("Shape-Tool type mismatch, tool type: LineTool, expected shape type Line");
                }

                l.X2 = canvasEventArgs.Point.X;
                l.Y2 = canvasEventArgs.Point.Y;

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
