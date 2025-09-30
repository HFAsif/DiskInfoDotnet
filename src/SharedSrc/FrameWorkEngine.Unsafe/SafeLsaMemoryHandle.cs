using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeLsaMemoryHandle : SafeBuffer
{
    internal static SafeLsaMemoryHandle InvalidHandle => new SafeLsaMemoryHandle(IntPtr.Zero);

    private SafeLsaMemoryHandle()
        : base(ownsHandle: true)
    {
    }

    internal SafeLsaMemoryHandle(IntPtr handle)
        : base(ownsHandle: true)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.LsaFreeMemory(handle) == 0;
    }
}
