using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Shapes;
using TikzFix.Model.Tool;
using TikzFix.Model.ToolImpl;
using System.Linq;
using TikzFix.Utils;
using System;
using System.Text.Json;
using System.IO;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        #region Tools

        private readonly ITool rectangleTool = new RectangleTool();
        private readonly ITool lineTool = new LineTool();
        private readonly ITool ellipseTool = new EllipseTool();

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


        private readonly ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        public ICollection<Shape> Shapes
        {
            get
            {
                return shapes;
            }
        }


        private ObservableCollection<Shape> selectedShapes = new ObservableCollection<Shape>();
        public ObservableCollection<Shape> SelectedShapes
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


        // TODO, observe this in canvas and draw (it can be null)
        private DrawingShape currentDrawingShape;
        public DrawingShape CurrentDrawingShape
        {
            get => currentDrawingShape;
            private set
            {
                if (value != currentDrawingShape)
                {
                    Shapes.Remove(currentDrawingShape?.Shape); // remove shape to stop drawing it
                    if (value != null)
                    {
                        Shapes.Add(value.Shape); // remove shape to stop drawing it
                    }
                }
                SetProperty(ref currentDrawingShape, value);
            }
        }

        public RelayCommand CancelDrawingCommand
        {
            get;
        } //Should be called whenever user wants to cancel drawing TODO: Add cancel functionality
        public RelayCommand<CanvasEventArgs> StepDrawingCommand
        {
            get;
        }  //Should be called when mouse button is pressed
        public RelayCommand<CanvasEventArgs> UpdateDrawingCommand
        {
            get;
        } //Should be called when mouse pointer is moved
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


            CurrentToolIndex = -1;

            CancelDrawingCommand = new RelayCommand(CancelDrawing);
            StepDrawingCommand = new RelayCommand<CanvasEventArgs>(StepDrawing);
            UpdateDrawingCommand = new RelayCommand<CanvasEventArgs>(UpdateDrawing, CanUpdateDrawing);
            ChangeToolCommand = new RelayCommand<int>(ChangeTool);


            DeleteSelectionCommand = new RelayCommand(DeleteSelection);
            CancelSelectionCommand = new RelayCommand(CancelSelection);

            SaveCommand = new RelayCommand(SaveTest);
            LoadCommand = new RelayCommand(LoadTest);
            GenerateTikzCommand = new RelayCommand(GenerateTikz);
        }

        #region Drawing
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
            HandleDrawingShape(CurrentTool?.GetShape(e)); //update shape with event args.
        }

        private void UpdateDrawing(CanvasEventArgs e)
        {
            HandleDrawingShape(CurrentTool?.GetShape(e)); //update shape with event args.
        }

        private bool CanUpdateDrawing(object _)
        {
            return CurrentDrawingShape?.Shape != null;
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
            SelectedShapes = new ObservableCollection<Shape>();
        }

        public void DeleteSelection()
        {
            foreach (Shape s in SelectedShapes)
            {
                Shapes.Remove(s);
            }

            //FIXME: Cannot do .Clear because Clear does not convey information about old items
            SelectedShapes = new ObservableCollection<Shape>();
        }

        #endregion

        #region IO

        public RelayCommand SaveCommand
        {
            get;
        }

        public RelayCommand LoadCommand
        {
            get;
        }

        public RelayCommand GenerateTikzCommand
        {
            get;
        }

        private ITool GetTool(string toolName)
        {
            foreach (ITool tool in Tools)
            {
                if (tool.GetType().Name == toolName)
                {
                    return tool;
                }
            }

            throw new Exception("Unsuported shape type. Currently suppoerted: [Line, Ellipse, Rectangle]");
        }

        private ITool GetTool(Shape shape)
        {
            return GetTool(shape.MapShapeWithTool());
        }

        private const string testFileName = "savedData.json";

        private void Save(string fileName)
        {
            List<LocalShapeData> canvasShapesData = new List<LocalShapeData>();

            foreach (Shape shape in Shapes)
            {
                canvasShapesData.Add(GetTool(shape).ConvertToShapeData(shape));
            }

            string jsonString = JsonSerializer.Serialize(canvasShapesData);
            File.WriteAllText(fileName, jsonString);
        }

        private void SaveTest()
        {
            Save(testFileName);
        }


        private void Load(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);

            Shapes.Clear();

            List<LocalShapeData> canvasShapesData = JsonSerializer.Deserialize<List<LocalShapeData>>(jsonString);

            ITool currentTool;

            foreach (LocalShapeData shapeData in canvasShapesData)
            {
                currentTool = GetTool(shapeData.ToolName);

                foreach (CanvasEventArgs canvasEventArgs in shapeData.KeyPoints)
                {
                    var r = currentTool.GetShape(canvasEventArgs);

                    if (r.ShapeState == ShapeState.FINISHED)
                    {
                        Shapes.Add(r.Shape);
                    }
                }
            }
        }

        private void LoadTest()
        {
            Load(testFileName);
        }


        private void GenerateTikz()
        {
            List<string> r = new List<string>();
            foreach (Shape shape in Shapes)
            {
                r.Add(GetTool(shape).GenerateTikzShape(shape));
            }

            Debug.WriteLine(string.Format(ITool.TIKZ_MAIN, string.Join("\n", r)));
        }

        #endregion

    }
}