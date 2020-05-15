using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Obstacle
    {
        public double Height;
        private Polygon Walls;
        public Obstacle(double height, Polygon walls)
        {
            Height = height;
            Walls = new Polygon(walls);
        }
        public bool Collides(Aircraft aircraft)
        {
            return false;
        }
        public override string ToString()
        {
            return $"(Obstacle: Height: {Height}; {Walls}); ";
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Walls.Draw(bitmap, color);
        }
    }
}
