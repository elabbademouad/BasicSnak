using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasicSnak
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MoteurDuJeux = new Engine();
            MoteurDuJeux.Initialize();
            this.DataContext = MoteurDuJeux;
            this.KeyDown += MainWindow_KeyDown;

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                MoteurDuJeux.FutureDirection = DirectionEnum.Left;
            if (e.Key == Key.Right)
                MoteurDuJeux.FutureDirection = DirectionEnum.Rigth;
            if (e.Key == Key.Down)
                MoteurDuJeux.FutureDirection = DirectionEnum.Bottom;
            if (e.Key == Key.Up)
                MoteurDuJeux.FutureDirection = DirectionEnum.Top;
            if (e.Key == Key.P)
                MoteurDuJeux.Speed = MoteurDuJeux.Speed - 20;
            if (e.Key == Key.M)
                MoteurDuJeux.Speed = MoteurDuJeux.Speed + 20;




        }

        public Engine MoteurDuJeux { get; set; }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (MoteurDuJeux.Pause)
            {
                MoteurDuJeux.Pause = false;
                MoteurDuJeux.BackGroundWorker.RunWorkerAsync();
                ((Button)sender).Content = "Pause";
            }
            else
            {
                MoteurDuJeux.Pause = true;
                ((Button)sender).Content = "Resume";
            }

        }

        private void btnNewgame_Click(object sender, RoutedEventArgs e)
        {
            MoteurDuJeux.NewGame = true;

            if (MoteurDuJeux.Pause)
            {
                MoteurDuJeux.Pause = false;
                MoteurDuJeux.BackGroundWorker.RunWorkerAsync();
            }
            else
            {
                MoteurDuJeux.Pause = true;

            }
        }
    }
}
