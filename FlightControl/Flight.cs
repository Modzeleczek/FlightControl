using FlightControl.Exceptions;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Flight : IDisposable
    {
        private List<Stage> Stages;

        private Flight() => Stages = new List<Stage>();
        public Flight(Flight o)
        {
            Stages = new List<Stage>(o.Stages.Count);
            for (int i = 0; i < o.Stages.Count; ++i)
                Stages.Add(new Stage(o.Stages[i]));
        }

        public bool RemoveCurrent()
        {
            Stages.RemoveAt(0);
            return Stages.Count > 0;
        }

        public Stage CurrentStage => Stages[0];

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

        public static Flight GetRandom(
            int stagesCount,
            int beginX, int beginY,
            int endX, int endY,
            double fromVelocity, double toVelocity,
            double fromAltitude, double toAltitude,
            Random rng)
        {
            Builder builder = new Builder();
            for (int i = 0; i < stagesCount; ++i)
                builder.Add(
                    rng.Next(beginX, endX),
                    rng.Next(beginY, endY),
                    fromVelocity + (toVelocity - fromVelocity) * rng.NextDouble(),
                    fromAltitude + (toAltitude - fromAltitude) * rng.NextDouble());
            return builder.Route;
        }

        public class Builder
        {
            public Flight Route { get; protected set; } = new Flight();

            protected Point End;

            public int Count => Route.Stages.Count;

            public void Add(double x, double y, double velocity, double altitude)
            {
                if (End == null)
                    End = new Point(x, y);
                else
                {
                    Line line = new Line(End.X, End.Y, x, y);
                    Route.Stages.Add(new Stage(line, velocity, altitude));
                    End.X = x;
                    End.Y = y;
                }
            }

            public void RemoveLast()
            {
                if (End != null)
                {
                    Route.Stages[Route.Stages.Count - 1].Dispose();
                    Route.Stages[Route.Stages.Count - 1] = null;
                    Route.Stages.RemoveAt(Route.Stages.Count - 1);
                    if (Route.Stages.Count == 0)
                        End = null;
                    else
                    {
                        End.X = Route.Stages[Route.Stages.Count - 1].Track.End.X;
                        End.Y = Route.Stages[Route.Stages.Count - 1].Track.End.Y;
                    }
                }
            }

            public void ReplaceLast(double x, double y)
            {
                Route.Stages[Route.Stages.Count - 1].Track.End.X = x;
                Route.Stages[Route.Stages.Count - 1].Track.End.Y = y;
                End.X = x;
                End.Y = y;
            }
        }
    }
}
