using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Windows.Shapes;

using TikzFix.Model.Tool;

namespace TikzFix.Model.FormatGenerator
{
    internal class JsonFormatGenerator : IFormatGenerator
    {
        private readonly Dictionary<Type, Func<Shape, LocalShapeData>> shapeMethodMap;

        public JsonFormatGenerator()
        {
            shapeMethodMap = new Dictionary<Type, Func<Shape, LocalShapeData>>()
            {
                [typeof(Line)] = JsonifyLine,
                [typeof(Rectangle)] = JsonifyRectangle,
                [typeof(Ellipse)] = JsonifyEllipse
            };
        }

        private LocalShapeData Convert(Shape shape)
        {
            if (shapeMethodMap.TryGetValue(shape.GetType(), out Func<Shape, LocalShapeData> f))
            {
                return f(shape);
            }

            return null;
        }

        public string ConvertMany(ICollection<Shape> shapes)
        {
            List<LocalShapeData> localShapesData = new List<LocalShapeData>(shapes.Count);
            foreach (Shape s in shapes)
            {
                localShapesData.Add(Convert(s));
            }

            return JsonSerializer.Serialize(localShapesData);
        }

        public LocalShapeData JsonifyLine(Shape s)
        {
            if (s is not Line l)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Line");
            }

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs((int)l.X1, (int)l.Y1, MouseState.DOWN),
                new CanvasEventArgs((int)l.X2, (int)l.Y2, MouseState.UP)
            };

            return new LocalShapeData("LineTool", keyPointList);
        }

        private LocalShapeData JsonifyRectangle(Shape s)
        {
            if (s is not Rectangle r)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Rectangle");
            }

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs((int)r.Margin.Left,(int)r.Margin.Top, MouseState.DOWN),
                new CanvasEventArgs((int)(r.Margin.Left+r.Width), (int)(r.Margin.Top+r.Height), MouseState.UP)
            };

            return new LocalShapeData("RectangleTool", keyPointList);
        }

        private LocalShapeData JsonifyEllipse(Shape s)
        {
            if (s is not Ellipse e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
            }
            int X1 = (int)(e.Margin.Left + e.Width / 2);
            int Y1 = (int)(e.Margin.Top + e.Height / 2);

            List<CanvasEventArgs> keyPointList = new List<CanvasEventArgs>
            {
                new CanvasEventArgs(X1, Y1, MouseState.DOWN),
                new CanvasEventArgs(X1+(int)(e.Width/2), (int)(Y1+e.Height/2), MouseState.UP)
            };

            return new LocalShapeData("EllipseTool", keyPointList);
        }

    }
}
