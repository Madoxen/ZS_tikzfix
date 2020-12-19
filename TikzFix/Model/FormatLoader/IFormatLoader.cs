using System.Collections.Generic;
using System.Windows.Shapes;
using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatLoader
{
    internal interface IFormatLoader
    {
        ICollection<TikzShape> ConvertMany(string data);
    }
}
