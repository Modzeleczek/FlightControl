using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Collections;

namespace FlightControl
{
    public class Radar : DispatcherTimer, IDisposable
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private WriteableBitmap MapBitmap, RoutesBitmap, AircraftsBitmap;
        public double DangerousDistance { get; set; } = 50;

        public int Framerate
        {
            get => (int)Interval.TotalMilliseconds;
            set => Interval = TimeSpan.FromMilliseconds(value);
        }

        public Radar(string mapFileName, int refreshingRateInMilliseconds, double dangerousDistance, Image mapImage, Image routesImage, Image aircraftsImage) : base()
        {
            MapBitmap = new WriteableBitmap((int)mapImage.Width, (int)mapImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            mapImage.Source = MapBitmap;
            MapBitmap.Lock();
            ObstaclesMap = new Map(mapFileName, MapBitmap);
            MapBitmap.Unlock();

            RoutesBitmap = new WriteableBitmap((int)routesImage.Width, (int)routesImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            routesImage.Source = RoutesBitmap;

            AircraftsBitmap = new WriteableBitmap((int)aircraftsImage.Width, (int)aircraftsImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            aircraftsImage.Source = AircraftsBitmap;
            Aircrafts = new List<Aircraft>();

            DangerousDistance = dangerousDistance;

            Tick += TimerTick;
            Framerate = refreshingRateInMilliseconds;
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            int i = 0;
            while (i < Aircrafts.Count)
            {
                Aircrafts[i].ClearRouteGraphics(RoutesBitmap);
                Aircrafts[i].ClearHitboxGraphics(AircraftsBitmap);
                if (!Aircrafts[i].Advance())
                {
                    Aircrafts[i].Dispose();
                    Aircrafts[i] = null;
                    Aircrafts.RemoveAt(i);
                }
                else
                {
                    Aircrafts[i].DrawRoute(RoutesBitmap);
                    Aircrafts[i].CollisionState = Aircraft.State.Normal;
                    ++i;
                }
            }
            IEnumerator<Obstacle> obstacles = ObstaclesMap.GetEnumerator();
            double distance; int j;
            for (i = 0; i < Aircrafts.Count; ++i)
            {
                for (j = 0; j < i; ++j)
                {
                    distance = Aircrafts[i].DistanceBetween(Aircrafts[j]);
                    if (distance == 0)
                    {
                        Aircrafts[i].CollisionState = Aircraft.State.Colliding;
                        break;
                    }
                    if (distance <= DangerousDistance)
                        Aircrafts[i].CollisionState = Aircraft.State.Close;
                }
                for (++j; j < Aircrafts.Count; ++j)
                {
                    distance = Aircrafts[i].DistanceBetween(Aircrafts[j]);
                    if (distance == 0)
                    {
                        Aircrafts[i].CollisionState = Aircraft.State.Colliding;
                        break;
                    }
                    if (distance <= DangerousDistance)
                        Aircrafts[i].CollisionState = Aircraft.State.Close;
                }
                if (Aircrafts[i].CollisionState != Aircraft.State.Colliding)
                {
                    obstacles.Reset();
                    while (obstacles.MoveNext())
                    {
                        distance = Aircrafts[i].DistanceBetween(obstacles.Current);
                        if (distance == 0)
                        {
                            Aircrafts[i].CollisionState = Aircraft.State.Colliding;
                            break;
                        }
                        if (distance <= DangerousDistance)
                            Aircrafts[i].CollisionState = Aircraft.State.Close;
                    }
                }
                Aircrafts[i].Draw(AircraftsBitmap);
            }
            RoutesBitmap.Unlock();
            AircraftsBitmap.Unlock();
        }

        public void AddAircraft(Aircraft aircraft)
        {
            aircraft.ScaleVelocity(Framerate / 1024.0);
            Aircrafts.Add(aircraft);
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            aircraft.DrawHitbox(AircraftsBitmap);
            aircraft.DrawRoute(RoutesBitmap);
            RoutesBitmap.Unlock();
            AircraftsBitmap.Unlock();
        }

        public void RemoveAircraft(double x, double y)
        {
            if (Aircrafts.Count == 0) return;
            Square cursor = new Square(new Point(x, y), 1);
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            for (int i = 0; i < Aircrafts.Count; ++i)
            {
                if (Aircrafts[i].DistanceBetween(cursor) == 0)
                {
                    Aircrafts[i].ClearRouteGraphics(RoutesBitmap);
                    Aircrafts[i].ClearHitboxGraphics(AircraftsBitmap);
                    Aircrafts[i].Dispose();
                    Aircrafts[i] = null;
                    Aircrafts.RemoveAt(i);
                    break;
                }
            }
            foreach (var a in Aircrafts)
            {
                a.DrawRoute(RoutesBitmap);
                a.Draw(AircraftsBitmap);
            }
            RoutesBitmap.Unlock();
            AircraftsBitmap.Unlock();
            cursor.Dispose();
        }

        public void ClearAircrafts()
        {
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            for (int i = 0; i < Aircrafts.Count; ++i)
            {
                Aircrafts[i].ClearRouteGraphics(RoutesBitmap);
                Aircrafts[i].ClearHitboxGraphics(AircraftsBitmap);
                Aircrafts[i].Dispose();
                Aircrafts[i] = null;
            }
            Aircrafts.Clear();
            RoutesBitmap.Unlock();
            AircraftsBitmap.Unlock();
        }

        public void RandomizeAircrafts(int count, Random rng)
        {
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            ClearAircrafts();

            for (int i = 0; i < count; ++i)
            {
                double random = rng.NextDouble();
                Aircraft generated;
                if (random >= 0.75)
                    generated = Plane.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng);
                else if (random >= 0.5)
                    generated = Helicopter.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng);
                else if (random >= 0.25)
                    generated = Glider.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng);
                else
                    generated = Balloon.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng);
                generated.DrawHitbox(AircraftsBitmap);
                generated.DrawRoute(RoutesBitmap);
                generated.ScaleVelocity(Framerate / 1024.0);
                Aircrafts.Add(generated);
            }
            RoutesBitmap.Unlock();
            AircraftsBitmap.Unlock();
        }

        public void Dispose()
        {
            Stop();
            Tick -= TimerTick;
            ObstaclesMap.Dispose();
            ObstaclesMap = null;
            for (int i = 0; i < Aircrafts.Count; ++i)
            {
                Aircrafts[i].Dispose();
                Aircrafts[i] = null;
            }
            Aircrafts.Clear();
            Aircrafts = null;
            MapBitmap = null;
            AircraftsBitmap = null;
        }
    }
}
