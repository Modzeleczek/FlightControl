using FlightControl.Exceptions;
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
            try
            {
                radar = new Radar("obstacles.txt", 32, 50, MapImage, RoutesImage, AircraftsImage);
            }
            catch (Exception ex) when (ex is MapLoadingException || ex is FormatException)
            {
                this.Close();
                throw;
            }

            AircraftsImage.MouseLeftButtonDown += ImageLeftClick;
            radar.Stop();
        }

        private void ImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            
        }
        
        private void ImageRightClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void PauseClick(object sender, RoutedEventArgs e)
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
            catch(FormatException)
            {
                MessageBox.Show($"Nieprawidłowy format liczby statków.", "Błąd", MessageBoxButton.OK);
            }
        }

        public void Dispose()
        {
            AircraftsImage.MouseLeftButtonDown -= ImageLeftClick;
            PauseButton.Click -= PauseClick;
            RandomizeAircraftsButton.Click -= RandomizeAircraftsClick;
            AddAircraftButton.Click -= AddAircraftClick;
            ResetButton.Click -= ResetClick;
            radar.Dispose();
            radar = null;
            Rng = null;
        }

        private void AddAircraftClick(object sender, RoutedEventArgs e)
        {
            bool running = radar.IsEnabled;
            if(running)
                radar.Stop();
            AdditionWindow additionWindow = new AdditionWindow(MapImage.Source as WriteableBitmap,
                RoutesImage.Source as WriteableBitmap, AircraftsImage.Source as WriteableBitmap);
            additionWindow.ShowDialog();
            if(additionWindow.Result != null)
                radar.AddAircraft(additionWindow.Result);
            additionWindow.Dispose();
            if(running)
                radar.Start();
        }

        private void ResetClick(object sender, RoutedEventArgs e) => radar.ClearAircrafts();
    }
}
