﻿

namespace FlightControl
{
    class Point
    {
        public double X;
        public double Y;
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point(Point o) : this(o.X, o.Y)
        {
        }
        public override bool Equals(object obj)
        {
            Point point = obj as Point;
            if (!(point is Point))
                return false;
            return (point.X == X && point.Y == Y);
        }
    }
}
