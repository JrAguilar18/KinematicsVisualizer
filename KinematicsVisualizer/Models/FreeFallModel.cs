namespace KinematicsVisualizer.Models
{
    public class FreeFallModel
    {
        public double AlturaInicial { get; set; }
        public double Aceleracion { get; set; } = 9.8;

        public List<(double Tiempo, double Posicion)> CalcularCaida()
        {
            var datos = new List<(double, double)>();

            // t_max = sqrt(2h / g)
            double tiempoMax = Math.Sqrt((2 * AlturaInicial) / Aceleracion);

            for (double t = 0; t <= tiempoMax; t += 0.1)
            {
                double y = AlturaInicial - 0.5 * Aceleracion * t * t;
                datos.Add((t, y));
            }

            return datos;
        }
    }
}
