using System.Collections.Generic;
using System.Windows.Threading;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace FlightControl
{
    public class Radar : DispatcherTimer
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private WriteableBitmap MapBitmap, FrontBitmap;
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

            FrontBitmap = new WriteableBitmap((int)aircraftsImage.Width, (int)aircraftsImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            aircraftsImage.Source = FrontBitmap;

            ObstaclesMap = new Map(mapFileName, 1280, 690);
            MapBitmap.Lock();
            ObstaclesMap.Draw(MapBitmap, (255 << 24) | (255 << 8));//green
            //MapBitmap.AddDirtyRect(new Int32Rect(0, 0, MapBitmap.PixelWidth, MapBitmap.PixelHeight));//Unnecessary after doing it in Line.Draw;
            MapBitmap.Unlock();

            Aircrafts = new List<Aircraft>();

            Tick += new EventHandler(TimerTick);
            Interval = TimeSpan.FromMilliseconds(refreshingRateInMilliseconds);
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            FrontBitmap.Lock();
            int i = 0;
            while(i < Aircrafts.Count)
            {
                if (!Aircrafts[i].Advance(FrontBitmap))
                    Aircrafts.RemoveAt(i);
                else
                    ++i;
            }
            //AircraftsBitmap.AddDirtyRect(new Int32Rect(0, 0, AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight));//Unnecessary after doing it in Line.Draw;
            FrontBitmap.Unlock();
        }
        public void AddAircraft(Plane plane)
        {
            Aircrafts.Add(new Plane(plane));
        }
        public void AddAircraft(Helicopter helicopter)
        {
            Aircrafts.Add(new Helicopter(helicopter));
        }
        public void AddAircraft(Glider glider)
        {
            Aircrafts.Add(new Glider(glider));
        }
        public void AddAircraft(Balloon balloon)
        {
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
        public void RandomizeAircrafts(int aircraftsCount, int fromStagesCount, int toStagesCount,
            double fromWidth, double toWidth,
            double fromHeight, double toHeight,
            double fromVelocity, double toVelocity,
            double fromAltitude, double toAltitude,
            Random rng)
        {
            Aircrafts.Clear();

            for (int i = 0; i < aircraftsCount; ++i)
            {
                Flight flight = Flight.GetRandom(
                        rng.Next(fromStagesCount, toStagesCount),
                        ObstaclesMap.Width, ObstaclesMap.Height,
                        fromVelocity, toVelocity, fromAltitude, toAltitude, rng);

                double random = rng.NextDouble();

                if(random >= 0.75)
                    Aircrafts.Add(new Plane(
                        flight, fromWidth + toWidth * rng.NextDouble(), fromHeight + toHeight * rng.NextDouble()));
                else if(random >= 0.5)
                    Aircrafts.Add(new Helicopter(
                        flight, fromWidth + toWidth * rng.NextDouble(), fromHeight + toHeight * rng.NextDouble()));
                else if (random >= 0.25)
                    Aircrafts.Add(new Glider(
                        flight, fromWidth + toWidth * rng.NextDouble(), fromHeight + toHeight * rng.NextDouble()));
                else
                    Aircrafts.Add(new Balloon(
                        flight, fromWidth + toWidth * rng.NextDouble(), fromHeight + toHeight * rng.NextDouble()));
                Aircrafts[Aircrafts.Count - 1].ScaleVelocity(1.0 / (double)RefreshingRate);
            }
        }
    }
}
