using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Plane : Aircraft
    {
        public Plane(Flight route, double width, double length)
            : base(route, width, length)
        {
        }
        public Plane(Plane o) : base(o)
        {
        }
        protected override void Draw(WriteableBitmap bitmap)
        {
            if (!Colliding)
                Hitbox.Draw(bitmap, (255 << 24) | (255 << 8));
            else
                Hitbox.Draw(bitmap, (255 << 24) | (255 << 16));//red 
        }
        protected override void DrawRoute(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, (255 << 24) | (255 << 8));
        }
    }
}
