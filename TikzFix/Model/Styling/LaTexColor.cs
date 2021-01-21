using System;
using System.Windows.Media;

namespace TikzFix.Model.Styling
{
    public static class LaTexColorExt
    {
        public static string GetLaTeXColorString(this Color c)
        {
            if (c.A == 0)
            {
                return "white!0";
            }

            byte maxDim = Math.Max(c.R, Math.Max(c.B, c.G));
            if (maxDim == 0)
            {
                return "black";
            }

            return "{ rgb: red," + c.R / (double)maxDim + "; green," + c.G / (double)maxDim + "; blue," + c.B / (double)maxDim + "}";
        }
    }
}
