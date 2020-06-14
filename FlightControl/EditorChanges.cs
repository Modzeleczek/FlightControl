using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightControl
{
    public partial class Flight {
        public partial class Editor
        {
            private abstract partial class Change
            {
                public abstract void Undo(Editor editor);
            }
            private abstract partial class Deletion : Change
            {
                protected Stage Deleted;
                public Deletion(Stage deleted) => Deleted = deleted;
            }
            private partial class FirstDeletion : Deletion
            {
                public FirstDeletion(Stage deleted) : base(deleted) { }
                public override void Undo(Editor editor) => editor.Route.Stages.Insert(0, Deleted);
            }
            private partial class MiddleDeletion : Deletion
            {
                private int Index;
                public MiddleDeletion(Stage deleted, int index) : base(deleted)
                {
                    Deleted = deleted;
                    Index = index;
                }
                public override void Undo(Editor editor)
                {
                    List<Stage> stages = editor.Route.Stages;
                    stages.Insert(Index, Deleted);
                    stages[Index - 1].Track.End.X = Deleted.Track.Start.X;
                    stages[Index - 1].Track.End.Y = Deleted.Track.Start.Y;
                    stages[Index + 1].Track.Start.X = Deleted.Track.End.X;
                    stages[Index + 1].Track.Start.Y = Deleted.Track.End.Y;
                }
            }
            private partial class LastDeletion : Deletion
            {
                public LastDeletion(Stage deleted) : base(deleted) { }
                public override void Undo(Editor editor)
                {
                    editor.Route.Stages.Add(Deleted);
                    editor.End.X = Deleted.Track.End.X;
                    editor.End.Y = Deleted.Track.End.Y;
                }
            }

            private partial class LastAddition : Change
            {
                public LastAddition() { }
                public override void Undo(Editor editor)
                {
                    List<Stage> stages = editor.Route.Stages;
                    stages.RemoveAt(stages.Count - 1);
                    editor.End.X = stages[stages.Count - 1].Track.End.X;
                    editor.End.Y = stages[stages.Count - 1].Track.End.Y;
                }
            }

            private abstract partial class Replacement : Change
            {
                protected double OldX, OldY;
                public Replacement(double oldX, double oldY)
                {
                    OldX = oldX;
                    OldY = oldY;
                }
            }
            private partial class FirstReplacement : Replacement
            {
                public FirstReplacement(double oldX, double oldY) : base(oldX, oldY) { }
                public override void Undo(Editor editor)
                {
                    List<Stage> stages = editor.Route.Stages;
                    stages[0].Track.Start.X = OldX;
                    stages[0].Track.Start.Y = OldY;
                }
            }
            private partial class MiddleReplacement : Replacement
            {
                private int Index;
                public MiddleReplacement(double oldX, double oldY, int index) : base(oldX, oldY)
                { Index = index; }
                public override void Undo(Editor editor)
                {
                    List<Stage> stages = editor.Route.Stages;
                    stages[Index].Track.Start.X = OldX;
                    stages[Index].Track.Start.Y = OldY;
                    stages[Index - 1].Track.End.X = OldX;
                    stages[Index - 1].Track.End.Y = OldY;
                }
            }
            private partial class LastReplacement : Replacement
            {
                public LastReplacement(double oldX, double oldY) : base(oldX, oldY) { }
                public override void Undo(Editor editor)
                {
                    List<Stage> stages = editor.Route.Stages;
                    stages[stages.Count - 1].Track.End.X = OldX;
                    stages[stages.Count - 1].Track.End.Y = OldY;
                    editor.End.X = OldX;
                    editor.End.Y = OldY;
                }
            }
        }
    }
}
