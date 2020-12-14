using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Shapes;

namespace TikzFix.Views
{
    /// <summary>
    /// Interaction logic for CollectionCanvas.xaml
    /// </summary>
    public partial class CollectionCanvas : UserControl
    {
        public CollectionCanvas()
        {
            InitializeComponent();
        }

        public ICollection<Shape> Shapes
        {
            get { return (ICollection<Shape>)GetValue(ShapesProperty); }
            set { SetValue(ShapesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Children.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShapesProperty =
            DependencyProperty.Register("Shapes", typeof(ICollection<Shape>), typeof(CollectionCanvas), new PropertyMetadata(null, OnShapesPropertyChanged));




        public int MyProperty
        {
            get { return (int)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(int), typeof(CollectionCanvas), new PropertyMetadata(0));



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
    }



}
