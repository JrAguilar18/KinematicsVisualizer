using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinematicsVisualizer.Models
{
    public class MRUModel
    {
        public double Velocidad { get; set; }
        public double Tiempo { get; set; }

        public List<(double Tiempo, double Posicion)> CalcularPosicion()
        {
            var datos = new List<(double, double)>();
            for (double t = 0; t <= Tiempo; t += 0.1)
            {
                double x = Velocidad * t;
                datos.Add((t, x));
            }
            return datos;
        }
    }
}
