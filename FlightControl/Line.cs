using FlightControl.Exceptions;
using System;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FlightControl
{
    public class Line
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public Line(double x1, double y1, double x2, double y2)
        {
            if (x1 == x2 && y1 == y2)
                throw new LineIsPointException($"Start: {Start} and end: {End} are equal.");
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }
        public Line(Line o) : this(o.Start.X, o.Start.Y, o.End.X, o.End.Y) { }
        public bool IsContinuedBy(Line next)
        {
            if (End.Equals(next.Start))
                return true;
            return false;
        }
        public override string ToString()
        {
            return $"(Line: ({Start.X},{Start.Y})->({End.X},{End.Y}))";
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            bitmap.DrawLine((int)Start.X, (int)Start.Y, (int)End.X, (int)End.Y, color);
        }
    }
}
