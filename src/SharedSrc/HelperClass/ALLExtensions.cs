using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace HelperClass
{
    public static class ALLExtensions
    {
        public static T HexToDec<T>(this string value) where T : IConvertible
        {
            var converted = ulong.Parse(value, System.Globalization.NumberStyles.HexNumber);
            return (T)Convert.ChangeType(converted, typeof(T));
        }

        public static bool FreePtr(this IntPtr ptr)
        {
            if (ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);
            }

            return ptr != IntPtr.Zero;
        }

        public static void StructToZeroHollowCast<T>(this ref T vals) where T : struct
        {
            var tType = typeof(T);
            var strSize = Marshal.SizeOf(tType);
            var allocs = Marshal.AllocHGlobal(strSize);
            NativeCallerExternal.RtlZeroMemory(allocs, (nuint)Marshal.SizeOf(tType));

            vals = Marshal.PtrToStructure<T>(allocs);

            allocs.FreePtr();
        }

        public static void Log(params object[] args)
        {
            Debug.WriteLine(args);
        }

        //#if NET48_OR_GREATER || NET5_0_OR_GREATER
        //        public static void StructToZeroHollowCast<T>(this ref T vals) where T : struct
        //        {
        //            var tType = typeof(T);
        //            var strSize = Marshal.SizeOf(tType);
        //            var allocs = Marshal.AllocHGlobal(strSize);
        //            NativeCallerExternal.RtlZeroMemory(allocs, (nuint)Marshal.SizeOf(tType));

        //            vals = Marshal.PtrToStructure<T>(allocs);

        //            allocs.FreePtr();
        //        }
        //#elif NET40
        //        public static void StructToZeroHollowCast<T>(this ref T vals) where T : struct
        //        {
        //        var tType = typeof(T);
        //        var strSize = System.Runtime.InteropServices.Marshal.SizeOf(tType);
        //        var allocs = System.Runtime.InteropServices.Marshal.AllocHGlobal(strSize);
        //        HelperClass.NativeCallerExternal.RtlZeroMemory(allocs, (nuint)System.Runtime.InteropServices.Marshal.SizeOf(tType));

        //        System.Runtime.InteropServices.Marshal.PtrToStructure(allocs, vals);

        //        allocs.FreePtr();

        //        }
        //#endif
    }
}
