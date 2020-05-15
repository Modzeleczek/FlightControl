using System.Collections.Generic;
using System.Windows.Threading;
using System;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FlightControl
{
    public class Radar
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private DispatcherTimer Timer;
        private WriteableBitmap ForegroundBitmap;
        public Radar(string mapFileName, int refreshingRateInMilliseconds, WriteableBitmap aircraftBitmap)
        {
            ObstaclesMap = new Map(mapFileName);
            Aircrafts = new List<Aircraft>();
            ForegroundBitmap = aircraftBitmap;

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            //timer.Tick += TimerTick;
            Timer.Interval = TimeSpan.FromMilliseconds(refreshingRateInMilliseconds);
        }
        private void TimerTick(object sender, EventArgs e)
        {
            ForegroundBitmap.Lock();
            foreach (var aircraft in Aircrafts)
            {
                aircraft.Draw(ForegroundBitmap, (255 << 24) | (255 << 16));
                aircraft.Advance();
            }
            ForegroundBitmap.AddDirtyRect(new Int32Rect(0, 0,
                ForegroundBitmap.PixelWidth, ForegroundBitmap.PixelHeight));
            ForegroundBitmap.Unlock();
        }
        public void Start()
        {
            Timer.Start();
        }
        public void Stop()
        {
            Timer.Stop();
        }
        public void SetRefreshingRate(int milliseconds)
        {
            Timer.Stop();
            Timer.Interval = TimeSpan.FromMilliseconds(milliseconds);
            Timer.Start();
        }
        public void DrawMap(WriteableBitmap bitmap)
        {
            ObstaclesMap.Draw(bitmap, (255 << 24) | (255 << 8));
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
            //if(index >= 0 && index < Aircrafts.Count)
                Aircrafts.RemoveAt(index);
        }
    }
}
