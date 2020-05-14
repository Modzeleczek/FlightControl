using System.Collections.Generic;
using System.Windows;

namespace FlightControl
{
    class Route : Curve
    {
        List<double> Altitude, Velocity, Direction;
        public Route(Point[] points) : base(points)
        {
        }
        public Route(Line[] lines) : base(lines)
        {
        }
        public Route(Route o) : base(o)
        {
        }
        public void AddDestination(Point destination, double altitude, double velocity)
        {
            Lines.Add(new Line(Lines[Lines.Count - 1].End.X, Lines[Lines.Count - 1].End.Y, 
                destination.X, destination.Y));
            Altitude.Add(altitude);
            Velocity.Add(velocity);
        }
    }
}
