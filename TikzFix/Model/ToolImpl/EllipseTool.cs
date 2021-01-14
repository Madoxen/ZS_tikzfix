﻿using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class EllipseTool : ITool
    {
        private Point firstPoint;
        private DrawingShape current;

        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                Ellipse ellipse = new Ellipse
                {
                    StrokeThickness = style.LineWidth.GetLineWidth(),
                };

                ellipse.SetStyle(style);
                current = new DrawingShape(new TikzEllipse(ellipse, style), ShapeState.START);
                firstPoint = canvasEventArgs.Point;
            }
            else
            {
                current.TikzShape.Shape.Width = Math.Abs(firstPoint.X - canvasEventArgs.Point.X) * 2;
                current.TikzShape.Shape.Height = Math.Abs(firstPoint.Y - canvasEventArgs.Point.Y) * 2;

                current.TikzShape.Shape.Margin = ShapeUtils.GetMarginEqual(firstPoint, canvasEventArgs.Point);
                Canvas.SetLeft(current.TikzShape.Shape, 0);
                Canvas.SetTop(current.TikzShape.Shape, 0);

                if (canvasEventArgs.MouseState == MouseState.UP)
                {
                    current.ShapeState = ShapeState.FINISHED;
                }
                else if (canvasEventArgs.MouseState == MouseState.MOVE)
                {
                    current.ShapeState = ShapeState.DRAWING;
                }
            }
            return current;
        }

        public void Reset()
        {
            firstPoint = null;
            current = null;
        }
    }
}