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

        private readonly TikzRectangle selectionRectangle;
        private Point selectionStartPoint;
        private readonly Effect selectionEffect = new DropShadowEffect() { Color = Colors.Aqua };

        public CollectionCanvas()
        {
            InitializeComponent();
            var rect = new Rectangle()
            {
                Stroke = Brushes.Aqua,
                StrokeThickness = 1,
                Fill = new SolidColorBrush()
                {
                    Opacity = 0.5,
                    Color = Colors.Aqua
                }
            };

            selectionRectangle = new TikzRectangle(rect, null); // TikzStyle not needed 

        }

        //Shapes to be drawn on underlaying canvas
        public ICollection<TikzShape> Shapes
        {
            get
            {
                return (ICollection<TikzShape>)GetValue(ShapesProperty);
            }
            set
            {
                SetValue(ShapesProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Children.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapesProperty =
            DependencyProperty.Register("Shapes", typeof(ICollection<TikzShape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesPropertyChanged));


        //Shapes selected by user when using selector 
        public ICollection<TikzShape> SelectedShapes
        {
            get
            {
                return (ICollection<TikzShape>)GetValue(SelectedShapesProperty);
            }
            set
            {
                SetValue(SelectedShapesProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SelectedShapes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedShapesProperty =
            DependencyProperty.Register("SelectedShapes", typeof(ICollection<TikzShape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesSelectedChanged));


        private static void OnShapesSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not CollectionCanvas cc)
                throw new ArgumentException("Value type mismatch: is " + d.GetType().Name + " required " + typeof(CollectionCanvas));

            if (cc.SelectedShapes is INotifyCollectionChanged new_icc)
                new_icc.CollectionChanged += cc.SelectedShapesCollectionChangedHandler;

            if (e.OldValue is INotifyCollectionChanged old_icc)
                old_icc.CollectionChanged -= cc.SelectedShapesCollectionChangedHandler;


            if (e.OldValue is ICollection<TikzShape> oldShapeCollection)
            {
                foreach (TikzShape s in oldShapeCollection)
                {
                    s.Shape.Effect = null;
                }
            }

            foreach (TikzShape s in cc.SelectedShapes)
            {
                s.Shape.Effect = cc.selectionEffect;
            }
        }

        private void SelectedShapesCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newItems = e.NewItems?.Cast<TikzShape>().ToList();
            var oldItems = e.OldItems?.Cast<TikzShape>().ToList();


            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TikzShape ele in newItems)
                    {
                        ele.Shape.Effect = selectionEffect;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (TikzShape ele in oldItems)
                    {
                        ele.Shape.Effect = null;
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    oldItems[e.OldStartingIndex].Shape.Effect = null;
                    newItems[e.NewStartingIndex].Shape.Effect = selectionEffect;
                    break;

            }
        }


        public bool CanvasSelectable
        {
            get
            {
                return (bool)GetValue(CanvasSelectableProperty);
            }
            set
            {
                SetValue(CanvasSelectableProperty, value);
            }
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
            if (selectionRectangle.Shape.Visibility == Visibility.Visible)
                return;

            Point pos = e.GetPosition(c);
            selectionStartPoint = pos;
            selectionRectangle.Shape.Visibility = Visibility.Visible;
            c.Children.Add(selectionRectangle.Shape);
            SelectedShapes.Clear(ResetEffect);
        }

        private void HandleSelectionMoved(object sender, MouseEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort

            Point pos = e.GetPosition(c);
            selectionRectangle.Shape.Width = Math.Abs(pos.X - selectionStartPoint.X);
            selectionRectangle.Shape.Height = Math.Abs(pos.Y - selectionStartPoint.Y);
            selectionRectangle.Shape.Margin = new Thickness(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), 0, 0);
        }


        private void HandleSelectionEnded(object sender, MouseButtonEventArgs e)
        {
            if (!CanvasSelectable)
                return; //Canvas is marked as not selectable, abort

            Point pos = e.GetPosition(c);
            var raycastResult = GetSelectedShapes(c, new RectangleGeometry(new Rect(Math.Min(pos.X, selectionStartPoint.X), Math.Min(pos.Y, selectionStartPoint.Y), selectionRectangle.Shape.Width, selectionRectangle.Shape.Height)));
            foreach (TikzShape s in raycastResult)
            {

                Debug.WriteLine("In");

                if (SelectedShapes == null)
                {
                    Debug.WriteLine("Null");
                }
                else
                {
                    Debug.WriteLine("NOT Null");
                }

                Debug.WriteLine(s);


                SelectedShapes.Add(s);
            }
            c.Children.Remove(selectionRectangle.Shape);
            selectionRectangle.Shape.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Geometry hittest against the shapes on the canvas
        /// </summary>
        /// <param name="element">Element that contains shapes</param>
        /// <param name="geometry">Geometry of hit test (in our case a selection rectangle)</param>
        /// <returns></returns>
        private IList<TikzShape> GetSelectedShapes(UIElement element, Geometry geometry)
        {
            var shapes = new List<TikzShape>();

            VisualTreeHelper.HitTest(element, null,
                result =>
                {
                    if (result.VisualHit is Shape shape)
                    {
                        foreach (TikzShape tikzShape in Shapes)
                        {
                            if (tikzShape.Shape == shape)
                            {
                                shapes.Add(tikzShape);
                                break;
                            }
                        }
                    }

                    return HitTestResultBehavior.Continue;
                },
                new GeometryHitTestParameters(geometry));


            shapes.Remove(selectionRectangle);
            return shapes;
        }

        #endregion

        private void ResetEffect(ICollection<TikzShape> shapes)
        {
            foreach (TikzShape s in shapes)
            {
                s.Shape.Effect = null;
            }
        }
    }
}
