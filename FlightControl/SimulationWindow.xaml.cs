using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace FlightControl
{
    /// <summary>
    /// Interaction logic for SimulationWindow.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        public Radar radar;
        private WriteableBitmap mapBitmap, aircraftBitmap;
        public SimulationWindow()
        {
            InitializeComponent();

            mapBitmap = new WriteableBitmap((int)MapImage.Width, (int)MapImage.Height, 96, 96, PixelFormats.Bgra32, null);
            MapImage.Source = mapBitmap;
            aircraftBitmap = new WriteableBitmap((int)MapImage.Width, (int)MapImage.Height, 96, 96, PixelFormats.Bgra32, null);


            radar = new Radar("obstacles.txt", 30, aircraftBitmap);

            mapBitmap.Lock();
            radar.DrawMap(mapBitmap);
            mapBitmap.AddDirtyRect(new Int32Rect(0, 0, mapBitmap.PixelWidth, mapBitmap.PixelHeight));
            mapBitmap.Unlock();
        }
        protected override void OnClosed(EventArgs e)
        {
            radar.Stop();
            radar = null;
            mapBitmap = null;
        }
    }
}
