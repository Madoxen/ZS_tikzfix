namespace TikzFix.Model.Styling
{
    public class TikzStyle
    {
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

        public LaTexColor StrokeColor
        {
            get; set;
        }

        public LaTexColor FillColor
        {
            get; set;
        }

        public TikzStyle(
            LaTexColor strokeColor,
            LaTexColor fillColor,
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
