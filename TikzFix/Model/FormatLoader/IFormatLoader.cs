using System.Collections.Generic;

using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatLoader
{
    internal interface IFormatLoader
    {
        ICollection<TikzShape> ConvertMany(string data);
    }
}
