namespace KinematicsVisualizer.Models
{
    public class MRUAModel
    {
        public double VelocidadInicial { get; set; }
        public double Aceleracion { get; set; }
        public double Tiempo { get; set; }

        public List<(double Tiempo, double Posicion)> CalcularPosicion()
        {
            var datos = new List<(double, double)>();
            for (double t = 0; t <= Tiempo; t += 0.1)
            {
                double x = VelocidadInicial * t + 0.5 * Aceleracion * t * t;
                datos.Add((t, x));
            }
            return datos;
        }
    }
}
