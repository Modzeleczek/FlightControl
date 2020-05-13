using System.Collections.Generic;

namespace FlightControl
{
    class Obstacle
    {
        public double Height { get; private set; }
        public Point[] Vertices { get; private set; }
        public Obstacle(double height, List<Point> vertices)
        {
            Height = height;
            Vertices = new Point[vertices.Count];
            for(int i = 0; i < Vertices.Length; ++i)
            {
                Vertices[i] = new Point(vertices[i]);
            }
        }
        public bool Collides(Aircraft aircraft)
        {
            return false;
        }
        public override string ToString()
        {
            string result = $"(Obstacle: {Height}; ";
            foreach (var v in Vertices)
            {
                result += $"{v.X},{v.Y}; ";
            }
            return result + ")";
        }
    }
}
