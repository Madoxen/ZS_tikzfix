using TikzFix.Model.Styling;

namespace TikzFix.Model.Tool
{
    interface ITool
    {
        public const string LINE_TOOL_NAME = "LineTool";
        public const string RECTANGLE_TOOL_NAME = "RectangleTool";
        public const string ELLIPSE_TOOL_NAME = "EllipseTool";

        DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style);
    }
}
