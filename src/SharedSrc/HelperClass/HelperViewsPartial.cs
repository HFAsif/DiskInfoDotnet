#if NET48_OR_GREATER || NET8_0_OR_GREATER || NET6_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP

namespace HelperClass;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

public static class HelperViews
{
    public static void MemCpyTripleStruct<T, F, Z>(ref readonly byte[] src, ref T t1, ref F f1, ref Z z1)
    {
        var tType = typeof(T);
        var fType = typeof(F);
        var zType = typeof(Z);

        var tSize = Marshal.SizeOf(tType);
        var fSize = Marshal.SizeOf(fType);
        var zSize = Marshal.SizeOf(zType);

        var tallocSrc = Marshal.AllocHGlobal(tSize);
        var fallocSrc = Marshal.AllocHGlobal(fSize);
        var zallocSrc = Marshal.AllocHGlobal(zSize);

        //Dip.RtlZeroMemory(tallocSrc, (nuint)Marshal.SizeOf(tType));
        //Dip.RtlZeroMemory(fallocSrc, (nuint)Marshal.SizeOf(fType));
        //Dip.RtlZeroMemory(zallocSrc, (nuint)Marshal.SizeOf(zType));

        Marshal.Copy(src, 0, tallocSrc, tSize);
        Marshal.Copy(src, 0, fallocSrc, fSize);
        Marshal.Copy(src, 0, zallocSrc, zSize);

        t1 = Marshal.PtrToStructure<T>(tallocSrc);
        f1 = Marshal.PtrToStructure<F>(fallocSrc);
        z1 = Marshal.PtrToStructure<Z>(zallocSrc);

        tallocSrc.FreePtr();
        fallocSrc.FreePtr();
        zallocSrc.FreePtr();
    }

    public static void StructToZeroStruct<T>(ref T strct) where T : struct
    {
        //var tType = typeof(T);
        var strSize = Marshal.SizeOf(strct);
        var allocs = Marshal.AllocHGlobal(strSize);
        NativeCallerExternal.RtlZeroMemory(allocs, (nuint)strSize);

        strct = Marshal.PtrToStructure<T>(allocs);
        allocs.FreePtr();

    }


    //#define RtlZeroMemory(Destination,Length) memset((Destination),0,(Length))
    public static void RtlZeroMemory(IntPtr destination, int length)
    {
        Marshal.Copy(new byte[length], 0, destination, length);
    }

    public static IntPtr RtlZeroMemoryModed<T>(ref T destinationTpPtr, out int length, bool deleteOldDel) where T : struct
    {
        length = 0;

        if (typeof(T) is object obj)
        {
            length = Marshal.SizeOf(destinationTpPtr);
            var AllocDesPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr<T>(destinationTpPtr, AllocDesPtr, deleteOldDel);
            var newbytes = new byte[length];
            Marshal.Copy(newbytes, 0, AllocDesPtr, length);
            return AllocDesPtr;
        }

        Debugger.Break();
        throw new OutOfMemoryException();
    }

    public static Type TypeConverter<T>() where T : struct
    {
        return typeof(T);
    }

    public static T CopyStruct<T>(ref object s1)
    {
        GCHandle handle = GCHandle.Alloc(s1, GCHandleType.Pinned);
        T typedStruct = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        handle.Free();
        return typedStruct;
    }

    

    public static int OffsetOf<T>(string propertyName)
    {
        var type = typeof(T);
        var fieldInfo = type.GetField(propertyName);
        if (fieldInfo == null)
        {
            throw new ArgumentException($"Field '{propertyName}' not found in type '{type.FullName}'.");
        }
        return Marshal.OffsetOf(type, propertyName).ToInt32();
    }

    public static IntPtr InternalAllocation<T>(T obj, out int size)
    {
        size = Marshal.SizeOf(obj);
        var Allocnptwb = Marshal.AllocHGlobal(size);
        return Allocnptwb;
    }

    public static void MemCpyByteArrToArr(ref byte[] dest, ref readonly byte[] src, uint len)
    {
        var AllocDest = Marshal.AllocHGlobal(dest.Length);
        Marshal.Copy(src, 0, AllocDest, dest.Length);
        Marshal.Copy(AllocDest, dest, 0, (int)len);
        AllocDest.FreePtr();
        //CopyMemory(ref asi.SmartReadData[0], ref nptwb.Buffer[0], 512);
    }

    public static void MemCpyStructToStruct<A, B, C>(ref readonly byte[] bytes, ref A a, ref B b, ref C c)
    {
        var a1 = typeof(A);
        var b1 = typeof(B);
        var c1 = typeof(C);

        //var arrSize = bytes.Length;
        var a1Size = Marshal.SizeOf(a1);
        var b1Size = Marshal.SizeOf(b1);
        var c1Size = Marshal.SizeOf(c1);

        var a1Alloc = Marshal.AllocHGlobal(a1Size);
        var b1Alloc = Marshal.AllocHGlobal(b1Size);
        var c1Alloc = Marshal.AllocHGlobal(c1Size);

        Marshal.StructureToPtr(a, a1Alloc, false);
        Marshal.StructureToPtr(b, b1Alloc, false);
        Marshal.StructureToPtr(c, c1Alloc, false);

        //var a1Array = new byte[a1Size];
        //var b1Array = new byte[b1Size];
        //var c1Array = new byte[c1Size];


        //Marshal.Copy(a1Alloc, a1Array, 0, a1Size);
        //Marshal.Copy(b1Alloc, b1Array, 0, b1Size);
        //Marshal.Copy(c1Alloc, c1Array, 0, c1Size);

        Marshal.Copy(bytes, 0, a1Alloc, bytes.Length);
        Marshal.Copy(bytes, 0, b1Alloc, bytes.Length);
        Marshal.Copy(bytes, 0, c1Alloc, bytes.Length);

        a = Marshal.PtrToStructure<A>(a1Alloc);
        b = Marshal.PtrToStructure<B>(b1Alloc);
        c = Marshal.PtrToStructure<C>(c1Alloc);

        a1Alloc.FreePtr();
        b1Alloc.FreePtr();
        c1Alloc.FreePtr();
    }

    public static void MemCpyStructToStruct<SrcDest>(ref SrcDest dest, ref SrcDest src)
    {
        var targetSizes = Marshal.SizeOf(src);
        var allocSrc = Marshal.AllocHGlobal(targetSizes);
        Marshal.StructureToPtr(src, allocSrc, true);
        var SrcArray = new byte[targetSizes];
        Marshal.Copy(allocSrc, SrcArray, 0, targetSizes);
        var allocDest = Marshal.AllocHGlobal(targetSizes);
        Marshal.Copy(SrcArray, 0, allocDest, targetSizes);
        dest = Marshal.PtrToStructure<SrcDest>(allocDest);
        allocSrc.FreePtr();
        allocDest.FreePtr();
    }

    public static void MemCpyStructToStruct<T>(ref T dest, ref readonly byte[] src, uint BytePosition, uint sizes)
    {
        var destSize = Marshal.SizeOf(dest);
        var destAlloc = Marshal.AllocHGlobal(destSize);
        Marshal.StructureToPtr(dest, destAlloc, false);

        Marshal.Copy(src, (int)BytePosition, destAlloc, (int)sizes);

        dest = Marshal.PtrToStructure<T>(destAlloc);
        destAlloc.FreePtr();

        #region MyRegion
        ////CopyMemory(ref asi.Attribute[j], ref asi.SmartReadData[i * Marshal.SizeOf(typeof(SMART_ATTRIBUTE)) + 2], (uint)Marshal.SizeOf(typeof(SMART_ATTRIBUTE)));

        ////var destSize = Marshal.SizeOf(dest);

        //var destSize = Marshal.SizeOf(dest);
        //var destAlloc = Marshal.AllocHGlobal(destSize);
        //Marshal.StructureToPtr(dest, destAlloc, true);

        //var destArray = new byte[destSize];
        //Marshal.Copy(destAlloc, destArray, 0, destSize);

        //var srcSize = Marshal.SizeOf(src);
        //var allocSrc = Marshal.AllocHGlobal(srcSize);
        //Marshal.StructureToPtr(src, allocSrc, true);
        //var srcArray = new byte[srcSize];


        //Buffer.BlockCopy(srcArray, 0, destArray, 0, (int)sizes);

        //Buffer.BlockCopy(srcArray, 0, Dest, 0, (int)sizes);

        #endregion

#if NET8_0_OR_GREATER
            //Unsafe.CopyBlock(ref destByte, in src, sizes);

#endif
    }


    public static void MemCpyByteArrayToStruct<T>(ref readonly byte[] src, ref T Dest, uint sizes)
    {
        var destSize = Marshal.SizeOf(Dest);
        var destAlloc = Marshal.AllocHGlobal(destSize);
        Marshal.StructureToPtr(Dest, destAlloc, false);
        Marshal.Copy(src, 0, destAlloc, (int)sizes);
        Dest = Marshal.PtrToStructure<T>(destAlloc);

        destAlloc.FreePtr();
    }

    public static void MemCpyByteArrayToStruct<T>(ref readonly T Src, ref byte[] Dest, uint sizes)
    {
        var allocSrc = Marshal.AllocHGlobal(Marshal.SizeOf(Src));
        Marshal.StructureToPtr(Src, allocSrc, true);
        var srcArray = new byte[sizes];
        Marshal.Copy(allocSrc, srcArray, 0, (int)sizes);
        Buffer.BlockCopy(srcArray, 0, Dest, 0, (int)sizes);
        Marshal.DestroyStructure<T>(allocSrc);
        allocSrc.FreePtr();
    }

#if NET5_0_OR_GREATER
        [SecurityCritical]
        public static object GetActiveObject(string progID)
        {
            object? ppunk = null;
            Guid clsid;
            try
            {
            NativeCallerExternal.CLSIDFromProgIDEx(progID, out clsid);
            }
            catch (Exception)
            {
            NativeCallerExternal.CLSIDFromProgID(progID, out clsid);
            }
            NativeCallerExternal.GetActiveObject(ref clsid, IntPtr.Zero, out ppunk);
            return ppunk;
        }
#endif
    public static IEnumerable<Assembly> GetAssemblies()
    {
        var list = new List<string>();
        var stack = new Stack<Assembly>();

#pragma warning disable CS8604 // Possible null reference argument.
        stack.Push(Assembly.GetEntryAssembly());
#pragma warning restore CS8604 // Possible null reference argument.

        do
        {
            var asm = stack.Pop();

            yield return asm;

            foreach (var reference in asm.GetReferencedAssemblies())
                if (!list.Contains(reference.FullName))
                {
                    stack.Push(Assembly.Load(reference));
                    list.Add(reference.FullName);
                }

        }
        while (stack.Count > 0);

    }

    public static DirectoryInfo TryGetSolutionDirectoryInfo(string? currentPath = null)
    {
        currentPath = AppDomain.CurrentDomain.BaseDirectory;
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }
        return directory ?? throw new InvalidOperationException();
    }

    public static List<string> TryGetSolutionAllproject(string SlnPath)
    {
        if (!File.Exists(SlnPath) && string.IsNullOrEmpty(SlnPath)) System.Diagnostics.Debugger.Break();

        var Content = File.ReadAllText(SlnPath);
        Regex projReg = new Regex(
            "Project\\(\"\\{[\\w-]*\\}\"\\) = \"([\\w _]*.*)\", \"(.*\\.(cs|vcx|vb)proj)\""
            , RegexOptions.Compiled);
        var matches = projReg.Matches(Content).Cast<Match>();
        var Projects = matches.Select(x => x.Groups[2].Value).ToList();
        for (int i = 0; i < Projects.Count; ++i)
        {
            if (!Path.IsPathRooted(Projects[i]))
#pragma warning disable CS8604 // Possible null reference argument.
                Projects[i] = Path.Combine(Path.GetDirectoryName(SlnPath),
                    Projects[i]);
#pragma warning restore CS8604 // Possible null reference argument.
            Projects[i] = Path.GetFullPath(Projects[i]);
        }

        return Projects;
    }
}

#endif