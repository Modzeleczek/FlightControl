using System.Collections.Generic;
using System.Windows.Threading;
using System;
using System.Windows.Media.Imaging;

namespace FlightControl
{
    class Radar
    {
        private Map ObstaclesMap;
        private List<Aircraft> Aircrafts;
        private DispatcherTimer Timer;
        private WriteableBitmap BackgroundBitmap;
        public Radar(string mapFileName, int refreshingRateInMilliseconds, WriteableBitmap backgroundBitmap)
        {
            ObstaclesMap = new Map(mapFileName);
            Aircrafts = new List<Aircraft>();
            BackgroundBitmap = backgroundBitmap;

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(TimerTick);
            //timer.Tick += TimerTick;
            Timer.Interval = TimeSpan.FromMilliseconds(refreshingRateInMilliseconds);
        }
        private void TimerTick(object sender, EventArgs e)
        {
            //ObstaclesMap.Draw()
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
    }
}
