

namespace FlightControl
{
    public class Helicopter : Aircraft
    {
        public Helicopter(Flight route, double width, double length)
            : base(route, width, length)
        {
        }
        public Helicopter(Helicopter o) : base(o)
        {
        }
    }
}
