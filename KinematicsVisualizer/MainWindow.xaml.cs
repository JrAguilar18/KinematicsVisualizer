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
        private const double gravedad = 980.0; // px/s²
        private const double damping = 0.7;
        private int rebotes = 0;
        private const int maxRebotes = 4;
        private const double sueloY = 400;

        private DateTime lastUpdateTime;
        private bool esPantallaCompleta = false;

        public MainWindow()
        {
            InitializeComponent();
            IniciarAnimacion();
            this.Loaded += (_, __) => this.Focus(); // Captura teclas
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
            velX = 180; // px/s
            velY = -500; // px/s (hacia arriba)

            lastUpdateTime = DateTime.Now;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
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

            // Rebote contra el "suelo"
            if (posY >= sueloY)
            {
                posY = sueloY;
                velY = -velY * damping;
                rebotes++;

                if (rebotes >= maxRebotes)
                {
                    timer.Stop();
                    return;
                }
            }

            // Dibujar trayectoria
            trayectoria.Points.Add(new Point(posX, posY));

            // Posicionar la pelota
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