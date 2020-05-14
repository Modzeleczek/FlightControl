using System;

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
        class NotEnoughElementsException : Exception
        {
            public NotEnoughElementsException() { }
            public NotEnoughElementsException(string message) : base(message) { }
        }
        class LinesNotConnectedException : Exception
        {
            public LinesNotConnectedException() { }
            public LinesNotConnectedException(string message) : base(message) { }
        }
    }
}
