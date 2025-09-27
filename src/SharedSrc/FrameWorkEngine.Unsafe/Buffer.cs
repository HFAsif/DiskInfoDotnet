

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkEngine;
/// <summary>Manipulates arrays of primitive types.</summary>
[ComVisible(true)]
[__DynamicallyInvokable]
public static class Buffer
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    private struct Block16
    {
    }

    [StructLayout(LayoutKind.Sequential, Size = 64)]
    private struct Block64
    {
    }

    /// <summary>Copies a specified number of bytes from a source array starting at a particular offset to a destination array starting at a particular offset.</summary>
    /// <param name="src">The source buffer.</param>
    /// <param name="srcOffset">The zero-based byte offset into <paramref name="src" />.</param>
    /// <param name="dst">The destination buffer.</param>
    /// <param name="dstOffset">The zero-based byte offset into <paramref name="dst" />.</param>
    /// <param name="count">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="src" /> or <paramref name="dst" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="src" /> or <paramref name="dst" /> is not an array of primitives.  
    /// -or-  
    /// The number of bytes in <paramref name="src" /> is less than <paramref name="srcOffset" /> plus <paramref name="count" />.  
    /// -or-  
    /// The number of bytes in <paramref name="dst" /> is less than <paramref name="dstOffset" /> plus <paramref name="count" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="srcOffset" />, <paramref name="dstOffset" />, or <paramref name="count" /> is less than 0.</exception>
    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecuritySafeCritical]
    [__DynamicallyInvokable]
    public static extern void BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecuritySafeCritical]
    internal static extern void InternalBlockCopy(Array src, int srcOffsetBytes, Array dst, int dstOffsetBytes, int byteCount);

    [SecurityCritical]
    internal unsafe static int IndexOfByte(byte* src, byte value, int index, int count)
    {
        byte* ptr;
        for (ptr = src + index; ((uint)(int)ptr & 3u) != 0; ptr++)
        {
            if (count == 0)
            {
                return -1;
            }
            if (*ptr == value)
            {
                return (int)(ptr - src);
            }
            count--;
        }
        uint num = (uint)((value << 8) + value);
        num = (num << 16) + num;
        while (count > 3)
        {
            uint num2 = *(uint*)ptr;
            num2 ^= num;
            uint num3 = 2130640639 + num2;
            num2 ^= 0xFFFFFFFFu;
            num2 ^= num3;
            if ((num2 & 0x81010100u) != 0)
            {
                int num4 = (int)(ptr - src);
                if (*ptr == value)
                {
                    return num4;
                }
                if (ptr[1] == value)
                {
                    return num4 + 1;
                }
                if (ptr[2] == value)
                {
                    return num4 + 2;
                }
                if (ptr[3] == value)
                {
                    return num4 + 3;
                }
            }
            count -= 4;
            ptr += 4;
        }
        while (count > 0)
        {
            if (*ptr == value)
            {
                return (int)(ptr - src);
            }
            count--;
            ptr++;
        }
        return -1;
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    private static extern bool IsPrimitiveTypeArray(Array array);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    private static extern byte _GetByte(Array array, int index);

    /// <summary>Retrieves the byte at the specified location in the specified array.</summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <returns>The byte at the specified location in the array.</returns>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    [SecuritySafeCritical]
    [__DynamicallyInvokable]
    public static byte GetByte(Array array, int index)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        if (!IsPrimitiveTypeArray(array))
        {
            throw new ArgumentException(EnvironmentEx.GetResourceString("Arg_MustBePrimArray"), "array");
        }
        if (index < 0 || index >= _ByteLength(array))
        {
            throw new ArgumentOutOfRangeException("index");
        }
        return _GetByte(array, index);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    private static extern void _SetByte(Array array, int index, byte value);

    /// <summary>Assigns a specified value to a byte at a particular location in a specified array.</summary>
    /// <param name="array">An array.</param>
    /// <param name="index">A location in the array.</param>
    /// <param name="value">A value to assign.</param>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is negative or greater than the length of <paramref name="array" />.</exception>
    /// <exception cref="T:System.OverflowException">
    ///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    [SecuritySafeCritical]
    [__DynamicallyInvokable]
    public static void SetByte(Array array, int index, byte value)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        if (!IsPrimitiveTypeArray(array))
        {
            throw new ArgumentException(EnvironmentEx.GetResourceString("Arg_MustBePrimArray"), "array");
        }
        if (index < 0 || index >= _ByteLength(array))
        {
            throw new ArgumentOutOfRangeException("index");
        }
        _SetByte(array, index, value);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    private static extern int _ByteLength(Array array);

    /// <summary>Returns the number of bytes in the specified array.</summary>
    /// <param name="array">An array.</param>
    /// <returns>The number of bytes in the array.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="array" /> is not a primitive.</exception>
    /// <exception cref="T:System.OverflowException">
    ///   <paramref name="array" /> is larger than 2 gigabytes (GB).</exception>
    [SecuritySafeCritical]
    [__DynamicallyInvokable]
    public static int ByteLength(Array array)
    {
        if (array == null)
        {
            throw new ArgumentNullException("array");
        }
        if (!IsPrimitiveTypeArray(array))
        {
            throw new ArgumentException(EnvironmentEx.GetResourceString("Arg_MustBePrimArray"), "array");
        }
        return _ByteLength(array);
    }

    [SecurityCritical]
    internal unsafe static void ZeroMemory(byte* src, long len)
    {
        while (len-- > 0)
        {
            src[len] = 0;
        }
    }

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void Memcpy(byte[] dest, int destIndex, byte* src, int srcIndex, int len)
    {
        if (len != 0)
        {
            fixed (byte* ptr = dest)
            {
                Memcpy(ptr + destIndex, src + srcIndex, len);
            }
        }
    }

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void Memcpy(byte* pDest, int destIndex, byte[] src, int srcIndex, int len)
    {
        if (len != 0)
        {
            fixed (byte* ptr = src)
            {
                Memcpy(pDest + destIndex, ptr + srcIndex, len);
            }
        }
    }

#if NET20 || NET35 || NET40
    [DotThreeFiveHelpers.Cores.MethodImplAttributeEx(DotThreeFiveHelpers.Cores.MethodImplOptionsEx.AggressiveInlining)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [FriendAccessAllowed]
    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void Memcpy(byte* dest, byte* src, int len)
    {
        Memmove(dest, src, (uint)len);
    }

    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void Memmove(byte* dest, byte* src, ulong len)
    {
        if ((ulong)((long)dest - (long)src) >= len && (ulong)((long)src - (long)dest) >= len)
        {
            byte* ptr = src + len;
            byte* ptr2 = dest + len;
            if (len > 16)
            {
                if (len > 64)
                {
                    if (len > 2048)
                    {
                        goto IL_010d;
                    }
                    ulong num = len >> 6;
                    do
                    {
                        *(Block64*)dest = *(Block64*)src;
                        dest += 64;
                        src += 64;
                        num--;
                    }
                    while (num != 0L);
                    len %= 64;
                    if (len <= 16)
                    {
                        *(Block16*)(ptr2 - 16) = *(Block16*)(ptr - 16);
                        return;
                    }
                }
                *(Block16*)dest = *(Block16*)src;
                if (len > 32)
                {
                    *(Block16*)(dest + 16) = *(Block16*)(src + 16);
                    if (len > 48)
                    {
                        *(Block16*)(dest + 32) = *(Block16*)(src + 32);
                    }
                }
                *(Block16*)(ptr2 - 16) = *(Block16*)(ptr - 16);
            }
            else if ((len & 0x18) != 0L)
            {
                *(long*)dest = *(long*)src;
                *(long*)(ptr2 - 8) = *(long*)(ptr - 8);
            }
            else if ((len & 4) != 0L)
            {
                *(int*)dest = *(int*)src;
                *(int*)(ptr2 - 4) = *(int*)(ptr - 4);
            }
            else if (len != 0L)
            {
                *dest = *src;
                if ((len & 2) != 0L)
                {
                    *(short*)(ptr2 - 2) = *(short*)(ptr - 2);
                }
            }
            return;
        }
        goto IL_010d;
    IL_010d:
        _Memmove(dest, src, len);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private unsafe static void _Memmove(byte* dest, byte* src, ulong len)
    {
        __Memmove(dest, src, len);
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private unsafe static extern void __Memmove(byte* dest, byte* src, ulong len);

    /// <summary>Copies a number of bytes specified as a long integer value from one address in memory to another.  
    ///  This API is not CLS-compliant.</summary>
    /// <param name="source">The address of the bytes to copy.</param>
    /// <param name="destination">The target address.</param>
    /// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
    /// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>
    ///   

#if NET20 || NET35 || NET40
    [DotThreeFiveHelpers.Cores.MethodImplAttributeEx(DotThreeFiveHelpers.Cores.MethodImplOptionsEx.AggressiveInlining)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [SecurityCritical]
    [CLSCompliant(false)]
    public unsafe static void MemoryCopy(void* source, void* destination, long destinationSizeInBytes, long sourceBytesToCopy)
    {
        if (sourceBytesToCopy > destinationSizeInBytes)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
        }
        Memmove((byte*)destination, (byte*)source, checked((ulong)sourceBytesToCopy));
    }

    /// <summary>Copies a number of bytes specified as an unsigned long integer value from one address in memory to another.  
    ///  This API is not CLS-compliant.</summary>
    /// <param name="source">The address of the bytes to copy.</param>
    /// <param name="destination">The target address.</param>
    /// <param name="destinationSizeInBytes">The number of bytes available in the destination memory block.</param>
    /// <param name="sourceBytesToCopy">The number of bytes to copy.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="sourceBytesToCopy" /> is greater than <paramref name="destinationSizeInBytes" />.</exception>

#if NET20 || NET35 || NET40
    [DotThreeFiveHelpers.Cores.MethodImplAttributeEx(DotThreeFiveHelpers.Cores.MethodImplOptionsEx.AggressiveInlining)]
#else
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
    [SecurityCritical]
    [CLSCompliant(false)]
    public unsafe static void MemoryCopy(void* source, void* destination, ulong destinationSizeInBytes, ulong sourceBytesToCopy)
    {
        if (sourceBytesToCopy > destinationSizeInBytes)
        {
            ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.sourceBytesToCopy);
        }
        Memmove((byte*)destination, (byte*)source, sourceBytesToCopy);
    }
}
