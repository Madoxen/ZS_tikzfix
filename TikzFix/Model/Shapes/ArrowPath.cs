using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TikzFix.Model.Shapes
{
    class ArrowPath : Shape
    {
        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(ArrowPath), new PropertyMetadata(null));


        public bool HasEndArrow
        {
            get { return (bool)GetValue(HasEndArrowProperty); }
            set { SetValue(HasEndArrowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasEndArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasEndArrowProperty =
            DependencyProperty.Register("HasEndArrow", typeof(bool), typeof(ArrowPath), new PropertyMetadata(true));

        public bool HasStartArrow
        {
            get { return (bool)GetValue(HasStartArrowProperty); }
            set { SetValue(HasStartArrowProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HasStartArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasStartArrowProperty =
            DependencyProperty.Register("HasStartArrow", typeof(bool), typeof(ArrowPath), new PropertyMetadata(true));

        //Angle in radians
        public double ArrowAngle
        {
            get { return (double)GetValue(ArrowAngleProperty); }
            set { SetValue(ArrowAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowAngleProperty =
            DependencyProperty.Register("ArrowAngle", typeof(double), typeof(ArrowPath), new PropertyMetadata(0.7853981625));

        public double ArrowLength
        {
            get { return (double)GetValue(ArrowLengthProperty); }
            set { SetValue(ArrowLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArrowLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowLengthProperty =
            DependencyProperty.Register("ArrowLength", typeof(double), typeof(ArrowPath), new PropertyMetadata(10.0));

        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g = new PathGeometry();
                g.AddGeometry(Data);
                return g;
            }
        }
    }
}
