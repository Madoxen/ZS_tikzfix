using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Shapes;

using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.Model.FormatLoader
{
    /// <summary>
    /// Reads local shape data from JSON string
    /// and then uses available tools to reconstruct scene
    /// </summary>
    internal class JsonFormatLoader : IFormatLoader
    {
        private readonly Dictionary<string, ITool> toolNameToolMap;

        public JsonFormatLoader()
        {
            toolNameToolMap = new Dictionary<string, ITool>()
            {
                ["LineTool"] = new LineTool(),
                ["RectangleTool"] = new RectangleTool(),
                ["EllipseTool"] = new EllipseTool(),
            };
        }


        public ICollection<Shape> ConvertMany(string data)
        {
            List<LocalShapeData> canvasShapesData = JsonSerializer.Deserialize<List<LocalShapeData>>(data);
            List<Shape> shapes = new List<Shape>(canvasShapesData.Count);
            ITool currentTool;

            foreach (LocalShapeData shapeData in canvasShapesData)
            {
                currentTool = toolNameToolMap[shapeData.ToolName];

                foreach (CanvasEventArgs canvasEventArgs in shapeData.KeyPoints)
                {
                    DrawingShape r = currentTool.GetShape(canvasEventArgs);

                    if (r.ShapeState == ShapeState.FINISHED)
                    {
                        shapes.Add(r.Shape);
                    }
                }
            }
            return shapes;
        }
    }
}
