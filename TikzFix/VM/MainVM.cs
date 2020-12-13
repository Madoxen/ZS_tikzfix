using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Shapes;

using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        private ITool currentTool = new LineTool();

        private readonly ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        public ICollection<Shape> Shapes
        {
            get
            {
                return shapes;
            }
            // private set { SetProperty<ICollection<Shape>>(ref shapes, value); }
        }

        // TODO, observe this in canvas and draw (it can be null)
        private DrawingShape currentDrawingShape;
        public DrawingShape CurrentDrawingShape
        {
            get => currentDrawingShape;
            private set
            {
                Shapes.Remove(currentDrawingShape?.Shape); // remove shape to stop drawing it
                if (value != null)
                {
                    Shapes.Add(value.Shape); // remove shape to stop drawing it
                }
                SetProperty(ref currentDrawingShape, value);
            }
        }





        public RelayCommand CancelDrawingCommand { get; } //Should be called whenever user wants to cancel drawing TODO: Add cancel functionality
        public RelayCommand<CanvasEventArgs> StepDrawingCommand { get; }  //Should be called when mouse button is pressed
        public RelayCommand<CanvasEventArgs> UpdateDrawingCommand { get; } //Should be called when mouse pointer is moved
        public RelayCommand CommitDrawingCommand { get; }




        public MainVM()
        {
            // TEST, add line from [1,1] to [50,50]
            DrawTestLine();
            CancelDrawingCommand = new RelayCommand(CancelDrawing);
            StepDrawingCommand = new RelayCommand<CanvasEventArgs>(StepDrawing);
            UpdateDrawingCommand = new RelayCommand<CanvasEventArgs>(UpdateDrawing, CanUpdateDrawing);

        }


        private void HandleDrawingShape(DrawingShape drawingShape)
        {
            switch (drawingShape.ShapeState)
            {
                case ShapeState.EMPTY:
                    // do nothing, ShapeCannot be drawn yet
                    CurrentDrawingShape = null;
                    break;

                case ShapeState.DRAWING:
                    // shape drawing isn't finished
                    // draw shape but do not add it to list
                    CurrentDrawingShape = drawingShape;
                    break;

                case ShapeState.FINISHED:
                    // drawing shape is finished, add to list
                    CurrentDrawingShape = null;
                    Shapes.Add(drawingShape.Shape);
                    break;

            }
        }

        private void CancelDrawing()
        {
            CurrentDrawingShape = null;
        }

        private void StepDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(currentTool.GetShape(e)); //update shape with event args.
        }

        private void UpdateDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(currentTool.GetShape(e)); //update shape with event args.
        }

        private bool CanUpdateDrawing(object _)
        {
            return CurrentDrawingShape?.Shape != null;
        }


        private void DrawTestLine()
        {
            // user clicked on point [1,1]
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(1, 1, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(20, 20, MouseState.MOVE)));
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(30, 30, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(40, 40, MouseState.UP)));

            // user clicked for the second time, at this point it should draw new line
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(50, 50, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(60, 60, MouseState.MOVE)));
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(70, 70, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(currentTool.GetShape(new CanvasEventArgs(80, 80, MouseState.UP)));
        }

    }
}
