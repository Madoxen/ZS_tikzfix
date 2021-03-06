﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TikzFix.Model.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class BezierTool : ITool
    {
        private sbyte click = 0;

        private Point firstPoint;
        private DrawingShape current;

        private bool secondPointSelected;
        private bool thirdPointSelected;

        private BezierSegment bezier;
        private PathFigure figure;
        private ArrowPath path;
        private PathFigure[] pf;

        public DrawingShape GetShape(CanvasEventArgs a, TikzStyle style)
        {
            if (click == 0) // first click, draw a straight line
            {
                if (a.MouseState == MouseState.DOWN)
                {
                    //FIXME: small bug here, sometimes when a user clicks for the first time long line appears for a split second, noticeable but should not influence performance

                    firstPoint = a.Point;

                    bezier = new BezierSegment()
                    {
                        Point3 = a.Point
                    };
                    figure = new PathFigure();
                    figure.Segments.Add(bezier);
                    path = new ArrowPath();

                    path.SetStyle(style);
                    path.Margin = ShapeUtils.GetMargin(firstPoint.X, firstPoint.Y);

                    pf = new PathFigure[] { figure };
                    path.Data = new PathGeometry(pf);

                    current = new DrawingShape(
                        new TikzBezier(path, style),
                        ShapeState.START
                    );



                }
                else if (a.MouseState == MouseState.MOVE)
                {
                    bezier.Point3 = GetPointWithoutMargin(firstPoint, a.Point);
                }
                else
                {
                    bezier.Point3 = GetPointWithoutMargin(firstPoint, a.Point);
                    click++;
                }

                return current;
            }
            else if (click == 1) // second click, select first control point
            {
                if (a.MouseState == MouseState.DOWN)
                {
                    secondPointSelected = true;
                    bezier.Point1 = GetPointWithoutMargin(firstPoint, a.Point);

                }
                else if (a.MouseState == MouseState.MOVE)
                {
                    if (secondPointSelected)
                    {
                        bezier.Point1 = GetPointWithoutMargin(firstPoint, a.Point);
                    }
                }
                else
                {
                    bezier.Point1 = GetPointWithoutMargin(firstPoint, a.Point);
                    click++;

                }

                return current;
            }
            else if (click == 2) // third click, select second control point
            {
                if (a.MouseState == MouseState.DOWN)
                {
                    thirdPointSelected = true;
                    bezier.Point2 = GetPointWithoutMargin(firstPoint, a.Point);

                }
                else if (a.MouseState == MouseState.MOVE)
                {
                    if (thirdPointSelected)
                    {
                        bezier.Point2 = GetPointWithoutMargin(firstPoint, a.Point);
                    }
                }
                else
                {
                    bezier.Point2 = GetPointWithoutMargin(firstPoint, a.Point);
                    current.ShapeState = ShapeState.FINISHED;
                    click = 0;
                    secondPointSelected = thirdPointSelected = false;
                    Canvas.SetLeft(current.TikzShape.Shape, 0);
                    Canvas.SetTop(current.TikzShape.Shape, 0);
                }


                return current;
            }
            else
            {
                throw new Exception("Sth went wrong, bezier line should be drawn in 3 clicks");
            }
        }


        public static Point GetPointWithoutMargin(Point first, Point second)
        {
            return new Point(second.X - first.X, second.Y - first.Y);
        }

        public static Point GetPointWithMargin(Point first, Point second)
        {
            return new Point(second.X + first.X, second.Y + first.Y);
        }

        public void Reset()
        {
            click = 0;

            current = null;

            secondPointSelected = false;
            thirdPointSelected = false;

            bezier = null;
            figure = null;
            path = null;
            pf = null;
        }
    }
}

