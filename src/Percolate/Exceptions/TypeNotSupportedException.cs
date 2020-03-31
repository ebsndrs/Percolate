using System;

namespace Percolate.Exceptions
{
    public class TypeNotSupportedException : PercolateException
    {
        public TypeNotSupportedException() : base() { }

        public TypeNotSupportedException(string message) : base(message) { }

        public TypeNotSupportedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
