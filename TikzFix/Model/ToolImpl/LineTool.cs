using System;
using System.Windows.Controls;

using TikzFix.Model.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Utils;

namespace TikzFix.Model.ToolImpl
{
    internal class LineTool : ITool
    {
        private Point firstPoint;
        private DrawingShape current;


        public DrawingShape GetShape(CanvasEventArgs canvasEventArgs, TikzStyle style)
        {
            if (canvasEventArgs.MouseState == MouseState.DOWN)
            {
                firstPoint = canvasEventArgs.Point;
                ArrowLine line = new ArrowLine
                {
                    X1 = firstPoint.X,
                    X2 = firstPoint.X,
                    Y1 = firstPoint.Y,
                    Y2 = firstPoint.Y,
                };

                line.SetStyle(style);
                current = new DrawingShape(new TikzLine(line, style), ShapeState.START);
            }
            else
            {
                if (current.TikzShape.Shape is not ArrowLine l)
                {
                    throw new Exception("Shape-Tool type mismatch, tool type: LineTool, expected shape type Line");
                }

                l.X2 = canvasEventArgs.Point.X;
                l.Y2 = canvasEventArgs.Point.Y;

                if (canvasEventArgs.ModKey)
                {
                    //Snap mode, from start to one of axis contraints
                    //Axis constraints are divided by 45deg angles

                    //Get angle between start point and mouse pointer
                    //origin Axis X 
                    double angle = Math.Atan2(l.Y2 - l.Y1, l.X2 - l.X1); //rads

                    //Determine constraint axis based on angle between X axis and current line
                    //22.5 -> pi/8

                    //X AXIS -> 0
                    //45 -> PI/4
                    //90 -> PI/2
                    //135 -> PI/2 + PI/4 = 3PI/4
                    //180 -> PI
                    //180 -> -PI
                    //225 -> -3PI/4
                    //270 -> -PI/2
                    //360 -> 0 

                    double snapAngle = Math.Ceiling(angle / (Math.PI / 8.0)) * (Math.PI / 8.0);
                    //calculate new points based on snapAngle

                    double maxDim = Math.Max(Math.Abs(l.X2 - l.X1), Math.Abs(l.Y2 - l.Y1));
                    l.X2 = l.X1 + Math.Cos(snapAngle) * maxDim;
                    l.Y2 = l.Y1 + Math.Sin(snapAngle) * maxDim;
                }

                if (canvasEventArgs.MouseState == MouseState.UP)
                {
                    current.ShapeState = ShapeState.FINISHED;
                    Canvas.SetLeft(l, 0);
                    Canvas.SetTop(l, 0);
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
            current = null;
            firstPoint = null;
        }
    }
}

