using FlightControl.Exceptions;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Stage : IDisposable
    {
        public Line Track { get; private set; }
        public double Length { get; private set; }
        public Vector Velocity { get; private set; }
        public int Altitude { get; private set; }

        public Stage(Line track, double velocityValue, int altitude)
        {
            if (track.Start.X < 0 || track.Start.X >= 1280 || track.Start.Y < 0 || track.Start.Y >= 720)
                throw new StageOutOfBoundsException($"Track.Start: {Track.Start} is beyond map's boundaries.");
            if (track.End.X < 0 || track.End.X >= 1280 || track.End.Y < 0 || track.End.Y >= 720)
                throw new StageOutOfBoundsException($"Track.End: {Track.End} is beyond map's boundaries.");

            Track = new Line(track);
            Length = Math.Sqrt(Math.Pow(Track.End.X - Track.Start.X, 2.0) + Math.Pow(Track.End.Y - Track.Start.Y, 2.0));
            Velocity = new Vector(
                velocityValue * (Track.End.X - Track.Start.X) / Length,
                velocityValue * (Track.End.Y - Track.Start.Y) / Length);
            Altitude = altitude;
        }
        public Stage(Stage o) : this(o.Track, o.Velocity.Length, o.Altitude) { }

        public void ScaleVelocity(double factor) => Velocity *= factor;

        public void Draw(WriteableBitmap bitmap, int color) => Track.Draw(bitmap, color);

        public void Dispose()
        {
            Track.Dispose();
            Track = null;
        }
    }
}
