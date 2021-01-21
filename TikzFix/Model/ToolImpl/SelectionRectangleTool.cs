using System;
using System.Windows;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class SelectionRectangleTool : ITool
    {
        private Tool.Point firstPoint;
        private DrawingShape current;

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                Rectangle rect = new Rectangle();
                rect.SetStyle(TikzStyle.SelectionStyle);
                current = new DrawingShape(new TikzRectangle(rect, null), ShapeState.START)
                {
                    RemoveOnFinish = true
                };
                firstPoint = canvasEventArgs.Point;
            }
            else
            {
                current.TikzShape.Shape.Width = Math.Abs(firstPoint.X - canvasEventArgs.Point.X);
                current.TikzShape.Shape.Height = Math.Abs(firstPoint.Y - canvasEventArgs.Point.Y);
                current.TikzShape.Shape.Margin = new Thickness(Math.Min(firstPoint.X, canvasEventArgs.Point.X), Math.Min(firstPoint.Y, canvasEventArgs.Point.Y), 0, 0);

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
            firstPoint = null;
            current = null;
        }
    }
}
