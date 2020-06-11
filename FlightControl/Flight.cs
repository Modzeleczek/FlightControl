using FlightControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Flight : IDisposable
    {
        private List<Stage> Stages;
        private Flight()
        {
            Stages = new List<Stage>();
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
        public Flight(Flight o) : this(o.Stages) { }
        public void AppendStage(Point destination, double velocity, double altitude)
        {
            Line line = new Line(Stages[Stages.Count - 1].Track.End.X, Stages[Stages.Count - 1].Track.End.Y,
                destination.X, destination.Y);
            Stages.Add(new Stage(line, velocity, altitude));
        }

        public int StagesCount => Stages.Count;
        public bool RemoveStage(int index)
        {
            Stages.RemoveAt(index);
            return Stages.Count > 0;
        }
        public Stage ActualStage => Stages[0];

        public void ScaleVelocity(double factor)
        {
            foreach (var stage in Stages)
                stage.ScaleVelocity(factor);
        }
        public void Draw(WriteableBitmap bitmap, int color)
        {
            foreach (var stage in Stages)
                stage.Draw(bitmap, color);
        }
        public static Flight GetRandom(
            int stagesCount,
            int beginX, int beginY,
            int endX, int endY,
            double fromVelocity, double toVelocity, 
            double fromAltitude, double toAltitude,
            Random rng)
        {
            Flight result = new Flight();
            int x, y, prevX = rng.Next(beginX, endX), prevY = rng.Next(beginY, endY);
            for (int i = 0; i < stagesCount; ++i)
            {
                x = rng.Next(beginX, endX);
                y = rng.Next(beginY, endY);
                result.Stages.Add(new Stage(
                    new Line(prevX, prevY, x, y),
                    fromVelocity + toVelocity * rng.NextDouble(),
                    fromAltitude + toAltitude * rng.NextDouble()));
                prevX = x;
                prevY = y;
            }
            return result;
        }

        public void Dispose()
        {
            for (int i = 0; i < Stages.Count; ++i)
            {
                Stages[i].Dispose();
                Stages[i] = null;
            }
            Stages.Clear();
            Stages = null;
        }
    }
}
