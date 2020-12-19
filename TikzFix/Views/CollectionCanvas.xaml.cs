using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using TikzFix.Model.TikzShapes;
using TikzFix.Utils;


namespace TikzFix.Views
{
    /// <summary>
    /// Interaction logic for CollectionCanvas.xaml
    /// </summary>
    public partial class CollectionCanvas : UserControl
    {

        private readonly Rectangle selectionRectangle;
        private Point selectionStartPoint;
        private Effect selectionEffect = new DropShadowEffect() { Color = Colors.Aqua };

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
        public ICollection<TikzShape> Shapes
        {
            get { return (ICollection<TikzShape>)GetValue(ShapesProperty); }
            set { SetValue(ShapesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Children.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapesProperty =
            DependencyProperty.Register("Shapes", typeof(ICollection<TikzShape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesPropertyChanged));


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
            if (d is not CollectionCanvas cc)
                throw new ArgumentException("Value type mismatch: is " + d.GetType().Name + " required " + typeof(CollectionCanvas));

            if (cc.SelectedShapes is INotifyCollectionChanged new_icc)
                new_icc.CollectionChanged += cc.SelectedShapesCollectionChangedHandler;

            if (e.OldValue is INotifyCollectionChanged old_icc)
                old_icc.CollectionChanged -= cc.SelectedShapesCollectionChangedHandler;


            if (e.OldValue is ICollection<Shape> oldShapeCollection)
            {
                foreach (Shape s in oldShapeCollection)
                {
                    s.Effect = null;
                }
            }

            foreach (Shape s in cc.SelectedShapes)
            {
                s.Effect = cc.selectionEffect;
            }
        }

        private void SelectedShapesCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<Shape>().ToList();
            var oldItems = e.OldItems?.Cast<Shape>().ToList();


            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Shape ele in newItems)
                    {
                        ele.Effect = selectionEffect;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (Shape ele in oldItems)
                    {
                        ele.Effect = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    oldItems[e.OldStartingIndex].Effect = null;
                    newItems[e.NewStartingIndex].Effect = selectionEffect;
                    break;

            }
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
            if (d is not CollectionCanvas cc)
                throw new ArgumentException("Value type mismatch: is " + d.GetType().Name + " required " + typeof(CollectionCanvas));

            if (cc.CanvasSelectable == false)
                cc.SelectedShapes?.Clear();
        }


        //Called when SetProperty is called in VM
        //Or otherwise INotifyPropertyChanged
        //If set to null, will simply Clear the collection
        private static void OnShapesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is CollectionCanvas cc)
            {
                cc.c.Children.Clear(); //clear all children
                foreach (TikzShape element in cc.Shapes)
                {
                    cc.c.Children.Add(element.Shape);
                }

                if (cc.Shapes is INotifyCollectionChanged icc)
                {
                    icc.CollectionChanged += cc.ShapesCollectionChanged;
                }

                if (e.NewValue == null)
                {
                    cc.c.Children.Clear();
                    cc.Shapes = null;
                }

                if (e.OldValue is INotifyCollectionChanged oldcc)
                {
                    oldcc.CollectionChanged -= cc.ShapesCollectionChanged;
                }
            }
        }

        /// <summary>
        /// Called whenever Shapes collection is changed
        /// </summary>
        private void ShapesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<TikzShape>().ToList();
            var oldItems = e.OldItems?.Cast<TikzShape>().ToList();

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TikzShape ele in newItems)
                    {
                        c.Children.Add(ele.Shape);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (TikzShape ele in oldItems)
                    {
                        c.Children.Remove(ele.Shape);
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    c.Children[e.OldStartingIndex] = newItems[0].Shape; //TODO: not sure if this works in all cases?
                    break;

                case NotifyCollectionChangedAction.Move:
                    throw new NotImplementedException();

                case NotifyCollectionChangedAction.Reset:
                    c.Children.Clear();
                    break;
            }
        }


        #region Selection handling
        private void HandleSelectionBegin(object sender, MouseButtonEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort
            if (selectionRectangle.Visibility == Visibility.Visible)
                return;

            Point pos = e.GetPosition(c);
            selectionStartPoint = pos;
            selectionRectangle.Visibility = Visibility.Visible;
            c.Children.Add(selectionRectangle);
            SelectedShapes.Clear(ResetEffect);
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
            var raycastResult = GetSelectedShapes(c, new RectangleGeometry(new Rect(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), selectionRectangle.Width, selectionRectangle.Height)));
            foreach (Shape s in raycastResult)
            {
                SelectedShapes.Add(s);
            }
            c.Children.Remove(selectionRectangle);
            selectionRectangle.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Geometry hittest against the shapes on the canvas
        /// </summary>
        /// <param name="element">Element that contains shapes</param>
        /// <param name="geometry">Geometry of hit test (in our case a selection rectangle)</param>
        /// <returns></returns>
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


            shapes.Remove(selectionRectangle);
            return shapes;
        }

        #endregion

        private void ResetEffect(ICollection<Shape> shapes)
        {
            foreach (Shape s in shapes)
            {
                s.Effect = null;
            }
        }
    }
}
