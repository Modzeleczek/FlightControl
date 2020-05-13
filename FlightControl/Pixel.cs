

namespace FlightControl
{
    class Pixel : Point
    {
        public byte R { get; protected set; }
        public byte G { get; protected set; }
        public byte B { get; protected set; }
        public Pixel(double x, double y, byte r, byte g, byte b) : base(x, y)
        {
            R = r;
            G = g;
            B = b;
        }
    }
}
