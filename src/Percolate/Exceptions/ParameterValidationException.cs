using System;

namespace Percolate.Exceptions
{
    public class ParameterValidationException : PercolateException
    {
        public ParameterValidationException() : base() { }

        public ParameterValidationException(string message) : base(message) { }

        public ParameterValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
