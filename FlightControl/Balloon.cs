

namespace FlightControl
{
    public class Balloon : Aircraft
    {
        public Balloon(Route route, double width, double length)
            : base(route, width, length)
        {
        }
        public Balloon(Balloon o) : base(o)
        {
        }
    }
}
