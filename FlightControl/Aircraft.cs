using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft : IDisposable
    {
        protected Flight Route;
        protected Rectangle Hitbox;
        public bool Colliding { get; protected set; }
        protected double StageProgress;

        protected Aircraft(Flight route, double width, double height)
        {
            Route = new Flight(route);
            Hitbox = new Rectangle(new Point(Route.ActualStage.Track.Start.X - width / 2, 
                Route.ActualStage.Track.Start.Y - height / 2), width, height);
            StageProgress = 0;
            Colliding = false;
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Hitbox.Width, o.Hitbox.Height) { }

        public bool Advance(WriteableBitmap bitmap)
        {
            ClearGraphics(bitmap);
            Hitbox.Position.Move(Route.ActualStage.Velocity.X, Route.ActualStage.Velocity.Y);
            StageProgress += Route.ActualStage.Velocity.Length;
            if (StageProgress >= Route.ActualStage.Length)
            {
                Hitbox.Place(Route.ActualStage.Track.End);
                if (!Route.RemoveStage(0))
                    return false;
                StageProgress = 0;
            }

            DrawRoute(bitmap);
            Draw(bitmap);
            Colliding = false;
            return true;
        }

        public void AppendStage(Point destination, double velocity, double altitude) =>
            Route.AppendStage(destination, velocity, altitude);

        public int StagesCount => Route.StagesCount;

        public bool RemoveStage(int index) => Route.RemoveStage(index);

        public bool Collides(Aircraft aircraft)
        {
            if (aircraft.Route.ActualStage.Altitude == this.Route.ActualStage.Altitude)
                return aircraft.Colliding = this.Colliding = this.Hitbox.Collides(aircraft.Hitbox);
            return false;
        }
        public bool Collides(Obstacle obstacle)
        {
            if (obstacle.Height >= this.Route.ActualStage.Altitude)
                return this.Colliding = this.Hitbox.Collides(obstacle.Hitbox);
            return false;
        }

        public void ScaleVelocity(double factor) => Route.ScaleVelocity(factor);

        public abstract void Draw(WriteableBitmap bitmap);

        public abstract void DrawRoute(WriteableBitmap bitmap);

        public void ClearGraphics(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, 0);
            Hitbox.Draw(bitmap, 0);
        }

        public void Dispose()
        {
            Route.Dispose();
            Hitbox.Dispose();
        }
    }
}
