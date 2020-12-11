using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TikzFix.VM
{
    class MainVM : BaseVM
    {
        private ObservableCollection<Shape> shapes;
        public ObservableCollection<Shape> Shapes
        {
            get { return shapes; }
            private set { SetProperty(ref shapes, value); }
        }


    }
}
