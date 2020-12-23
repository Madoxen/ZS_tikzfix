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
        private Point firstPoint;
        private DrawingShape current;

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                Rectangle rect = new Rectangle
                {
                    Stroke = new SolidColorBrush(style.StrokeColor.GetColor()),
                    StrokeThickness = style.LineWidth.GetLineWidth(),
                    Fill = new SolidColorBrush(style.FillColor.GetColor()),
                };

                current = new DrawingShape(new TikzRectangle(rect, style), ShapeState.START);

                firstPoint = canvasEventArgs.Point;
            }
            else
            {
                current.TikzShape.Shape.Width = Math.Abs(firstPoint.X - canvasEventArgs.Point.X);
                current.TikzShape.Shape.Height = Math.Abs(firstPoint.Y - canvasEventArgs.Point.Y);

                current.TikzShape.Shape.Margin = ShapeUtils.GetMarginLower(firstPoint, canvasEventArgs.Point);

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
