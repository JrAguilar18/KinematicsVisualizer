using System.Windows;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.ViewModels;

namespace KinematicsVisualizer.Views
{
    public partial class VerticalThrowWindow : Window
    {
        public VerticalThrowWindow() : this(GraphType.PosicionVsTiempo) // valor por defecto
        {
        }
        public VerticalThrowWindow(GraphType tipoGrafica)
        {
            InitializeComponent();
            var vm = new VerticalThrowViewModel(tipoGrafica);
            this.DataContext = vm;
            vm.PlotControl = this.plot;
        }

        private void VolverAlMenu(object sender, RoutedEventArgs e)
        {
            var menu = new MainWindow();
            menu.Show();
            Application.Current.MainWindow = menu;
            this.Close();
        }
    }
}