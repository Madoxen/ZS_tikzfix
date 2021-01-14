using System;
using System.ComponentModel;
using System.Windows.Media;

namespace TikzFix.Model.Styling
{
    public enum LineType
    {
        SOLID,
        DOTTED,
        DASHED,
        [Description("Dash dotted")]
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
                LineType.DASHED => "dashed",
                _ => throw new ArgumentException("LineType cannot be converted"),
            };
        }

        public static DoubleCollection GetDashArray(this LineType lineType)
        {

            /* From: https://docs.microsoft.com/en-us/dotnet/api/system.windows.shapes.shape.strokedasharray?view=net-5.0#System_Windows_Shapes_Shape_StrokeDashArray
             Each Double in the collection specifies the length of a dash or gap relative to the Thickness of the pen. For example, a value of 1 creates a dash or gap that has the same length as the thickness of the pen (a square).
                The first item in the collection, which is located at index 0, specifies the length of a dash; the second item, which is located at index 1, specifies the length of a gap.
                Objects with an even index value specify dashes; objects with an odd index value specify gaps.
             */
            return lineType switch
            {
                LineType.SOLID => null,
                LineType.DOTTED => new DoubleCollection(new double[] { 1.0, 1.0 }),
                LineType.DASHDOTTED => new DoubleCollection(new double[] { 2.0, 0.5, 0.5, 0.5 }),
                LineType.DASHED => new DoubleCollection(new double[] { 2.0, 0.5 }),
                _ => throw new ArgumentException("LineType cannot be converted"),
            };
        }
    }
}
