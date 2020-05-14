﻿using System;
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
        Map map;
        public MainWindow()
        {
            InitializeComponent();

            int w = 500, h = 480;
            writeableBitmap = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
            image.Source = writeableBitmap;

            Terrain terrain = new Terrain();

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

            writeableBitmap.Lock();

            map = new Map("obstacles.txt");
            Debug.WriteLine(map);

            //line(0, 100, 100, 0, (255 << 24) | (255 << 8));
            for (int i = 0; i < map.GetNumberOfObstacles(); ++i)
            {
                foreach(var wall in map[i].Walls)
                    wall.Draw(writeableBitmap, (255 << 24) | (255 << 8));
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0,
                writeableBitmap.PixelWidth, writeableBitmap.PixelHeight));
            writeableBitmap.Unlock();
        }
    }
}