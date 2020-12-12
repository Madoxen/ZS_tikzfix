namespace TikzFix.Model.Tool
{
    interface ITool
    {
        DrawingShape GetShape(CanvasEvent canvasEvent);
    }
}
