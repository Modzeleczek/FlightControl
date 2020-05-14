using FlightControl.Exceptions;
using System.IO;
using System.Drawing;

namespace FlightControl
{
    class Terrain
    {
        public readonly int Width = 512, Height = 512;
        private int[] Heights;
        public int this[int x, int y]
        {
            get
            {
                return Heights[x + y * Width];
            }
        }
        public Terrain()
        {
            Heights = new int[Width * Height];
            Generate();
        }
        public Terrain(string inputFileName)
        {
            Load(inputFileName);
        }
        private void Generate()
        {
            FastNoise fastNoise = new FastNoise();
            fastNoise.SetCellularDistanceFunction(FastNoise.CellularDistanceFunction.Euclidean);
            fastNoise.SetCellularReturnType(FastNoise.CellularReturnType.Distance2Sub);
            fastNoise.SetFrequency(0.01);
            fastNoise.SetInterp(FastNoise.Interp.Quintic);

            fastNoise.SetFractalOctaves(5);
            fastNoise.SetFractalLacunarity(2.0);
            fastNoise.SetFractalGain(0.5);

            fastNoise.SetFractalType(FastNoise.FractalType.FBM);
            int i = 0;
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Heights[i++] = 20 + (int)(64 * fastNoise.GetSimplexFractal(x, y));
                }
            }
        }
        public void Save(string fileName)
        {
            using (Bitmap bitmap = new Bitmap(Width, Height))
            {
                int i = 0;
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        int color = Heights[i++];
                        if (color < 0)
                            color = 0;
                        else if (color > 255)
                            color = 255;
                        bitmap.SetPixel(x, y, Color.FromArgb(255, color, color, color));
                    }
                }
                bitmap.Save(fileName);
            }
        }

        public void Load(string fileName)
        {
            if (!File.Exists(fileName))
                throw new TerrainLoadingException($"Cannot open file {fileName}.");

            using (Bitmap bitmap = new Bitmap(fileName))
            {
                Heights = new int[Width * Height];
                if (bitmap.Width != Width)
                    throw new TerrainLoadingException($"BMP width has to be {Width}.");
                else if(bitmap.Height != Height)
                    throw new TerrainLoadingException($"BMP height has to be {Height}.");

                int i = 0;
                for(int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                        Heights[i++] = bitmap.GetPixel(x, y).B;
                }
            }
        }
    }
}
