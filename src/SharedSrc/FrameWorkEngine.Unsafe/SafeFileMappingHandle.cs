using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
[SecurityCritical]
internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [SecurityCritical]
    internal SafeFileMappingHandle()
        : base(ownsHandle: true)
    {
    }

    [SecurityCritical]
    internal SafeFileMappingHandle(IntPtr handle, bool ownsHandle)
        : base(ownsHandle)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.CloseHandle(handle);
    }
}

