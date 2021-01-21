using System.Collections.Generic;
using System.Text.Json;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using System.Linq;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            SaveData d = new SaveData();
            d.svgRawData = new List<SvgData>();
            d.localShapeData = new List<LocalShapeData>();
            foreach (TikzShape s in shapes)
            {
                if (s is TikzPath p)
                {
                    //Prepare path data
                    Point move = new Point((int)Canvas.GetLeft(p.Shape), (int)Canvas.GetTop(p.Shape));
                    //Modify move data
                    

                    d.svgRawData.Add(new SvgData() { data = p.RawData, translate = move});
                    continue;
                }
               d.localShapeData.Add(s.GenerateLocalData());
            }

            return JsonSerializer.Serialize(d);
        }
    }
}
