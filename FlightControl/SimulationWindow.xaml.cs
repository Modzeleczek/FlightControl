using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FlightControl
{
    public partial class SimulationWindow : Window
    {
        private Radar radar;
        private bool Running;
        private Random Rng;
        public SimulationWindow()
        {
            InitializeComponent();

            Rng = new Random();

            radar = new Radar("obstacles.txt", 32, MapImage, AircraftsImage);
            radar.RandomizeAircrafts(1, 2, 5, 10, 20, 10, 20, 200, 300, 10, 100, Rng);

            radar.Start();
            Running = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            radar.Stop();
            radar = null;
        }

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                radar.Stop();
                (sender as Button).Content = "Start";
            }
            else
            {
                radar.Start();
                (sender as Button).Content = "Stop";
            }
            Running = !Running;
        }
    }
}
