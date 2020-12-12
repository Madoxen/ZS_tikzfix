using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        private ITool currentTool = new LineTool();

        private ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        public ICollection<Shape> Shapes
        {
            get
            {
                return shapes;
            }
            // private set { SetProperty<ICollection<Shape>>(ref shapes, value); }
        }

        // TODO, observe this in canvas and draw (it can be null)
        private Shape currentDrawingShape;

        public Shape CurrentDrawingShape
        {
            get => currentDrawingShape;
            private set
            {
                currentDrawingShape = value;
                RaisePropertyChanged(nameof(CurrentDrawingShape));
            }
        }



        public MainVM()
        {
            // TEST, add line from [1,1] to [50,50]
            DrawTestLine();
        }


        private void HandleDrawingShape(DrawingShape drawingShape)
        {
            switch (drawingShape.ShapeState)
            {
                case ShapeState.EMPTY:
                    {
                        // do nothing, ShapeCannot be drawn yet
                        CurrentDrawingShape = null;
                        break;
                    }
                case ShapeState.DRAWING:
                    {
                        // shape drawing isn't finished
                        // draw shape but do not add it to list

                        CurrentDrawingShape = drawingShape.Shape;
                        break;
                    }
                case ShapeState.FINISHED:
                    {
                        // drawing shape is finished, add to list
                        CurrentDrawingShape = null;
                        Shapes.Add(drawingShape.Shape);
                        break;
                    }
            }
        }


        private void DrawTestLine()
        {
            // user clicked on point [1,1]
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(1, 1, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(20, 20, MouseState.MOVE)));
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(30, 30, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(40, 40, MouseState.UP)));

            // user clicked for the second time, at this point it should draw new line
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(50, 50, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(60, 60, MouseState.MOVE)));
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(70, 70, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(currentTool.GetShape(new CanvasEvent(80, 80, MouseState.UP)));
        }
    }
}
