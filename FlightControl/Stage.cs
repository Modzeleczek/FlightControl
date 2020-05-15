using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Stage
    {
        public Line Track;
        //public double Direction;
        public double Length;
        public Vector Velocity;
        public double Altitude;
        /// <summary>
        /// Flight stage constructor.
        /// </summary>
        /// <param name="track">Line from start to end.</param>
        /// <param name="velocityValue">Velocity vector norm in pixels per second.</param>
        /// <param name="altitude">Flight altitude on this stage.</param>
        public Stage(Line track, double velocityValue, double altitude)
        {
            Track = new Line(track);
            Length = Math.Sqrt(Math.Pow(Track.End.X - Track.Start.X, 2.0) + Math.Pow(Track.End.Y - Track.Start.Y, 2.0));
            Velocity = new Vector(
                velocityValue * (Track.End.X - Track.Start.X) / Length,
                velocityValue * (Track.End.Y - Track.Start.Y) / Length);
            Altitude = altitude;
        }
        public Stage(Stage o) : this(o.Track, o.Velocity.Length, o.Altitude)
        {
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Track.Draw(bitmap, color);
        }
    }
}
