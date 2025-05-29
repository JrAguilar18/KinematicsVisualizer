using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Services;
using ScottPlot;
using ScottPlot.WPF;
using System.Collections.Generic;
using System.Windows.Input;

namespace KinematicsVisualizer.ViewModels
{
    public class MRUViewModel : BaseViewModel
    {
        private readonly GraphType tipoGrafica;

        public MRUViewModel(GraphType tipoGrafica)
        {
            this.tipoGrafica = tipoGrafica;
            plotService = new PlotService();
            GraficarCommand = new RelayCommand(_ => Graficar(), _ => Velocidad > 0 && Tiempo > 0);
        }

        private double velocidad;
        public double Velocidad
        {
            get => velocidad;
            set { velocidad = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double tiempo;
        public double Tiempo
        {
            get => tiempo;
            set { tiempo = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
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

            switch (tipoGrafica)
            {
                case GraphType.PosicionVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, Velocidad * t));

                    plotService.PlotXY(PlotControl, datos, "MRU: Posición vs Tiempo", "Tiempo (s)", "Posición (m)");
                    EcuacionVisible = $"x(t) = v·t";
                    break;

                case GraphType.VelocidadVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, Velocidad));

                    plotService.PlotXY(PlotControl, datos, "MRU: Velocidad vs Tiempo", "Tiempo (s)", "Velocidad (m/s)");
                    EcuacionVisible = $"v(t) = v (constante)";
                    break;

                case GraphType.AceleracionVsTiempo:
                    for (double t = 0; t <= Tiempo; t += 0.1)
                        datos.Add((t, 0));

                    plotService.PlotXY(PlotControl, datos, "MRU: Aceleración vs Tiempo", "Tiempo (s)", "Aceleración (m/s²)");
                    EcuacionVisible = "a(t) = 0";
                    break;
            }
        }
    }
}