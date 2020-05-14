using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightControl
{
    public partial class MainWindow : Window
    {
        WriteableBitmap writeableBitmap;
        //Map map;
        public MainWindow()
        {
            InitializeComponent();

            //map = new Map("obstacles.txt");
            //textBlock.Text = map.ToString();

            //Debug.WriteLine(map[0]);

            int w = 500, h = 480;
            writeableBitmap = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
            image.Source = writeableBitmap;

            Terrain terrain = new Terrain("output.bmp");

            FastNoise fn = new FastNoise();
            writeableBitmap.Lock();
            unsafe
            {
                int* pBackBuffer = (int*)writeableBitmap.BackBuffer;
                for (int y = 0; y < h; ++y)
                {
                    for (int x = 0; x < w; ++x)
                    {
                        int height = terrain[x, y];
                        *(pBackBuffer++) = (255 << 24) | (height << 16) | (height << 8) | height;
                    }
                }
            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, w, h));
            writeableBitmap.Unlock();

            /*FastNoise fan = new FastNoise();
            for (int y = 0; y < 5; ++y)
            {
                for (int x = 0; x < 5; ++x)
                    Debug.WriteLine(100*fan.GetPerlin(x, y));
            }*/

            //line(0, 100, 100, 0, (255 << 24) | (255 << 8));
            /*for (int i = 0; i < map.GetNumberOfObstacles(); ++i)
            {
                for(int j = 0; j < map[i].Vertices.Length - 1; ++j)
                    DrawLine((int)map[i].Vertices[j].X,
                        (int)map[i].Vertices[j].Y,
                        (int)map[i].Vertices[j+1].X,
                        (int)map[i].Vertices[j+1].Y,
                        (255 << 24) | (255 << 8));
                DrawLine((int)map[i].Vertices[map[i].Vertices.Length - 1].X,
                    (int)map[i].Vertices[map[i].Vertices.Length - 1].Y,
                    (int)map[i].Vertices[0].X,
                    (int)map[i].Vertices[0].Y,
                    (255 << 24) | (255 << 8));
            }*/
        }

        //https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public void DrawLine(int x, int y, int x2, int y2, int color)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            writeableBitmap.Lock();
            for (int i = 0; i <= longest; i++)
            {
                //PutPixel(x, y, color);
                unsafe
                {
                    int* pBackBuffer = (int*)writeableBitmap.BackBuffer;
                    *(pBackBuffer + x + y*writeableBitmap.PixelWidth) = color;
                }
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0,
                writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            writeableBitmap.Unlock();
        }
    }
}
