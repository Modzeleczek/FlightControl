using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Helicopter : Aircraft
    {
        public Helicopter(Flight route)
            : base(route, 20, 20)
        {
        }

        public Helicopter(Helicopter o) : base(o) { }
        public override void Draw(WriteableBitmap bitmap)
        {
            if (!Colliding)
                Hitbox.Draw(bitmap, (255 << 24) | (102 << 16) | (145 << 8) | 255);
            else
                Hitbox.Draw(bitmap, (255 << 24) | (255 << 16));//red
        }

        public override void DrawRoute(WriteableBitmap bitmap)
        {
            Route.Draw(bitmap, (255 << 24) | (102 << 16) | (145 << 8) | 255);
        }

        public static Helicopter GetRandom(int mapWidth, int mapHeight, Random rng, int refreshingRate)
        {
            Flight flight = Flight.GetRandom(
                rng.Next(10, 15),
                20, 20,
                mapWidth - 1 - 20,
                mapHeight - 1 - 20,
                120, 200, 100, 500, rng);
            Helicopter helicopter = new Helicopter(flight);
            helicopter.ScaleVelocity(1.0 / refreshingRate);
            return helicopter;
        }
    }
}
