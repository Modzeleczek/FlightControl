using System.Collections.Generic;

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
