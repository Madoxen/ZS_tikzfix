﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.Model.TikzShapes
{
    internal class TikzBezier : TikzShape
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

            Point firstPoint = new Point((int)Canvas.GetLeft(path) + (int)path.Margin.Left, (int)Canvas.GetTop(path) + (int)path.Margin.Top);


            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
                {
                    // prob it can be done with 3 points instead of 6 but I got strange bugs
                    new CanvasEventArgs(firstPoint, MouseState.DOWN),
                    new CanvasEventArgs(PointExtensions.CreateFromPoint(BezierTool.GetPointWithMargin(firstPoint, PointExtensions.CreateFromPoint(b.Point3))), MouseState.UP),
                    new CanvasEventArgs(PointExtensions.CreateFromPoint(BezierTool.GetPointWithMargin(firstPoint, PointExtensions.CreateFromPoint(b.Point1))), MouseState.DOWN),
                    new CanvasEventArgs(PointExtensions.CreateFromPoint(BezierTool.GetPointWithMargin(firstPoint, PointExtensions.CreateFromPoint(b.Point1))), MouseState.UP),
                    new CanvasEventArgs(PointExtensions.CreateFromPoint(BezierTool.GetPointWithMargin(firstPoint, PointExtensions.CreateFromPoint(b.Point2))), MouseState.DOWN),
                    new CanvasEventArgs(PointExtensions.CreateFromPoint(BezierTool.GetPointWithMargin(firstPoint, PointExtensions.CreateFromPoint(b.Point2))), MouseState.UP),
                };
            return new LocalShapeData(ITool.BEZIER_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            BezierSegment b = (path.Data as PathGeometry).Figures[0].Segments[0] as BezierSegment;

            return $"\\filldraw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, {TikzStyle.LineEnding.GetLineEndingTikz()}, fill={TikzStyle.FillColor.GetLaTeXColorString()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineWidth.GetLineWidthTikz()},{TikzStyle.LineType.GetLineTypeTikz()}] ({Canvas.GetLeft(path) + path.Margin.Left}pt,{Canvas.GetTop(path) + path.Margin.Top}pt) .. controls ({b.Point1.X + Canvas.GetLeft(path) + path.Margin.Left}pt,{b.Point1.Y + Canvas.GetTop(path) + path.Margin.Top}pt) and ({b.Point2.X + Canvas.GetLeft(path) + path.Margin.Left}pt,{b.Point2.Y + Canvas.GetTop(path) + path.Margin.Top}pt) .. ({b.Point3.X + Canvas.GetLeft(path) + path.Margin.Left}pt,{b.Point3.Y + Canvas.GetTop(path) + path.Margin.Top}pt);";
        }
    }
}
