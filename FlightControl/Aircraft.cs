

using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft
    {
        protected Route Route;
        protected Hitbox Bounds;
        protected Aircraft(Route route, double width, double length)
        {
            Route = new Route(route);
            Bounds = new Hitbox(route[0].Start.X, route[0].Start.Y, width, length);
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Bounds.Width, o.Bounds.Height)
        {
        }
        public void Move(double dx, double dy)
        {
            Bounds.Move(dx, dy);
        }
        public void AddFlight(Point destination, double altitude, double velocity)
        {
            Route.AddFlight(destination, altitude, velocity);
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Route.Draw(bitmap, color);
        }
    }
}
