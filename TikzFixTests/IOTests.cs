using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
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

        //Loads prepared specified json file and checks count of various shapes
        [TestMethod]
        public void LoadJsonCountTest()
        {
            //Arrange
            string jsonTikz = File.ReadAllText("TestData/test.json");

            //Act
            int lineCount = 0;
            int bezierCount = 0;
            int rectCount = 0;
            int ellipseCount = 0;
            int pathCount = 0;
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
                else if (shape is TikzPath)
                {
                    pathCount++;
                }
            }

            Debug.WriteLine(ellipseCount);
            Debug.WriteLine(lineCount);
            Debug.WriteLine(bezierCount);
            Debug.WriteLine(rectCount);
            Debug.WriteLine(pathCount);

            //Assert
            Assert.AreEqual(1,ellipseCount);
            Assert.AreEqual(2,lineCount);
            Assert.AreEqual(2,bezierCount);
            Assert.AreEqual(1,rectCount);
            Assert.AreEqual(496,pathCount);
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
