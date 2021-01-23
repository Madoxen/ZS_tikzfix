using Microsoft.Win32;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using TikzFix.Model.FormatGenerator;
using TikzFix.Model.FormatLoader;
using TikzFix.Model.TikzShapes;

namespace TikzFix.VM
{
    /// <summary>
    /// Contains commands that operate on files
    /// </summary>
    internal class FileVM : BaseVM
    {

        /// <summary>
        /// Collection of shapes to load to and save from
        /// </summary>
        /// <param name="shapes"></param>
        public FileVM()
        {
            SaveCommand = new RelayCommand<int>(OpenSave);
            LoadCommand = new RelayCommand(OpenLoad);
        }

        private readonly List<IFormatGenerator> formatGenerators = new List<IFormatGenerator>()
        {
            new JsonFormatGenerator(),
            new TikzFormatGenerator(),
        };

        private int currentFormatGeneratorIndex = 0;
        public IFormatGenerator CurrentFormatGenerator => currentFormatGeneratorIndex >= 0 ? formatGenerators[currentFormatGeneratorIndex] : null;


        private readonly List<IFormatLoader> formatLoaders = new List<IFormatLoader>()
        {
            new JsonFormatLoader(),
            new TikzFormatLoader()
        };

        private int currentFormatLoaderIndex = 0;
        public IFormatLoader CurrentFormatLoader => currentFormatLoaderIndex >= 0 ? formatLoaders[currentFormatLoaderIndex] : null;


        private ICollection<TikzShape> shapes;
        public ICollection<TikzShape> Shapes
        {
            get => shapes;
            set => SetProperty(ref shapes, value);
        }

        public RelayCommand<int> SaveCommand
        {
            get;
        }

        public RelayCommand LoadCommand
        {
            get;
        }


        private void OpenSave(int formatGeneratorIndex)
        {
            string fileExtFilter = formatGeneratorIndex switch
            {
                1 => "Tikzfix drawing file | *.tikzfix",
                0 => "JSON file | *.json"
            };
            currentFormatGeneratorIndex = formatGeneratorIndex;
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = fileExtFilter
            }; //this will make that method untestable
            bool? result = dialog.ShowDialog(); //this will block the thread until dialog is closed 

            // Get the selected file name
            if (result == true)
            {
                Save(dialog.FileName);
            }
        }

        private void OpenLoad()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = "Supported formats | *.json; *.tex"
            }; //this will make that method untestable
            bool? result = dialog.ShowDialog(); //this will block the thread until dialog is closed 

            // Get the selected file name
            if (result == true)
            {
                Load(dialog.FileName);
            }
        }

        private void Save(string fileName)
        {
            string dataString = CurrentFormatGenerator.ConvertMany(Shapes);
            File.WriteAllText(fileName, dataString);
        }

        private void Load(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            Debug.WriteLine(ext);
            if (ext == null)
            {
                currentFormatLoaderIndex = 0;
            }
            else
            {
                switch (ext)
                {
                    case ".json":
                        currentFormatLoaderIndex = 0;
                        break;
                    case ".tex":
                        currentFormatLoaderIndex = 1;
                        break;
                }
            }

            string dataString = File.ReadAllText(fileName);
            shapes.Clear();
            foreach (TikzShape s in CurrentFormatLoader.ConvertMany(dataString))
            {
                shapes.Add(s);
            }
        }
    }

}
