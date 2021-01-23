using System.Windows;

namespace TikzFix.Model.Tool
{
    public class CanvasEventArgs
    {
        public CanvasEventArgs(Point point, MouseState mouseState, bool modKey = false)
        {
            Point = point;
            MouseState = mouseState;
            ModKey = modKey;
        }

        public Point Point
        {
            get;
        }

        public MouseState MouseState
        {
            get;
        }

        public bool ModKey
        {
            get;
        }

        public override string ToString()
        {
            return $"Point {Point}. {MouseState}";
        }
    }
}
