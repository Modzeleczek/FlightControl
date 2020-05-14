﻿using System;

namespace FlightControl
{
    namespace Exceptions
    {
        class MapLoadingException : Exception
        {
            public MapLoadingException() { }
            public MapLoadingException(string message) : base(message) { }
        }
        class TerrainLoadingException : Exception
        {
            public TerrainLoadingException() { }
            public TerrainLoadingException(string message) : base(message) { }
        }
    }
}