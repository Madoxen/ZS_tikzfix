using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TikzFix.Model.Styling
{
    public enum LaTexColor
    {
        TRANSPARENT,
        BLACK,
        WHITE,
        BLUE,
        GREEN,
        RED,
    }


    public static class LaTexColorExt
    {
        public static string GetLaTeXColorName(this LaTexColor laTexColor)
        {
            if (laTexColor == LaTexColor.TRANSPARENT)
            {
                return "white!0";
            }
            return Enum.GetName(laTexColor).ToLower();
        }

        public static Color GetColor(this LaTexColor laTexColor)
        {
            return laTexColor switch
            {
                LaTexColor.TRANSPARENT => Color.FromArgb(0, 0, 0, 0),
                LaTexColor.BLACK => Color.FromRgb(0, 0, 0),
                LaTexColor.WHITE => Color.FromRgb(255, 255, 255),
                LaTexColor.BLUE => Color.FromRgb(0, 0, 255),
                LaTexColor.GREEN => Color.FromRgb(0, 255, 0),
                LaTexColor.RED => Color.FromRgb(255, 0, 0),
                _ => throw new ArgumentException("LaTexColor cannot be converted"),
            };
        }
    }


}
