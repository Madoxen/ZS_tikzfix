using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TikzFix.Model.Styling
{
    public enum LineType
    {
        SOLID,
        DOTTED,
        DASHED,
        DASHDOTTED
    }

    public static class LineTypeExt
    {
        public static string GetLineTypeTikz(this LineType lineType)
        {
            return lineType switch
            {
                LineType.SOLID => "solid",
                LineType.DOTTED => "dotted",
                LineType.DASHDOTTED => "dashdotted",
                LineType.DASHED=> "dashed",
                _ => throw new ArgumentException("LineType cannot be converted"),
            };
        }
    }
}
