using System.Collections.Generic;
using System.Windows.Shapes;

namespace TikzFix.Model.FormatLoader
{
    internal interface IFormatLoader
    {
        ICollection<Shape> ConvertMany(string data);
    }
}
