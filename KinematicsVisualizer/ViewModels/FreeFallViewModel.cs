using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Services;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.Generic;
using System.Windows.Input;

namespace KinematicsVisualizer.ViewModels
{
    public class FreeFallViewModel : BaseViewModel
    {
        private readonly GraphType tipoGrafica;

        public FreeFallViewModel(GraphType tipoGrafica)
        {
            this.tipoGrafica = tipoGrafica;
            plotService = new PlotService();
            GraficarCommand = new RelayCommand(_ => Graficar(), _ => AlturaInicial > 0 && Aceleracion > 0);
        }

        private double alturaInicial;
        public double AlturaInicial
        {
            get => alturaInicial;
            set { alturaInicial = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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

            // tiempo hasta el suelo: h = ½ g t² → t = sqrt(2h/g)
            double tiempoMax = Math.Sqrt((2 * AlturaInicial) / Aceleracion);

            switch (tipoGrafica)
            {
                case GraphType.PosicionVsTiempo:
                    for (double t = 0; t <= tiempoMax; t += 0.1)
                        datos.Add((t, AlturaInicial - 0.5 * Aceleracion * t * t));

                    plotService.PlotXY(PlotControl, datos, "Caída Libre: Altura vs Tiempo", "Tiempo (s)", "Altura (m)");
                    EcuacionVisible = $"y(t) = h₀ - ½·g·t²";
                    break;

                case GraphType.VelocidadVsTiempo:
                    for (double t = 0; t <= tiempoMax; t += 0.1)
                        datos.Add((t, -Aceleracion * t));

                    plotService.PlotXY(PlotControl, datos, "Caída Libre: Velocidad vs Tiempo", "Tiempo (s)", "Velocidad (m/s)");
                    EcuacionVisible = $"v(t) = -g·t";
                    break;

                case GraphType.AceleracionVsTiempo:
                    for (double t = 0; t <= tiempoMax; t += 0.1)
                        datos.Add((t, -Aceleracion));

                    plotService.PlotXY(PlotControl, datos, "Caída Libre: Aceleración vs Tiempo", "Tiempo (s)", "Aceleración (m/s²)");
                    EcuacionVisible = $"a(t) = -g (constante)";
                    break;
            }
        }
    }
}