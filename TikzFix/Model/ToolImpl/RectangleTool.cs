using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    class RectangleTool : ITool
    {
        private const int DEF_STROKE_THICKNESS = 2;

        public SolidColorBrush StrokeColor
        {
            get; set;
        } = Brushes.Black;


        public SolidColorBrush FillColor
        {
            get; set;
        } = Brushes.Transparent;

        private int x1, y1;
        private DrawingShape current;

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                current = new DrawingShape(
                new Rectangle
                {
                    Stroke = StrokeColor,
                    StrokeThickness = DEF_STROKE_THICKNESS,
                    Fill = FillColor
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
            current.Shape.Width = Math.Abs(x1 - canvasEventArgs.X);
            current.Shape.Height = Math.Abs(y1 - canvasEventArgs.Y);

            current.Shape.Margin = ShapeUtils.GetMargin(
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

        public LocalShapeData ConvertToShapeData(Shape shape)
        {
            if (shape is not Rectangle e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Rectangle");
            }

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs((int)e.Margin.Left,(int)e.Margin.Top, MouseState.DOWN),
                new CanvasEventArgs((int)(e.Margin.Left+e.Width), (int)(e.Margin.Top+e.Height), MouseState.UP)
            };

            return new LocalShapeData(GetType().Name, keyPointList);
        }

        public string GenerateTikzShape(Shape shape)
        {
            if (shape is not Rectangle e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Rectangle");
            }

            return $"\\draw ({(int)e.Margin.Left},{(int)e.Margin.Top}) rectangle ({(int)(e.Width + e.Margin.Left)},{(int)(e.Height + e.Margin.Top)});";
        }
    }
}
