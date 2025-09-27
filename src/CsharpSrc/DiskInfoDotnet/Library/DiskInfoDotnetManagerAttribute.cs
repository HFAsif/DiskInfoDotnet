using System;
using System.Collections.Generic;
using System.Text;

namespace DiskInfoDotnet.Library
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class DiskInfoDotnetManagerAttribute : Attribute
    {
        public bool BTAPassThroughSmart { get; private set; }

        public DiskInfoDotnetManagerAttribute()
        {
            BTAPassThroughSmart = true;
        }
    }
}
