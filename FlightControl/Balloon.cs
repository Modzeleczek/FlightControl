using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Balloon : Aircraft
    {
        public Balloon(Flight route) : base(route, 15) { }
        public Balloon(Balloon o) : base(o) { }

        public override void DrawHitbox(WriteableBitmap bitmap) => Hitbox.Draw(bitmap, (255 << 24) | (153 << 8) | 255);

        public override void DrawRoute(WriteableBitmap bitmap) => Route.Draw(bitmap, (128 << 24) | (153 << 8) | 255);

        public static Balloon GetRandom(int mapWidth, int mapHeight, Random rng)
        {
            Flight flight = Flight.GetRandom(
                rng.Next(5, 10),
                15, 15,
                mapWidth - 1 - 15,
                mapHeight - 1 - 15,
                50, 100, 20, 100, rng);
            return new Balloon(flight);
        }
    }
}
