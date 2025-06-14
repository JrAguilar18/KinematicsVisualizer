﻿using KinematicsVisualizer.Models;
using KinematicsVisualizer.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace KinematicsVisualizer.Views
{

    public partial class FreeFallWindow : Window
    {
        public FreeFallWindow() : this(GraphType.PosicionVsTiempo) // valor por defecto
        {
        }
        public FreeFallWindow(GraphType tipoGrafica)
        {
            InitializeComponent();
            var vm = new FreeFallViewModel(tipoGrafica);
            this.DataContext = vm;
            vm.PlotControl = this.plot;
        }

        private void VolverAlMenu(object sender, RoutedEventArgs e)
        {
            var menu = new MainWindow();
            menu.Show();
            Application.Current.MainWindow = menu;
            this.Close();
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}