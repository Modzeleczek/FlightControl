using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Plane : Aircraft
    {
        public Plane(Flight route)
            : base(route, 40, 40)
        {
        }
        public Plane(Plane o) : base(o) { }

        public override void Draw(WriteableBitmap bitmap)
        {
            if (!Colliding)
                Hitbox.Draw(bitmap, (255 << 24) | (255 << 8));
            else
                Hitbox.Draw(bitmap, (255 << 24) | (255 << 16));//red 
        }

        public override void DrawRoute(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, (255 << 24) | (255 << 8));
        }

        public static Plane GetRandom(int mapWidth, int mapHeight, Random rng, int refreshingRate)
        {
            Flight flight = Flight.GetRandom(
                rng.Next(10, 30),
                40, 40,
                mapWidth - 1 - 40,
                mapHeight - 1 - 40,
                100, 300, 300, 1000, rng);
            Plane plane = new Plane(flight);
            plane.ScaleVelocity(1.0 / refreshingRate);
            return plane;
        }
    }
}
