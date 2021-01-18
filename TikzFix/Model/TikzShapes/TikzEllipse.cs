using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    class TikzEllipse : TikzShape
    {
        public TikzEllipse(Ellipse ellipse, TikzStyle tikzStyle) : base(ellipse, tikzStyle)
        {

        }

        private Ellipse ellipse;
        public override Shape Shape
        {
            get => ellipse;
            set
            {
                if (value is not Ellipse e)
                {
                    throw new ArgumentException("TikzEllipse Shape has to be Ellipse");
                }
                ellipse = e;
            }
        }
        public override LocalShapeData GenerateLocalData()
        {
            int X1 = (int)(Canvas.GetLeft(ellipse) + ellipse.Width / 2);
            int Y1 = (int)(Canvas.GetTop(ellipse) + ellipse.Height / 2);

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
                {
                    new CanvasEventArgs(new Point(X1, Y1), MouseState.DOWN),
                    new CanvasEventArgs(new Point(X1 + (int)(ellipse.Width / 2), (int)(Y1 + ellipse.Height / 2)), MouseState.UP)
                };

            return new LocalShapeData("EllipseTool", keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            return $"\\filldraw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, fill={TikzStyle.FillColor.GetLaTeXColorString()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineWidth.GetLineWidthTikz()},{TikzStyle.LineType.GetLineTypeTikz()}] ({(int)(Canvas.GetLeft(ellipse) + ellipse.Width / 2)}pt,{(int)(Canvas.GetTop(ellipse) + ellipse.Height / 2)}pt) ellipse ({(int)(ellipse.Width / 2)}pt and {(int)(ellipse.Height / 2)}pt);";
        }
    }
}
