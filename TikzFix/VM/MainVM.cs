using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        private ObservableCollection<Shape> shapes = new ObservableCollection<Shape>();
        public ICollection<Shape> Shapes
        {
            get { return shapes; }
           // private set { SetProperty<ICollection<UIElement>>(ref (ICollection<UIElement>)shapes, value); }
        }



        public MainVM()
        {

            // Add a Line Element
            Line myLine = new Line
            {
                Stroke = System.Windows.Media.Brushes.LightSteelBlue,
                X1 = 1,
                X2 = 50,
                Y1 = 1,
                Y2 = 50,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                StrokeThickness = 2
            };
            Shapes.Add(myLine);
            Debug.WriteLine("rE?");
        }




    }
}
