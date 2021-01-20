using System.Collections.Generic;
using System.Text.Json;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using System.Linq;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            SaveData d = new SaveData();
            d.svgRawData = new List<string>();
            d.localShapeData = new List<LocalShapeData>();
            foreach (TikzShape s in shapes)
            {
                if (s is TikzPath p)
                {
                    d.svgRawData.Add(p.RawData);
                    continue;
                }
               d.localShapeData.Add(s.GenerateLocalData());
            }

            return JsonSerializer.Serialize(d);
        }
    }
}
