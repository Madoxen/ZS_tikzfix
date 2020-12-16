using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TikzFix.Views
{
    /// <summary>
    /// Interaction logic for CollectionCanvas.xaml
    /// </summary>
    public partial class CollectionCanvas : UserControl
    {

        private readonly Rectangle selectionRectangle;
        private Point selectionStartPoint;

        public CollectionCanvas()
        {
            InitializeComponent();
            selectionRectangle = new Rectangle()
            {
                Stroke = Brushes.Aqua,
                StrokeThickness = 1,
                Fill = new SolidColorBrush()
                {
                    Opacity = 0.5,
                    Color = Colors.Aqua
                }
            };
        }

        //Shapes to be drawn on underlaying canvas
        public ICollection<Shape> Shapes
        {
            get { return (ICollection<Shape>)GetValue(ShapesProperty); }
            set { SetValue(ShapesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Children.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapesProperty =
            DependencyProperty.Register("Shapes", typeof(ICollection<Shape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesPropertyChanged));


        //Shapes selected by user when using selector 
        public ICollection<Shape> SelectedShapes
        {
            get { return (ICollection<Shape>)GetValue(SelectedShapesProperty); }
            set { SetValue(SelectedShapesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedShapes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedShapesProperty =
            DependencyProperty.Register("SelectedShapes", typeof(ICollection<Shape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesSelectedChanged));


        private static void OnShapesSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CollectionCanvas collectionCanvas)
                throw new ArgumentException("Value type mismatch: is " + d.GetType().Name + " required " + typeof(CollectionCanvas));

            if (e.NewValue is not ICollection<Shape> shapeCollection)
                throw new ArgumentException("Value type mismatch: is " + e.NewValue.GetType().Name + "required ICollection<Shape>");

            collectionCanvas.SelectedShapes = shapeCollection;
        }



        public bool CanvasSelectable
        {
            get { return (bool)GetValue(CanvasSelectableProperty); }
            set { SetValue(CanvasSelectableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanvasSelectable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanvasSelectableProperty =
            DependencyProperty.Register("CanvasSelectable", typeof(bool), typeof(CollectionCanvas), new PropertyMetadata(true, OnCanvasSelectableChanged));


        private static void OnCanvasSelectableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CollectionCanvas collectionCanvas)
                throw new ArgumentException("Value type mismatch: is " + d.GetType().Name + " required " + typeof(CollectionCanvas));

            if (e.NewValue is not bool b)
                throw new ArgumentException("Value type mismatch: is " + e.NewValue.GetType().Name + "required bool");

            collectionCanvas.CanvasSelectable = b;
            if (collectionCanvas.CanvasSelectable == false)
                collectionCanvas.SelectedShapes.Clear();
        }


        //Called when SetProperty is called in VM
        //Or otherwise INotifyPropertyChanged
        //If set to null, will simply Clear the collection
        private static void OnShapesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CollectionCanvas collectionCanvas)
            {
                if (e.NewValue is ICollection<Shape> collection)
                {
                    collectionCanvas.c.Children.Clear(); //clear all children
                    foreach (Shape element in collection)
                    {
                        collectionCanvas.c.Children.Add(element);
                    }

                    if (collection is INotifyCollectionChanged cc)
                    {
                        cc.CollectionChanged += collectionCanvas.CollectionChanged;
                    }

                    collectionCanvas.Shapes = collection;
                }
                else if (e.NewValue == null)
                {
                    collectionCanvas.c.Children.Clear();
                    collectionCanvas.Shapes = null;
                }
                else
                {
                    throw new ArgumentException("Value type mismatch: is " + e.NewValue.GetType().Name + "required ICollection<Shape>");
                }

                if (e.OldValue is INotifyCollectionChanged oldcc)
                {
                    oldcc.CollectionChanged -= collectionCanvas.CollectionChanged;
                }
            }
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<Shape>().ToList();
            var oldItems = e.OldItems?.Cast<Shape>().ToList();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Shape ele in newItems)
                    {
                        c.Children.Add(ele);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (Shape ele in oldItems)
                    {
                        c.Children.Remove(ele);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    c.Children[e.OldStartingIndex] = (Shape)newItems.ElementAt(0); //TODO: not sure if this works in all cases?
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();
                    break;
            }
        }





        private void HandleSelectionBegin(object sender, MouseButtonEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort
            if (selectionRectangle.Visibility == Visibility.Visible)
                return;

            selectionRectangle.Visibility = Visibility.Visible;
            Point pos = e.GetPosition(c);
            selectionStartPoint = pos;
            c.Children.Add(selectionRectangle);
        }

        private void HandleSelectionMoved(object sender, MouseEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort

            Point pos = e.GetPosition(c);
            selectionRectangle.Width = Math.Abs(pos.X - selectionStartPoint.X);
            selectionRectangle.Height = Math.Abs(pos.Y - selectionStartPoint.Y);
            selectionRectangle.Margin = new Thickness(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), 0, 0);
        }


        private void HandleSelectionEnded(object sender, MouseButtonEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort

            Point pos = e.GetPosition(c);
            Debug.WriteLine(GetSelectedShapes(c, new RectangleGeometry(new Rect(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), selectionRectangle.Width, selectionRectangle.Height))).Count);
            SelectedShapes = GetSelectedShapes(c, new RectangleGeometry(new Rect(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), selectionRectangle.Width, selectionRectangle.Height)));
            c.Children.Remove(selectionRectangle);
            selectionRectangle.Visibility = Visibility.Collapsed;
        }


        private IList<Shape> GetSelectedShapes(UIElement element, Geometry geometry)
        {
            var shapes = new List<Shape>();

            VisualTreeHelper.HitTest(element, null,
                result =>
                {
                    var shape = result.VisualHit as Shape;

                    if (shape != null)
                    {
                        shapes.Add(shape);
                    }

                    return HitTestResultBehavior.Continue;
                },
                new GeometryHitTestParameters(geometry));

            return shapes;
        }
    }
}
