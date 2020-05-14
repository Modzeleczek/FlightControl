using FlightControl.Exceptions;
using System.Collections.Generic;

namespace FlightControl
{
    class ClosedCurve : Curve
    {
        public ClosedCurve(List<Line> lines) : base(lines)
        {
            if (!Lines[Lines.Count - 1].IsContinuedBy(Lines[0]))
                throw new LinesNotConnectedException($"{Lines[Lines.Count - 1]} is not continued by {Lines[0]}," + 
                    $"so the curve is not closed.");
        }
        public ClosedCurve(List<Point> points) : base(points)
        {
            if (!Lines[Lines.Count - 1].IsContinuedBy(Lines[0]))
                throw new LinesNotConnectedException($"{Lines[Lines.Count - 1]} is not continued by {Lines[0]}, " +
                    $"so the curve is not closed.");
        }
        public ClosedCurve(ClosedCurve o) : base(o)
        {
        }
    }
}
