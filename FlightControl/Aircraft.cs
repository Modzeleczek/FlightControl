using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft
    {
        protected Flight Route;
        readonly public double Width, Height;
        protected Point Position;
        protected double StageProgress;
        protected Aircraft(Flight route, double width, double height)
        {
            Route = new Flight(route);
            Position = new Point(route[0].Track.Start);
            Width = width;
            Height = height;

            StageProgress = 0;
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Width, o.Height)
        {
        }
        public bool Advance()
        {
            StageProgress += Route[0].Velocity;
            Position.X += Route[0].Velocity * Math.Cos(Route[0].Direction);
            Position.Y += Route[0].Velocity * Math.Sin(Route[0].Direction);
            if (StageProgress >= Route[0].Length)
            {
                Route.RemoveStage(0);
                StageProgress = 0;
                if (Route.IsCompleted)
                    return false;
            }
            return true;
        }
        public bool Collides(Aircraft aircraft)
        {
            if (aircraft.Position.X + aircraft.Width >= Position.X && aircraft.Position.X <= Position.X + Width)
                if (aircraft.Position.Y + aircraft.Height >= Position.Y && aircraft.Position.Y <= Position.Y + Height)
                    return true;
            return false;
        }
        public void AddStage(Point destination, double altitude, double velocity)
        {
            Route.AddStage(destination, altitude, velocity);
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Position.Draw(bitmap, color);
        }
        public void DrawRoute(WriteableBitmap bitmap, int color)
        {
            Route.Draw(bitmap, color);
        }
    }
}
