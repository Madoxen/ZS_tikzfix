using System.Windows.Shapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.ToolImpl
{
    class LineTool : ITool
    {
        private const int DEF_STROKE_THICKNESS = 2;

        private int x1, y1;
        private bool firstClick = true;

        public DrawingShape GetShape(CanvasEventArgs canvasEvent)
        {
            if (canvasEvent.MouseState == MouseState.DOWN)
            {
                if (firstClick)
                {
                    x1 = canvasEvent.X;
                    y1 = canvasEvent.Y;
                    firstClick = false;

                    // return EMPTY_SHAPE when DOWN | MOVE
                    // if need it can be changed to draw first point
                    // to show user where was clikced before selecting second point
                    return DrawingShape.EMPTY_SHAPE;
                }
                else
                {
                    firstClick = true;
                    return new DrawingShape(
                        new Line
                        {
                            Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                            X1 = x1,
                            X2 = canvasEvent.X,
                            Y1 = y1,
                            Y2 = canvasEvent.Y,
                            StrokeThickness = DEF_STROKE_THICKNESS
                        },
                        ShapeState.FINISHED
                    );
                }
            }
            else if (canvasEvent.MouseState == MouseState.UP)
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
