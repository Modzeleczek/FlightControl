using System.Windows.Media.Imaging;

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
        protected override void Draw(WriteableBitmap bitmap)
        {
            Position.Draw(bitmap, (255 << 24) | (102 << 16) | (145 << 8) | 255);
        }
        protected override void DrawRoute(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, (255 << 24) | (102 << 16) | (145 << 8) | 255);
        }
    }
}
