using System.Windows.Shapes;
using TikzFix.Model.Styling;

namespace TikzFix.Model.Tool
{
    interface ITool
    {
        DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style);
    }
}
