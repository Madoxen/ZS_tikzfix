using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    class EllipseTool : ITool
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

        public LocalShapeData ConvertToShapeData(Shape shape)
        {
            if (shape is not Ellipse e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
            }
            int X1 = (int)(e.Margin.Left + e.Width / 2);
            int Y1 = (int)(e.Margin.Top + e.Height / 2);

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs(X1, Y1, MouseState.DOWN),
                new CanvasEventArgs(X1+(int)(e.Width/2), (int)(Y1+e.Height/2), MouseState.UP)
            };

            return new LocalShapeData(GetType().Name, keyPointList);
        }

        public string GenerateTikzShape(Shape shape)
        {
            if (shape is not Ellipse e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
            }

            return $"\\draw ({(int)(e.Margin.Left + e.Width / 2)},{(int)(e.Margin.Top + e.Height / 2)}) ellipse ({(int)(shape.Width / 2)} and {(int)(shape.Height / 2)});";
        }
    }
}