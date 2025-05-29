using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Services;
using ScottPlot.WPF;
using System.Windows.Input;

namespace KinematicsVisualizer.ViewModels
{
    public class MRUAViewModel : BaseViewModel
    {
        private readonly GraphType tipoGrafica;
        private readonly PlotService plotService;

        public MRUAViewModel(GraphType tipoGrafica)
        {
            this.tipoGrafica = tipoGrafica;
            plotService = new PlotService();
            GraficarCommand = new RelayCommand(_ => Graficar(), _ => Tiempo > 0);
        }

        private double velocidadInicial;
        public double VelocidadInicial
        {
            get => velocidadInicial;
            set { velocidadInicial = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double aceleracion;
        public double Aceleracion
        {
            get => aceleracion;
            set { aceleracion = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double tiempo;
        public double Tiempo
        {
            get => tiempo;
            set { tiempo = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public ICommand GraficarCommand { get; }
        public WpfPlot PlotControl { get; set; }

        private string ecuacionVisible;
        public string EcuacionVisible
        {
            get => ecuacionVisible;
            set { ecuacionVisible = value; OnPropertyChanged(); }
        }

        private void Graficar()
        {
            var datos = new List<(double x, double y)>();

            switch (tipoGrafica)
            {
                case GraphType.PosicionVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, VelocidadInicial * t + 0.5 * Aceleracion * t * t));
                    plotService.PlotXY(PlotControl, datos, "MRUA: Posición vs Tiempo", "Tiempo (s)", "Posición (m)");
                    EcuacionVisible = $"x(t) = v₀·t + ½·a·t²";
                    break;

                case GraphType.VelocidadVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, VelocidadInicial + Aceleracion * t));
                    plotService.PlotXY(PlotControl, datos, "MRUA: Velocidad vs Tiempo", "Tiempo (s)", "Velocidad (m/s)");
                    EcuacionVisible = $"v(t) = v₀ + a·t";
                    break;

                case GraphType.AceleracionVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, Aceleracion));
                    plotService.PlotXY(PlotControl, datos, "MRUA: Aceleración vs Tiempo", "Tiempo (s)", "Aceleración (m/s²)");
                    EcuacionVisible = $"a(t) = a (constante)";
                    break;
            }
        }
    }
}