using System.Windows.Media;

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

        public SolidColorBrush StrokeColor
        {
            get; set;
        }

        public SolidColorBrush FillColor
        {
            get; set;
        }

        public TikzStyle(
            SolidColorBrush strokeColor,
            SolidColorBrush fillColor,
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
    }
}
