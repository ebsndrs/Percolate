﻿using System;

namespace Percolate.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class DisablePercolateAttribute : Attribute
    {
    }
}