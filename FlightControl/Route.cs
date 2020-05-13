using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightControl
{
    class Route
    {
        private List<Point> Points;
        public Route()
        {
            Points = new List<Point>();
        }
        public void AddPoint(Point point)
        {
            Points.Add(new Point(point));
        }
    }
}
