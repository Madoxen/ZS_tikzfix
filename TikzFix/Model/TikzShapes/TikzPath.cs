using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.TikzShapes
{
    class TikzPath : TikzShape
    {
        public TikzPath(Path path, TikzStyle style, string rawData) : base(path, style)
        {
            this.path = path;
            this.rawData = rawData;
            pathData = SVGParser.GetSVGProperty(rawData, "d");
        }

        private string pathData;
        private string rawData;
        private Path path;
        public override Shape Shape
        {
            get => path;
            set
            {
                if (value is not Path p)
                {
                    throw new ArgumentException("TikzLine Shape has to be Line");
                }
                path = p;
            }
        }

        public string RawData
        {
            get => rawData;
        }

        public override LocalShapeData GenerateLocalData()
        {
            throw new NotImplementedException();
        }

        public override string GenerateTikz()
        {
            return $"\\draw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, {TikzStyle.LineWidth.GetLineWidthTikz()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineEnding.GetLineEndingTikz()}, {TikzStyle.LineType.GetLineTypeTikz()}] svg \"{pathData}\";";
        }
    }
}
