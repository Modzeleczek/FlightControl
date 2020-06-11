﻿using System;
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
            //radar.RandomizeAircrafts(5, 2, 5, 50, 100, 50, 100, 200, 300, 10, 100, Rng);

            List<Stage> stages = new List<Stage>();
            stages.Add(new Stage(new Line(50, 50, 500, 600), 200, 10));
            Flight flight = new Flight(stages);

            radar.AddAircraft(new Plane(flight, 30, 30));

            stages.Clear();
            stages.Add(new Stage(new Line(50, 50, 500, 400), 200, 10));
            flight = new Flight(stages);
            radar.AddAircraft(new Helicopter(flight, 30, 30));

            radar.Start();
            Running = true;

            /*AircraftsImage.MouseLeftButtonDown += new MouseButtonEventHandler((s, e) =>
            {
                /*int x = (int)e.GetPosition(sender as Image).X,
                y = (int)e.GetPosition(sender as Image).Y;* /
                //radar.RandomizeAircrafts(20, 5, 10, 10, 20, 20, 40, 150, 100, 50, 100, Rng);
            });*/
        }

        /*private void AircraftsImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*int x = (int)e.GetPosition(sender as Image).X,
                y = (int)e.GetPosition(sender as Image).Y;* /
            radar.RandomizeAircrafts(20, 5, 10, 10, 20, 20, 40, 150, 100, 50, 100, Rng);
        }*/

        //protected override void OnClosed(EventArgs e) => Dispose();

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
            radar.Dispose();
            radar = null;
            Rng = null;
        }
    }
}
