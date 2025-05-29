using ScottPlot;
using ScottPlot.WPF;

namespace KinematicsVisualizer.Services
{
    public class PlotService
    {
        public void PlotXY(WpfPlot plot, List<(double x, double y)> datos, string titulo,
                   string xlabel = "Tiempo (s)", string ylabel = "Posición (m)")
        {
            plot.Plot.Clear();

            var xs = datos.Select(d => d.x).ToArray();
            var ys = datos.Select(d => d.y).ToArray();

            var scatter = plot.Plot.Add.Scatter(xs, ys);
            scatter.Color = Colors.DarkRed;
            scatter.LineWidth = 2;

            plot.Plot.Title(titulo);
            plot.Plot.XLabel(xlabel);
            plot.Plot.YLabel(ylabel);

            // auto ajuste con margen
            plot.Plot.Axes.AutoScale();
            plot.Refresh();
        }
    }
}
