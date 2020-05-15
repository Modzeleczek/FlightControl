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
        bool Running;
        public SimulationWindow()
        {
            InitializeComponent();

            radar = new Radar("obstacles.txt", 32, MapImage, AircraftsImage);
            List<Stage> stages = new List<Stage>
            {
                new Stage(new Line(0, 0, 50, 100), 90, 10),
                new Stage(new Line(50, 100, 130, 200), 30, 10),
                new Stage(new Line(130, 200, 300, 252), 60, 10),
                new Stage(new Line(300, 252, 500, 175), 10, 10)
            };
            Flight route = new Flight(stages);
            radar.AddAircraft(new Plane(route, 15, 10));

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
