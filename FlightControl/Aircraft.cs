using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public abstract class Aircraft : Collidable, IDisposable
    {
        protected Flight Route;
        protected double StageProgress;
        public enum State : byte { Normal, Close, Colliding }
        public State CollisionState { get; set; }

        protected Aircraft(Flight route, int size) : base(route.CurrentStage.Track.Start, size)
        {
            Route = new Flight(route);
            StageProgress = 0;
        }
        protected Aircraft(Aircraft o) : this(o.Route, o.Hitbox.Side) { }

        public bool Advance()
        {
            Hitbox.Position.Move(Route.CurrentStage.Velocity.X, Route.CurrentStage.Velocity.Y);
            StageProgress += Route.CurrentStage.Velocity.Length;
            if (StageProgress >= Route.CurrentStage.Length)
            {
                Hitbox.Place(Route.CurrentStage.Track.End);//Wyrównanie hitboxa po zakończeniu odcinka (bez tego zbacza z trasy)
                if (!Route.RemoveCurrent())
                    return false;
                StageProgress = 0;
            }
            return true;
        }

        public void ScaleVelocity(double factor) => Route.ScaleVelocity(factor);

        protected override int Height => Route.CurrentStage.Altitude;

        protected override int DistanceY(int altitude) => Math.Abs(this.Route.CurrentStage.Altitude - altitude);

        public void Draw(WriteableBitmap bitmap)
        {
            switch (CollisionState)
            {
                case State.Normal:
                    DrawHitbox(bitmap);//kolor zależny od rodzaju statku
                    break;
                case State.Close:
                    Hitbox.Draw(bitmap, (255 << 24) | (255 << 16) | (165 << 8));//pomarańczowy
                    break;
                case State.Colliding:
                    Hitbox.Draw(bitmap, (255 << 24) | (255 << 16));//czerwony
                    break;
            }
        }

        public abstract void DrawHitbox(WriteableBitmap bitmap);
      
        public abstract void DrawRoute(WriteableBitmap bitmap);

        public void ClearHitboxGraphics(WriteableBitmap bitmap) => Hitbox.Draw(bitmap, 0);//przezroczysty

        public void ClearRouteGraphics(WriteableBitmap bitmap) => Route.Draw(bitmap, 0);//przezroczysty

        public void Dispose()
        {
            Route.Dispose();
            Hitbox.Dispose();
        }
    }
}
