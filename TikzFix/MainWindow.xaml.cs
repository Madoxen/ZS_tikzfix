using System.Windows;

namespace TikzFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            //TODO: search for a way to avoid something like that
            FileVM.Shapes = MainVM.Shapes;
            StyleVM.NewStyle += MainVM.ApplyStyle;

        }
    }
}
