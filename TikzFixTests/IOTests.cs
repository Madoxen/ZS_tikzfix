using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using System.Windows.Media;

using TikzFix.Model.FormatGenerator;
using TikzFix.Model.FormatLoader;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

using TikzFixTests;


namespace TikzFix.Tests
{
    [STATestClass]
    public class IOTests
    {

        //imo no need for io dialog tests because of filters
        private static readonly TikzStyle style = new TikzStyle(Color.FromArgb(255, 0, 0, 0), Color.FromArgb(255, 0, 0, 0));

        //Converter test
        [TestMethod]
        public void ConvertTikzJsonTest()
        {
            //Arrange
            string jsonTikz = @"
[{""ToolName"":""RectangleTool"",""KeyPoints"":[{""Point"":{""X"":182,""Y"":60},""MouseState"":0},{""Point"":{""X"":409,""Y"":213},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""LineTool"",""KeyPoints"":[{""Point"":{""X"":114,""Y"":258},""MouseState"":0},{""Point"":{""X"":183,""Y"":211},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""LineTool"",""KeyPoints"":[{""Point"":{""X"":181,""Y"":57},""MouseState"":0},{""Point"":{""X"":86,""Y"":127},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""LineTool"",""KeyPoints"":[{""Point"":{""X"":409,""Y"":62},""MouseState"":0},{""Point"":{""X"":310,""Y"":147},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""LineTool"",""KeyPoints"":[{""Point"":{""X"":406,""Y"":211},""MouseState"":0},{""Point"":{""X"":305,""Y"":297},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""BezierTool"",""KeyPoints"":[{""Point"":{""X"":195,""Y"":254},""MouseState"":0},{""Point"":{""X"":267,""Y"":179},""MouseState"":2},{""Point"":{""X"":238,""Y"":89},""MouseState"":0},{""Point"":{""X"":238,""Y"":89},""MouseState"":2},{""Point"":{""X"":217,""Y"":279},""MouseState"":0},{""Point"":{""X"":217,""Y"":279},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}},{""ToolName"":""EllipseTool"",""KeyPoints"":[{""Point"":{""X"":286,""Y"":166},""MouseState"":0},{""Point"":{""X"":306,""Y"":305},""MouseState"":2}],""Style"":{""LineEnding"":0,""LineWidth"":2,""LineType"":0,""StrokeColor"":{""ColorContext"":null,""A"":255,""R"":0,""G"":0,""B"":0,""ScA"":1,""ScR"":0,""ScG"":0,""ScB"":0},""FillColor"":{""ColorContext"":null,""A"":0,""R"":0,""G"":0,""B"":0,""ScA"":0,""ScR"":0,""ScG"":0,""ScB"":0}}}]
";

            //Act
            int lineCount = 0;
            int bezierCount = 0;
            int rectCount = 0;
            int ellipseCount = 0;
            foreach (TikzShape shape in new JsonFormatLoader().ConvertMany(jsonTikz))
            {
                if (shape is TikzRectangle)
                {
                    rectCount++;
                }
                else if (shape is TikzLine)
                {
                    lineCount++;
                }
                else if (shape is TikzBezier)
                {
                    bezierCount++;
                }
                else if (shape is TikzEllipse)
                {
                    ellipseCount++;
                }
            }

            //Assert
            Assert.AreEqual(ellipseCount, 1);
            Assert.AreEqual(lineCount, 4);
            Assert.AreEqual(bezierCount, 1);
            Assert.AreEqual(rectCount, 1);

        }

        //Converter test
        [TestMethod]
        public void DrawShapeGenerateJsonFromItAndConvertItBackToShape()
        {
            ITool CurrentTool = new RectangleTool();
            CurrentTool.GetShape(new CanvasEventArgs(new Point(100, 200), MouseState.DOWN), style);
            CurrentTool.GetShape(new CanvasEventArgs(new Point(80, 170), MouseState.MOVE), style);
            CurrentTool.GetShape(new CanvasEventArgs(new Point(60, 100), MouseState.MOVE), style);

            TikzShape[] shapes = new TikzShape[] {
            CurrentTool.GetShape(new CanvasEventArgs(new Point(40, 40), MouseState.UP), style).TikzShape };

            JsonFormatGenerator jsonGen = new JsonFormatGenerator();
            string jsonTikz = jsonGen.ConvertMany(shapes);

            Assert.IsTrue(new JsonFormatLoader().ConvertMany(jsonTikz).ToList().Single(shape => shape is TikzRectangle) != null);

        }

    }
}
