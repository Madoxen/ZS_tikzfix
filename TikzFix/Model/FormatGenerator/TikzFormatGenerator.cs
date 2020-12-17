using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace TikzFix.Model.FormatGenerator
{
    internal class TikzFormatGenerator : IFormatGenerator
    {
        private readonly Dictionary<Type, Func<Shape, string>> shapeTypeMethodMap;
        // todo, change x/yscale to 1 and scale shape to fill generated drawing
        private const string TIKZ_MAIN = "\\begin{{tikzpicture}}[xscale=0.1, yscale=-0.1]\n{0}\n\\end{{tikzpicture}}"; // y scale has to be negative, tikz is mirrored vertically


        public TikzFormatGenerator()
        {
            shapeTypeMethodMap = new Dictionary<Type, Func<Shape, string>>()
            {
                [typeof(Line)] = TikzifyLine,
                [typeof(Rectangle)] = TikzifyRectangle,
                [typeof(Ellipse)] = TikzifyEllipse
            };
        }

        public string Convert(Shape shape)
        {
            //Get convertion method from shapeType map
            //ASK: Should this be dropped or should exception happen
            if (shapeTypeMethodMap.TryGetValue(shape.GetType(), out Func<Shape, string> f))
            {
                return f(shape);
            }

            return "";
        }

        public string ConvertMany(ICollection<Shape> shapes)
        {
            List<string> lines = new List<string>(shapes.Count);
            foreach (Shape s in shapes)
            {
                lines.Add(Convert(s));
            }

            return string.Format(TIKZ_MAIN, string.Join("\n", lines));
        }


        public string TikzifyLine(Shape s)
        {
            if (s is not Line l)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Line");
            }

            return $"\\draw ({l.X1},{l.Y1})--({l.X2},{l.Y2});";
        }

        private string TikzifyRectangle(Shape s)
        {
            if (s is not Rectangle r)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Rectangle");
            }

            return $"\\draw ({(int)r.Margin.Left},{(int)r.Margin.Top}) rectangle ({(int)(r.Width + r.Margin.Left)},{(int)(r.Height + r.Margin.Top)});";
        }

        private string TikzifyEllipse(Shape s)
        {
            if (s is not Ellipse e)
            {
                throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
            }

            return $"\\draw ({(int)(e.Margin.Left + e.Width / 2)},{(int)(e.Margin.Top + e.Height / 2)}) ellipse ({(int)(s.Width / 2)} and {(int)(s.Height / 2)});";
        }
    }
}
