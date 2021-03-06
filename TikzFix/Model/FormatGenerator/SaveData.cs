﻿using System.Collections.Generic;
using System.Windows;

using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    public class SaveData
    {
        public List<LocalShapeData> localShapeData { get; set; }
        public List<SvgData> svgRawData { get; set; }

        public SaveData()
        {

        }
    }

    public class SvgData
    {
        public string data { get; set; }
        public TikzStyle style { get; set; }
        public Point translate { get; set; }
    }
}
