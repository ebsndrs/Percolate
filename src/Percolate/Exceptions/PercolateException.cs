using System;

namespace Percolate.Exceptions
{
    public class PercolateException : Exception
    {
        public PercolateException() { }

        public PercolateException(string message) : base(message) { }

        public PercolateException(string message, Exception innerException) : base(message, innerException) { }
    }
}
