

using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft
    {
        protected Route Route;
        readonly public double Width, Height;
        protected Point Position;
        protected Aircraft(Route route, double width, double height)
        {
            Route = new Route(route);
            Position = new Point(route[0].Start.X, route[0].Start.Y);
            Width = width;
            Height = height;
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Width, o.Height)
        {
        }
        /*public void Move(double dx, double dy)
        {
            Position.X += dx;
            Position.Y += dy;
        }*/
        public void Advance()
        {
            
        }
        public bool CollidesWith(Aircraft aircraft)
        {
            if (aircraft.Position.X + aircraft.Width >= Position.X && aircraft.Position.X <= Position.X + Width)
            {
                if (aircraft.Position.Y + aircraft.Height >= Position.Y && aircraft.Position.Y <= Position.Y + Height)
                    return true;
                else
                    return false;
            }
            return false;
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
