using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    class Obstacle
    {
        public double Height;
        public ClosedCurve Walls;
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
            /*string result = $"(Obstacle: {Height}; ";
            for(int i = 0; i < Walls.LinesCount; ++i)
                result += $"({Walls[i].Start.X},{Walls[i].Start.Y})->({Walls[i].End.X},{Walls[i].End.Y}); ";
            return result + ")";*/
            return $"(Obstacle: Height: {Height}; {Walls}); ";
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            Walls.Draw(bitmap, color);
        }
    }
}
