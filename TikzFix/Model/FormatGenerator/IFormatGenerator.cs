using System.Collections.Generic;
using System.Windows.Shapes;

namespace TikzFix.Model.FormatGenerator
{
    internal interface IFormatGenerator
    {
        string ConvertMany(ICollection<Shape> shapes);
    }
}
