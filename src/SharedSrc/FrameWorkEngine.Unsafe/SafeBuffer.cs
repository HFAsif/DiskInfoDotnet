#if !NET45_OR_GREATER

using HelperClass;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace FrameWorkEngine;
/// <summary>Provides a controlled memory buffer that can be used for reading and writing. Attempts to access memory outside the controlled buffer (underruns and overruns) raise exceptions.</summary>
[SecurityCritical]
[__DynamicallyInvokable]
public abstract class SafeBuffer : SafeHandleZeroOrMinusOneIsInvalid
{
    private static readonly UIntPtr Uninitialized = ((UIntPtr.Size == 4) ? ((UIntPtr)uint.MaxValue) : ((UIntPtr)ulong.MaxValue));

    private UIntPtr _numBytes;

    /// <summary>Gets the size of the buffer, in bytes.</summary>
    /// <returns>The number of bytes in the memory buffer.</returns>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public ulong ByteLength
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [__DynamicallyInvokable]
        get
        {
            if (_numBytes == Uninitialized)
            {
                throw NotInitialized();
            }
            return (ulong)_numBytes;
        }
    }

    /// <summary>Creates a new instance of the <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> class, and specifies whether the buffer handle is to be reliably released.</summary>
    /// <param name="ownsHandle">
    ///   <see langword="true" /> to reliably release the handle during the finalization phase; <see langword="false" /> to prevent reliable release (not recommended).</param>
    [__DynamicallyInvokable]
    protected SafeBuffer(bool ownsHandle)
        : base(ownsHandle)
    {
        _numBytes = Uninitialized;
    }

    /// <summary>Defines the allocation size of the memory region in bytes. You must call this method before you use the <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> instance.</summary>
    /// <param name="numBytes">The number of bytes in the buffer.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="numBytes" /> is less than zero.  
    /// -or-  
    /// <paramref name="numBytes" /> is greater than the available address space.</exception>
    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public void Initialize(ulong numBytes)
    {
        if (numBytes < 0)
        {
            throw new ArgumentOutOfRangeException("numBytes", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (IntPtr.Size == 4 && numBytes > uint.MaxValue)
        {
            throw new ArgumentOutOfRangeException("numBytes", EnvironmentEx.GetResourceString("ArgumentOutOfRange_AddressSpace"));
        }
        if (numBytes >= (ulong)Uninitialized)
        {
            throw new ArgumentOutOfRangeException("numBytes", EnvironmentEx.GetResourceString("ArgumentOutOfRange_UIntPtrMax-1"));
        }
        _numBytes = (UIntPtr)numBytes;
    }

    /// <summary>Specifies the allocation size of the memory buffer by using the specified number of elements and element size. You must call this method before you use the <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> instance.</summary>
    /// <param name="numElements">The number of elements in the buffer.</param>
    /// <param name="sizeOfEachElement">The size of each element in the buffer.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="numElements" /> is less than zero.  
    /// -or-  
    /// <paramref name="sizeOfEachElement" /> is less than zero.  
    /// -or-  
    /// <paramref name="numElements" /> multiplied by <paramref name="sizeOfEachElement" /> is greater than the available address space.</exception>
    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public void Initialize(uint numElements, uint sizeOfEachElement)
    {
        if (numElements < 0)
        {
            throw new ArgumentOutOfRangeException("numElements", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (sizeOfEachElement < 0)
        {
            throw new ArgumentOutOfRangeException("sizeOfEachElement", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (IntPtr.Size == 4 && numElements * sizeOfEachElement > uint.MaxValue)
        {
            throw new ArgumentOutOfRangeException("numBytes", EnvironmentEx.GetResourceString("ArgumentOutOfRange_AddressSpace"));
        }
        if (numElements * sizeOfEachElement >= (ulong)Uninitialized)
        {
            throw new ArgumentOutOfRangeException("numElements", EnvironmentEx.GetResourceString("ArgumentOutOfRange_UIntPtrMax-1"));
        }
        _numBytes = (UIntPtr)checked(numElements * sizeOfEachElement);
    }

    /// <summary>Defines the allocation size of the memory region by specifying the number of value types. You must call this method before you use the <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> instance.</summary>
    /// <param name="numElements">The number of elements of the value type to allocate memory for.</param>
    /// <typeparam name="T">The value type to allocate memory for.</typeparam>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="numElements" /> is less than zero.  
    /// -or-  
    /// <paramref name="numElements" /> multiplied by the size of each element is greater than the available address space.</exception>
    [CLSCompliant(false)]
    [__DynamicallyInvokable]
    public void Initialize<T>(uint numElements) where T : struct
    {
        Initialize(numElements, InternalExtensions.AlignedSizeOf<T>());
    }

    /// <summary>Obtains a pointer from a <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> object for a block of memory.</summary>
    /// <param name="pointer">A byte pointer, passed by reference, to receive the pointer from within the <see cref="T:System.Runtime.InteropServices.SafeBuffer" /> object. You must set this pointer to <see langword="null" /> before you call this method.</param>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    public unsafe void AcquirePointer(ref byte* pointer)
    {
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }
        pointer = null;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
        }
        finally
        {
            bool success = false;
            DangerousAddRef(ref success);
            pointer = (byte*)(void*)handle;
        }
    }

    /// <summary>Releases a pointer that was obtained by the <see cref="M:System.Runtime.InteropServices.SafeBuffer.AcquirePointer(System.Byte*@)" /> method.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    [__DynamicallyInvokable]
    public void ReleasePointer()
    {
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }
        DangerousRelease();
    }

    /// <summary>Reads a value type from memory at the specified offset.</summary>
    /// <param name="byteOffset">The location from which to read the value type. You may have to consider alignment issues.</param>
    /// <typeparam name="T">The value type to read.</typeparam>
    /// <returns>The value type that was read from memory.</returns>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [__DynamicallyInvokable]
    public unsafe T Read<T>(ulong byteOffset) where T : struct
    {
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }

        uint num = InternalExtensions.SizeOfType(typeof(T));
        byte* ptr = (byte*)(void*)handle + byteOffset;
        SpaceCheck(ptr, num);
        bool success = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
            DangerousAddRef(ref success);
            GenericPtrToStructure<T>(ptr, out var structure, num);
            return structure;
        }
        finally
        {
            if (success)
            {
                DangerousRelease();
            }
        }
    }

    /// <summary>Reads the specified number of value types from memory starting at the offset, and writes them into an array starting at the index.</summary>
    /// <param name="byteOffset">The location from which to start reading.</param>
    /// <param name="array">The output array to write to.</param>
    /// <param name="index">The location in the output array to begin writing to.</param>
    /// <param name="count">The number of value types to read from the input array and to write to the output array.</param>
    /// <typeparam name="T">The value type to read.</typeparam>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> is less than zero.  
    /// -or-  
    /// <paramref name="count" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The length of the array minus the index is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [__DynamicallyInvokable]
    public unsafe void ReadArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
    {
        if (array == null)
        {
            throw new ArgumentNullException("array", EnvironmentEx.GetResourceString("ArgumentNull_Buffer"));
        }
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (array.Length - index < count)
        {
            throw new ArgumentException(EnvironmentEx.GetResourceString("Argument_InvalidOffLen"));
        }
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }
        uint sizeofT = InternalExtensions.SizeOfType(typeof(T));
        uint num = InternalExtensions.AlignedSizeOf<T>();
        byte* ptr = (byte*)(void*)handle + byteOffset;
        SpaceCheck(ptr, checked((ulong)(num * count)));
        bool success = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
            DangerousAddRef(ref success);
            for (int i = 0; i < count; i++)
            {
                GenericPtrToStructure<T>(ptr + num * i, out array[i + index], sizeofT);
            }
        }
        finally
        {
            if (success)
            {
                DangerousRelease();
            }
        }
    }

    /// <summary>Writes a value type to memory at the given location.</summary>
    /// <param name="byteOffset">The location at which to start writing. You may have to consider alignment issues.</param>
    /// <param name="value">The value to write.</param>
    /// <typeparam name="T">The value type to write.</typeparam>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [__DynamicallyInvokable]
    public unsafe void Write<T>(ulong byteOffset, T value) where T : struct
    {
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }
        uint num = InternalExtensions.SizeOfType(typeof(T));
        byte* ptr = (byte*)(void*)handle + byteOffset;
        SpaceCheck(ptr, num);
        bool success = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
            DangerousAddRef(ref success);
            GenericStructureToPtr(ref value, ptr, num);
        }
        finally
        {
            if (success)
            {
                DangerousRelease();
            }
        }
    }

    /// <summary>Writes the specified number of value types to a memory location by reading bytes starting from the specified location in the input array.</summary>
    /// <param name="byteOffset">The location in memory to write to.</param>
    /// <param name="array">The input array.</param>
    /// <param name="index">The offset in the array to start reading from.</param>
    /// <param name="count">The number of value types to write.</param>
    /// <typeparam name="T">The value type to write.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///   <paramref name="index" /> or <paramref name="count" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The length of the input array minus <paramref name="index" /> is less than <paramref name="count" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="Overload:System.Runtime.InteropServices.SafeBuffer.Initialize" /> method has not been called.</exception>
    [CLSCompliant(false)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    [__DynamicallyInvokable]
    public unsafe void WriteArray<T>(ulong byteOffset, T[] array, int index, int count) where T : struct
    {
        if (array == null)
        {
            throw new ArgumentNullException("array", EnvironmentEx.GetResourceString("ArgumentNull_Buffer"));
        }
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException("count", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
        }
        if (array.Length - index < count)
        {
            throw new ArgumentException(EnvironmentEx.GetResourceString("Argument_InvalidOffLen"));
        }
        if (_numBytes == Uninitialized)
        {
            throw NotInitialized();
        }
        uint sizeofT = InternalExtensions.SizeOfType(typeof(T));
        uint num = InternalExtensions.AlignedSizeOf<T>();
        byte* ptr = (byte*)(void*)handle + byteOffset;
        SpaceCheck(ptr, checked((ulong)(num * count)));
        bool success = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
            DangerousAddRef(ref success);
            for (int i = 0; i < count; i++)
            {
                GenericStructureToPtr(ref array[i + index], ptr + num * i, sizeofT);
            }
        }
        finally
        {
            if (success)
            {
                DangerousRelease();
            }
        }
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private unsafe void SpaceCheck(byte* ptr, ulong sizeInBytes)
    {
        if ((ulong)_numBytes < sizeInBytes)
        {
            NotEnoughRoom();
        }
        if ((ulong)(ptr - (byte*)(void*)handle) > (ulong)_numBytes - sizeInBytes)
        {
            NotEnoughRoom();
        }
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private static void NotEnoughRoom()
    {
        throw new ArgumentException(EnvironmentEx.GetResourceString("Arg_BufferTooSmall"));
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private static InvalidOperationException NotInitialized()
    {
        return new InvalidOperationException(EnvironmentEx.GetResourceString("InvalidOperation_MustCallInitialize"));
    }

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void GenericPtrToStructure<T>(byte* ptr, out T structure, uint sizeofT) where T : struct
    {
        structure = default(T);
        PtrToStructureNative(ptr, __makeref(structure), sizeofT);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private unsafe static extern void PtrToStructureNative(byte* ptr, TypedReference structure, uint sizeofT);

    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    internal unsafe static void GenericStructureToPtr<T>(ref T structure, byte* ptr, uint sizeofT) where T : struct
    {
        StructureToPtrNative(__makeref(structure), ptr, sizeofT);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    private unsafe static extern void StructureToPtrNative(TypedReference structure, byte* ptr, uint sizeofT);
}
#endif