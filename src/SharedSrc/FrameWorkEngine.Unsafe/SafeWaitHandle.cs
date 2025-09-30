
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
/// <summary>Represents a wrapper class for a wait handle.</summary>
[SecurityCritical]
[__DynamicallyInvokable]
public sealed class SafeWaitHandle : SafeHandleZeroOrMinusOneIsInvalid
{
    private SafeWaitHandle()
        : base(ownsHandle: true)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.Win32.SafeHandles.SafeWaitHandle" /> class.</summary>
    /// <param name="existingHandle">An <see cref="T:System.IntPtr" /> object that represents the pre-existing handle to use.</param>
    /// <param name="ownsHandle">
    ///   <see langword="true" /> to reliably release the handle during the finalization phase; <see langword="false" /> to prevent reliable release (not recommended).</param>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    public SafeWaitHandle(IntPtr existingHandle, bool ownsHandle)
        : base(ownsHandle)
    {
        SetHandle(existingHandle);
    }

    [SecurityCritical]
    protected override bool ReleaseHandle()
    {
        return Win32Native.CloseHandle(handle);
    }
}

