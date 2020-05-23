using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Rectangle
    {
        public Point Position;
        public readonly double Width, Height;
        public Rectangle(Point position, double width, double height)
        {
            Position = new Point(position);
            Width = width;
            Height = height;
        }
        public Rectangle(Rectangle o) : this(o.Position, o.Width, o.Height)
        {
        }
        public bool Collides(Rectangle rectangle)
        {
            if (rectangle.Position.X + rectangle.Width >= Position.X
                && rectangle.Position.X <= Position.X + Width)
                if (rectangle.Position.Y + rectangle.Height >= Position.Y
                    && rectangle.Position.Y <= Position.Y + Height)
                    return true;
            return false;
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            bitmap.DrawRectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height, color);
        }
    }
}
