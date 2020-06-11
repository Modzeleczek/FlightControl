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

            Tick += new EventHandler(TimerTick);
            Interval = TimeSpan.FromMilliseconds(refreshingRateInMilliseconds);
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            AircraftsBitmap.Lock();
            int i = 0, j;
            while(i < Aircrafts.Count)
            {
                if (!Aircrafts[i].Colliding)
                {
                    for (j = i + 1; j < Aircrafts.Count; ++j)
                    {
                        if (Aircrafts[i].Collides(Aircrafts[j]))
                            break;
                    }
                    for (j = 0; j < ObstaclesMap.Obstacles.Count; ++j)
                    {
                        if (Aircrafts[i].Collides(ObstaclesMap.Obstacles[j]))
                            break;
                    }
                }
                if (!Aircrafts[i].Advance(AircraftsBitmap))
                    Aircrafts.RemoveAt(i);
                else
                    ++i;
            }
            //AircraftsBitmap.AddDirtyRect(new Int32Rect(0, 0, AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight));//Unnecessary after doing it in Line.Draw;
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
            int aircraftsCount = 20, fromStagesCount = 5, toStagesCount = 10,
            fromWidth = 10, toWidth = 10,
            fromHeight = 20, toHeight = 40,
            fromVelocity = 150, toVelocity = 100,
            fromAltitude = 50, toAltitude = 100;

            AircraftsBitmap.Lock();
            foreach (var aircraft in Aircrafts)
                aircraft.ClearGraphics(AircraftsBitmap);
            Aircrafts.Clear();
            AircraftsBitmap.Unlock();
            for (int i = 0; i < aircraftsCount; ++i)
            {
                double width = fromWidth + toWidth * rng.NextDouble(),
                    height = fromHeight + toHeight * rng.NextDouble();
                Flight flight = Flight.GetRandom(
                        rng.Next(fromStagesCount, toStagesCount),
                        (int)Math.Ceiling(width), (int)Math.Ceiling(height),
                        (int)Math.Floor(AircraftsBitmap.PixelWidth-1 - width),
                        (int)Math.Floor(AircraftsBitmap.PixelHeight-1 - height),
                        fromVelocity, toVelocity, fromAltitude, toAltitude, rng);

                double random = rng.NextDouble();
                if(random >= 0.75)
                    Aircrafts.Add(new Plane(flight, width, height));
                else if(random >= 0.5)
                    Aircrafts.Add(new Helicopter(flight, width, height));
                else if (random >= 0.25)
                    Aircrafts.Add(new Glider(flight, width, height));
                else
                    Aircrafts.Add(new Balloon(flight, width, height));
                Aircrafts[i].ScaleVelocity(1.0 / RefreshingRate);
            }
        }
        public void RandomizeFlights(Random rng)
        {
            AircraftsBitmap.Lock();
            foreach (var aircraft in Aircrafts)
            {
                aircraft.ClearGraphics(AircraftsBitmap);
                aircraft.RandomizeFlight(3, 10, AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight, RefreshingRate, rng);
            }
            AircraftsBitmap.Unlock();
        }

        public void Dispose()
        {
            
        }
    }
}
