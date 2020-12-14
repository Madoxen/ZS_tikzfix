using System.Windows.Shapes;

namespace TikzFix.Model.Tool
{
    class DrawingShape
    {
        public DrawingShape(Shape shape, ShapeState shapeState)
        {
            Shape = shape;
            ShapeState = shapeState;
        }

        public Shape Shape
        {
            get; set;
        }

        public ShapeState ShapeState
        {
            get; set;
        }
    }
}
