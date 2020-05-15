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
    public partial class SimulationWindow : Window
    {
        private Radar radar;
        public SimulationWindow()
        {
            InitializeComponent();

            radar = new Radar("obstacles.txt", 32, MapImage, RoutesImage, AircraftsImage);

            List<Stage> stages = new List<Stage>
            {
                new Stage(new Line(0, 0, 50, 100), 5, 10),
                new Stage(new Line(50, 100, 130, 200), 10, 10),
                new Stage(new Line(130, 200, 300, 252), 15, 10),
                new Stage(new Line(300, 252, 500, 175), 20, 10)
            };
            Flight route = new Flight(stages);
            radar.AddAircraft(new Plane(route, 15, 10));

            Debug.WriteLine(radar.RefreshingRate);

            radar.Start();
        }
        protected override void OnClosed(EventArgs e)
        {
            radar.Stop();
            radar = null;
        }
    }
}
