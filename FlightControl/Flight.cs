using FlightControl.Exceptions;
using System;
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
        public Flight(Flight o) : this(o.Stages)
        {
        }
        public void AppendStage(Point destination, double velocity, double altitude)
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
        public static Flight GetRandom(int stagesCount, int mapWidth, int mapHeight, 
            double fromVelocity, double toVelocity, 
            double fromAltitude, double toAltitude,
            Random rng)
        {
            Flight result = new Flight();
            int x, y, prevX = rng.Next(0, mapWidth), prevY = rng.Next(0, mapHeight);
            for (int i = 0; i < stagesCount; ++i)
            {
                x = rng.Next(0, mapWidth);
                y = rng.Next(0, mapHeight);
                result.Stages.Add(new Stage(
                    new Line(prevX, prevY,
                    x, y),
                    fromVelocity + toVelocity * rng.NextDouble(),
                    fromAltitude + toAltitude * rng.NextDouble()));
                prevX = x;
                prevY = y;
            }
            return result;
        }
    }
}
