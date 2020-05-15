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
            AircraftImage.Source = aircraftBitmap;

            radar = new Radar("obstacles.txt", 500, aircraftBitmap);

            List<Point> destinations = new List<Point>
            {
                new Point(0, 0),
                new Point(50, 100),
                new Point(130, 200)
            };
            Route route = new Route(destinations);
            radar.AddAircraft(new Plane(route, 20, 20));

            mapBitmap.Lock();
            radar.DrawMap(mapBitmap);
            mapBitmap.AddDirtyRect(new Int32Rect(0, 0, mapBitmap.PixelWidth, mapBitmap.PixelHeight));
            mapBitmap.Unlock();

            radar.Start();
        }
        protected override void OnClosed(EventArgs e)
        {
            radar.Stop();
            radar = null;
            mapBitmap = null;
        }
    }
}
