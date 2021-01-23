using System.Collections.Generic;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            SaveData d = new SaveData
            {
                svgRawData = new List<SvgData>(),
                localShapeData = new List<LocalShapeData>()
            };
            foreach (TikzShape s in shapes)
            {
                if (s is TikzPath p)
                {
                    //Prepare path data
                    Point move = new Point((int)Canvas.GetLeft(p.Shape), (int)Canvas.GetTop(p.Shape));
                    //Modify move data


                    d.svgRawData.Add(new SvgData() { data = p.RawData, translate = move });
                    continue;
                }
                d.localShapeData.Add(s.GenerateLocalData());
            }

            return JsonSerializer.Serialize(d);
        }
    }
}
