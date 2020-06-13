using FlightControl.Exceptions;
using System.Windows.Media.Imaging;
using System;
using System.Windows;

namespace FlightControl
{
    public class Line : IDisposable
    {
        public Point Start { get; private set; }
        public Point End { get; private set; }

        public Line(double x1, double y1, double x2, double y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }
        public Line(Line o) : this(o.Start.X, o.Start.Y, o.End.X, o.End.Y) { }

        public bool IsContinuedBy(Line next) => End.Equals(next.Start);

        public override string ToString() => $"(Line: ({Start.X},{Start.Y})->({End.X},{End.Y}))";

        //Algorithm: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        unsafe public void Draw(WriteableBitmap bitmap, int color)
        {
            int x1 = (int)Start.X, y1 = (int)Start.Y, x2 = (int)End.X, y2 = (int)End.Y;
            
            int x = x1, y = y1;
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

            int width = bitmap.PixelWidth;
            //int* pBackBuffer = (int*)bitmap.BackBuffer;// + x + y * width;
            int* pBackBuffer = (int*)bitmap.BackBuffer + x + y * width;
            for (int i = 0; i <= longest; i++)
            {
                //*(pBackBuffer + x + y * width) = color;
                *pBackBuffer = color;

                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    //x += dx1;
                    pBackBuffer += dx1;
                    //y += dy1;
                    pBackBuffer += width * dy1;
                }
                else
                {
                    //x += dx2;
                    pBackBuffer += dx2;
                    //y += dy2;
                    pBackBuffer += width * dy2;
                }
            }
            //It is essential to update locked WriteableBitmap after editing its BackBuffer and before unlocking it.
            if (w >= 0)
            {
                if (h >= 0)
                    bitmap.AddDirtyRect(new Int32Rect(x1, y1, w + 1, h + 1));
                else
                    bitmap.AddDirtyRect(new Int32Rect(x1, y2, w + 1, -h + 1));
            }
            else
            {
                if (h >= 0)
                    bitmap.AddDirtyRect(new Int32Rect(x2, y1, -w + 1, h + 1));
                else
                    bitmap.AddDirtyRect(new Int32Rect(x2, y2, -w + 1, -h + 1));
            }
        }

        public double Length => Math.Sqrt(Math.Pow(Start.X - End.X, 2.0) + Math.Pow(Start.Y - End.Y, 2.0));

        public void Dispose()
        {
            Start = null;
            End = null;
        }
    }
}
