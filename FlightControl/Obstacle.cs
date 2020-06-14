using System.Windows.Media.Imaging;
using System;

namespace FlightControl
{
    public abstract class Obstacle : Collidable, IDisposable
    {
        public int Height { get; protected set; }
        protected Obstacle(Point position, int size, int height) : base(position, size) => Height = height;

        public void Dispose()
        {
            Hitbox.Dispose();
            Hitbox = null;
        }
    }

    public class Building : Obstacle
    {
        public Building(Point position, int size, int height, WriteableBitmap bitmapToDraw) : base(position, size, height) =>
            Hitbox.Draw(bitmapToDraw, (255 << 24) | (128 << 16) | (128 << 8) | 128);
    }

    public class Tree : Obstacle
    {
        public Tree(Point position, int size, int height, WriteableBitmap bitmapToDraw) : base(position, size, height) =>
            Hitbox.Draw(bitmapToDraw, (255 << 24) | (255 << 8));
    }
}