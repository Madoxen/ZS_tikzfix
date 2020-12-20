using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Shapes;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            List<LocalShapeData> localShapesData = new List<LocalShapeData>(shapes.Count);
            foreach (TikzShape s in shapes)
            {
                localShapesData.Add(s.GenerateLocalData());
            }

            return JsonSerializer.Serialize(localShapesData);
        }

        //private LocalShapeData JsonifyEllipse(Shape s)
        //{
        //    if (s is not Ellipse e)
        //    {
        //        throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
        //    }
        //    int X1 = (int)(e.Margin.Left + e.Width / 2);
        //    int Y1 = (int)(e.Margin.Top + e.Height / 2);
        //
        //    List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
        //    {
        //        new CanvasEventArgs(X1, Y1, MouseState.DOWN),
        //        new CanvasEventArgs(X1+(int)(e.Width/2), (int)(Y1+e.Height/2), MouseState.UP)
        //    };
        //
        //    return new LocalShapeData("EllipseTool", keyPointList);
        //}

    }
}
