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
        private SimulationWindow SecondWindow;
        private bool MusicPlaying = false;
        private SoundPlayer Player = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + "\\musicfinal.wav");
        
        public MainWindow() => InitializeComponent();

        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            if (StartSimulationButton.IsEnabled)
            {
                StartSimulationButton.IsEnabled = false;
                try
                {
                    SecondWindow = new SimulationWindow();
                    SecondWindow.Show();
                    SecondWindow.Closing += SecondWindowClosing;
                }
                catch (Exception ex) when (ex is MapLoadingException || ex is FormatException)
                {
                    MessageBox.Show(
                        $"Program zakończy się, ponieważ nie udało się wczytać mapy.\nSzczegóły: {ex.Message}",
                        "Błąd",
                        MessageBoxButton.OK);
                    this.Close();
                    App.Current.Shutdown();
                }
            }
        }

        private void SecondWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StartSimulationButton.IsEnabled = true;
            SecondWindow.Closing -= SecondWindowClosing;
            SecondWindow.Dispose();
            SecondWindow = null;
            GC.Collect();
        }

        private void EasterEggClick(object sender, RoutedEventArgs e) => MessageBox.Show("Gratulacje! Odkryłeś EasterEgg'a.", "EasterEgg");

        private void MusicStartStop(object sender, RoutedEventArgs e)
        {
            if (!MusicPlaying)
            {
                Player.PlayLooping();
                MusicButton.Content = "Wyłącz muzykę";
            }
            else
            {
                Player.Stop();
                MusicButton.Content = "Włącz muzykę";
            }
            MusicPlaying = !MusicPlaying;
        }

        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Na pewno chcesz opuścić program?", "Na pewno chcesz kontynuować?", MessageBoxButton.YesNo)
                == MessageBoxResult.Yes)
            {
                Player.Stop();
                this.Close();
            }
        }
    }
}
