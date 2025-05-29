using System.Windows;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Views;
using KinematicsVisualizer.ViewModels;

namespace KinematicsVisualizer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is not MainMenuViewModel vm)
                return;

            var movimiento = vm.MovimientoSeleccionado;
            var grafica = vm.GraficaSeleccionada;

            Window? ventana = movimiento switch
            {
                MotionType.MRU => new MRUWindow(grafica),
                MotionType.MRUA => new MRUAWindow(grafica),
                MotionType.FreeFall => new FreeFallWindow(grafica),
                MotionType.VerticalThrow => new VerticalThrowWindow(grafica),
                MotionType.ParabolicThrow => new ParabolicThrowWindow(grafica),
                _ => null
            };

            ventana?.Show();

            Application.Current.MainWindow = ventana;
            this.Close(); // Cierra el menú completamente
        }
    }
}