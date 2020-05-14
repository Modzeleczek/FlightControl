using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    class Obstacle
    {
        public double Height;
        private ClosedCurve Walls;
        public Obstacle(double height, ClosedCurve walls)
        {
            Height = height;
            Walls = new ClosedCurve(walls);
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
