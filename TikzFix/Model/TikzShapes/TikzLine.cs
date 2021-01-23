using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

using TikzFix.Model.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    internal class TikzLine : TikzShape
    {
        public TikzLine(ArrowLine line, TikzStyle tikzStyle) : base(line, tikzStyle)
        {

        }

        private ArrowLine line;
        public override Shape Shape
        {
            get => line;
            set
            {
                if (value is not ArrowLine l)
                {
                    throw new ArgumentException("TikzLine Shape has to be Line");
                }
                line = l;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {
            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
                {
                    new CanvasEventArgs(new Point((int)line.X1 + (int)Canvas.GetLeft(line), (int)line.Y1 + (int)Canvas.GetTop(line)), MouseState.DOWN),
                    new CanvasEventArgs(new Point((int)line.X2 + (int)Canvas.GetLeft(line), (int)line.Y2 + (int)Canvas.GetTop(line)), MouseState.UP)
                };

            return new LocalShapeData(ITool.LINE_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            return $"\\draw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, {TikzStyle.LineWidth.GetLineWidthTikz()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineEnding.GetLineEndingTikz()}, {TikzStyle.LineType.GetLineTypeTikz()}] ({line.X1 + (int)Canvas.GetLeft(line)}pt,{line.Y1 + (int)Canvas.GetTop(line)}pt)--({line.X2 + (int)Canvas.GetLeft(line)}pt,{line.Y2 + (int)Canvas.GetTop(line)}pt);";
        }
    }
}
