
namespace HelperClass
{
    using System;
    using System.Runtime.InteropServices;
    using Cta = CAtaSmartArtificial;
    using static ImportantElements_Properties;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public partial class WindowsVersionCheckerAttribute : Attribute
    {
        //public bool Insalled = false;
        public bool m_bAtaPassThrough;
        public bool m_bAtaPassThroughSmart;
        public bool m_bNVMeStorageQuery;
        public IntPtr hMutexJMicron { get; private set; }

        bool IsWindowsVersionOrGreaterFx(ushort wMajorVersion, ushort wMinorVersion, ushort wServicePackMajor = 0)
        {
            OSVERSIONINFOEXW osvi = new OSVERSIONINFOEXW
            {
                dwOSVersionInfoSize = (uint)Marshal.SizeOf(typeof(OSVERSIONINFOEXW)),
                dwMajorVersion = 0,
                dwMinorVersion = 0,
                dwBuildNumber = 0,
                dwPlatformId = 0,
                szCSDVersion = new char[128],
                wServicePackMajor = 0,
                wServicePackMinor = 0,
                wSuiteMask = 0,
                wProductType = 0,
                wReserved = 0

            };

            var dwlConditionMask = VerSetConditionMask(
                VerSetConditionMask(
                    VerSetConditionMask(
                        0, VER_MAJORVERSION, VER_GREATER_EQUAL),
                    VER_MINORVERSION, VER_GREATER_EQUAL),
                VER_SERVICEPACKMAJOR, VER_GREATER_EQUAL);

            osvi.dwMajorVersion = wMajorVersion;
            osvi.dwMinorVersion = wMinorVersion;
            osvi.wServicePackMajor = wServicePackMajor;

            return VerifyVersionInfoW(ref osvi, VER_MAJORVERSION | VER_MINORVERSION | VER_SERVICEPACKMAJOR, dwlConditionMask) != false;
        }

        public bool WindowsVesionChecker()
        {
            m_bAtaPassThrough = false;
            m_bAtaPassThroughSmart = false;
            m_bNVMeStorageQuery = false;

            if (IsWindowsVersionOrGreaterFx(10, 0))
            {
                m_bAtaPassThrough = true;
                m_bAtaPassThroughSmart = true;
                m_bNVMeStorageQuery = true;
            }
            else if (IsWindowsVersionOrGreaterFx(6, 0) || IsWindowsVersionOrGreaterFx(5, 2))
            {
                m_bAtaPassThrough = true;
                m_bAtaPassThroughSmart = true;
            }
            else if (IsWindowsVersionOrGreaterFx(5, 1))
            {
                if (IsWindowsVersionOrGreaterFx(5, 1, 2))
                {
                    m_bAtaPassThrough = true;
                    m_bAtaPassThroughSmart = true;
                }
            }

            hMutexJMicron = Cta.CreateWorldMutex("Access_JMicron_SMART");
            return true;
        }
    }
}
