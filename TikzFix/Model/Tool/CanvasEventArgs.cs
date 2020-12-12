namespace TikzFix.Model.Tool
{
    class CanvasEventArgs
    {
        public CanvasEventArgs(int x, int y, MouseState mouseState)
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
