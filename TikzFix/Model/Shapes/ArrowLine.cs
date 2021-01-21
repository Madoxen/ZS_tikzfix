using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Utils;

namespace TikzFix.Model.Shapes
{
    internal class ArrowLine : Shape
    {

        public double X1
        {
            get => (double)GetValue(X1Property);
            set => SetValue(X1Property, value);
        }

        // Using a DependencyProperty as the backing store for X1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X1Property =
            DependencyProperty.Register("X1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public double Y1
        {
            get => (double)GetValue(Y1Property);
            set => SetValue(Y1Property, value);
        }

        // Using a DependencyProperty as the backing store for Y1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y1Property =
            DependencyProperty.Register("Y1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public double X2
        {
            get => (double)GetValue(X2Property);
            set => SetValue(X2Property, value);
        }

        // Using a DependencyProperty as the backing store for X2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty X2Property =
            DependencyProperty.Register("X2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public double Y2
        {
            get => (double)GetValue(Y2Property);
            set => SetValue(Y2Property, value);
        }

        // Using a DependencyProperty as the backing store for Y2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Y2Property =
            DependencyProperty.Register("Y2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        //Angle in radians
        public double ArrowAngle
        {
            get => (double)GetValue(ArrowAngleProperty);
            set => SetValue(ArrowAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for ArrowAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowAngleProperty =
            DependencyProperty.Register("ArrowAngle", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.7853981625,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public double ArrowLength
        {
            get => (double)GetValue(ArrowLengthProperty);
            set => SetValue(ArrowLengthProperty, value);
        }

        // Using a DependencyProperty as the backing store for ArrowLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowLengthProperty =
            DependencyProperty.Register("ArrowLength", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(10.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));





        public bool HasEndArrow
        {
            get => (bool)GetValue(HasEndArrowProperty);
            set => SetValue(HasEndArrowProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasEndArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasEndArrowProperty =
            DependencyProperty.Register("HasEndArrow", typeof(bool), typeof(ArrowLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public bool HasStartArrow
        {
            get => (bool)GetValue(HasStartArrowProperty);
            set => SetValue(HasStartArrowProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasStartArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasStartArrowProperty =
            DependencyProperty.Register("HasStartArrow", typeof(bool), typeof(ArrowLine), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g = new PathGeometry();
                Geometry mainLine = new LineGeometry(new Point(X1, Y1), new Point(X2, Y2));
                g.AddGeometry(mainLine);
                //Calculate direction vector
                Vector dir = new Vector(X1 - X2, Y1 - Y2);
                dir.Normalize();

                //Add arrows at both ends
                if (StrokeStartLineCap == PenLineCap.Triangle)
                {
                    Geometry a11 = new LineGeometry(new Point(X1, Y1), (VectorUtils.Rotate(-dir, ArrowAngle) * ArrowLength) + new Point(X1, Y1));
                    Geometry a12 = new LineGeometry(new Point(X1, Y1), (VectorUtils.Rotate(-dir, -ArrowAngle) * ArrowLength) + new Point(X1, Y1));
                    g.AddGeometry(a11);
                    g.AddGeometry(a12);
                }

                if (StrokeEndLineCap == PenLineCap.Triangle)
                {
                    Geometry a21 = new LineGeometry(new Point(X2, Y2), (VectorUtils.Rotate(dir, ArrowAngle) * ArrowLength) + new Point(X2, Y2));
                    Geometry a22 = new LineGeometry(new Point(X2, Y2), (VectorUtils.Rotate(dir, -ArrowAngle) * ArrowLength) + new Point(X2, Y2));
                    g.AddGeometry(a21);
                    g.AddGeometry(a22);
                }

                return g;
            }
        }


        public ArrowLine()
        {

        }
    }
}
