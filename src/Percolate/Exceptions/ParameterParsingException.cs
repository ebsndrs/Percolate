using System;

namespace Percolate.Exceptions
{
    public class ParameterParsingException : PercolateException
    {
        public ParameterParsingException() { }

        public ParameterParsingException(string message) : base(message) { }

        public ParameterParsingException(string message, Exception inner) : base(message, inner) { }
    }
}
