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
        protected Polygon Hitbox;
        protected Aircraft(Flight route, double width, double height)
        {
            Route = new Flight(route);
            Position = new Point(Route[0].Track.Start);
            Width = width;
            Height = height;
            Hitbox = new Polygon(
                new System.Collections.Generic.List<Point>
                {
                    new Point(Position.X - width/2, Position.Y - height/2),
                    new Point(Position.X - width/2, Position.Y + height - 1 - height/2),
                    new Point(Position.X + width - 1 - width/2, Position.Y + height - 1 - height/2),
                    new Point(Position.X + width - 1 - width/2, Position.Y - height/2),
                    new Point(Position.X - width/2, Position.Y - height/2)
                }
            );

            StageProgress = 0;
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Width, o.Height)
        {
        }
        public bool Advance(double deltaTime, WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, 0);//remove old route pixels
            Position.Draw(bitmap, 0);//remove old position pixel
            Hitbox.Draw(bitmap, 0);//remove old hitbox pixels
            Position.X += Route[0].Velocity.X * deltaTime;
            Position.Y += Route[0].Velocity.Y * deltaTime;
            Hitbox.Move(Route[0].Velocity.X * deltaTime, Route[0].Velocity.Y * deltaTime);
            StageProgress += Route[0].Velocity.Length * deltaTime;
            if (StageProgress >= Route[0].Length)
            {
                Position.X = Route[0].Track.End.X;
                Position.Y = Route[0].Track.End.Y;
                if (!Route.RemoveStage(0))
                    return false;
                StageProgress = 0;
            }
            DrawRoute(bitmap);
            Hitbox.Draw(bitmap, (255 << 24) | (255 << 8));
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
        protected abstract void Draw(WriteableBitmap bitmap);
        protected abstract void DrawRoute(WriteableBitmap bitmap);
        //protected abstract void DrawHitbox(WriteableBitmap bitmap);
    }
}
