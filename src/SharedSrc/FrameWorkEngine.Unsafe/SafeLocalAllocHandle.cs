using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeLocalAllocHandle : SafeBuffer
{
    internal static SafeLocalAllocHandle InvalidHandle => new SafeLocalAllocHandle(IntPtr.Zero);

    private SafeLocalAllocHandle()
        : base(ownsHandle: true)
    {
    }

    internal SafeLocalAllocHandle(IntPtr handle)
        : base(ownsHandle: true)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.LocalFree(handle) == IntPtr.Zero;
    }
}

