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
            List<Stage> stages = new List<Stage>(10);
            /*{
                new Stage(new Line(0, 0, 50, 100), 90, 10),
                new Stage(new Line(50, 100, 130, 200), 30, 10),
                new Stage(new Line(130, 200, 300, 252), 60, 10),
                new Stage(new Line(300, 252, 500, 175), 10, 10)
            };*/
            //radar.AddAircraft(new Plane(new Flight(stages), 15, 10));

            Random random = new Random();
            int x, y, prevX = random.Next(0, 1280), prevY = random.Next(0, 690);
            //stages.Clear();
            for(int i = 1; i < 10; ++i)
            {
                x = random.Next(0, 1280);
                y = random.Next(0, 690);
                stages.Add(new Stage(
                    new Line(prevX, prevY,
                    x, y),
                    50.0 + 100.0 * random.NextDouble(),
                    10));
                prevX = x;
                prevY = y;
            }
            radar.AddAircraft(new Helicopter(new Flight(stages), 20, 20));

            /*stages.Clear();
            for (int i = 1; i < 10; ++i)
            {
                x = random.Next(0, 1280);
                y = random.Next(0, 690);
                stages.Add(new Stage(
                    new Line(prevX, prevY,
                    x, y),
                    50.0 + 100.0 * random.NextDouble(),
                    10));
                prevX = x;
                prevY = y;
            }
            radar.AddAircraft(new Glider(new Flight(stages), 20, 20));
            stages.Clear();
            radar.AddAircraft(new Balloon(new Flight(stages), 20, 20));*/

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
