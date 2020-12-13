﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        #region Tools

        private readonly ITool rectangleTool = new RectangleTool();
        private readonly ITool lineTool = new LineTool();
        private readonly ITool ellipseTool = new EllipseTool();

        public readonly List<ITool> Tools = new List<ITool>();

        public int CurrentToolIndex { get; }

        public ITool CurrentTool => Tools[CurrentToolIndex];

        #endregion


        private readonly ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        public ICollection<Shape> Shapes
        {
            get { return shapes; }
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
            Tools.Add(lineTool);
            Tools.Add(rectangleTool);
            Tools.Add(ellipseTool);

            // TEST
            // Add line from [1,1] to [50,50]
            CurrentToolIndex = 0;
            DrawTestLine();

            // Add rectangle from [100,200] to [40,40]
            CurrentToolIndex = 1;
            DrawTestRectangle();

            // Add elipse from [50,50] to [0,25]
            CurrentToolIndex = 2;
            DrawTestEllipse();


            CurrentToolIndex = 2;

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
                    // CurrentDrawingShape = null;
                    CurrentDrawingShape = drawingShape;
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
            HandleDrawingShape(CurrentTool.GetShape(e)); //update shape with event args.
        }

        private void UpdateDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(CurrentTool.GetShape(e)); //update shape with event args.
        }

        private bool CanUpdateDrawing(object _)
        {
            return CurrentDrawingShape?.Shape != null;
        }

        #region tests

        private void DrawTestLine()
        {
            // user clicked on point [1,1]
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(1, 1, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(20, 20, MouseState.MOVE)));
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(30, 30, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(40, 40, MouseState.UP)));

            // user clicked for the second time, at this point it should draw new line
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(50, 50, MouseState.DOWN)));

            // user hold mouse and move
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(60, 60, MouseState.MOVE)));
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(70, 70, MouseState.MOVE)));

            // user release mouse
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(80, 80, MouseState.UP)));
        }

        private void DrawTestRectangle()
        {
            // user clicked on point [100,200]
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(100, 200, MouseState.DOWN)));

            // user hold mouse and move, this should update current rectangle on canvas
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(80, 170, MouseState.MOVE)));
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(60, 100, MouseState.MOVE)));

            // user release mouse, at this point rectangle shouldn't be modified by any mouse action
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(40, 40, MouseState.UP)));
        }

        private void DrawTestEllipse()
        {
            // user clicked on point [50,50]
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(50, 50, MouseState.DOWN)));


            // user hold mouse and move, this should update current ellipse on canvas
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(80, 170, MouseState.MOVE)));
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(60, 100, MouseState.MOVE)));

            // user release mouse, at this point ellipse shouldn't be modified by any mouse action
            HandleDrawingShape(CurrentTool.GetShape(new CanvasEventArgs(0, 25, MouseState.UP)));
        }

        #endregion
    }
}