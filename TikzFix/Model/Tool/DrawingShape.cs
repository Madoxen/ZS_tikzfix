using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.Tool
{
    class DrawingShape
    {
        public DrawingShape(TikzShape shape, ShapeState shapeState)
        {
            TikzShape = shape;
            ShapeState = shapeState;
        }

        public TikzShape TikzShape
        {
            get; set;
        }

        public ShapeState ShapeState
        {
            get; set;
        }
    }
}
