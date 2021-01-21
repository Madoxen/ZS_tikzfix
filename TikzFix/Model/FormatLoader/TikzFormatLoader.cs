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
    //Pseudo Tikz Loader
    class TikzFormatLoader : IFormatLoader
    {

        public ICollection<TikzShape> ConvertMany(string data)
        {
            try
            {
                File.WriteAllText("job.txt", data);
                //3. Call latex for conversion to DVI
                Process latex = new Process();
                latex.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                latex.StartInfo.FileName = "latex";
                latex.StartInfo.Arguments = "-shell-escape -quiet job.txt";
                latex.StartInfo.UseShellExecute = true;
                latex.StartInfo.CreateNoWindow = true;
                latex.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                latex.Start();
                latex.WaitForExit(100000); //wait 10s before terminating 
                if (latex.ExitCode != 0)
                    throw new Exception("Latex conversion failed");
                
                //4. Call dvisvgm to convert dvi to svg that we can easily parse
                Process dvi2svg = new Process();
                dvi2svg.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dvi2svg.StartInfo.FileName = "dvisvgm";
                dvi2svg.StartInfo.Arguments = "-n -v 0 -o job.svg job.dvi";
                dvi2svg.StartInfo.CreateNoWindow = true;
                dvi2svg.StartInfo.UseShellExecute = true;
                dvi2svg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                dvi2svg.Start();
                dvi2svg.WaitForExit(100000); //wait 10s before terminating 
                if (dvi2svg.ExitCode != 0)
                    throw new Exception("DVI conversion failed");

                string svg = File.ReadAllText("job.svg");

                return SVGParser.Parse(svg);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return new List<TikzShape> { };
            }
        }
    }
}
