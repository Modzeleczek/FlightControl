using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft
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
            Route.Draw(bitmap, 0);//remove old route pixels
            //Position.Draw(bitmap, 0);//remove old position pixel
            Hitbox.Draw(bitmap, 0);//remove old hitbox pixels

            Hitbox.Position.X += Route.ActualStage.Velocity.X;
            Hitbox.Position.Y += Route.ActualStage.Velocity.Y;
            StageProgress += Route.ActualStage.Velocity.Length;
            if (StageProgress >= Route.ActualStage.Length)
            {
                Hitbox.Position.X = Route.ActualStage.Track.End.X;
                Hitbox.Position.Y = Route.ActualStage.Track.End.Y;
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
                //return true;
            return false;
        }
        public bool Collides(Obstacle obstacle)
        {
            if (obstacle.Height >= this.Route.ActualStage.Altitude)
                return this.Colliding = this.Hitbox.Collides(obstacle.Hitbox);
                //return true;
            return false;
        }
        public void ScaleVelocity(double factor) => Route.ScaleVelocity(factor);
        protected abstract void Draw(WriteableBitmap bitmap);
        protected abstract void DrawRoute(WriteableBitmap bitmap);
        public void ClearGraphics(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, 0);//remove old route pixels
            Hitbox.Draw(bitmap, 0);//remove old hitbox pixels
        }
        public void RandomizeFlight(int fromStagesCount, int toStagesCount, int mapWidth, int mapHeight, int fps, Random rng)
        {
            Route = Flight.GetRandom(
                rng.Next(fromStagesCount, toStagesCount),
                (int)Math.Ceiling(Hitbox.Width), (int)Math.Ceiling(Hitbox.Height),
                (int)Math.Floor(mapWidth - 1 - Hitbox.Width),
                (int)Math.Floor(mapHeight - 1 - Hitbox.Height),
                100, 100, 5, 100, rng);
            Hitbox.Position.X = Route.ActualStage.Track.Start.X;
            Hitbox.Position.Y = Route.ActualStage.Track.Start.Y;
            ScaleVelocity(1.0 / fps);
        }
    }
}
