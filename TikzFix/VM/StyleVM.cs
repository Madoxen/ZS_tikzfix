using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TikzFix.Model.Styling;

namespace TikzFix.VM
{
    /// <summary>
    /// Provides user-defined style for other objects 
    /// </summary>
    class StyleVM : BaseVM
    {
        // TODO make it observable and add UI, user should be able to change any prop in TikzStyle
        // TODO add line types to canvas/tools. When ITool impl get style it should apply line type e.g. dotted
        public static TikzStyle CurrentStyle
        {
            get; private set;
        }

        private Color strokeColor = Color.FromRgb(0,0,0);
        public Color StrokeColor
        {
            get { return strokeColor; }
            set
            {
                SetProperty(ref strokeColor, value);
                RebuildStyle();
            }
        }

        private Color fillColor = Color.FromArgb(0,0,0,0);
        public Color FillColor
        {
            get { return fillColor; }
            set
            {
                SetProperty(ref fillColor, value);
                RebuildStyle();
            }
        }

        private LineEnding lineEnding = LineEnding.NONE;
        public LineEnding LineEnding
        {
            get { return lineEnding; }
            set
            {
                SetProperty(ref lineEnding, value);
                RebuildStyle();
            }
        }

        private LineWidth lineWidth = LineWidth.THIN;
        public LineWidth LineWidth
        {
            get { return lineWidth; }
            set
            {
                SetProperty(ref lineWidth, value);
                RebuildStyle();
            }
        }


        private LineType lineType = LineType.SOLID;
        public LineType LineType
        {
            get { return lineType; }
            set
            {
                SetProperty(ref lineType, value);
                RebuildStyle();
            }
        }

        public StyleVM()
        {
            RebuildStyle();
        }

        private void RebuildStyle()
        {
            CurrentStyle = new TikzStyle(StrokeColor, FillColor, LineEnding, LineWidth, LineType);
        }
    }
}
