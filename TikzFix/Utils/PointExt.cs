using TikzFix.Model.Tool;

namespace TikzFix.Utils
{
    internal static class PointExt
    {
        public static System.Windows.Point GetSystemPoint(this Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
    }
}
