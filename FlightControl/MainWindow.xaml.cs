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
using System.Windows.Forms;

namespace FlightControl
{
    public partial class MainWindow : Window
    {
        private SimulationWindow SecondWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            SecondWindow = new SimulationWindow();
            SecondWindow.Show();
        }
        private void ChangeAmount(object sender, RoutedEventArgs e)
        {

        }
        private void Random(object sender, RoutedEventArgs e)
        {

        }
        private void RandomRoutes(object sender, RoutedEventArgs e)
        {

        }
        private void Egg(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("Gratulacje! Odkryłeś EasterEgg'a.");
        }

        private void RandomVelocity(object sender, RoutedEventArgs e)
        {
            
        }
        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            DialogResult dr = System.Windows.Forms.MessageBox.Show("Na pewno chcesz opuścić program?", "Na pewno chcesz kontynuować?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
