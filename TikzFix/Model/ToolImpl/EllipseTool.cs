using System;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class EllipseTool : ITool
    {
        private int x1, y1;
        private DrawingShape current;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                Ellipse ellipse = new Ellipse
                {
                    Stroke = new SolidColorBrush(style.StrokeColor.GetColor()),
                    StrokeThickness = style.LineWidth.GetLineWidth(),
                    Fill = new SolidColorBrush(style.FillColor.GetColor()),
                };

                current = new DrawingShape(new TikzEllipse(ellipse, style), ShapeState.START);

                x1 = canvasEventArgs.X;
                y1 = canvasEventArgs.Y;
            }
            else
            {
                current.TikzShape.Shape.Width = Math.Abs(x1 - canvasEventArgs.X) * 2;
                current.TikzShape.Shape.Height = Math.Abs(y1 - canvasEventArgs.Y) * 2;

                current.TikzShape.Shape.Margin = ShapeUtils.GetMargin(
                    x1 - Math.Abs(x1 - canvasEventArgs.X),
                    y1 - Math.Abs(y1 - canvasEventArgs.Y)
                );

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