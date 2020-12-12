using System.Windows.Shapes;

namespace TikzFix.Model.Tool
{
    class DrawingShape
    {
        public static DrawingShape EMPTY_SHAPE = new DrawingShape(null, ShapeState.EMPTY);

        public DrawingShape(Shape shape, ShapeState shapeState)
        {
            Shape = shape;
            ShapeState = shapeState;
        }

        public Shape Shape
        {
            get;
        }

        public ShapeState ShapeState
        {
            get;
        }
    }
}
