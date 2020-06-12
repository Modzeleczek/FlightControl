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
        private Random Rng;

        public SimulationWindow()
        {
            InitializeComponent();

            Rng = new Random();
            radar = new Radar("obstacles.txt", 32, MapImage, RoutesImage, AircraftsImage);

            /*List<Stage> stages = new List<Stage>
            {
                new Stage(new Line(50, 50, 500, 600), 195, 10)
            };
            Flight flight = new Flight(stages);
            Plane plane = new Plane(flight);
            plane.ScaleVelocity(32 / 1024.0);
            radar.AddAircraft(plane);

            stages.Clear();
            stages.Add(new Stage(new Line(50, 50, 500, 400), 160, 10));
            flight = new Flight(stages);

            Helicopter helicopter = new Helicopter(flight);
            helicopter.ScaleVelocity(32 / 1024.0);
            radar.AddAircraft(helicopter);*/

            WriteableBitmap b = AircraftsImage.Source as WriteableBitmap;
            b.Lock();
            DistanceMeter = new Line(0, 0, 1279, 300);
            DistanceMeter.Draw(b, (255 << 24) | (255 << 16));
            b.Unlock();

            AircraftsImage.MouseLeftButtonDown += ImagesLeftClick;
            AircraftsImage.MouseRightButtonDown += ImagesRightClick;

            radar.Start();
        }

        Line DistanceMeter;
        private void ImagesLeftClick(object sender, MouseButtonEventArgs e)
        {
            WriteableBitmap b = AircraftsImage.Source as WriteableBitmap;
            b.Lock();
            DistanceMeter.Draw(b, 0);

            int x = (int)e.GetPosition(AircraftsImage).X,
                y = (int)e.GetPosition(AircraftsImage).Y;
            DistanceMeter.Start.X = x;
            DistanceMeter.Start.Y = y;
            debugTB.Text = $"{DistanceMeter}, {DistanceMeter.Length}";

            DistanceMeter.Draw(b, (255 << 24) | (255 << 16));
            b.Unlock();
        }

        private void ImagesRightClick(object sender, MouseButtonEventArgs e)
        {
            WriteableBitmap b = AircraftsImage.Source as WriteableBitmap;
            b.Lock();
            DistanceMeter.Draw(b, 0);

            int x = (int)e.GetPosition(AircraftsImage).X,
                y = (int)e.GetPosition(AircraftsImage).Y;
            DistanceMeter.End.X = x;
            DistanceMeter.End.Y = y;
            debugTB.Text = $"{DistanceMeter}, {DistanceMeter.Length}";

            DistanceMeter.Draw(b, (255 << 24) | (255 << 16));
            b.Unlock();
        }

        private void PauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (radar.IsEnabled)
            {
                radar.Stop();
                PauseButton.Content = "Start";
            }
            else
            {
                radar.Start();
                PauseButton.Content = "Stop";
            }
        }

        private void RandomizeAircraftsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = int.Parse(AircraftsCountInputTextBox.Text);
                radar.RandomizeAircrafts(count, Rng);
            }
            catch(FormatException ex)
            {
                MessageBox.Show(
                    $"Nieprawidłowy format liczby statków.\nSzczegóły:\n{ex.Message}",
                    "Błąd",
                    MessageBoxButton.OK);
            }
        }

        public void Dispose()
        {
            AircraftsImage.MouseLeftButtonDown -= ImagesLeftClick;
            AircraftsImage.MouseRightButtonDown -= ImagesRightClick;
            radar.Dispose();
            radar = null;
            Rng = null;
        }
    }
}
