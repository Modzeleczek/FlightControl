using FlightControl.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Map
    {
        public readonly int Width, Height;
        private List<Obstacle> Obstacles;
        public Map(string fileName, int width, int height)
        {
            Width = width;
            Height = height;

            Obstacles = new List<Obstacle>();
            if (!File.Exists(fileName))
                throw new MapLoadingException($"Cannot open file {fileName}.");
            using (StreamReader reader = new StreamReader(fileName))
            {
                if (reader.EndOfStream)
                    throw new MapLoadingException("Input file empty.");
                string line = reader.ReadLine();
                if (!line.StartsWith("#"))
                    throw new MapLoadingException("Input file not beginning with #.");
                do
                {
                    if (reader.EndOfStream)
                        throw new MapLoadingException("No height after #.");
                    line = reader.ReadLine();
                    int h = int.Parse(line);
                    List<Point> obstacleVertices = new List<Point>();
                    while (true)
                    {
                        if (reader.EndOfStream)
                            throw new MapLoadingException("Input file not ending with #.");
                        line = reader.ReadLine();
                        if (!line.StartsWith("#"))
                        {
                            string[] parts = line.Split(';');
                            if (parts.Length == 2)
                            {
                                int x = int.Parse(parts[0]);
                                int y = int.Parse(parts[1]);
                                obstacleVertices.Add(new Point(x, y));
                            }
                            else
                                throw new MapLoadingException("Missing coordinates.");
                        }
                        else
                            break;
                    }
                    obstacleVertices.Add(new Point(obstacleVertices[0]));
                    Obstacles.Add(new Obstacle(h, new Polygon(obstacleVertices)));
                    obstacleVertices.Clear();
                } while (!reader.EndOfStream);
            }
        }
        public Obstacle this[int index]
        {
            get
            {
                return Obstacles[index];
            }
        }
        public int ObstaclesCount
        {
            get
            {
                return Obstacles.Count;
            }
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            foreach (var obstacle in Obstacles)
                obstacle.Draw(bitmap, color);
        }
        public override string ToString()
        {
            string result = $"(Map: {Width}x{Height}; ";
            foreach (var obstacle in Obstacles)
                result += obstacle.ToString() + "; ";
            return result + ")";
        }
    }
}
