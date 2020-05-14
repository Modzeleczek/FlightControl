using FlightControl.Exceptions;
using System.Collections.Generic;
using System.IO;

namespace FlightControl
{
    class Map
    {
        public readonly int Width = 512, Height = 512;
        private List<Obstacle> Obstacles;
        public Map(string fileName)
        {
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
                    List<Point> obstaclePoints = new List<Point>();
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
                                obstaclePoints.Add(new Point(x, y));
                            }
                            else
                                throw new MapLoadingException("Missing coordinates.");
                        }
                        else
                            break;
                    }
                    Obstacles.Add(new Obstacle(h, obstaclePoints));
                    obstaclePoints.Clear();
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

        public int GetNumberOfObstacles()
        {
            return Obstacles.Count;
        }

        public override string ToString()
        {
            string result = $"(Map: {Width}x{Height}; ";
            foreach (var o in Obstacles)
            {
                result += o.ToString() + "; ";
            }
            return result + ")";
        }
    }
}
