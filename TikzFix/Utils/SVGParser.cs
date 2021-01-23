using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

using TikzFix.Model.Styling;
using TikzFix.Model.TikzShapes;

namespace TikzFix.Utils
{
    internal static class SVGParser
    {
        private static readonly Regex[] svgRegexes = new Regex[]{
          new Regex(@"<path.*"), //path line
         new Regex(@" d='(.*?)'"), //data 
        };

        public static List<TikzShape> Parse(string data)
        {
            MatchCollection matches = svgRegexes[0].Matches(data);
            List<TikzShape> result = new List<TikzShape>();

            foreach (Match m in matches)
            {
                string line = m.Value;
                string d = svgRegexes[1].Match(line).Groups[1].Value;
                System.Windows.Shapes.Path p = new System.Windows.Shapes.Path
                {
                    Data = Geometry.Parse(d)
                };
                //Parse style

                string rawStrokeColor = GetSVGProperty(line, "stroke");
                string rawFillColor = GetSVGProperty(line, "fill");
                string rawStrokeWidth = GetSVGProperty(line, "stroke-width");
                string rawStrokeDasharray = GetSVGProperty(line, "stroke-dasharray");
                string rawStrokeOpacity = GetSVGProperty(line, "stroke-opacity");
                string rawFillOpacity = GetSVGProperty(line, "fill-opacity");

                Debug.WriteLine(rawStrokeColor);

                double strokeOpacity = double.TryParse(rawStrokeOpacity, out double o) ? o : 1.0;
                double fillOpacity = double.TryParse(rawFillOpacity, out double of) ? of : 1.0;
                TikzStyle s = new TikzStyle(parseSVGColor(rawStrokeColor, strokeOpacity),
                    parseSVGColor(rawFillColor, fillOpacity), LineEnding.NONE, praseSVGStrokeWidth(rawStrokeWidth), parseSVGDashArray(rawStrokeDasharray));

                Debug.WriteLine(s);
                p.SetStyle(s);
                result.Add(new TikzPath(p, s, line));
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="property"></param>
        /// <returns>property data or empty if property does not exist</returns>
        public static string GetSVGProperty(string data, string property)
        {
            try
            {
                return Regex.Match(data, ' ' + property + "='(.*?)'").Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }



        //Parses SVG hex color
        public static Color parseSVGColor(string rawColor, double opacity = 1.0)
        {
            if (rawColor == "none" || string.IsNullOrEmpty(rawColor))
            {
                return Colors.Transparent;
            }

            Color c = (Color)ColorConverter.ConvertFromString(rawColor);
            return Color.FromArgb((byte)Math.Round(opacity * 255), c.R, c.G, c.B);
        }

        public static LineWidth praseSVGStrokeWidth(string rawWidth)
        {
            if (string.IsNullOrEmpty(rawWidth))
            {
                return LineWidth.THIN;
            }

            double val = double.Parse(rawWidth);
            if (val <= .19926)
            {
                return LineWidth.VERY_THIN;
            }

            if (val <= .3986)
            {
                return LineWidth.THIN;
            }

            if (val <= .79702)
            {
                return LineWidth.THICK;
            }

            return LineWidth.ULTRA_THICK;
        }

        public static LineType parseSVGDashArray(string rawDashArray)
        {
            //Dotted -> first value different then second value
            //Dashed -> first and second values are the same
            //Dashed-Dotted -> 4 or more values
            if (string.IsNullOrEmpty(rawDashArray))
            {
                return LineType.SOLID;
            }

            double[] values = rawDashArray.Split(',').Select(x => double.Parse(x)).ToArray();
            if (values.Length > 2)
            {
                return LineType.DASHDOTTED;
            }

            if (values[0] != values[1])
            {
                return LineType.DOTTED;
            }

            if (values[0] == values[1])
            {
                return LineType.DASHED;
            }

            return LineType.SOLID;
        }



    }
}
