using System.Windows.Media.Imaging;
using System;

namespace FlightControl
{
    public class Square : IDisposable
    {
        public Point Position { get; private set; }
        public int Side { get; }

        public Square(Point position, int side)
        {
            Position = new Point(position.X - side / 2, position.Y - side / 2);
            Side = side;
        }
        public Square(Square o) : this(o.Position, o.Side) { }

        public double DistanceBetween(Square square)
        {
            double distance = 0;
            if(!(Position.X + Side > square.Position.X && Position.X < square.Position.X + square.Side))//nie przecinają się w osi X
                distance += Math.Abs(Position.X + Side / 2 - (square.Position.X + square.Side / 2)) - (Side + square.Side) / 2;
            if (!(Position.Y + Side > square.Position.Y && Position.Y < square.Position.Y + square.Side))//nie przecinają się w osi Y
                distance += Math.Abs(Position.Y + Side / 2 - (square.Position.Y + square.Side / 2)) - (Side + square.Side) / 2;
            return distance;
        }

        public void Place(Point centerPosition)
        {
            Position.X = centerPosition.X - Side / 2;
            Position.Y = centerPosition.Y - Side / 2;
        }

        unsafe public void Draw(WriteableBitmap bitmap, int color)
        {
            if(Position.X >= 0 && Position.X + Side < bitmap.PixelWidth)
            {
                if(Position.Y >= 0 && Position.Y + Side < bitmap.PixelHeight)
                {
                    int bitmapWidth = bitmap.PixelWidth;
                    int* p1, p2;
                    p1 = (int*)bitmap.BackBuffer + (int)Position.X + bitmapWidth * (int)Position.Y;//lewy górny róg
                    p2 = p1 + bitmapWidth * (Side - 1);//lewy dolny róg
                    for (int x = Side - 1; x >= 0; --x)
                    {
                        *p1 = *p2 = color;
                        ++p1;
                        ++p2;
                    }

                    p1 = (int*)bitmap.BackBuffer + (int)Position.X + bitmapWidth * (int)Position.Y +  bitmapWidth;//lewy górny róg + 1 wiersz
                    p2 = p1 + Side - 1;//prawy górny róg + 1 wiersz
                    for (int y = Side - 2; y >= 1; --y)
                    {
                        *p1 = *p2 = color;
                        p1 += bitmapWidth;
                        p2 += bitmapWidth;
                    }
                    bitmap.AddDirtyRect(new System.Windows.Int32Rect((int)Position.X, (int)Position.Y, Side, Side));
                }
            }
        }

        public void Dispose() => Position = null;
    }
}
