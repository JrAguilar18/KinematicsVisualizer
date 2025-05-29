using KinematicsVisualizer.Infrastructure;
using KinematicsVisualizer.Models;

namespace KinematicsVisualizer.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        public List<MotionType> Movimientos { get; } = Enum.GetValues(typeof(MotionType)).Cast<MotionType>().ToList();

        public List<GraphType> GraficasDisponibles =>
            MovimientoSeleccionado == MotionType.ParabolicThrow
            ? new() { GraphType.PosicionVsTiempo, GraphType.VelocidadVsTiempo, GraphType.AceleracionVsTiempo, GraphType.XY }
            : new() { GraphType.PosicionVsTiempo, GraphType.VelocidadVsTiempo, GraphType.AceleracionVsTiempo };

        private MotionType movimientoSeleccionado;
        public MotionType MovimientoSeleccionado
        {
            get => movimientoSeleccionado;
            set
            {
                movimientoSeleccionado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(GraficasDisponibles));
            }
        }

        private GraphType graficaSeleccionada;
        public GraphType GraficaSeleccionada
        {
            get => graficaSeleccionada;
            set
            {
                graficaSeleccionada = value;
                OnPropertyChanged();
            }
        }
    }
}
