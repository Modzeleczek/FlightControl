using System.Windows.Media.Imaging;
using System.Windows;

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
        public override string ToString()
        {
            return $"({X},{Y})";
        }
        unsafe public void Draw(WriteableBitmap bitmap, int color)
        {
            *((int*)bitmap.BackBuffer + (int)X + (int)Y * bitmap.PixelWidth) = color;
            bitmap.AddDirtyRect(new Int32Rect((int)X, (int)Y, 1, 1));
        }
    }
}
