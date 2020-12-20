using System;
using System.Collections.Generic;
using TikzFix.Model.Styling;

namespace TikzFix.Model.Tool
{
    public class LocalShapeData
    {
        public LocalShapeData(string toolName, List<CanvasEventArgs> keyPoints, TikzStyle style)
        {
            ToolName = toolName;
            KeyPoints = keyPoints;
            Style = style;
        }

        public string ToolName
        {
            get; set;
        }

        public List<CanvasEventArgs> KeyPoints
        {
            get; set;
        }

        public TikzStyle Style
        {
            get; set;
        }

        public override string ToString()
        {
            return $"{ToolName}. {string.Join(", ", KeyPoints)} Style {Style}";
        }
    }
}
