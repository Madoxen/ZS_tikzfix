using System;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class RectangleTool : ITool
    {


        private int x1, y1;
        private DrawingShape current;

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                var rect = new Rectangle
                {
                    Stroke = new SolidColorBrush(style.StrokeColor.GetColor()),
                    StrokeThickness = style.LineWidth.GetLineWidth(),
                    Fill = new SolidColorBrush(style.FillColor.GetColor()),
                };

                current = new DrawingShape(new TikzRectangle(rect, style), ShapeState.START);

                x1 = canvasEventArgs.X;
                y1 = canvasEventArgs.Y;
            }
            else
            {
                current.TikzShape.Shape.Width = Math.Abs(x1 - canvasEventArgs.X);
                current.TikzShape.Shape.Height = Math.Abs(y1 - canvasEventArgs.Y);

                current.TikzShape.Shape.Margin = ShapeUtils.GetMargin(
                    Math.Min(x1, canvasEventArgs.X),
                    Math.Min(y1, canvasEventArgs.Y)
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
