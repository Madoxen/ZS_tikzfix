using System.Windows.Shapes;

namespace TikzFix.Model.Tool
{
    interface ITool
    {
        DrawingShape GetShape(CanvasEventArgs canvasEventArgs);
    }
}
