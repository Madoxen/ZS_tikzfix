using System.Collections.Generic;
using System.Text.Json;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            List<LocalShapeData> localShapesData = new List<LocalShapeData>(shapes.Count);
            foreach (TikzShape s in shapes)
            {
                localShapesData.Add(s.GenerateLocalData());
            }

            return JsonSerializer.Serialize(localShapesData);
        }
    }
}
