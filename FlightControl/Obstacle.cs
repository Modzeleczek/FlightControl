using System.Windows.Media.Imaging;
using System;

namespace FlightControl
{
    public abstract class Obstacle : IDisposable
    {
        public double Height { get; protected set; }
        public Rectangle Hitbox { get; protected set; }
        protected Obstacle(double height, Rectangle hitbox)
        {
            Height = height;
            Hitbox = new Rectangle(hitbox);
        }
        public void Dispose()
        {
            Hitbox.Dispose();
            Hitbox = null;
        }
    }

    public class Building : Obstacle
    {
        public Building(double height, Rectangle hitbox, WriteableBitmap bitmap) : base(height, hitbox)
        {
            Hitbox.Draw(bitmap, (255 << 24) | (128 << 16) | (128 << 8) | 128);
        }
    }

    public class Tree : Obstacle
    {
        public Tree(double height, Rectangle hitbox, WriteableBitmap bitmap) : base(height, hitbox)
        {
            Hitbox.Draw(bitmap, (255 << 24) | (255 << 8));
        }
    }
}