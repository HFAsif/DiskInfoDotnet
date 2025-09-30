using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [SecurityCritical]
    internal SafeFindHandle()
        : base(ownsHandle: true)
    {
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.FindClose(handle);
    }
}
