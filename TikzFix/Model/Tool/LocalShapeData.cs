using System.Collections.Generic;

namespace TikzFix.Model.Tool
{
    class LocalShapeData
    {
        public LocalShapeData(string toolName, List<CanvasEventArgs> keyPoints)
        {
            ToolName = toolName;
            KeyPoints = keyPoints;
        }

        public string ToolName
        {
            get; set;
        }

        public List<CanvasEventArgs> KeyPoints
        {
            get; set;
        }

        public override string ToString()
        {
            return $"{ToolName}. {string.Join(", ", KeyPoints)}";
        }
    }
}
