using System.Collections.Generic;
using System.Windows.Threading;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace FlightControl
{
    public class Radar
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private DispatcherTimer Timer;
        private WriteableBitmap MapBitmap, FrontBitmap;
        public int RefreshingRate
        {
            get
            {
                return (int)Timer.Interval.TotalMilliseconds;
            }
            set
            {
                Timer.Stop();
                Timer.Interval = TimeSpan.FromMilliseconds(value);
                Timer.Start();
            }
        }
        public Radar(string mapFileName, int refreshingRateInMilliseconds, Image mapImage, Image aircraftsImage)
        {
            MapBitmap = new WriteableBitmap((int)mapImage.Width, (int)mapImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            mapImage.Source = MapBitmap;

            FrontBitmap = new WriteableBitmap((int)aircraftsImage.Width, (int)aircraftsImage.Height,
                96, 96, PixelFormats.Bgra32, null);
            aircraftsImage.Source = FrontBitmap;

            ObstaclesMap = new Map(mapFileName);
            MapBitmap.Lock();
            ObstaclesMap.Draw(MapBitmap, (255 << 24) | (255 << 8));//green
            //MapBitmap.AddDirtyRect(new Int32Rect(0, 0, MapBitmap.PixelWidth, MapBitmap.PixelHeight));//Unnecessary after doing it in Line.Draw;
            MapBitmap.Unlock();

            Aircrafts = new List<Aircraft>();

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            RefreshingRate = refreshingRateInMilliseconds;
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            FrontBitmap.Lock();
            int i = 0;
            while(i < Aircrafts.Count)
            {
                if (!Aircrafts[i].Advance(1.0 / 32.0, FrontBitmap))
                    Aircrafts.RemoveAt(i);
                else
                    ++i;
            }
            //AircraftsBitmap.AddDirtyRect(new Int32Rect(0, 0, AircraftsBitmap.PixelWidth, AircraftsBitmap.PixelHeight));//Unnecessary after doing it in Line.Draw;
            FrontBitmap.Unlock();
        }
        public void Start()
        {
            Timer.Start();
        }
        public void Stop()
        {
            Timer.Stop();
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
        public void RemoveAircraft(int index)
        {
            Aircrafts.RemoveAt(index);
        }
    }
}
