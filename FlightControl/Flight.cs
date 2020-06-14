using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public partial class Flight : IDisposable
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
            Editor editor = new Editor();
            for (int i = 0; i < stagesCount; ++i)
                editor.AddLast(
                    rng.Next(beginX, endX),
                    rng.Next(beginY, endY),
                    fromVelocity + (toVelocity - fromVelocity) * rng.NextDouble(),
                    fromAltitude + (toAltitude - fromAltitude) * rng.NextDouble());
            return editor.Route;
        }

        public partial class Editor
        {
            private abstract partial class Change { }
            private abstract partial class Deletion : Change { }
            private partial class FirstDeletion : Deletion { }
            private partial class MiddleDeletion : Deletion { }
            private partial class LastDeletion : Deletion { }
            private partial class LastAddition : Change { }
            private abstract partial class Replacement : Change { }
            private partial class FirstReplacement : Replacement { }
            private partial class MiddleReplacement : Replacement { }
            private partial class LastReplacement : Replacement { }

            private Stack<Change> Changes = new Stack<Change>();
            public Flight Route { get; private set; }
            private Point End;

            public Editor() => Route = new Flight();
            public Editor(Flight flight)
            {
                Route = flight;
                End = new Point(flight.Stages[flight.Stages.Count - 1].Track.End);
            }

            public void Undo() => Changes.Pop().Undo(this);

            public int Count => Route.Stages.Count;

            public void AddLast(double x, double y, double velocity, double altitude)
            {
                if (End == null)
                    End = new Point(x, y);
                else
                {
                    Changes.Push(new LastAddition());
                    Line line = new Line(End.X, End.Y, x, y);
                    Route.Stages.Add(new Stage(line, velocity, altitude));
                    End.X = x;
                    End.Y = y;
                }
            }

            public void DeleteFirst()
            {
                Changes.Push(new FirstDeletion(Route.Stages[0]));
                Route.Stages.RemoveAt(0);
                if (Route.Stages.Count == 0)
                    End = null;
            }

            public void DeleteMiddle(int index)
            {
                Changes.Push(new MiddleDeletion(Route.Stages[index], index));
                Route.Stages[index - 1].Track.End.X = Route.Stages[index + 1].Track.Start.X;
                Route.Stages[index - 1].Track.End.Y = Route.Stages[index + 1].Track.Start.Y;
                Route.Stages.RemoveAt(index);
            }

            public void DeleteLast()
            {
                Changes.Push(new LastDeletion(Route.Stages[Route.Stages.Count - 1]));
                Route.Stages.RemoveAt(Route.Stages.Count - 1);
                if (Route.Stages.Count == 0)
                    End = null;
                else
                {
                    End.X = Route.Stages[Route.Stages.Count - 1].Track.End.X;
                    End.Y = Route.Stages[Route.Stages.Count - 1].Track.End.Y;
                }
            }

            public void ReplaceFirst(double x, double y)
            {
                Changes.Push(new FirstReplacement(Route.Stages[0].Track.Start.X, Route.Stages[0].Track.Start.Y));
                Route.Stages[0].Track.Start.X = x;
                Route.Stages[0].Track.Start.Y = y;
            }

            public void ReplaceMiddle(double x, double y, int index)
            {
                Changes.Push(new MiddleReplacement(Route.Stages[index].Track.Start.X, Route.Stages[index].Track.Start.Y, index));
                Route.Stages[index].Track.Start.X = x;
                Route.Stages[index].Track.Start.Y = y;
                Route.Stages[index - 1].Track.End.X = x;
                Route.Stages[index - 1].Track.End.Y = y;
            }

            public void ReplaceLast(double x, double y)
            {
                int index = Route.Stages.Count;
                Changes.Push(new LastReplacement(Route.Stages[index - 1].Track.End.X, Route.Stages[index - 1].Track.End.Y));
                Route.Stages[index - 1].Track.End.X = x;
                Route.Stages[index - 1].Track.End.Y = y;
                End.X = x;
                End.Y = y;
            }
        }
    }
}
