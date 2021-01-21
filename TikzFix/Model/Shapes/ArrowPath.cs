using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

using TikzFix.Utils;

namespace TikzFix.Model.Shapes
{
    internal class ArrowPath : Shape
    {
        public Geometry Data
        {
            get => (Geometry)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        // Using a DependencyProperty as the backing store for Data.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(Geometry), typeof(ArrowPath), new FrameworkPropertyMetadata(null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        //Angle in radians
        public double ArrowAngle
        {
            get => (double)GetValue(ArrowAngleProperty);
            set => SetValue(ArrowAngleProperty, value);
        }

        // Using a DependencyProperty as the backing store for ArrowAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowAngleProperty =
            DependencyProperty.Register("ArrowAngle", typeof(double), typeof(ArrowPath), new FrameworkPropertyMetadata(0.7853981625,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));




        public double ArrowLength
        {
            get => (double)GetValue(ArrowLengthProperty);
            set => SetValue(ArrowLengthProperty, value);
        }

        // Using a DependencyProperty as the backing store for ArrowLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArrowLengthProperty =
            DependencyProperty.Register("ArrowLength", typeof(double), typeof(ArrowPath), new FrameworkPropertyMetadata(10.0,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));





        public bool HasEndArrow
        {
            get => (bool)GetValue(HasEndArrowProperty);
            set => SetValue(HasEndArrowProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasEndArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasEndArrowProperty =
            DependencyProperty.Register("HasEndArrow", typeof(bool), typeof(ArrowPath), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public bool HasStartArrow
        {
            get => (bool)GetValue(HasStartArrowProperty);
            set => SetValue(HasStartArrowProperty, value);
        }

        // Using a DependencyProperty as the backing store for HasStartArrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HasStartArrowProperty =
            DependencyProperty.Register("HasStartArrow", typeof(bool), typeof(ArrowPath), new FrameworkPropertyMetadata(true,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        protected override Geometry DefiningGeometry
        {
            get
            {
                PathGeometry g = new PathGeometry();
                g.AddGeometry(Data);

                //This returns aproximation of our curve
                PathGeometry p = Data.GetFlattenedPathGeometry();

                Debug.WriteLine(p.Figures.First().Segments.First().GetType());
                Debug.WriteLine(p.Figures.Last().Segments.Last().GetType());


                //FIXME: Probably caused by bug in BezierTool
                //see:  fixme (line 34 BezierTool.cs)
                if (p.Figures.First().Segments.First() is not PolyLineSegment)
                {
                    return g;
                }

                if (p.Figures.Last().Segments.Last() is not PolyLineSegment)
                {
                    return g;
                }

                //Start vector
                PolyLineSegment firstSeg = (PolyLineSegment)p.Figures.First().Segments.First();
                Point startingPoint = p.Figures.First().StartPoint; //The starting point is being kept out of the Point collection
                Vector start = new Vector(firstSeg.Points.First().X - startingPoint.X, firstSeg.Points.First().Y - startingPoint.Y); //Calculate direction vector
                start.Normalize();


                PolyLineSegment lastSeg = (PolyLineSegment)p.Figures.Last().Segments.Last();
                Point endingPoint = lastSeg.Points.Last();
                Vector end = new Vector(lastSeg.Points[^2].X - endingPoint.X, lastSeg.Points[^2].Y - endingPoint.Y);
                end.Normalize();

                //Add arrows at both ends
                if (StrokeStartLineCap == PenLineCap.Triangle)
                {
                    Geometry a11 = new LineGeometry(startingPoint, (VectorUtils.Rotate(start, ArrowAngle) * ArrowLength) + startingPoint);
                    Geometry a12 = new LineGeometry(startingPoint, (VectorUtils.Rotate(start, -ArrowAngle) * ArrowLength) + startingPoint);
                    g.AddGeometry(a11);
                    g.AddGeometry(a12);
                }

                if (StrokeEndLineCap == PenLineCap.Triangle)
                {
                    Geometry a21 = new LineGeometry(endingPoint, (VectorUtils.Rotate(end, ArrowAngle) * ArrowLength) + endingPoint);
                    Geometry a22 = new LineGeometry(endingPoint, (VectorUtils.Rotate(end, -ArrowAngle) * ArrowLength) + endingPoint);
                    g.AddGeometry(a21);
                    g.AddGeometry(a22);
                }

                return g;
            }
        }
    }
}
