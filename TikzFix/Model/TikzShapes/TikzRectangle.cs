using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;
using TikzFix.Model.Styling;
using TikzFix.Model.Tool;

namespace TikzFix.Model.TikzShapes
{
    class TikzRectangle : TikzShape
    {
        public TikzRectangle(Rectangle rectangle, TikzStyle tikzStyle) : base(rectangle, tikzStyle)
        {

        }

        private Rectangle rectangle;
        public override Shape Shape
        {
            get => rectangle;
            set
            {
                if (value is not Rectangle r)
                {
                    throw new ArgumentException("TikzRectangle Shape has to be Rectangle");
                }
                rectangle = r;
            }
        }

        public override LocalShapeData GenerateLocalData()
        {

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs(new Point((int)Canvas.GetLeft(rectangle),(int)Canvas.GetTop(rectangle)), MouseState.DOWN),
                new CanvasEventArgs(new Point((int)(Canvas.GetLeft(rectangle)+rectangle.Width), (int)(Canvas.GetTop(rectangle)+rectangle.Height)), MouseState.UP)
            };

            return new LocalShapeData(ITool.RECTANGLE_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            return $"\\filldraw[color={TikzStyle.StrokeColor.GetLaTeXColorString()}, fill={TikzStyle.FillColor.GetLaTeXColorString()}, fill opacity={TikzStyle.FillColor.A / 255.0}, draw opacity={TikzStyle.StrokeColor.A / 255.0}, {TikzStyle.LineWidth.GetLineWidthTikz()},{TikzStyle.LineType.GetLineTypeTikz()}] ({(int)Canvas.GetLeft(rectangle)}pt,{(int)Canvas.GetTop(rectangle)}pt) rectangle ({(int)(rectangle.Width + Canvas.GetLeft(rectangle))}pt,{(int)(rectangle.Height + Canvas.GetTop(rectangle))}pt);";
        }
    }
}
