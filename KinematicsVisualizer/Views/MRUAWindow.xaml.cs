using System.Windows;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.ViewModels;

namespace KinematicsVisualizer.Views
{
    public partial class MRUAWindow : Window
    {
        public MRUAWindow(GraphType tipoGrafica)
        {
            InitializeComponent();
            var vm = new MRUAViewModel(tipoGrafica);
            this.DataContext = vm;
            vm.PlotControl = this.plot;
        }

        private void VolverAlMenu(object sender, RoutedEventArgs e)
        {
            var menu = new MainWindow();
            Application.Current.MainWindow = menu;
            menu.Show();
            this.Close();
        }
    }
}