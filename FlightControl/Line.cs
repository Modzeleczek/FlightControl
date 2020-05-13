

namespace FlightControl
{
    class Line
    {
        public Point A, B;
        public Line(double x1, double y1, double x2, double y2)
        {
            A = new Point(x1, y1);
            B = new Point(x2, y2);
        }
    }
}
