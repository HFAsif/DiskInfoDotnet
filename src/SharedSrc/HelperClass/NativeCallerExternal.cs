namespace HelperClass;
using System;
using System.Runtime.InteropServices;
using System.Security;

public class NativeCallerExternal
{
    [DllImport("ole32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    public static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

    [DllImport("ole32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    public static extern void CLSIDFromProgID([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

    [DllImport("oleaut32.dll", PreserveSig = false)]
    [SuppressUnmanagedCodeSecurity]
    [SecurityCritical]
    public static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);



    [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Memcmp(IntPtr buf1, IntPtr buf2, uint size);

    [DllImport("kernel32.dll")]
    public static extern void RtlZeroMemory(IntPtr ptr, UIntPtr cnt);

    [DllImport("kernel32.dll")]
    public static extern bool VirtualLock(IntPtr lpAddress, UIntPtr dwSize);

    [DllImport("kernel32.dll")]
    public static extern bool VirtualUnlock(IntPtr lpAddress, UIntPtr dwSize);


    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LocalAlloc([In] uint uFlags, [In] UIntPtr uBytes);

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    public static extern void RtlCopyMemory(IntPtr Destination, IntPtr Source, uint Length);

    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateFile([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
    [In] uint dwDesiredAccess,
    [In] uint dwShareMode,
    [In, Optional] IntPtr lpSecurityAttributes,
    [In] uint dwCreationDisposition,
    [In] uint dwFlagsAndAttributes,
    [In, Optional] IntPtr hTemplateFile
    );

    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int strncpy_s(
    byte[] destination,
    /*UIntPtr*/ nint sizeInBytes,
    byte[] source,
    /*UIntPtr*/ nint maxCount
        );

    //CMD_IDE_PATH_THROUGH
    //SENDCMDINPARAMS

    [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool safeCloseHandle([In] IntPtr hObject);
}
