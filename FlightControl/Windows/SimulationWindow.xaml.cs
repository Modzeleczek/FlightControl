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
                RadarInstance = new Radar("Resources/obstacles.txt", 32, 50, MapImage, RoutesImage, AircraftsImage);
                FramerateTextBox.Text = 32.ToString();
                DangerousDistanceTextBox.Text = 50.ToString();
            }
            catch (Exception ex) when (ex is MapLoadingException || ex is FormatException)
            {
                this.Close();
                throw;
            }

            AircraftsImage.MouseRightButtonDown += ImageRightClick;
            RadarInstance.Stop();
        }
        
        private void ImageRightClick(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(AircraftsImage);
            RadarInstance.RemoveAircraft(position.X, position.Y);
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
                RadarInstance.RandomizeAircrafts(int.Parse(AircraftsCountTextBox.Text), Rng);
            }
            catch(FormatException)
            {
                MessageBox.Show($"Nieprawidłowy format liczby statków.", "Błąd", MessageBoxButton.OK);
            }
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

        private void HelpClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("'Start/Stop' uruchamia/zatrzymuje symulację.\n" +
                "'Reset' usuwa wszystkie statki.\n" +
                "Wpisz liczbę statków, a następnie kliknij 'Losuj statki', aby wygenerować statki.\n" +
                "'Dodaj statek' otwiera kreator nowego statku.\n" +
                "Kliknięcie prawym przyciskiem myszy na statku usuwa go.\n" +
                "'Niebezpieczna odległość' oznacza maksymalną odległość między dwoma obiektami, " +
                "aby ich zbliżenie było uznane za niebezpieczne.\n" +
                "'Częstotliwość' oznacza odstęp pomiędzy kolejnymi dwoma odświeżeniami radaru w milisekundach.",
                "Pomoc", MessageBoxButton.OK);
        }

        private void DangerousDistanceChange(object sender, RoutedEventArgs e)
        {
            try
            {
                RadarInstance.DangerousDistance = double.Parse(DangerousDistanceTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Nieprawidłowy format niebezpiecznej odległości.", "Błąd", MessageBoxButton.OK);
            }
        }

        private void FramerateChange(object sender, RoutedEventArgs e)
        {
            try
            {
                RadarInstance.Framerate = int.Parse(FramerateTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show($"Nieprawidłowy format częstotliwości.", "Błąd", MessageBoxButton.OK);
            }
        }

        public void Dispose()
        {
            AircraftsImage.MouseRightButtonDown -= ImageRightClick;
            PauseButton.Click -= PauseClick;
            RandomizeAircraftsButton.Click -= RandomizeAircraftsClick;
            AddAircraftButton.Click -= AddAircraftClick;
            ResetButton.Click -= ResetClick;
            HelpButton.Click -= HelpClick;
            DangerousDistanceButton.Click -= DangerousDistanceChange;
            FramerateButton.Click -= FramerateChange;
            RadarInstance.Dispose();
            RadarInstance = null;
            Rng = null;
        }
    }
}
