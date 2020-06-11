using System.Windows.Media.Imaging;
using System;

namespace FlightControl
{
    public class Rectangle : IDisposable
    {
        public Point Position { get; private set; }
        public double Width { get; }
        public double Height { get; }

        public Rectangle(Point position, double width, double height)
        {
            Position = new Point(position);
            Width = width;
            Height = height;
        }
        public Rectangle(Rectangle o) : this(o.Position, o.Width, o.Height) { }

        public bool Collides(Rectangle rectangle)
        {
            if (rectangle.Position.X + rectangle.Width >= Position.X
                && rectangle.Position.X <= Position.X + Width)
                if (rectangle.Position.Y + rectangle.Height >= Position.Y
                    && rectangle.Position.Y <= Position.Y + Height)
                    return true;
            return false;
        }

        public void Place(Point position)
        {
            Position.X = position.X - Width / 2;
            Position.Y = position.Y - Height / 2;
        }

        public void Draw(WriteableBitmap bitmap, int color) =>
            bitmap.DrawRectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height, color);

        public void Dispose() => Position = null;
    }
}
