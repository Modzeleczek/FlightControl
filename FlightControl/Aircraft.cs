﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightControl
{
    abstract class Aircraft
    {
        public Route Route { get; protected set; }
        protected Point Position;
        protected double Altitude;
        protected Aircraft(double initialX, double initialY, double initialAltitude)
        {
            Position = new Point(initialX, initialY);
            Altitude = initialAltitude;
        }
        public void Move(double dx, double dy, double dAlt)
        {
            Position.X += dx;
            Position.Y += dy;
            Altitude += dAlt;
        }
        public void AddDestination(Point destination)
        {
            Route.AddPoint(destination);
        }
    }
}
