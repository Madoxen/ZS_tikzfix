using System.Windows.Shapes;
using TikzFix.Model.JsonFormatter;

namespace TikzFix.Model.Tool
{
    interface ITool
    {
        public const string TIKZ_MAIN = "\\begin{{tikzpicture}}[xscale=1, yscale=1]\n{0}\n\\end{{tikzpicture}}";

        DrawingShape GetShape(CanvasEventArgs canvasEventArgs);

        LocalShapeData ConvertToShapeData(Shape shape);

        string GenerateTikzShape(Shape shape);
    }
}
