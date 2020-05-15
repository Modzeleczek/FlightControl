using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Balloon : Aircraft
    {
        public Balloon(Flight route, double width, double length)
            : base(route, width, length)
        {
        }
        public Balloon(Balloon o) : base(o)
        {
        }
        public override void Draw(WriteableBitmap bitmap)
        {
            Position.Draw(bitmap, (255 << 24) | (153 << 8) | 255);
        }
        public override void DrawRoute(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, (255 << 24) | (153 << 8) | 255);
        }
    }
}
