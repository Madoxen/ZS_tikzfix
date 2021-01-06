using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

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
