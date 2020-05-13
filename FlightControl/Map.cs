using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightControl
{
    class Map
    {
        const int Width = 512, Height = 512;
        List<Obstacle> Obstacles;
        //Pixel[] Pixels;
        public Map(string fileName)
        {
            //Pixels = new Pixel[Width * Height];
            Obstacles = new List<Obstacle>();
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    if (reader.EndOfStream)
                        throw new Exception("File is empty.");
                    string line = reader.ReadLine();
                    if (!line.StartsWith("#"))
                        throw new Exception("File not beginning with #.");
                    do
                    {
                        if (reader.EndOfStream)
                            throw new Exception("No height after #.");
                        line = reader.ReadLine();
                        int h = int.Parse(line);
                        List<Point> obstaclePoints = new List<Point>();
                        while (true)
                        {
                            if (reader.EndOfStream)
                                throw new Exception("File not ending with #.");
                            line = reader.ReadLine();
                            if (!line.StartsWith("#"))
                            {
                                string[] parts = line.Split(';');
                                int x = int.Parse(parts[0]);
                                int y = int.Parse(parts[1]);
                                obstaclePoints.Add(new Point(x, y));
                            }
                            else
                                break;
                        }
                        Obstacles.Add(new Obstacle(h, obstaclePoints));
                        obstaclePoints.Clear();
                    } while (!reader.EndOfStream);
                }
            }
            else
                throw new Exception($"Cannot open file {fileName}.");
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
