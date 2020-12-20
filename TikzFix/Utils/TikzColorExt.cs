using System.Windows.Media;

namespace TikzFix.Utils
{
    public static class ColorExt
    {
        public const string COLOR_DEF_FORMAT = "C_{0}_{1}_{2}";

        public static string DefineColorTikz(this Color color)
        {
            return string.Format(COLOR_DEF_FORMAT, color.R, color.G, color.B);
        }
    }
}
