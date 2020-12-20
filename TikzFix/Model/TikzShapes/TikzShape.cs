using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    public abstract class TikzShape
    {
        public abstract LocalShapeData GenerateLocalData();
        public abstract string GenerateTikz();

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
