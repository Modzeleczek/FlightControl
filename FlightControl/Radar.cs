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
            get
            {
                return (int)Interval.TotalMilliseconds;
            }
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
        public int AircraftsCount
        {
            get
            {
                return Aircrafts.Count;
            }
        }
        public void RemoveAircraft(int index)
        {
            Aircrafts.RemoveAt(index);
        }
        public void RandomizeAircrafts(
            int aircraftsCount, int fromStagesCount, int toStagesCount,
            double fromWidth, double toWidth,
            double fromHeight, double toHeight,
            double fromVelocity, double toVelocity,
            double fromAltitude, double toAltitude,
            Random rng)
        {
            AircraftsBitmap.Lock();
            foreach (var aircraft in Aircrafts)
                aircraft.ClearDrawings(AircraftsBitmap);
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

        public void Dispose()
        {
            
        }
    }
}
