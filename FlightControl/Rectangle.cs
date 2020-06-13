using System.Windows.Media.Imaging;
using System;

namespace FlightControl
{
    public class Rectangle : IDisposable
    {
        public Point Position { get; private set; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle(Point position, double width, double height)
        {
            Position = new Point(position.X - width / 2, position.Y - height / 2);
            Width = (int)width;
            Height = (int)height;
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

        unsafe public void Draw(WriteableBitmap bitmap, int color)
        {
            if(Position.X >= 0 && Position.X + Width < bitmap.PixelWidth)
            {
                if(Position.Y >= 0 && Position.Y + Height < bitmap.PixelHeight)
                {
                    int bitmapWidth = bitmap.PixelWidth;
                    int* p1, p2;
                    p1 = (int*)bitmap.BackBuffer + (int)Position.X + bitmapWidth * (int)Position.Y;
                    p2 = p1 + bitmapWidth * (Height - 1);
                    for (int x = Width - 1; x >= 0; --x)
                    {
                        *p1 = *p2 = color;
                        ++p1;
                        ++p2;
                    }

                    p1 = (int*)bitmap.BackBuffer + (int)Position.X + bitmapWidth * (int)Position.Y +  bitmapWidth;
                    p2 = p1 + Width - 1;
                    for (int y = Height - 2; y >= 1; --y)
                    {
                        *p1 = *p2 = color;
                        p1 += bitmapWidth;
                        p2 += bitmapWidth;
                    }
                    bitmap.AddDirtyRect(new System.Windows.Int32Rect((int)Position.X, (int)Position.Y, Width, Height));
                }
            }
        }

        public void Dispose() => Position = null;
    }
}
