using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Tool;

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
        private bool firstClick = true;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                if (firstClick)
                {
                    x1 = canvasEventArgs.X;
                    y1 = canvasEventArgs.Y;
                    firstClick = false;

                    // return EMPTY_SHAPE when DOWN | MOVE
                    // if need it can be changed to draw first point
                    // to show user where was clicked before selecting second point
                    return DrawingShape.EMPTY_SHAPE;
                }
                else
                {
                    firstClick = true;
                    return new DrawingShape(
                        new Line
                        {
                            Stroke = StrokeColor,
                            X1 = x1,
                            X2 = canvasEventArgs.X,
                            Y1 = y1,
                            Y2 = canvasEventArgs.Y,
                            StrokeThickness = DEF_STROKE_THICKNESS
                        },
                        ShapeState.FINISHED
                    );
                }
            }
            else if (canvasEventArgs.MouseState == MouseState.UP)
            {
                return DrawingShape.EMPTY_SHAPE;
            }
            else
            {
                return DrawingShape.EMPTY_SHAPE;
            }
        }
    }
}
