using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    class BezierTool : ITool
    {
        private SByte click = 0;

        private Point firstPoint;
        private DrawingShape current;

        private bool secondPointSelected;
        private bool thirdPointSelected;

        BezierSegment bezier;
        PathFigure figure;
        Path path;
        PathFigure[] pf;

        public DrawingShape GetShape(CanvasEventArgs a, TikzStyle style)
        {
            Debug.WriteLine(a.ToString());

            if (click == 0) // first click, draw a straight line
            {

                if (a.MouseState == MouseState.DOWN)
                {
                    firstPoint = a.Point;

                    bezier = new BezierSegment()
                    {
                        Point3 = a.Point.GetSystemPoint()
                    };
                    figure = new PathFigure();
                    figure.Segments.Add(bezier);
                    path = new Path();
                    path.ApplyStyle(style);
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
                    bezier.Point3 = GetPointWithMargin(firstPoint, a.Point);
                }
                else
                {
                    bezier.Point3 = GetPointWithMargin(firstPoint, a.Point);
                    click++;
                }

                return current;
            }
            else if (click == 1) // second click, select first control point
            {
                if (a.MouseState == MouseState.DOWN)
                {
                    secondPointSelected = true;
                    bezier.Point1 = GetPointWithMargin(firstPoint, a.Point);

                }
                else if (a.MouseState == MouseState.MOVE)
                {
                    if (secondPointSelected)
                    {
                        bezier.Point1 = GetPointWithMargin(firstPoint, a.Point);
                    }
                }
                else
                {
                    bezier.Point1 = GetPointWithMargin(firstPoint, a.Point);
                    click++;
                }

                return current;
            }
            else if (click == 2) // third click, select second control point
            {
                if (a.MouseState == MouseState.DOWN)
                {
                    thirdPointSelected = true;
                    bezier.Point2 = GetPointWithMargin(firstPoint, a.Point);

                }
                else if (a.MouseState == MouseState.MOVE)
                {
                    if (thirdPointSelected)
                    {
                        bezier.Point2 = GetPointWithMargin(firstPoint, a.Point);
                    }
                }
                else
                {
                    current.ShapeState = ShapeState.FINISHED;
                    click = 0;
                    secondPointSelected = thirdPointSelected = false;
                }
                return current;
            }
            else
            {
                throw new Exception("Sth went wrong, bezier line should be drawn in 3 clicks");
            }
        }


        private System.Windows.Point GetPointWithMargin(Point first, Point second)
        {
            return new System.Windows.Point(second.X - first.X, second.Y - first.Y);
        }


    }
}
