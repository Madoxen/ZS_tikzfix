using System;
using System.Collections.Generic;
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
                new CanvasEventArgs((int)rectangle.Margin.Left,(int)rectangle.Margin.Top, MouseState.DOWN),
                new CanvasEventArgs((int)(rectangle.Margin.Left+rectangle.Width), (int)(rectangle.Margin.Top+rectangle.Height), MouseState.UP)
            };

            return new LocalShapeData(ITool.RECTANGLE_TOOL_NAME, keyPointList, TikzStyle);
        }

        public override string GenerateTikz()
        {
            return $"\\filldraw[color={TikzStyle.StrokeColor.GetLaTeXColorName()}, fill={TikzStyle.FillColor.GetLaTeXColorName()}, {TikzStyle.LineWidth.GetLineWidthTikz()},{TikzStyle.LineType.GetLineTypeTikz()}] ({(int)rectangle.Margin.Left},{(int)rectangle.Margin.Top}) rectangle ({(int)(rectangle.Width + rectangle.Margin.Left)},{(int)(rectangle.Height + rectangle.Margin.Top)});";
        }
    }
}
