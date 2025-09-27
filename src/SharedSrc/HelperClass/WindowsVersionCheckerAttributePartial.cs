
namespace HelperClass
{
    using System.Runtime.InteropServices;
    public partial class WindowsVersionCheckerAttribute
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct OSVERSIONINFOEXW
        {
            public uint dwOSVersionInfoSize;
            public uint dwMajorVersion;
            public uint dwMinorVersion;
            public uint dwBuildNumber;
            public uint dwPlatformId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] szCSDVersion;     // Maintenance string for PSS usage
            public ushort wServicePackMajor;
            public ushort wServicePackMinor;
            public ushort wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        [DllImport("kernel32.dll")]
        static extern ulong VerSetConditionMask(
        [In] ulong ConditionMask,
        [In] uint TypeMask,
        [In] byte Condition
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool VerifyVersionInfoW(
        /*[In]*/ /*OSVERSIONINFOEXW*/ ref OSVERSIONINFOEXW lpVersionInformation,
        [In] uint dwTypeMask,
        [In] ulong dwlConditionMask
        );
    }

}
