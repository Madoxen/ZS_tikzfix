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
                    bezier.Point3 = new System.Windows.Point(a.Point.X - firstPoint.X, a.Point.Y - firstPoint.Y);
                }
                else
                {
                    bezier.Point3 = new System.Windows.Point(a.Point.X - firstPoint.X, a.Point.Y - firstPoint.Y);
                    click++;
                }

                return current;
            }
            else if (click == 1) // second click, select first control point
            {
                return current;
            }
            else if (click == 2) // third click, select second control point
            {
                return current;
            }
            else
            {
                throw new Exception("Sth went wrong, bezier line should be drawn in 3 clicks");
            }
        }


    }
}
