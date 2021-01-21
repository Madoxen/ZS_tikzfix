using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Controls;

using TikzFix.Model.FormatGenerator;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;
using TikzFix.Utils;

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
                [ITool.BEZIER_TOOL_NAME] = new BezierTool(),
            };
        }


        public ICollection<TikzShape> ConvertMany(string data)
        {
            SaveData d = JsonSerializer.Deserialize<SaveData>(data);
            List<TikzShape> result = new List<TikzShape>();

            ITool currentTool;

            foreach (LocalShapeData shapeData in d.localShapeData)
            {
                currentTool = toolNameToolMap[shapeData.ToolName];

                foreach (CanvasEventArgs canvasEventArgs in shapeData.KeyPoints)
                {
                    DrawingShape r = currentTool.GetShape(canvasEventArgs, shapeData.Style);

                    if (r.ShapeState == ShapeState.FINISHED)
                    {
                        result.Add(r.TikzShape);
                    }
                }
            }

            foreach (SvgData raw in d.svgRawData)
            {
                List<TikzShape> c = SVGParser.Parse(raw.data);
                foreach (TikzShape s in c)
                {
                    Canvas.SetLeft(s.Shape, raw.translate.X);
                    Canvas.SetTop(s.Shape, raw.translate.Y);
                }

                result.AddRange(c);
            }

            return result;
        }
    }
}
