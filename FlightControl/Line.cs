using FlightControl.Exceptions;
using System;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FlightControl
{
    public class Line
    {
        public Point Start, End;
        public Line(double x1, double y1, double x2, double y2)
        {
            if (x1 == x2 && y1 == y2)
                throw new LineIsPointException($"Start: {Start} and end: {End} are equal.");
            Start = new Point(x1, y1);
            End = new Point(x2, y2);
        }
        public Line(Line o) : this(o.Start.X, o.Start.Y, o.End.X, o.End.Y)
        {
        }
        public bool IsContinuedBy(Line next)
        {
            if (End.Equals(next.Start))
                return true;
            return false;
        }
        public void Move(double dx, double dy)
        {
            Start.Move(dx, dy);
            End.Move(dx, dy);
        }
        public override string ToString()
        {
            return $"(Line: ({Start.X},{Start.Y})->({End.X},{End.Y})); ";
        }

        //Algorithm: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        unsafe public void Draw(WriteableBitmap bitmap, int color)
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

            int* pBackBuffer = (int*)bitmap.BackBuffer;
            for (int i = 0; i <= longest; i++)
            {
                //PutPixel(x, y, color);
                *(pBackBuffer + x + y * bitmap.PixelWidth) = color;

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
            //Essential to update locked WriteableBitmap after editing its BackBuffer and before unlocking it.
            if (Start.X < End.X)
            {
                if (Start.Y < End.Y)
                    bitmap.AddDirtyRect(
                        new Int32Rect((int)Start.X, (int)Start.Y, (int)(End.X - Start.X), (int)(End.Y - Start.Y)
                        ));
                else
                    bitmap.AddDirtyRect(
                        new Int32Rect((int)Start.X, (int)End.Y, (int)(End.X - Start.X), (int)(Start.Y - End.Y)
                        ));
            }
            else
            {
                if (Start.Y < End.Y)
                    bitmap.AddDirtyRect(
                        new Int32Rect((int)End.X, (int)Start.Y, (int)(Start.X - End.X), (int)(End.Y - Start.Y)
                        ));
                else
                    bitmap.AddDirtyRect(
                        new Int32Rect((int)End.X, (int)End.Y, (int)(Start.X - End.X), (int)(Start.Y - End.Y)
                        ));
            }
        }
    }
}
