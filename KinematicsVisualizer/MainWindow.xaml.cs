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

        // Cambia el valor de gravedad global:
        private const double gravedad = 1200.0;
        private const double damping = 0.7;
        private int rebotes = 0;
        private const int maxRebotes = 4;

        private DateTime lastUpdateTime;
        private bool esPantallaCompleta = false;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (_, __) =>
            {
                IniciarAnimacion();
                this.Focus();
                lastUpdateTime = DateTime.Now;
            };
        }

        private double SueloY => AnimacionCanvas.ActualHeight - pelota.Height;

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

            // Dentro de IniciarAnimacion()
            posX = 0;
            posY = SueloY;
            velX = 280;    // ← aumentado
            velY = -850;   // ← aumentado


            // Posicionar pelota inicialmente
            Canvas.SetLeft(pelota, posX);
            Canvas.SetTop(pelota, posY);

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60fps
            };
            timer.Tick += OnFrame;
            timer.Start();
        }

        private void OnFrame(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double deltaTime = (now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = now;

            // Física
            velY += gravedad * deltaTime;
            posX += velX * deltaTime;
            posY += velY * deltaTime;

            // Rebote contra el suelo
            if (posY >= SueloY)
            {
                posY = SueloY;
                velY = -velY * damping;
                rebotes++;

                if (rebotes >= maxRebotes)
                {
                    timer.Stop();
                    return;
                }
            }

            // Límite derecho (opcional)
            if (posX >= AnimacionCanvas.ActualWidth - pelota.Width)
            {
                timer.Stop();
                return;
            }

            // Dibujar trayectoria
            trayectoria.Points.Add(new Point(posX, posY));

            // Actualizar posición de la pelota
            Canvas.SetLeft(pelota, posX);       // ✅ No compensamos el centro
            Canvas.SetTop(pelota, posY);        // ✅ Rebote visual desde el borde
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                TogglePantallaCompleta();
            }
            else if (e.Key == Key.Escape && esPantallaCompleta)
            {
                SalirDePantallaCompleta();
            }
        }

        private void TogglePantallaCompleta()
        {
            if (!esPantallaCompleta)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.ResizeMode = ResizeMode.NoResize;
                esPantallaCompleta = true;
            }
            else
            {
                SalirDePantallaCompleta();
            }
        }

        private void SalirDePantallaCompleta()
        {
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Normal;
            this.ResizeMode = ResizeMode.CanResize;
            esPantallaCompleta = false;
        }
    }
}