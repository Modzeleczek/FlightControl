using System;

namespace FlightControl
{
    namespace Exceptions
    {
        public class MapLoadingException : Exception
        {
            public MapLoadingException() { }
            public MapLoadingException(string message) : base(message) { }
        }
        public class StageOutOfBoundsException : Exception
        {
            public StageOutOfBoundsException() { }
            public StageOutOfBoundsException(string message) : base(message) { }
        }
    }
}
