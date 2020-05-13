using System;

namespace FlightControl
{
    namespace Exceptions
    {
        [Serializable]
        class MapLoadingException : Exception
        {
            public MapLoadingException() { }
            public MapLoadingException(string message) : base(message) { }
        }
    }
}
