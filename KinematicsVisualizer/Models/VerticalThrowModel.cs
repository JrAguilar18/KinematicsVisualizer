using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinematicsVisualizer.Models
{
    public class VerticalThrowModel
    {
        public double VelocidadInicial { get; set; }
        public double Aceleracion { get; set; } = 9.8;

        public List<(double Tiempo, double Altura)> CalcularTrayectoria()
        {
            var datos = new List<(double, double)>();

            // Tiempo total de vuelo: t_total = 2 * v0 / g
            double tiempoTotal = 2 * VelocidadInicial / Aceleracion;

            for (double t = 0; t <= tiempoTotal; t += 0.1)
            {
                double y = VelocidadInicial * t - 0.5 * Aceleracion * t * t;
                datos.Add((t, y));
            }

            return datos;
        }
    }
}
