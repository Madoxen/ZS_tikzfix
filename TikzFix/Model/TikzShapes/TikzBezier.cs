using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    class TikzBezier : TikzShape
    {
        public TikzBezier(Path path, TikzStyle tikzStyle) : base(path, tikzStyle)
        {

        }

        private Path path;
        public override Shape Shape
        {
            get => path;
            set
            {
                if (value is not Path p)
                {
                    throw new ArgumentException("TikzBezier Shape has to be Path");
                }
                path = p;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {
            throw new NotImplementedException();
            //List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            //    {
            //        new CanvasEventArgs((int)line.X1, (int)line.Y1, MouseState.DOWN),
            //        new CanvasEventArgs((int)line.X2, (int)line.Y2, MouseState.UP)
            //    };

            //return new LocalShapeData(ITool.LINE_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            throw new NotImplementedException();
            //return $"\\draw[{TikzStyle.StrokeColor.GetLaTeXColorName()}, {TikzStyle.LineWidth.GetLineWidthTikz()}, {TikzStyle.LineEnding.GetLineEndingTikz()}, {TikzStyle.LineType.GetLineTypeTikz()}] ({line.X1},{line.Y1})--({line.X2},{line.Y2});";
        }
    }
}
