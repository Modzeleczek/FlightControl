using System.Collections.Generic;

namespace FlightControl
{
    public class Route : Curve
    {
        List<double> Altitude, Velocity;
        public Route(List<Point> points) : base(points)
        {
        }
        public Route(List<Line> lines) : base(lines)
        {
        }
        public Route(Route o) : base(o)
        {
        }
        public void AddFlight(Point destination, double altitude, double velocity)
        {
            Lines.Add(new Line(Lines[Lines.Count - 1].End.X, Lines[Lines.Count - 1].End.Y, 
                destination.X, destination.Y));
            Altitude.Add(altitude);
            Velocity.Add(velocity);
        }
    }
}
