﻿

namespace FlightControl
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point(Point original)
        {
            X = original.X;
            Y = original.Y;
        }
    }
}
