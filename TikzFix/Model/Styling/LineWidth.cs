using System;

namespace TikzFix.Model.Styling
{
    public enum LineWidth
    {
        VERY_THIN = 1,
        THIN = 2,
        THICK = 3,
        ULTRA_THICK = 4
    }

    public static class LineWidthExtension
    {
        public static double GetLineWidth(this LineWidth lineWidth)
        {
            return lineWidth switch
            {
                LineWidth.VERY_THIN => 1.0,
                LineWidth.THIN => 2.0,
                LineWidth.THICK => 3.0,
                LineWidth.ULTRA_THICK => 4.0,
                _ => throw new ArgumentException("LineWidth cannot be converted"),
            };
        }

        public static string GetLineWidthTikz(this LineWidth lineWidth)
        {
            return Enum.GetName(lineWidth).ToLower().Replace("_", " ");
        }

    }
}
