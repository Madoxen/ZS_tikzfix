namespace TikzFix.Model.Tool
{
    class CanvasEvent
    {
        public CanvasEvent(int x, int y, MouseState mouseState)
        {
            X = x;
            Y = y;
            MouseState = mouseState;
        }

        public int X
        {
            get;
        }

        public int Y
        {
            get;
        }

        public MouseState MouseState
        {
            get;
        }
    }
}
