

namespace FlightControl
{
    public class Glider : Aircraft
    {
        public Glider(Flight route, double width, double length)
            : base(route, width, length)
        {
        }
        public Glider(Glider o) : base(o)
        {
        }
    }
}
