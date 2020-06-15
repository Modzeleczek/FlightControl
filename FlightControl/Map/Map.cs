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
        private List<Obstacle> Obstacles { get; set; }

        public Map(string fileName, WriteableBitmap bitmapToDraw)
        {
            /* Ładowanie nieruchomych przeszkód z pliku. */
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
                    int height = int.Parse(parts[1]),
                        x = int.Parse(parts[2]),
                        y = int.Parse(parts[3]),
                        squareSide = int.Parse(parts[4]);

                    if (x - squareSide / 2 < 0 || x + squareSide / 2 >= bitmapToDraw.PixelWidth)
                        throw new MapLoadingException("Obstacle's hitbox is out of bitmap's bounds (x).");
                    if (y - squareSide / 2 < 0 || y + squareSide / 2 >= bitmapToDraw.PixelHeight)
                        throw new MapLoadingException("Obstacle's hitbox is out of bitmap's bounds (y).");

                    if (type == 't')
                        Obstacles.Add(new Tree(new Point(x, y), squareSide, height, bitmapToDraw));
                    else if (type == 'b')
                        Obstacles.Add(new Building(new Point(x, y), squareSide, height, bitmapToDraw));
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
