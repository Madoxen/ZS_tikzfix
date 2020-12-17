using System;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class EllipseTool : ITool
    {
        private const int DEF_STROKE_THICKNESS = 2;

        public SolidColorBrush StrokeColor
        {
            get; set;
        } = Brushes.Black;


        private int x1, y1;
        private DrawingShape current;



        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                current = new DrawingShape(
                new Ellipse
                {
                    Stroke = StrokeColor,
                    StrokeThickness = DEF_STROKE_THICKNESS,
                },
                ShapeState.START
            );

                x1 = canvasEventArgs.X;
                y1 = canvasEventArgs.Y;
            }
            else
            {
                UpdateCurrent(canvasEventArgs);
            }
            return current;
        }


        private void UpdateCurrent(CanvasEventArgs canvasEventArgs)
        {
            current.Shape.Width = Math.Abs(x1 - canvasEventArgs.X) * 2;
            current.Shape.Height = Math.Abs(y1 - canvasEventArgs.Y) * 2;

            current.Shape.Margin = ShapeUtils.GetMargin(
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
    }
}