using FlightControl.Exceptions;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Curve
    {
        protected List<Line> Lines;
        public Line this[int index]
        {
            get
            {
                return Lines[index];
            }
        }
        public int LinesCount
        {
            get
            {
                return Lines.Count;
            }
        }
        public Curve(List<Line> lines)
        {
            if (lines.Count == 0)
                throw new NotEnoughElementsException("Lines array is 0 length.");

            Lines = new List<Line>(lines.Count);
            Lines.Add(new Line(lines[0]));
            for (int i = 1; i < lines.Count; ++i)
            {
                if (!lines[i - 1].IsContinuedBy(lines[i]))
                    throw new LinesNotConnectedException($"{lines[i - 1]} is not continued by {lines[i]}");
                Lines.Add(new Line(lines[i]));
            }
        }
        public Curve(List<Point> points)
        {
            if (points.Count < 2)
                throw new NotEnoughElementsException("Points array length is less than 2.");

            Lines = new List<Line>(points.Count - 1);
            Lines.Add(new Line(points[0].X, points[0].Y, points[1].X, points[1].Y));
            for (int i = 1; i < points.Count - 1; ++i)
            {
                Line line = new Line(points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
                if (!Lines[i - 1].IsContinuedBy(line))
                    throw new LinesNotConnectedException($"{Lines[i - 1]} is not continued by {line}");
                Lines.Add(line);
            }
        }
        public Curve(Curve o)
        {
            Lines = new List<Line>(o.Lines.Count);
            for (int i = 0; i < o.Lines.Count; ++i)
                Lines.Add(new Line(o.Lines[i]));
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            foreach (var line in Lines)
                line.Draw(bitmap, color);
        }
        public override string ToString()
        {
            string result = "(Curve: ";
            foreach (var line in Lines)
                result += line.ToString() + "; ";
            return result + "); ";
        }
    }
}
