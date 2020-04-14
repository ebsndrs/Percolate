using System;

namespace Percolate
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class DisablePercolateAttribute : Attribute { }
}
