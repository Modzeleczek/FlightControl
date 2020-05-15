

namespace FlightControl
{
    public class Glider : Aircraft
    {
        public Glider(Route route, double width, double length)
            : base(route, width, length)
        {
        }
        public Glider(Glider o) : base(o)
        {
        }
    }
}
