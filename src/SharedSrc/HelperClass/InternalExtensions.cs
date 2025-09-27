#if !NET6_0_OR_GREATER 
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace HelperClass;
internal static class InternalExtensions
{
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    internal static uint AlignedSizeOf<T>() where T : struct
    {
        uint num = SizeOfType(typeof(T));
        if (num == 1 || num == 2)
        {
            return num;
        }
        if (IntPtr.Size == 8 && num == 4)
        {
            return num;
        }
        return AlignedSizeOfType(typeof(T));
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal static extern uint SizeOfType(Type type);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private static extern uint AlignedSizeOfType(Type type);
}
#endif