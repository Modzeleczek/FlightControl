using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public static class WriteableBitmapExtensions
    {
        unsafe public static void DrawPoint(this WriteableBitmap bitmap, int x, int y, int color)
        {
            *((int*)bitmap.BackBuffer + x + y * bitmap.PixelWidth) = color;
            bitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
        }
        //Algorithm: https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        unsafe public static void DrawLine(this WriteableBitmap bitmap, int x1, int y1, int x2, int y2, int color)
        {
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
        public static void DrawRectangle(this WriteableBitmap bitmap, int x, int y, int width, int height, int color)
        {
            bitmap.DrawLine(
                x, y,
                x + width, y,
                color);
            bitmap.DrawLine(
                x + width, y,
                x + width, y + height,
                color);
            bitmap.DrawLine(
                x + width, y + height,
                x, y + height,
                color);
            bitmap.DrawLine(
                x, y + height,
                x, y,
                color);
        }
    }
}
