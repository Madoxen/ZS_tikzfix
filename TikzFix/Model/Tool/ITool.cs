namespace TikzFix.Model.Tool
{
    interface ITool
    {
        DrawingShape GetShape(CanvasEventArgs canvasEventArgs);
    }
}
