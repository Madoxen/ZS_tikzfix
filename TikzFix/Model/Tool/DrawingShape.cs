using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.Tool
{
    internal class DrawingShape
    {
        public DrawingShape(TikzShape shape, ShapeState shapeState)
        {
            TikzShape = shape;
            ShapeState = shapeState;
        }

        public bool RemoveOnFinish
        {
            get; set;
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
