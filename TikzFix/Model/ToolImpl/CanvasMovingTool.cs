using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.ToolImpl
{
    internal class CanvasMovingTool : ITool
    {

        public virtual DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            return null;
        }

        public void Reset()
        {

        }
    }
}
