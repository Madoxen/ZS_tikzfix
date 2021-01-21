using System;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    public abstract class TikzShape
    {
        public abstract LocalShapeData GenerateLocalData();
        public abstract string GenerateTikz();

        public Guid Id = Guid.NewGuid();

        public abstract Shape Shape
        {
            get; set;
        }

        public TikzStyle TikzStyle
        {
            get; set;
        }

        protected TikzShape(Shape shape, TikzStyle tikzStyle)
        {
            Shape = shape;
            TikzStyle = tikzStyle;
        }
    }
}
