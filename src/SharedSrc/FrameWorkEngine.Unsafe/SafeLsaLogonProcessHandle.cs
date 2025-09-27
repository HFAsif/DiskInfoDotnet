using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeLsaLogonProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    internal static SafeLsaLogonProcessHandle InvalidHandle => new SafeLsaLogonProcessHandle(IntPtr.Zero);

    private SafeLsaLogonProcessHandle()
        : base(ownsHandle: true)
    {
    }

    internal SafeLsaLogonProcessHandle(IntPtr handle)
        : base(ownsHandle: true)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.LsaDeregisterLogonProcess(handle) >= 0;
    }
}

