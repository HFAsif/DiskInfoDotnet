using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeLsaReturnBufferHandle : SafeBuffer
{
    internal static SafeLsaReturnBufferHandle InvalidHandle => new SafeLsaReturnBufferHandle(IntPtr.Zero);

    private SafeLsaReturnBufferHandle()
        : base(ownsHandle: true)
    {
    }

    internal SafeLsaReturnBufferHandle(IntPtr handle)
        : base(ownsHandle: true)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.LsaFreeReturnBuffer(handle) >= 0;
    }
}
