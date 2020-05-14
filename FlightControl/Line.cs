

using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    class Line
    {
        public Point Start, End;
        public Line(double x1, double y1, double x2, double y2)
        {
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }
        public Line(Line o) : this(o.Start.X, o.Start.Y, o.End.X, o.End.Y)
        {
        }

        //https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public void Draw(WriteableBitmap bitmap, int color)
        {
            //(int x, int y, int x2, int y2, int color)
            int x = (int)Start.X, y = (int)Start.Y, x2 = (int)End.X, y2 = (int)End.Y;
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

            for (int i = 0; i <= longest; i++)
            {
                //PutPixel(x, y, color);
                unsafe
                {
                    int* pBackBuffer = (int*)bitmap.BackBuffer;
                    *(pBackBuffer + x + y * bitmap.PixelWidth) = color;
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
        }
        public static Line[] CurveFromPoints(Point[] points)
        {
            if (points.Length == 0)
                throw new System.Exception("Points array is 0 length.");

            Line[] curve = new Line[points.Length];
            for (int i = 0; i < curve.Length - 1; ++i)
            {
                curve[i] = new Line(points[i].X, points[i].Y,
                    points[i + 1].X, points[i + 1].Y);
            }
            curve[curve.Length - 1] = new Line(points[points.Length - 1].X, points[points.Length - 1].Y,
                points[0].X, points[0].X);
            return curve;
        }
    }
}
