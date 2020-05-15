

namespace FlightControl
{
    abstract class Aircraft
    {
        protected Route Route;
        protected Hitbox Bounds;
        protected Aircraft(Route route, double width, double length)
        {
            Bounds = new Hitbox(route[0].Start.X, route[0].Start.Y, width, length);
        }
        public void Move(double dx, double dy)
        {
            Bounds.Move(dx, dy);
        }
        public void AddFlight(Point destination, double altitude, double velocity)
        {
            Route.AddFlight(destination, altitude, velocity);
        }
    }
}
