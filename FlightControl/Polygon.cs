using FlightControl.Exceptions;
using System.Collections.Generic;

namespace FlightControl
{
    public class Polygon : Curve
    {
        public Polygon(List<Line> lines) : base(lines)
        {
            if (!Lines[Lines.Count - 1].IsContinuedBy(Lines[0]))
                throw new LinesNotConnectedException($"{Lines[Lines.Count - 1]} is not continued by {Lines[0]}, " + 
                    "so the polygon is not closed.");
        }
        public Polygon(List<Point> points) : base(points)
        {
            if (!Lines[Lines.Count - 1].IsContinuedBy(Lines[0]))
                throw new LinesNotConnectedException($"{Lines[Lines.Count - 1]} is not continued by {Lines[0]}, " +
                    "so the polygon is not closed.");
        }
        public Polygon(Polygon o) : base(o)
        {
        }
    }
}
