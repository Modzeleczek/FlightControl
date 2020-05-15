

namespace FlightControl
{
    public class Plane : Aircraft
    {
        public Plane(Route route, double width, double length)
            : base(route, width, length)
        {
        }
        public Plane(Plane o) : base(o)
        {
        }
    }
}
