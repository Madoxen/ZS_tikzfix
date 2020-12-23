using System.Collections.Generic;
using System.Collections.ObjectModel;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;
using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using TikzFix.Utils;

namespace TikzFix.VM
{
    /// <summary>
    /// Main window VM, maintains main shape collection that is shown to the user by canvas
    /// also maintains all available tools and selection functionality
    /// </summary>
    class MainVM : BaseVM
    {
        #region Tools

        private readonly ITool rectangleTool = new RectangleTool();
        private readonly ITool lineTool = new LineTool();
        private readonly ITool ellipseTool = new EllipseTool();
        private readonly ITool bezierTool = new BezierTool();
        public readonly List<ITool> Tools = new List<ITool>();


        private int currentToolIndex;
        public int CurrentToolIndex
        {
            get
            {
                return currentToolIndex;
            }
            set
            {
                SetProperty(ref currentToolIndex, value);
                CanvasSelectable = value < 0;
            }
        }

        public ITool CurrentTool => CurrentToolIndex >= 0 ? Tools[CurrentToolIndex] : null;

        #endregion


        private readonly ObservableCollection<TikzShape> shapes = new ObservableCollection<TikzShape>();
        public ICollection<TikzShape> Shapes
        {
            get
            {
                return shapes;
            }
        }


        private ObservableCollection<TikzShape> selectedShapes = new ObservableCollection<TikzShape>();
        public ObservableCollection<TikzShape> SelectedShapes
        {
            get
            {
                return selectedShapes;
            }
            set
            {
                SetProperty(ref selectedShapes, value);
            }
        }

        private bool canvasSelectable = false;
        public bool CanvasSelectable
        {
            get
            {
                return canvasSelectable;
            }
            set
            {
                SetProperty(ref canvasSelectable, value);
            }
        }

        private DrawingShape currentDrawingShape;
        public DrawingShape CurrentDrawingShape
        {
            get => currentDrawingShape;
            private set
            {
                if (value != currentDrawingShape)
                {
                    Shapes.Remove(currentDrawingShape?.TikzShape); // remove shape to stop drawing it
                    if (value != null)
                    {
                        Shapes.Add(value.TikzShape); // remove shape to stop drawing it
                    }
                }
                SetProperty(ref currentDrawingShape, value);
            }
        }

        public RelayCommand CancelDrawingCommand
        {
            get;
        }

        //Should be called whenever user wants to cancel drawing TODO: Add cancel functionality
        public RelayCommand<CanvasEventArgs> StepDrawingCommand
        {
            get;
        }

        //Should be called when mouse button is pressed
        public RelayCommand<CanvasEventArgs> UpdateDrawingCommand
        {
            get;
        }

        //Should be called when mouse pointer is moved
        public RelayCommand CommitDrawingCommand
        {
            get;
        }


        public RelayCommand<int> ChangeToolCommand
        {
            get;
        }
        public RelayCommand CancelSelectionCommand
        {
            get;
        }
        public RelayCommand DeleteSelectionCommand
        {
            get;
        }


        public MainVM()
        {
            Tools.Add(lineTool);
            Tools.Add(rectangleTool);
            Tools.Add(ellipseTool);
            Tools.Add(bezierTool);

            CurrentToolIndex = 3;

            CancelDrawingCommand = new RelayCommand(CancelDrawing);
            StepDrawingCommand = new RelayCommand<CanvasEventArgs>(StepDrawing);
            UpdateDrawingCommand = new RelayCommand<CanvasEventArgs>(UpdateDrawing, CanUpdateDrawing);
            ChangeToolCommand = new RelayCommand<int>(ChangeTool);


            DeleteSelectionCommand = new RelayCommand(DeleteSelection);
            CancelSelectionCommand = new RelayCommand(CancelSelection);
        }

        #region Drawing

        // TODO make it observable and add UI, user should be able to change any prop in TikzStyle
        // TODO add line types to canvas/tools. When ITool impl get style it should apply line type e.g. dotted
        private readonly TikzStyle CurrentSelectedStyle = new TikzStyle(
            LaTexColor.BLUE,
            LaTexColor.RED,
            LineEnding.NONE,
            LineWidth.THIN,
            LineType.SOLID
            );

        private void HandleDrawingShape(DrawingShape drawingShape)
        {
            if (drawingShape == null)
                return;

            switch (drawingShape.ShapeState)
            {
                case ShapeState.START:
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
                    Shapes.Add(drawingShape.TikzShape);
                    break;
            }
        }


        private void CancelDrawing()
        {
            CurrentDrawingShape = null;
        }

        private void StepDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(CurrentTool?.GetShape(e, CurrentSelectedStyle)); //update shape with event args.
        }

        private void UpdateDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(CurrentTool?.GetShape(e, CurrentSelectedStyle)); //update shape with event args.
        }

        private bool CanUpdateDrawing(object _)
        {
            return CurrentDrawingShape?.TikzShape != null;
        }

        private void ChangeTool(int index)
        {
            CurrentToolIndex = index;
        }
        #endregion

        #region Selection Commands

        public void CancelSelection()
        {
            //FIXME: Cannot do .Clear because Clear does not convey information about old items
            SelectedShapes = new ObservableCollection<TikzShape>();
        }

        public void DeleteSelection()
        {
            foreach (TikzShape s in SelectedShapes)
            {
                Shapes.Remove(s);
            }

            //FIXME: Cannot do .Clear because Clear does not convey information about old items
            SelectedShapes = new ObservableCollection<TikzShape>();
        }

        #endregion
    }
}