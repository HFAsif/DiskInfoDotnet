using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace FrameWorkEngine;
//
// Summary:
//     Represents a safe handle to the Windows registry.
[SecurityCritical]
public sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    [SecurityCritical]
    internal SafeRegistryHandle()
        : base(ownsHandle: true)
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Microsoft.Win32.SafeHandles.SafeRegistryHandle
    //     class.
    //
    // Parameters:
    //   preexistingHandle:
    //     An object that represents the pre-existing handle to use.
    //
    //   ownsHandle:
    //     true to reliably release the handle during the finalization phase; false to prevent
    //     reliable release.
    [SecurityCritical]
    public SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle)
        : base(ownsHandle)
    {
        SetHandle(preexistingHandle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return RegCloseKey(handle) == 0;
    }

    [DllImport("advapi32.dll")]
    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal static extern int RegCloseKey(IntPtr hKey);
}
#if false // Decompilation log
'12' items in cache
#endif
