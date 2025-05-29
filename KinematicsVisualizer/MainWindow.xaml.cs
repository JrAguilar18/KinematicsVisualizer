using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KinematicsVisualizer.Views
{
    public partial class MainWindow : Window
    {
        private Ellipse pelota;
        private Polyline trayectoria;
        private DispatcherTimer timer;
        private double posX, posY;
        private double velX, velY;
        private const double gravedad = 9.8;
        private const double reboteDamping = 0.7;
        private int reboteCount = 0;
        private const int maxRebotes = 4;

        private DateTime lastFrameTime;

        public MainWindow()
        {
            InitializeComponent();
            IniciarAnimacion();
        }

        private void IniciarAnimacion()
        {
            pelota = new Ellipse
            {
                Width = 16,
                Height = 16,
                Fill = Brushes.OrangeRed
            };

            trayectoria = new Polyline
            {
                Stroke = Brushes.SteelBlue,
                StrokeThickness = 2
            };

            AnimacionCanvas.Children.Add(trayectoria);
            AnimacionCanvas.Children.Add(pelota);

            // Valores iniciales
            posX = 50;
            posY = 0;
            velX = 100; // píxeles por segundo
            velY = -180;

            lastFrameTime = DateTime.Now;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };
            timer.Tick += OnFrame;
            timer.Start();
        }

        private void OnFrame(object sender, EventArgs e)
        {
            // Δt real
            var now = DateTime.Now;
            double dt = (now - lastFrameTime).TotalSeconds;
            lastFrameTime = now;

            // Física
            velY += gravedad * 100 * dt; // gravedad en px/s²
            posX += velX * dt;
            posY += velY * dt;

            // Rebote
            if (posY >= 250)
            {
                posY = 250;
                velY = -velY * reboteDamping;
                reboteCount++;

                if (reboteCount >= maxRebotes)
                {
                    timer.Stop();
                    return;
                }
            }

            // Dibujar trayectoria
            trayectoria.Points.Add(new Point(posX, posY));

            // Mover pelota
            Canvas.SetLeft(pelota, posX - pelota.Width / 2);
            Canvas.SetTop(pelota, posY - pelota.Height / 2);
        }

        private void MovimientoCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MovimientoCombo.SelectedItem is ComboBoxItem item)
            {
                string movimiento = item.Content.ToString();
                Window? siguiente = movimiento switch
                {
                    "MRU" => new MRUWindow(),
                    "MRUA" => new MRUAWindow(),
                    "Caída Libre" => new FreeFallWindow(),
                    "Tiro Vertical" => new VerticalThrowWindow(),
                    "Tiro Parabólico" => new ParabolicThrowWindow(),
                    _ => null
                };

                if (siguiente != null)
                {
                    siguiente.Show();
                    this.Close();
                }
            }
        }
    }
}