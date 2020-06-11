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
        private WriteableBitmap MapBitmap, AircraftsBitmap;

        public int RefreshingRate
        {
            get => (int)Interval.TotalMilliseconds;
            set
            {
                Stop();
                Interval = TimeSpan.FromMilliseconds(value);
                Start();
            }
        }

        public Radar(string mapFileName, int refreshingRateInMilliseconds, Image mapImage, Image aircraftsImage) : base()
        {
            MapBitmap = new WriteableBitmap((int)mapImage.Width, (int)mapImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            mapImage.Source = MapBitmap;

            AircraftsBitmap = new WriteableBitmap((int)aircraftsImage.Width, (int)aircraftsImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            aircraftsImage.Source = AircraftsBitmap;

            MapBitmap.Lock();
            ObstaclesMap = new Map(mapFileName, MapBitmap);
            MapBitmap.Unlock();

            Aircrafts = new List<Aircraft>();

            Tick += TimerTick;
            Interval = TimeSpan.FromMilliseconds(refreshingRateInMilliseconds);
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            AircraftsBitmap.Lock();
            int i = 0, j;
            IEnumerator<Obstacle> obstacles = ObstaclesMap.GetEnumerator();
            while (i < Aircrafts.Count)
            {
                if (!Aircrafts[i].Colliding)
                {
                    for (j = i + 1; j < Aircrafts.Count; ++j)
                    {
                        if (Aircrafts[i].Collides(Aircrafts[j]))
                            break;
                    }
                    /*for (j = 0; j < ObstaclesMap.Obstacles.Count; ++j)
                    {
                        if (Aircrafts[i].Collides(ObstaclesMap.Obstacles[j]))
                            break;
                    }*/
                    obstacles.Reset();
                    while(obstacles.MoveNext())
                    {
                        if (Aircrafts[i].Collides(obstacles.Current))
                            break;
                    }
                }
                if (!Aircrafts[i].Advance(AircraftsBitmap))
                {
                    Aircrafts[i].Dispose();
                    Aircrafts[i] = null;
                    Aircrafts.RemoveAt(i);
                }
                else
                    ++i;
            }
            AircraftsBitmap.Unlock();
        }

        public void AddAircraft(Plane plane)
        {
            plane.ScaleVelocity(1.0 / RefreshingRate);
            Aircrafts.Add(new Plane(plane));
        }
        public void AddAircraft(Helicopter helicopter)
        {
            helicopter.ScaleVelocity(1.0 / RefreshingRate);
            Aircrafts.Add(new Helicopter(helicopter));
        }
        public void AddAircraft(Glider glider)
        {
            glider.ScaleVelocity(1.0 / RefreshingRate);
            Aircrafts.Add(new Glider(glider));
        }
        public void AddAircraft(Balloon balloon)
        {
            balloon.ScaleVelocity(1.0 / RefreshingRate);
            Aircrafts.Add(new Balloon(balloon));
        }

        public int AircraftsCount => Aircrafts.Count;

        public void RemoveAircraft(int index) => Aircrafts.RemoveAt(index);

        public void RandomizeAircrafts(Random rng)
        {
            int aircraftsCount = 20;

            AircraftsBitmap.Lock();
            for(int i = 0; i < Aircrafts.Count; ++i)
            {
                Aircrafts[i].ClearGraphics(AircraftsBitmap);
                Aircrafts[i].Dispose();
                Aircrafts[i] = null;
            }
            Aircrafts.Clear();

            for (int i = 0; i < aircraftsCount; ++i)
            {
                double random = rng.NextDouble();
                Aircraft generated;
                if (random >= 0.75)
                    generated = Plane.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng, RefreshingRate);
                else if (random >= 0.5)
                    generated = Helicopter.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng, RefreshingRate);
                else if (random >= 0.25)
                    generated = Glider.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng, RefreshingRate);
                else
                    generated = Balloon.GetRandom(AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, rng, RefreshingRate);
                generated.Draw(AircraftsBitmap);
                generated.DrawRoute(AircraftsBitmap);
                Aircrafts.Add(generated);
            }
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
