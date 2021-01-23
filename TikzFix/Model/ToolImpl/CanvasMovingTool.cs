using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.ToolImpl
{
    internal class CanvasMovingTool : ITool
    {
        private Point previousMousePos;
        private ICollection<TikzShape> shapes;
        private DrawingShape dummy = new DrawingShape(new TikzLine(new Shapes.ArrowLine(), null), ShapeState.START);

        public CanvasMovingTool(ICollection<TikzShape> shapes)
        {
            this.shapes = shapes;
        }

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                dummy = new DrawingShape(new TikzLine(new Shapes.ArrowLine(), null), ShapeState.START);
                previousMousePos = canvasEventArgs.Point;
            }
            else
            {
                Point mousePos = canvasEventArgs.Point;
                Vector delta = mousePos - previousMousePos;
                previousMousePos = mousePos;
                foreach (TikzShape s in shapes)
                {
                    Canvas.SetLeft(s.Shape, Canvas.GetLeft(s.Shape) + delta.X);
                    Canvas.SetTop(s.Shape, Canvas.GetTop(s.Shape) + delta.Y);
                }

                if (canvasEventArgs.MouseState == MouseState.UP)
                {
                    dummy.ShapeState = ShapeState.FINISHED;
                }
                else if (canvasEventArgs.MouseState == MouseState.MOVE)
                {
                    dummy.ShapeState = ShapeState.DRAWING;
                }
            }
            dummy.RemoveOnFinish = true;
            return dummy;
        }

        public void Reset()
        {
            //Do nothing
        }
    }
}
