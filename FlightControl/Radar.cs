using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace FlightControl
{
    public class Radar : DispatcherTimer, IDisposable
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private WriteableBitmap MapBitmap, RoutesBitmap, AircraftsBitmap;

        public int Framerate
        {
            get => (int)Interval.TotalMilliseconds;
            set
            {
                Stop();
                Interval = TimeSpan.FromMilliseconds(value);
                Start();
            }
        }

        public Radar(string mapFileName, int refreshingRateInMilliseconds, Image mapImage, Image routesImage, Image aircraftsImage) : base()
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

            Tick += TimerTick;
            Framerate = refreshingRateInMilliseconds;
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            int i = 0, j;
            IEnumerator<Obstacle> obstacles = ObstaclesMap.GetEnumerator();
            while (i < Aircrafts.Count)
            {
                Aircrafts[i].ClearGraphics(AircraftsBitmap);
                Aircrafts[i].ClearRouteGraphics(RoutesBitmap);
                if (!Aircrafts[i].Advance())
                {
                    Aircrafts[i].Dispose();
                    Aircrafts[i] = null;
                    Aircrafts.RemoveAt(i);
                }
                else
                    ++i;
            }
            for(i = 0; i < Aircrafts.Count; ++i)
            {
                if (!Aircrafts[i].Colliding)
                {
                    for (j = i + 1; j < Aircrafts.Count; ++j)
                    {
                        if (Aircrafts[i].Collides(Aircrafts[j]))
                            break;
                    }
                    if (!Aircrafts[i].Colliding)
                    {
                        obstacles.Reset();
                        while (obstacles.MoveNext())
                        {
                            if (Aircrafts[i].Collides(obstacles.Current))
                                break;
                        }
                    }
                }
                Aircrafts[i].DrawRoute(RoutesBitmap);
                Aircrafts[i].Draw(AircraftsBitmap);
            }
            //RoutesBitmap.AddDirtyRect(new Int32Rect(0, 0, 1280, 720));
            RoutesBitmap.Unlock();
            //AircraftsBitmap.AddDirtyRect(new Int32Rect(0, 0, 1280, 720));
            AircraftsBitmap.Unlock();
        }

        public void AddAircraft(Plane plane) => Aircrafts.Add(new Plane(plane));
        public void AddAircraft(Helicopter helicopter) => Aircrafts.Add(new Helicopter(helicopter));
        public void AddAircraft(Glider glider) => Aircrafts.Add(new Glider(glider));
        public void AddAircraft(Balloon balloon) => Aircrafts.Add(new Balloon(balloon));

        public int AircraftsCount => Aircrafts.Count;

        public void RemoveAircraft(int index) => Aircrafts.RemoveAt(index);

        public void RandomizeAircrafts(int count, Random rng)
        {
            AircraftsBitmap.Lock();
            RoutesBitmap.Lock();
            for(int i = 0; i < Aircrafts.Count; ++i)
            {
                Aircrafts[i].ClearRouteGraphics(RoutesBitmap);
                Aircrafts[i].ClearGraphics(AircraftsBitmap);
                Aircrafts[i].Dispose();
                Aircrafts[i] = null;
            }
            Aircrafts.Clear();

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
                generated.Draw(AircraftsBitmap);
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
