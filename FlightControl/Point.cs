using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Point
    {
        public double X;
        public double Y;
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point(Point o) : this(o.X, o.Y) { }
        public void Move(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }
        public override bool Equals(object obj)
        {
            Point point = obj as Point;
            if (!(point is Point))
                return false;
            return (point.X == X && point.Y == Y);
        }
        public override string ToString() => $"({X},{Y})";
        unsafe public void Draw(WriteableBitmap bitmap, int color) => bitmap.DrawPoint((int)X, (int)Y, color);
    }
}
