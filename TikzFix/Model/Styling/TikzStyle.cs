using System.Windows.Media;

namespace TikzFix.Model.Styling
{
    public class TikzStyle
    {
        public static TikzStyle SelectionStyle = new TikzStyle(Colors.Aqua, Colors.Aqua, lineWidth: LineWidth.VERY_THIN);

        public LineEnding LineEnding
        {
            get; set;
        }

        public LineWidth LineWidth
        {
            get; set;
        }

        public LineType LineType
        {
            get; set;
        }

        public Color StrokeColor
        {
            get; set;
        }

        public Color FillColor
        {
            get; set;
        }


        public TikzStyle(
            Color strokeColor,
            Color fillColor,
            LineEnding lineEnding = LineEnding.NONE,
            LineWidth lineWidth = LineWidth.THIN,
            LineType lineType = LineType.SOLID
            )
        {
            LineEnding = lineEnding;
            LineWidth = lineWidth;
            LineType = lineType;
            StrokeColor = strokeColor;
            FillColor = fillColor;
        }

        public override string ToString()
        {
            return $"{LineEnding} {LineWidth} {LineType} {StrokeColor} {FillColor}";
        }


    }
}
