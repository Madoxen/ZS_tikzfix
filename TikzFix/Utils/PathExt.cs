using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;

namespace TikzFix.Utils
{
    public static class PathExt
    {
        public  static void ApplyStyle(this Path path, TikzStyle style)
        {
            path.Fill = new SolidColorBrush(style.FillColor.GetColor());
            path.StrokeThickness = style.LineWidth.GetLineWidth();
            path.Stroke = new SolidColorBrush(style.StrokeColor.GetColor());
        }
    }
}
