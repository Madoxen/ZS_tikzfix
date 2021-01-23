using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class EllipseTool : ITool
    {
        private Point firstPoint;
        private DrawingShape current;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                Ellipse ellipse = new Ellipse
                {
                    StrokeThickness = style.LineWidth.GetLineWidth(),
                };

                ellipse.SetStyle(style);
                current = new DrawingShape(new TikzEllipse(ellipse, style), ShapeState.START);
                firstPoint = canvasEventArgs.Point;
            }
            else
            {
                double x = Math.Min(canvasEventArgs.Point.X, firstPoint.X);
                double y = Math.Min(canvasEventArgs.Point.Y, firstPoint.Y);

                double w = Math.Max(canvasEventArgs.Point.X, firstPoint.X) - x;
                double h = Math.Max(canvasEventArgs.Point.Y, firstPoint.Y) - y;

                if (canvasEventArgs.ModKey)
                {
                    if (canvasEventArgs.Point.X > firstPoint.X)
                    {
                        w = h;
                    }
                    else if (canvasEventArgs.Point.Y > firstPoint.Y)
                    {
                        h = w;
                    }
                    else
                    {
                        double b = Math.Min(firstPoint.X - canvasEventArgs.Point.X, firstPoint.Y - canvasEventArgs.Point.Y);
                        w = b;
                        h = b;
                        x = firstPoint.X - b;
                        y = firstPoint.Y - b;
                    }
                }

                Canvas.SetLeft(current.TikzShape.Shape, x);
                Canvas.SetTop(current.TikzShape.Shape, y);

                current.TikzShape.Shape.Width = w;
                current.TikzShape.Shape.Height = h;

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

        public void Reset()
        {
            current = null;
        }
    }
}