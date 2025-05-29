using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;
using KinematicsVisualizer.Services;
using ScottPlot.WPF;
using System.Windows.Input;

namespace KinematicsVisualizer.ViewModels
{
    public class ParabolicThrowViewModel : BaseViewModel
    {
        private readonly GraphType tipoGrafica;

        public ParabolicThrowViewModel(GraphType tipoGrafica)
        {
            this.tipoGrafica = tipoGrafica;
            plotService = new PlotService();
            GraficarCommand = new RelayCommand(_ => Graficar(), _ => VelocidadInicial > 0 && Aceleracion > 0 && Angulo > 0 && Angulo < 90);
        }

        private double velocidadInicial;
        public double VelocidadInicial
        {
            get => velocidadInicial;
            set { velocidadInicial = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double angulo;
        public double Angulo
        {
            get => angulo;
            set { angulo = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private double aceleracion = 9.8;
        public double Aceleracion
        {
            get => aceleracion;
            set { aceleracion = value; OnPropertyChanged(); CommandManager.InvalidateRequerySuggested(); }
        }

        private ComponentAxis componenteSeleccionado = ComponentAxis.Y;
        public ComponentAxis ComponenteSeleccionado
        {
            get => componenteSeleccionado;
            set { componenteSeleccionado = value; OnPropertyChanged(); }
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
            double rad = Angulo * Math.PI / 180.0;
            double v0x = VelocidadInicial * Math.Cos(rad);
            double v0y = VelocidadInicial * Math.Sin(rad);
            double tiempoVuelo = 2 * v0y / Aceleracion;
            double t;

            switch (tipoGrafica)
            {
                case GraphType.XY:
                    t = 0;
                    while (t < tiempoVuelo)
                    {
                        double x = v0x * t;
                        double y = v0y * t - 0.5 * Aceleracion * t * t;
                        if (y < 0) y = 0;
                        datos.Add((x, y));
                        t += 0.05;
                    }
                    datos.Add((v0x * tiempoVuelo, 0));

                    plotService.PlotXY(PlotControl, datos, "Tiro Parabólico: Trayectoria (X vs Y)", "x (m)", "y (m)");
                    EcuacionVisible = "y(x) = x·tan(θ) - (g·x²)/(2·v₀²·cos²θ)";
                    break;

                case GraphType.PosicionVsTiempo:
                    for (t = 0; t <= tiempoVuelo; t += 0.05)
                    {
                        double pos = componenteSeleccionado == ComponentAxis.X
                            ? v0x * t
                            : v0y * t - 0.5 * Aceleracion * t * t;
                        datos.Add((t, pos));
                    }

                    string etiquetaPos = componenteSeleccionado == ComponentAxis.X ? "x(t)" : "y(t)";
                    string eqPos = componenteSeleccionado == ComponentAxis.X
                        ? "x(t) = v₀·cos(θ)·t"
                        : "y(t) = v₀·sin(θ)·t - ½·g·t²";

                    plotService.PlotXY(PlotControl, datos, $"Tiro Parabólico: {etiquetaPos} vs Tiempo", "t (s)", etiquetaPos);
                    EcuacionVisible = eqPos;
                    break;

                case GraphType.VelocidadVsTiempo:
                    for (t = 0; t <= tiempoVuelo; t += 0.05)
                    {
                        double v = componenteSeleccionado == ComponentAxis.X
                            ? v0x
                            : v0y - Aceleracion * t;
                        datos.Add((t, v));
                    }

                    string etiquetaVel = componenteSeleccionado == ComponentAxis.X ? "vₓ(t)" : "vᵧ(t)";
                    string eqVel = componenteSeleccionado == ComponentAxis.X
                        ? "vₓ(t) = v₀·cos(θ)"
                        : "vᵧ(t) = v₀·sin(θ) - g·t";

                    plotService.PlotXY(PlotControl, datos, $"Tiro Parabólico: {etiquetaVel} vs Tiempo", "t (s)", etiquetaVel);
                    EcuacionVisible = eqVel;
                    break;

                case GraphType.AceleracionVsTiempo:
                    for (t = 0; t <= tiempoVuelo; t += 0.05)
                    {
                        double a = componenteSeleccionado == ComponentAxis.X ? 0 : -Aceleracion;
                        datos.Add((t, a));
                    }

                    string etiquetaAcel = componenteSeleccionado == ComponentAxis.X ? "aₓ(t)" : "aᵧ(t)";
                    string eqAcel = componenteSeleccionado == ComponentAxis.X
                        ? "aₓ(t) = 0"
                        : "aᵧ(t) = -g";

                    plotService.PlotXY(PlotControl, datos, $"Tiro Parabólico: {etiquetaAcel} vs Tiempo", "t (s)", etiquetaAcel);
                    EcuacionVisible = eqAcel;
                    break;
            }
        }
    }
}