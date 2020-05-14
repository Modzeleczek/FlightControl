using FlightControl.Exceptions;

namespace FlightControl
{
    class ClosedCurve : Curve
    {
        public ClosedCurve(Line[] lines) : base(lines)
        {
            if (!Lines[Lines.Count - 1].IsContinuedBy(Lines[0]))
                throw new LinesNotConnectedException($"{Lines[Lines.Count - 1]} is not continued by {Lines[0]}," + 
                    $"so the curve is not closed.");
        }
        public ClosedCurve(Point[] points) : base(points)
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
