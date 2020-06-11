using FlightControl.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using System;
using System.Collections;

namespace FlightControl
{
    public class Map : IDisposable, IEnumerable<Obstacle>
    {
        protected List<Obstacle> Obstacles { get; set; }
        public Map(string fileName, WriteableBitmap bitmapToDraw)
        {
            /* Loading immobile obstacles from file. */
            if (!File.Exists(fileName))
                throw new MapLoadingException($"Cannot open file {fileName}.");
            Obstacles = new List<Obstacle>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                if (reader.EndOfStream)
                    throw new MapLoadingException("Input file empty.");
                do
                {
                    string[] parts = reader.ReadLine().Split(';');
                    if (parts[0].Length != 1)
                        throw new MapLoadingException("Ambiguous obstacle type.");

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
                        throw new MapLoadingException("Unknown obstacle type.");
                } while (!reader.EndOfStream);
            }
        }

        public void Dispose()
        {
            for (int i = 0; i < Obstacles.Count; ++i)
            {
                Obstacles[i].Dispose();
                Obstacles[i] = null;
            }
            Obstacles.Clear();
            Obstacles = null;
        }

        public IEnumerator<Obstacle> GetEnumerator() => Obstacles.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Obstacles.GetEnumerator();
    }
}
