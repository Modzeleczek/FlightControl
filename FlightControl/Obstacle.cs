using System.Collections.Generic;

namespace FlightControl
{
    class Obstacle
    {
        public double Height { get; private set; }
        public Line[] Walls { get; private set; }
        public Obstacle(double height, Line[] walls)
        {
            Height = height;
            Walls = new Line[walls.Length];
            for(int i = 0; i < Walls.Length; ++i)
                Walls[i] = new Line(walls[i]);
        }
        public bool Collides(Aircraft aircraft)
        {
            return false;
        }
        public override string ToString()
        {
            string result = $"(Obstacle: {Height}; ";
            foreach (var w in Walls)
                result += $"({w.Start.X},{w.Start.Y})->({w.End.X},{w.End.Y}); ";
            return result + ")";
        }
    }
}
