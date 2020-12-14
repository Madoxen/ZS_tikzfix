using System;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    class LineTool : ITool
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
                    x1 = canvasEventArgs.X;
                    y1 = canvasEventArgs.Y;

                    current = new DrawingShape(
                    new Line
                    {
                        Stroke = StrokeColor,
                        X1 = x1,
                        X2 = x1,
                        Y1 = y1,
                        Y2 = y1,
                        StrokeThickness = DEF_STROKE_THICKNESS
                    }, ShapeState.START);
            }
            else
            {
                if (current.Shape is not Line l)
                    throw new Exception("Shape-Tool type mismatch, tool type: LineTool, expected shape type Line");

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
