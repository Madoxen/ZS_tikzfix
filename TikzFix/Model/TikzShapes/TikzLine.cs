using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.TikzShapes
{
    class TikzLine : TikzShape
    {
        public TikzLine(Line line, TikzStyle tikzStyle) : base(line, tikzStyle)
        {

        }

        private Line line;
        public override Shape Shape
        {
            get => line;
            set
            {
                if (value is not Line l)
                {
                    throw new ArgumentException("TikzLine Shape has to be Line");
                }
                line = l;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {
            if (Shape is not Line l)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Line");
            }

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
                {
                    new CanvasEventArgs((int)l.X1, (int)l.Y1, MouseState.DOWN),
                    new CanvasEventArgs((int)l.X2, (int)l.Y2, MouseState.UP)
                };

            return new LocalShapeData("LineTool", keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            return $"\\draw[{TikzStyle.StrokeColor.GetLaTeXColorName()}, {TikzStyle.LineWidth.GetLineWidthTikz()}, {TikzStyle.LineEnding.GetLineEndingTikz()}, {TikzStyle.LineType.GetLineTypeTikz()}] ({line.X1},{line.Y1})--({line.X2},{line.Y2});";
        }
    }
}
