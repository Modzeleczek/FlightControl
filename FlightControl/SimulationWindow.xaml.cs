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
        private Radar RadarInstance;
        private Random Rng;

        public SimulationWindow()
        {
            InitializeComponent();

            Rng = new Random();
            try
            {
                RadarInstance = new Radar("obstacles.txt", 32, 50, MapImage, RoutesImage, AircraftsImage);
            }
            catch (Exception ex) when (ex is MapLoadingException || ex is FormatException)
            {
                this.Close();
                throw;
            }

            AircraftsImage.MouseLeftButtonDown += ImageLeftClick;
            RadarInstance.Stop();
        }

        private void ImageLeftClick(object sender, MouseButtonEventArgs e)
        {
            
        }
        
        private void ImageRightClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void PauseClick(object sender, RoutedEventArgs e)
        {
            if (RadarInstance.IsEnabled)
            {
                RadarInstance.Stop();
                PauseButton.Content = "Start";
            }
            else
            {
                RadarInstance.Start();
                PauseButton.Content = "Stop";
            }
        }

        private void RandomizeAircraftsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                int count = int.Parse(AircraftsCountInputTextBox.Text);
                RadarInstance.RandomizeAircrafts(count, Rng);
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
            RadarInstance.Dispose();
            RadarInstance = null;
            Rng = null;
        }

        private void AddAircraftClick(object sender, RoutedEventArgs e)
        {
            bool running = RadarInstance.IsEnabled;
            if(running)
                RadarInstance.Stop();
            AdditionWindow additionWindow = new AdditionWindow(MapImage.Source as WriteableBitmap,
                RoutesImage.Source as WriteableBitmap, AircraftsImage.Source as WriteableBitmap);
            additionWindow.ShowDialog();
            if(additionWindow.Result != null)
                RadarInstance.AddAircraft(additionWindow.Result);
            additionWindow.Dispose();
            if(running)
                RadarInstance.Start();
        }

        private void ResetClick(object sender, RoutedEventArgs e) => RadarInstance.ClearAircrafts();
    }
}
