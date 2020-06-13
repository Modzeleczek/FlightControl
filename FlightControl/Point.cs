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
        
        unsafe public void Draw(WriteableBitmap bitmap, int color)
        {
            *((int*)bitmap.BackBuffer + (int)X + (int)Y * bitmap.PixelWidth) = color;
            bitmap.AddDirtyRect(new System.Windows.Int32Rect((int)X, (int)Y, 1, 1));
        }
    }
}
