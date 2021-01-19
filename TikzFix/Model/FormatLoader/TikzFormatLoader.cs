using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TikzFix.Model.TikzShapes;

namespace TikzFix.Model.FormatLoader
{
    class TikzFormatLoader : IFormatLoader
    {

        const string BEGIN_ENVIROMENT_TOKEN = "\\begin{tikzpicture}";
        const string END_ENVIROMENT_TOKEN = "\\end{tikzpicture}";
        const string bins = @"texmfs\install\miktex\bin\x64";
        


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

                //3. Call latex for conversion
                Process latex = new Process();
                latex.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                latex.StartInfo.FileName = "latex";
                latex.StartInfo.Arguments = "-shell-escape -quiet job.txt";
                latex.StartInfo.UseShellExecute = true;
                latex.Start();
                bool latexSuccess = latex.WaitForExit(10000); //wait 10s before terminating 
                if (latexSuccess == false)
                    throw new Exception("Latex conversion failed");

                //4. Call dvisvgm to convert dvi to svg that we can easily parse

                Process dvi2svg = new Process();
                dvi2svg.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dvi2svg.StartInfo.FileName = "dvisvgm";
                dvi2svg.StartInfo.Arguments = "-n -v 0 job.dvi";
                dvi2svg.StartInfo.UseShellExecute = true;
                dvi2svg.Start();
                bool dviSuccess = dvi2svg.WaitForExit(10000); //wait 10s before terminating 
                if (dviSuccess == false)
                    throw new Exception("DVI conversion failed");

                string svg = File.ReadAllText("job.svg");
                Debug.WriteLine(svg);

                //5. Parse SVG to tikzShapes
                return new List<TikzShape>{ }; 
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
                return new List<TikzShape>{ }; 
            }
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
