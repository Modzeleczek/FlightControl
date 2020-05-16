using FlightControl.Exceptions;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Flight
    {
        private List<Stage> Stages;
        public Stage this[int index]
        {
            get
            {
                return Stages[index];
            }
        }
        public Flight(List<Stage> stages)
        {
            if (stages.Count == 0)
                throw new NotEnoughElementsException("Flight cannot be created, because stages' list is empty.");

            Stages = new List<Stage>(stages.Count);
            Stages.Add(new Stage(stages[0]));
            for (int i = 1; i < stages.Count; ++i)
            {
                if(!stages[i - 1].Track.IsContinuedBy(stages[i].Track))
                    throw new LinesNotConnectedException($"{stages[i - 1].Track} is not continued by {stages[i].Track}");
                Stages.Add(new Stage(stages[i]));
            }
        }
        public Flight(Flight o) : this(o.Stages)
        {
        }
        public void AddStage(Point destination, double velocity, double altitude)
        {
            Line line = new Line(Stages[Stages.Count - 1].Track.End.X, Stages[Stages.Count - 1].Track.End.Y,
                destination.X, destination.Y);
            Stages.Add(new Stage(line, velocity, altitude));
        }
        public bool RemoveStage(int index)
        {
            Stages.RemoveAt(index);
            if (Stages.Count == 0)
                return false;
            return true;
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            foreach (var stage in Stages)
                stage.Draw(bitmap, color);
        }
    }
}
