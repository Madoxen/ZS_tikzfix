using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using TikzFix.Model.Styling;

namespace TikzFix.Model.TikzShapes
{
    public abstract class TikzShape
    {
        public Shape Shape
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
