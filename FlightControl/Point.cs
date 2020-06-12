using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
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
            if (point == null)
                return false;
            return (point.X == X && point.Y == Y);
        }

        public override string ToString() => $"({X},{Y})";
        
        public void Draw(WriteableBitmap bitmap, int color) => bitmap.DrawPoint((int)X, (int)Y, color);
    }
}
