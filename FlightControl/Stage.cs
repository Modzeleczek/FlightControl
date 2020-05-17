using FlightControl.Exceptions;
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
            if (track.Start.X < 0 || track.Start.X >= 1280 || track.Start.Y < 0 || track.Start.Y >= 690
                || track.End.X < 0 || track.End.X >= 1280 || track.End.Y < 0 || track.End.Y >= 690)
                throw new StageOutOfBoundsException($"Track.Start: {Track.Start} or Track.End: {Track.End} is beyond map's boundaries.");

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
