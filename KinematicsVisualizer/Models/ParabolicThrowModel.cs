using System;

namespace KinematicsVisualizer.Models
{
    public class ParabolicThrowModel
    {
        public double VelocidadInicial { get; set; }
        public double AnguloGrados { get; set; }
        public double Aceleracion { get; set; } = 9.8;

        public List<(double X, double Y)> CalcularTrayectoria()
        {
            var datos = new List<(double X, double Y)>();

            double anguloRad = AnguloGrados * Math.PI / 180.0;
            double v0x = VelocidadInicial * Math.Cos(anguloRad);
            double v0y = VelocidadInicial * Math.Sin(anguloRad);

            double tiempoVuelo = (2 * v0y) / Aceleracion;

            for (double t = 0; t <= tiempoVuelo; t += 0.05)
            {
                double x = v0x * t;
                double y = v0y * t - 0.5 * Aceleracion * t * t;
                if (y < 0) y = 0;
                datos.Add((x, y));
            }

            return datos;
        }
    }
}
