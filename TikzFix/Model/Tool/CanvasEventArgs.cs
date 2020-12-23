namespace TikzFix.Model.Tool
{
    public class CanvasEventArgs
    {
        public CanvasEventArgs(Point point, MouseState mouseState)
        {
            Point = point;
            MouseState = mouseState;
        }

        public Point Point
        {
            get;
        }

        public MouseState MouseState
        {
            get;
        }

        public override string ToString()
        {
            return $"Point {Point}. {MouseState}";
        }
    }
}
