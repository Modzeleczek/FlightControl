using System;
using System.Windows;
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
        public bool Advance(double deltaTime, WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, 0);//remove old route pixels
            Position.Draw(bitmap, 0);//remove old position pixel
            Position.X += Route[0].Velocity.X * deltaTime;
            Position.Y += Route[0].Velocity.Y * deltaTime;
            StageProgress += Route[0].Velocity.Length * deltaTime;
            if (StageProgress >= Route[0].Length)
            {
                if (!Route.RemoveStage(0))
                    return false;
                StageProgress = 0;
            }
            DrawRoute(bitmap);
            Draw(bitmap);
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
        public abstract void Draw(WriteableBitmap bitmap);
        public abstract void DrawRoute(WriteableBitmap bitmap);
    }
}
