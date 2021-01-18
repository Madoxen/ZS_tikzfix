using System.Collections.Generic;
using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatGenerator
{
    internal class TikzFormatGenerator : IFormatGenerator
    {
        // y scale has to be negative, tikz is mirrored vertically
        public const string TIKZ_MAIN = "\\begin{{tikzpicture}}[yscale=-1]\n{0}\n\\end{{tikzpicture}}";

        public string ConvertMany(ICollection<TikzShape> shapes)
        {
            List<string> lines = new List<string>(shapes.Count);
            foreach (TikzShape s in shapes)
            {
                lines.Add(s.GenerateTikz());
            }
            return string.Format(TIKZ_MAIN, string.Join("\n", lines));
        }
    }
}
