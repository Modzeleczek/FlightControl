using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Media;
using FlightControl.Exceptions;
namespace FlightControl
{
    public partial class MainWindow : Window
    {
        private SimulationWindow Simulation;
        private bool MusicPlaying = false;
        //private SoundPlayer Player = new SoundPlayer("Resources/musicfinal.wav");//Muzyka ważyła 35 MB, więc usunęliśmy ją, żeby zmieścić projekt na CEZ.
        
        public MainWindow() => InitializeComponent();

        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            if (StartSimulationButton.IsEnabled)
            {
                try
                {
                    Simulation = new SimulationWindow();
                    StartSimulationButton.IsEnabled = false;
                    Simulation.Show();
                    Simulation.Closing += SecondWindowClosing;
                }
                catch (Exception ex) when (ex is MapLoadingException || ex is FormatException)
                {
                    MessageBox.Show($"Nie można uruchomić symulacji, ponieważ nie udało się wczytać mapy.", "Błąd", MessageBoxButton.OK);
                }
            }
        }

        private void SecondWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartSimulationButton.IsEnabled = true;
            Simulation.Closing -= SecondWindowClosing;
            Simulation.Dispose();
            Simulation = null;
            GC.Collect();
        }

        private void MusicStartStop(object sender, RoutedEventArgs e)
        {
            if (!MusicPlaying)
            {
                //Player.PlayLooping();
                MusicButton.Content = "Wyłącz muzykę";
            }
            else
            {
                //Player.Stop();
                MusicButton.Content = "Włącz muzykę";
            }
            MusicPlaying = !MusicPlaying;
        }

        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Na pewno chcesz opuścić program?", "Na pewno chcesz kontynuować?", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                //Player.Stop();
                this.Close();
            }
        }
    }
}
