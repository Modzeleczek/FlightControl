using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    public class Glider : Aircraft
    {
        public Glider(Flight route) : base(route, 30) { }
        public Glider(Glider o) : base(o) { }

        public override void DrawHitbox(WriteableBitmap bitmap) => Hitbox.Draw(bitmap, (255 << 24) | (255 << 16) | (102 << 8) | 255);

        public override void DrawRoute(WriteableBitmap bitmap) => Route.Draw(bitmap, (128 << 24) | (255 << 16) | (102 << 8) | 255);

        public static Glider GetRandom(int mapWidth, int mapHeight, Random rng)
        {
            Flight flight = Flight.GetRandom(
                rng.Next(3, 6),
                30, 30,
                mapWidth - 1 - 30,
                mapHeight - 1 - 30,
                80, 150, 100, 300, rng);
            return new Glider(flight);
        }
    }
}
