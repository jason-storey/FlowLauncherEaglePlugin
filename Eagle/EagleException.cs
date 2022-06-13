using System;

namespace Eagle
{
    public class EagleException : Exception
    {
        public EagleException(string message,Exception innerException) : base(message,innerException) { }
    }
}