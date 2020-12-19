using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace TikzFix.Tests
{
    using Model;

    using System.Windows.Shapes;

    using TikzFix.Model.Tool;
    using TikzFix.Model.ToolImpl;

    using TikzFixTests;

    using VM;


    [STATestClass]
    public class DrawingTests
    {
        /*
        // TEST
        
        CurrentToolIndex = 0;
            DrawTestLine();


        CurrentToolIndex = 1;
            DrawTestRectangle();

      
        CurrentToolIndex = 2;
            DrawTestEllipse();
        */

        [TestMethod]
        // Should create line from [1,1] to [50,50]
        public void LineTest()
        {
            Line mockLine1 = new Line();
            Line mockLine2 = new Line();
            ITool CurrentTool = new LineTool();

            // user clicked on point [1,1]
            CurrentTool.GetShape(new CanvasEventArgs(1, 1, MouseState.DOWN));

            // user hold mouse and move
            CurrentTool.GetShape(new CanvasEventArgs(20, 20, MouseState.MOVE));
            CurrentTool.GetShape(new CanvasEventArgs(30, 30, MouseState.MOVE));
           
            // user release mouse
            Line l = (Line)CurrentTool.GetShape(new CanvasEventArgs(50, 50, MouseState.UP)).TikzShape;

            Assert.AreEqual(1, l.X1);
            Assert.AreEqual(50, l.X2);
            Assert.AreEqual(1, l.Y1);
            Assert.AreEqual(50, l.Y2);
        }

        [TestMethod]
        // Should create rectangle from [100,200] to [40,40]
        public void RectangleTest()
        {
            ITool CurrentTool = new RectangleTool();
            // user clicked on point [100,200]
            CurrentTool.GetShape(new CanvasEventArgs(100, 200, MouseState.DOWN));

            // user hold mouse and move, this should update current rectangle on canvas
            CurrentTool.GetShape(new CanvasEventArgs(80, 170, MouseState.MOVE));
            CurrentTool.GetShape(new CanvasEventArgs(60, 100, MouseState.MOVE));

            // user release mouse, at this point rectangle shouldn't be modified by any mouse action
            Rectangle r = (Rectangle)CurrentTool.GetShape(new CanvasEventArgs(40, 40, MouseState.UP)).TikzShape;

            Assert.AreEqual(60, r.Width);
            Assert.AreEqual(160, r.Height);
        }

        [TestMethod]
        // Should create elipse from [50,50] to [0,25]
        public void EllipseTest()
        {

            ITool CurrentTool = new EllipseTool();
            // user clicked on point [50,50]
            CurrentTool.GetShape(new CanvasEventArgs(50, 50, MouseState.DOWN));


            // user hold mouse and move, this should update current ellipse on canvas
            CurrentTool.GetShape(new CanvasEventArgs(80, 170, MouseState.MOVE));
            CurrentTool.GetShape(new CanvasEventArgs(60, 100, MouseState.MOVE));

            // user release mouse, at this point ellipse shouldn't be modified by any mouse action
            Ellipse e = (Ellipse)CurrentTool.GetShape(new CanvasEventArgs(0, 25, MouseState.UP)).TikzShape;


            Assert.AreEqual(50, e.Width);
            Assert.AreEqual(25, e.Height);
        }
    }
}
