using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;

using TikzFix.Model.FormatGenerator;
using TikzFix.Model.FormatLoader;
using TikzFix.Model.TikzShapes;
using TikzFix.Model.Tool;

namespace TikzFix.VM
{
    /// <summary>
    /// Contains commands that operate on files
    /// </summary>
    class FileVM : BaseVM
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
        public IFormatGenerator currentFormatGenerator => currentFormatGeneratorIndex >= 0 ? formatGenerators[currentFormatGeneratorIndex] : null;


        private readonly List<IFormatLoader> formatLoaders = new List<IFormatLoader>()
        {
            new JsonFormatLoader(),
        };

        private int currentFormatLoaderIndex = 0;
        public IFormatLoader currentFormatLoader => currentFormatLoaderIndex >= 0 ? formatLoaders[currentFormatLoaderIndex] : null;


        private ICollection<TikzShape> shapes;
        public ICollection<TikzShape> Shapes
        {
            get
            {
                return shapes;
            }
            set
            {
                SetProperty(ref shapes, value);
            }
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
            currentFormatGeneratorIndex = formatGeneratorIndex;
            SaveFileDialog dialog = new SaveFileDialog(); //this will make that method untestable
            bool? result = dialog.ShowDialog(); //this will block the thread until dialog is closed 

            // Get the selected file name
            if (result == true)
            {
                Save(dialog.FileName);
            }
        }

        private void OpenLoad()
        {
            OpenFileDialog dialog = new OpenFileDialog(); //this will make that method untestable
            bool? result = dialog.ShowDialog(); //this will block the thread until dialog is closed 

            // Get the selected file name
            if (result == true)
            {
                Load(dialog.FileName);
            }
        }

        private void Save(string fileName)
        {
            string dataString = currentFormatGenerator.ConvertMany(Shapes);
            File.WriteAllText(fileName, dataString);

            //List<LocalShapeData> localShapesData = new List<LocalShapeData>(shapes.Count);
            //foreach (TikzShape s in shapes)
            //{
            //    Debug.WriteLine(s.GenerateTikz());
            //    localShapesData.Add(s.GenerateLocalData());
            //}

            //Debug.WriteLine(string.Join(", ", localShapesData));
            //var x = JsonSerializer.Serialize(localShapesData);

            //File.WriteAllText(fileName, x);
        }

        private void Load(string fileName)
        {
            string dataString = File.ReadAllText(fileName);
            shapes.Clear();
            foreach (TikzShape s in currentFormatLoader.ConvertMany(dataString))
            {
                shapes.Add(s);
            }
        }
    }
}
