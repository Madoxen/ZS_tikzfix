using System.Collections.Generic;

using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatGenerator
{
    internal interface IFormatGenerator
    {
        string ConvertMany(ICollection<TikzShape> shapes);
    }
}
