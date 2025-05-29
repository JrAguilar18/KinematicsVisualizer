using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Services;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.Generic;
using System.Windows.Input;

namespace KinematicsVisualizer.ViewModels
{
    public class VerticalThrowViewModel : BaseViewModel
    {
        private readonly GraphType tipoGrafica;

        public VerticalThrowViewModel(GraphType tipoGrafica)
        {
            this.tipoGrafica = tipoGrafica;
            plotService = new PlotService();
            GraficarCommand = new RelayCommand(_ => Graficar(), _ => VelocidadInicial > 0 && Aceleracion > 0);
        }

        private double velocidadInicial;
        public double VelocidadInicial
        {
            get => velocidadInicial;
            set { velocidadInicial = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double aceleracion = 9.8;
        public double Aceleracion
        {
            get => aceleracion;
            set { aceleracion = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        public ICommand GraficarCommand { get; }
        public WpfPlot PlotControl { get; set; }

        private readonly PlotService plotService;

        private string ecuacionVisible;
        public string EcuacionVisible
        {
            get => ecuacionVisible;
            set { ecuacionVisible = value; OnPropertyChanged(); }
        }

        private void Graficar()
        {
            var datos = new List<(double x, double y)>();

            // t_total = 2 * v₀ / g (subida + bajada)
            double tiempoTotal = (2 * VelocidadInicial) / Aceleracion;

            switch (tipoGrafica)
            {
                case GraphType.PosicionVsTiempo:
                    for (double t = 0; t <= tiempoTotal; t += 0.1)
                        datos.Add((t, VelocidadInicial * t - 0.5 * Aceleracion * t * t));

                    plotService.PlotXY(PlotControl, datos, "Tiro Vertical: Altura vs Tiempo", "Tiempo (s)", "Altura (m)");
                    EcuacionVisible = $"y(t) = v₀·t - ½·g·t²";
                    break;

                case GraphType.VelocidadVsTiempo:
                    for (double t = 0; t <= tiempoTotal; t += 0.1)
                        datos.Add((t, VelocidadInicial - Aceleracion * t));

                    plotService.PlotXY(PlotControl, datos, "Tiro Vertical: Velocidad vs Tiempo", "Tiempo (s)", "Velocidad (m/s)");
                    EcuacionVisible = $"v(t) = v₀ - g·t";
                    break;

                case GraphType.AceleracionVsTiempo:
                    for (double t = 0; t <= tiempoTotal; t += 0.1)
                        datos.Add((t, -Aceleracion));

                    plotService.PlotXY(PlotControl, datos, "Tiro Vertical: Aceleración vs Tiempo", "Tiempo (s)", "Aceleración (m/s²)");
                    EcuacionVisible = $"a(t) = -g (constante)";
                    break;
            }
        }
    }
}