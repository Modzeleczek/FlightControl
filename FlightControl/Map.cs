using FlightControl.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Map
    {
        public List<Obstacle> Obstacles { get; protected set; }
        public Map(string fileName, WriteableBitmap bitmapToDraw)
        {
            /* Loading immobile obstacles from file. */
            Obstacles = new List<Obstacle>();
            if (!File.Exists(fileName))
                throw new MapLoadingException($"Cannot open file {fileName}.");
            using (StreamReader reader = new StreamReader(fileName))
            {
                if (reader.EndOfStream)
                    throw new MapLoadingException("Input file empty.");
                do
                {
                    string[] parts = reader.ReadLine().Split(';');
                    if (parts[0].Length != 1)
                        throw new MapLoadingException("Ambigious obstacle type.");

                    char type = parts[0][0];
                    int h = int.Parse(parts[1]),
                        x = int.Parse(parts[2]),
                        y = int.Parse(parts[3]),
                        rectangleWidth = int.Parse(parts[4]),
                        rectangleHeight = int.Parse(parts[5]);

                    if (x - rectangleWidth < 0 || x + rectangleWidth >= bitmapToDraw.PixelWidth)
                        throw new MapLoadingException("Obstacle's hitbox is out of bitmap's bounds (x).");
                    if (y - rectangleHeight < 0 || y + rectangleHeight >= bitmapToDraw.PixelHeight)
                        throw new MapLoadingException("Obstacle's hitbox is out of bitmap's bounds (y).");

                    Rectangle hitbox = new Rectangle(new Point(x, y), rectangleWidth, rectangleHeight);

                    if (type == 't')
                        Obstacles.Add(new Tree(h, hitbox, bitmapToDraw));
                    else if (type == 'b')
                        Obstacles.Add(new Building(h, hitbox, bitmapToDraw));
                    else
                        throw new MapLoadingException("Unspecified obstacle type.");
                } while (!reader.EndOfStream);
            }
        }
    }
}
