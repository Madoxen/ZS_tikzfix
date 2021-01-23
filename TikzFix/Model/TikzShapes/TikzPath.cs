using System;
using System.Windows.Controls;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.TikzShapes
{
    internal class TikzPath : TikzShape
    {
        public TikzPath(Path path, TikzStyle style, string rawData) : base(path, style)
        {
            this.path = path;
            this.rawData = rawData;
            pathData = SVGParser.GetSVGProperty(rawData, "d");
            Canvas.SetLeft(this.path, 0);
            Canvas.SetTop(this.path, 0);
        }

        private readonly string pathData;
        private readonly string rawData;
        private Path path;
        public override Shape Shape
        {
            get => path;
            set
            {
                if (value is not Path p)
                {
                    throw new ArgumentException("TikzLine Shape has to be Path");
                }
                path = p;
            }
        }

        public string RawData => rawData;
        public string PathData => pathData;

        public override LocalShapeData GenerateLocalData()
        {
            throw new NotImplementedException();
        }

        public override string GenerateTikz()
        {
            return $"\\draw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, {TikzStyle.LineWidth.GetLineWidthTikz()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineEnding.GetLineEndingTikz()}, {TikzStyle.LineType.GetLineTypeTikz()}, shift={{({Canvas.GetLeft(path)}pt,{Canvas.GetTop(path)}pt)}}] svg \"{pathData}\";";
        }
    }
}
