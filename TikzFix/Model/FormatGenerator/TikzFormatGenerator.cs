using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatGenerator
{
    internal class TikzFormatGenerator : IFormatGenerator
    {
        // y scale has to be negative, tikz is mirrored vertically
        public const string TIKZ_MAIN = "\\begin{{tikzpicture}}[xscale=0.01, yscale=-0.01]\n{0}\n\\end{{tikzpicture}}";


        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            List<string> lines = new List<string>(shapes.Count);
            foreach (TikzShape s in shapes)
            {
                lines.Add(s.GenerateTikz());
            }
            return string.Format(TIKZ_MAIN, string.Join("\n", lines));
        }

        //private string TikzifyRectangle(Shape s)
        //{
        //    if (s is not Rectangle r)
        //    {
        //        throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Rectangle");
        //    }

        //    return $"\\draw ({(int)r.Margin.Left},{(int)r.Margin.Top}) rectangle ({(int)(r.Width + r.Margin.Left)},{(int)(r.Height + r.Margin.Top)});";
        //}

        //private string TikzifyEllipse(Shape s)
        //{
        //    if (s is not Ellipse e)
        //    {
        //        throw new Exception($"Shape-Tool type mismatch, tool type: {GetType().Name}, expected shape type Ellipse");
        //    }

        //    return $"\\draw ({(int)(e.Margin.Left + e.Width / 2)},{(int)(e.Margin.Top + e.Height / 2)}) ellipse ({(int)(s.Width / 2)} and {(int)(s.Height / 2)});";
        //}
    }
}
