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
    public partial class SimulationWindow : Window, IDisposable
    {
        private Radar radar;
        private bool Running;
        private Random Rng;
        public SimulationWindow()
        {
            InitializeComponent();

            Rng = new Random();
            radar = new Radar("../../obstacles.txt", 32, MapImage, AircraftsImage);

            List<Stage> stages = new List<Stage>
            {
                new Stage(new Line(50, 50, 500, 600), 200, 10)
            };
            Flight flight = new Flight(stages);

            radar.AddAircraft(new Plane(flight));

            stages.Clear();
            stages.Add(new Stage(new Line(50, 50, 500, 400), 200, 10));
            flight = new Flight(stages);
            radar.AddAircraft(new Helicopter(flight));

            AircraftsImage.MouseLeftButtonDown += LeftClick;

            radar.Start();
            Running = true;
        }

        private void LeftClick(object sender, MouseButtonEventArgs e)
        {
            int x = (int)e.GetPosition(AircraftsImage).X,
                y = (int)e.GetPosition(AircraftsImage).Y;
            debugTB.Text = $"{x}, {y}";
        }

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (Running)
            {
                radar.Stop();
                PauseButton.Content = "Start";
            }
            else
            {
                radar.Start();
                PauseButton.Content = "Stop";
            }
            Running = !Running;
        }
        private void RandomizeAircrafts(object sender, RoutedEventArgs e) => radar.RandomizeAircrafts(Rng);

        public void Dispose()
        {
            AircraftsImage.MouseLeftButtonDown -= LeftClick;
            radar.Dispose();
            radar = null;
            Rng = null;
        }
    }
}
