using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    public class SaveData
    {
        public List<LocalShapeData> localShapeData { get; set; }
        public List<string> svgRawData { get; set; }

        public SaveData()
        {

        }
    }
}
