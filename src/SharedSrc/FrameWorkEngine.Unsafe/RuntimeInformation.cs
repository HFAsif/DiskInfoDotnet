using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace FrameWorkEngine;

/// <summary>Provides information about the .NET runtime installation.</summary>
public static class RuntimeInformation
{
    private const string FrameworkName = ".NET Framework";

    private static string s_frameworkDescription;

    private static string s_osDescription = null;

    private static object s_osLock = new object();

    private static object s_processLock = new object();

    private static Architecture? s_osArch = null;

    private static Architecture? s_processArch = null;

#if NET45_OR_GREATER
    /// <summary>Returns a string that indicates the name of the .NET installation on which an app is running.</summary>
    /// <returns>The name of the .NET installation on which the app is running.</returns>
    public static string FrameworkDescription
    {
        get
        {
            if (s_frameworkDescription == null)
            {
                AssemblyFileVersionAttribute assemblyFileVersionAttribute = (AssemblyFileVersionAttribute)typeof(object).GetTypeInfo().Assembly.GetCustomAttribute(typeof(AssemblyFileVersionAttribute));
                s_frameworkDescription = ".NET Framework " + assemblyFileVersionAttribute.Version;
            }
            return s_frameworkDescription;
        }
    }
#endif

    /// <summary>Gets a string that describes the operating system on which the app is running.</summary>
    /// <returns>The description of the operating system on which the app is running.</returns>
    public static string OSDescription
    {
        [SecuritySafeCritical]
        get
        {
            if (s_osDescription == null)
            {
                s_osDescription = RtlGetVersion();
            }
            return s_osDescription;
        }
    }

    /// <summary>Gets the platform architecture on which the current app is running.</summary>
    /// <returns>The platform architecture on which the current app is running.</returns>
    public static Architecture OSArchitecture
    {
        [SecuritySafeCritical]
        get
        {
            lock (s_osLock)
            {
                if (!s_osArch.HasValue)
                {
                    Win32Native.GetNativeSystemInfo(out var lpSystemInfo);
                    s_osArch = GetArchitecture(lpSystemInfo.wProcessorArchitecture);
                }
            }
            return s_osArch.Value;
        }
    }

    /// <summary>Gets the process architecture of the currently running app.</summary>
    /// <returns>The process architecture of the currently running app.</returns>
    public static Architecture ProcessArchitecture
    {
        [SecuritySafeCritical]
        get
        {
            lock (s_processLock)
            {
                if (!s_processArch.HasValue)
                {
                    Win32Native.SYSTEM_INFO lpSystemInfo = default(Win32Native.SYSTEM_INFO);
                    Win32Native.GetSystemInfo(ref lpSystemInfo);
                    s_processArch = GetArchitecture(lpSystemInfo.wProcessorArchitecture);
                }
            }
            return s_processArch.Value;
        }
    }

    /// <summary>Indicates whether the current application is running on the specified platform.</summary>
    /// <param name="osPlatform">A platform.</param>
    /// <returns>
    ///   <see langword="true" /> if the current app is running on the specified platform; otherwise, <see langword="false" />.</returns>
    public static bool IsOSPlatform(OSPlatform osPlatform)
    {
        return OSPlatform.Windows == osPlatform;
    }

    private static Architecture GetArchitecture(ushort wProcessorArchitecture)
    {
        Architecture result = Architecture.X86;
        switch (wProcessorArchitecture)
        {
            case 12:
                result = Architecture.Arm64;
                break;
            case 5:
                result = Architecture.Arm;
                break;
            case 9:
                result = Architecture.X64;
                break;
            case 0:
                result = Architecture.X86;
                break;
        }
        return result;
    }

    [SecuritySafeCritical]
    private static string RtlGetVersion()
    {
        Win32Native.RTL_OSVERSIONINFOEX lpVersionInformation = default(Win32Native.RTL_OSVERSIONINFOEX);
        lpVersionInformation.dwOSVersionInfoSize = (uint)Marshal.SizeOf(lpVersionInformation);
        if (Win32Native.RtlGetVersion(out lpVersionInformation) == 0)
        {
            return string.Format("{0} {1}.{2}.{3} {4}", "Microsoft Windows", lpVersionInformation.dwMajorVersion, lpVersionInformation.dwMinorVersion, lpVersionInformation.dwBuildNumber, lpVersionInformation.szCSDVersion);
        }
        return "Microsoft Windows";
    }
}