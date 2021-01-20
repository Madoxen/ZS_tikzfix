using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using TikzFix.Model.TikzShapes;
using TikzFix.Utils;
using TikzFix.Model.Styling;
using System.Windows.Ink;

namespace TikzFix.Model.FormatLoader
{
    class TikzFormatLoader : IFormatLoader
    {

        const string BEGIN_ENVIROMENT_TOKEN = "\\begin{tikzpicture}";
        const string END_ENVIROMENT_TOKEN = "\\end{tikzpicture}";
        const string bins = @"texmfs\install\miktex\bin\x64";
        Regex[] svgRegexes = new Regex[]{
          new Regex(@"<path.*"), //path line
         new Regex(@"d='(.*?)'"), //data 
    };


        public ICollection<TikzShape> ConvertMany(string data)
        {
            try
            {
                //1. find tizkpicture enviroment
                string env = FindEnviroment(data);

                //2. Create document with standalone package
                string doc = @"
\documentclass[tikz]{standalone}
\begin{document}"
                + env +
                @"\end{document}";


                File.WriteAllText("job.txt", doc);

                //3. Call latex for conversion to DVI
                Process latex = new Process();
                latex.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                latex.StartInfo.FileName = "latex";
                latex.StartInfo.Arguments = "-shell-escape -quiet job.txt";
                latex.StartInfo.UseShellExecute = true;
                latex.StartInfo.CreateNoWindow = true;
                latex.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                latex.Start();
                bool latexSuccess = latex.WaitForExit(10000); //wait 10s before terminating 
                if (latexSuccess == false)
                    throw new Exception("Latex conversion failed");

                //4. Call dvisvgm to convert dvi to svg that we can easily parse
                Process dvi2svg = new Process();
                dvi2svg.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dvi2svg.StartInfo.FileName = "dvisvgm";
                dvi2svg.StartInfo.Arguments = "-n -v 0 job.dvi";
                dvi2svg.StartInfo.CreateNoWindow = true;
                dvi2svg.StartInfo.UseShellExecute = true;
                dvi2svg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                dvi2svg.Start();
                bool dviSuccess = dvi2svg.WaitForExit(10000); //wait 10s before terminating 
                if (dviSuccess == false)
                    throw new Exception("DVI conversion failed");

                string svg = File.ReadAllText("job.svg");
         


                MatchCollection matches = svgRegexes[0].Matches(svg);
                List<TikzShape> result = new List<TikzShape>();

                foreach (Match m in matches)
                {
                    string line = m.Value;
                    string d = svgRegexes[1].Match(line).Groups[1].Value;
                    System.Windows.Shapes.Path p = new System.Windows.Shapes.Path();
                    p.Data = Geometry.Parse(d);
                    //Parse style


                    string rawStrokeColor = GetSVGProperty(line, "stroke");
                    string rawFillColor = GetSVGProperty(line, "fill");
                    string rawStrokeWidth = GetSVGProperty(line, "stroke-width");
                    string rawStrokeDasharray = GetSVGProperty(line, "stroke-dasharray");
                    string rawStrokeMiterlimit = GetSVGProperty(line, "stroke-miterlimit");
                    string rawStrokeOpacity = GetSVGProperty(line, "stroke-opacity");
                    string rawFillOpacity = GetSVGProperty(line, "fill-opacity");

                    
                    
                    double strokeOpacity = double.TryParse(rawStrokeOpacity, out double o) ? o : 1.0;
                    double fillOpacity = double.TryParse(rawFillOpacity, out double of) ? of : 1.0;
                    TikzStyle s = new TikzStyle(parseSVGColor(rawStrokeColor, strokeOpacity),
                        parseSVGColor(rawFillColor, fillOpacity), LineEnding.NONE, praseSVGStrokeWidth(rawStrokeWidth), parseSVGDashArray(rawStrokeDasharray));
                    p.SetStyle(s);

                    Debug.WriteLine(rawStrokeColor);
                    result.Add(new TikzPath(p, s, d));
                }
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new List<TikzShape> { };
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="property"></param>
        /// <returns>property data or empty if property does not exist</returns>
        private string GetSVGProperty(string data, string property)
        {
            try
            {
                return Regex.Match(data, property + "='(.*?)'").Groups[1].Value;
            }
            catch
            {
                return "";
            }
        }



        //Parses SVG hex color
        private Color parseSVGColor(string rawColor, double opacity = 1.0)
        {
            if (rawColor == "none" || String.IsNullOrEmpty(rawColor))
                return Colors.Transparent;
            Color c = (Color)ColorConverter.ConvertFromString(rawColor);
            Debug.WriteLine(opacity * 255);
            return Color.FromArgb((byte)Math.Round(opacity * 255), c.R, c.G, c.B);
        }

        private LineWidth praseSVGStrokeWidth(string rawWidth)
        {
            if (string.IsNullOrEmpty(rawWidth))
                return LineWidth.THIN;

            double val = Double.Parse(rawWidth);
            if (val <= .19926)
                return LineWidth.VERY_THIN;
            if (val <= .3986)
                return LineWidth.THIN;
            if (val <= .79702)
                return LineWidth.THICK;
            return LineWidth.ULTRA_THICK;
        }

        private LineType parseSVGDashArray(string rawDashArray)
        {
            //Dotted -> first value different then second value
            //Dashed -> first and second values are the same
            //Dashed-Dotted -> 4 or more values
            if (string.IsNullOrEmpty(rawDashArray))
                return LineType.SOLID;

            double[] values = rawDashArray.Split(',').Select(x => Double.Parse(x)).ToArray();
            if (values.Length > 2)
                return LineType.DASHDOTTED;

            if (values[0] != values[1])
                return LineType.DOTTED;

            if (values[0] == values[1])
                return LineType.DASHED;

            return LineType.SOLID;

        }

        //Returns enviroment (text block) specified by BEGIN_ENVIROMENT_TOKEN and END_ENVIROMENT_TOKEN
        public string FindEnviroment(string data)
        {
            //Finding first enviroment can be done via regex
            //Finding end of enviroment is harder because enviroments can be embedded within each other
            int env_start = data.IndexOf(BEGIN_ENVIROMENT_TOKEN);
            int token_count = 1;
            int last_token = env_start + BEGIN_ENVIROMENT_TOKEN.Length;

            if (env_start < 0)
                return "";

            while (token_count > 0)
            {
                int curr = data.IndexOf(BEGIN_ENVIROMENT_TOKEN, last_token); //find next begin env
                int end_curr = data.IndexOf(END_ENVIROMENT_TOKEN, last_token);

                //If we found begin token earlier than end token
                if (curr < end_curr && curr > -1)
                {
                    token_count++;
                }
                //if ending token found
                if (end_curr > -1)
                {
                    token_count--;
                    last_token = end_curr + END_ENVIROMENT_TOKEN.Length;
                }
                else if (token_count > 0)
                {
                    throw new Exception("No closing tag detected, but token_count is bigger than 0");
                }
            }

            return data[env_start..(last_token)];
        }


    }
}
