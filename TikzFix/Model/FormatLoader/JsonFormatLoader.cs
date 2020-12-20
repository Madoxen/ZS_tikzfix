using System.Collections.Generic;
using System.Text.Json;
using TikzFix.Model.TikzShapes;
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
                [ITool.LINE_TOOL_NAME] = new LineTool(),
                [ITool.RECTANGLE_TOOL_NAME] = new RectangleTool(),
                [ITool.ELLIPSE_TOOL_NAME] = new EllipseTool(),
            };
        }


        public ICollection<TikzShape> ConvertMany(string data)
        {
            List<LocalShapeData> canvasShapesData = JsonSerializer.Deserialize<List<LocalShapeData>>(data);
            List<TikzShape> shapes = new List<TikzShape>();

            ITool currentTool;

            foreach (LocalShapeData shapeData in canvasShapesData)
            {
                currentTool = toolNameToolMap[shapeData.ToolName];

                foreach (CanvasEventArgs canvasEventArgs in shapeData.KeyPoints)
                {
                    DrawingShape r = currentTool.GetShape(canvasEventArgs, shapeData.Style);

                    if (r.ShapeState == ShapeState.FINISHED)
                    {
                        shapes.Add(r.TikzShape);
                    }
                }
            }
            return shapes;
        }
    }
}
