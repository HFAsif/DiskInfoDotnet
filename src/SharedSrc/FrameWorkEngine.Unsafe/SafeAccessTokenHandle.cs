using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace FrameWorkEngine;

//
// Summary:
//     Provides a safe handle to a Windows thread or process access token. For more
//     information, see Access Tokens.
[SecurityCritical]
public sealed class SafeAccessTokenHandle : SafeHandle
{
    //
    // Summary:
    //     Returns an invalid handle by instantiating a Microsoft.Win32.SafeHandles.SafeAccessTokenHandle
    //     object with System.IntPtr.Zero.
    //
    // Returns:
    //     Returns a Microsoft.Win32.SafeHandles.SafeAccessTokenHandle object.
    public static SafeAccessTokenHandle InvalidHandle
    {
        [SecurityCritical]
        get
        {
            return new SafeAccessTokenHandle(IntPtr.Zero);
        }
    }

    //
    // Summary:
    //     Gets a value that indicates whether the handle is invalid.
    //
    // Returns:
    //     true if the handle is not valid; otherwise, false.
    public override bool IsInvalid
    {
        [SecurityCritical]
        get
        {
            if (!(handle == IntPtr.Zero))
            {
                return handle == new IntPtr(-1);
            }

            return true;
        }
    }

    private SafeAccessTokenHandle()
        : base(IntPtr.Zero, ownsHandle: true)
    {
    }

    //
    // Summary:
    //     Initializes a new instance of the Microsoft.Win32.SafeHandles.SafeAccessTokenHandle
    //     class.
    //
    // Parameters:
    //   handle:
    //     An System.IntPtr object that represents the pre-existing handle to use. Using
    //     System.IntPtr.Zero returns an invalid handle.
    public SafeAccessTokenHandle(IntPtr handle)
        : base(IntPtr.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.CloseHandle(handle);
    }
}
#if false // Decompilation log
'12' items in cache
#endif