﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.Model.TikzShapes
{
    class TikzBezier : TikzShape
    {
        public TikzBezier(ArrowPath path, TikzStyle tikzStyle) : base(path, tikzStyle)
        {

        }

        private ArrowPath path;
        public override Shape Shape
        {
            get => path;
            set
            {
                if (value is not ArrowPath p)
                {
                    throw new ArgumentException("TikzBezier Shape has to be Path");
                }
                path = p;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {
            BezierSegment b = (path.Data as PathGeometry).Figures[0].Segments[0] as BezierSegment;

            Point firstPoint = new Point((int)path.Margin.Left, (int)path.Margin.Top);

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
                {
                    // prob it can be done with 3 points instead of 6 but I got strange bugs
                    new CanvasEventArgs(firstPoint, MouseState.DOWN),
                    new CanvasEventArgs(new Point(BezierTool.GetPointWithMargin(firstPoint, new Point(b.Point3))), MouseState.UP),
                    new CanvasEventArgs(new Point(BezierTool.GetPointWithMargin(firstPoint, new Point(b.Point1))), MouseState.DOWN),
                    new CanvasEventArgs(new Point(BezierTool.GetPointWithMargin(firstPoint, new Point(b.Point1))), MouseState.UP),
                    new CanvasEventArgs(new Point(BezierTool.GetPointWithMargin(firstPoint, new Point(b.Point2))), MouseState.DOWN),
                    new CanvasEventArgs(new Point(BezierTool.GetPointWithMargin(firstPoint, new Point(b.Point2))), MouseState.UP),
                };

            return new LocalShapeData(ITool.BEZIER_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            BezierSegment b = (path.Data as PathGeometry).Figures[0].Segments[0] as BezierSegment;

            return $"\\filldraw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, {TikzStyle.LineEnding.GetLineEndingTikz()} ,fill={TikzStyle.FillColor.GetLaTeXColorString()}, fill opacity={(double)TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineWidth.GetLineWidthTikz()},{TikzStyle.LineType.GetLineTypeTikz()}] ({path.Margin.Left},{path.Margin.Top}) .. controls ({b.Point1.X + path.Margin.Left},{b.Point1.Y + path.Margin.Top}) and ({b.Point2.X + path.Margin.Left},{b.Point2.Y + path.Margin.Top}) .. ({b.Point3.X + path.Margin.Left},{b.Point3.Y + path.Margin.Top});";
        }
    }
}
