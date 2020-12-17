using System.Windows.Shapes;

namespace TikzFix.Model.Tool
{
    interface ITool
    {
        // todo, change x/yscale to 1 and scale shape to fill generated drawing
        public const string TIKZ_MAIN = "\\begin{{tikzpicture}}[xscale=0.1, yscale=-0.1]\n{0}\n\\end{{tikzpicture}}"; // y scale has to be negative, tikz is mirrored vertically

        DrawingShape GetShape(CanvasEventArgs canvasEventArgs);

        LocalShapeData ConvertToShapeData(Shape shape);

        string GenerateTikzShape(Shape shape);
    }
}
