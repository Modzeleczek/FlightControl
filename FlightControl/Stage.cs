

using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Stage
    {
        public Line Track;
        public double Direction;
        public double Velocity, Altitude;
        public Stage(Line line, double velocity, double altitude)
        {
            Track = new Line(line);
            if(Track.Start.X != Track.End.X)
                Direction = Math.Atan2(Track.End.Y - Track.Start.Y, Track.End.X - Track.Start.X);
            else
            {
                if (Track.End.Y > Track.Start.Y)
                    Direction = Math.PI / 2.0;
                else
                    Direction = -Math.PI / 2.0;
            }

            Velocity = velocity;
            Altitude = altitude;
        }
        public Stage(Stage o) : this(o.Track, o.Velocity, o.Altitude)
        {
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Track.Draw(bitmap, color);
        }
    }
}
