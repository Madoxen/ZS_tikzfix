namespace TikzFix.Model.Tool
{
    public class Point
    {
        public int X
        {
            get; set;
        }

        public int Y
        {
            get; set;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(System.Windows.Point p)
        {
            X = (int)p.X;
            Y = (int)p.Y;
        }

        public Point()
        {
            // empty ctor needed in deserialization
        }
    }
}
