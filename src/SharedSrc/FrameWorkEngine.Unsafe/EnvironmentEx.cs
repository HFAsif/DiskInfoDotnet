using Microsoft.Win32;
using System;
using System.Collections;
#if NET45_OR_GREATER
using System.Diagnostics.Contracts;
#endif
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Threading;


namespace FrameWorkEngine;
/// <summary>Provides information about, and means to manipulate, the current environment and platform. This class cannot be inherited.</summary>
[ComVisible(true)]
[__DynamicallyInvokable]
public static class EnvironmentEx
{
    internal sealed class ResourceHelper
    {
        internal class GetResourceStringUserData
        {
            public ResourceHelper m_resourceHelper;

            public string m_key;

            public CultureInfo m_culture;

            public string m_retVal;

            public bool m_lockWasTaken;

            public GetResourceStringUserData(ResourceHelper resourceHelper, string key, CultureInfo culture)
            {
                m_resourceHelper = resourceHelper;
                m_key = key;
                m_culture = culture;
            }
        }

        private string m_name;

        private ResourceManager SystemResMgr;

        private Stack currentlyLoading;

        internal bool resourceManagerInited;

        private int infinitelyRecursingCount;

        internal ResourceHelper(string name)
        {
            m_name = name;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal string GetResourceString(string key)
        {
            if (key == null || key.Length == 0)
            {
                return "[Resource lookup failed - null or empty resource name]";
            }
            return GetResourceString(key, null);
        }

        [SecuritySafeCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        internal string GetResourceString(string key, CultureInfo culture)
        {
            if (key == null || key.Length == 0)
            {
                return "[Resource lookup failed - null or empty resource name]";
            }
            GetResourceStringUserData getResourceStringUserData = new GetResourceStringUserData(this, key, culture);
            RuntimeHelpers.TryCode code = GetResourceStringCode;
            RuntimeHelpers.CleanupCode backoutCode = GetResourceStringBackoutCode;
            RuntimeHelpers.ExecuteCodeWithGuaranteedCleanup(code, backoutCode, getResourceStringUserData);
            return getResourceStringUserData.m_retVal;
        }

        [SecuritySafeCritical]
        private void GetResourceStringCode(object userDataIn)
        {
            GetResourceStringUserData getResourceStringUserData = (GetResourceStringUserData)userDataIn;
            ResourceHelper resourceHelper = getResourceStringUserData.m_resourceHelper;
            string key = getResourceStringUserData.m_key;
            CultureInfo culture = getResourceStringUserData.m_culture;
            MonitorEx.Enter(resourceHelper, ref getResourceStringUserData.m_lockWasTaken);
            if (resourceHelper.currentlyLoading != null && resourceHelper.currentlyLoading.Count > 0 && resourceHelper.currentlyLoading.Contains(key))
            {
                if (resourceHelper.infinitelyRecursingCount > 0)
                {
                    getResourceStringUserData.m_retVal = "[Resource lookup failed - infinite recursion or critical failure detected.]";
                    return;
                }
                resourceHelper.infinitelyRecursingCount++;
                string message = "Infinite recursion during resource lookup within mscorlib.  This may be a bug in mscorlib, or potentially in certain extensibility points such as assembly resolve events or CultureInfo names.  Resource name: " + key;
                Assert.Fail("[mscorlib recursive resource lookup bug]", message, -2146232797, TraceFormat.NoResourceLookup);
                FailFast(message);
            }
            if (resourceHelper.currentlyLoading == null)
            {
                resourceHelper.currentlyLoading = new Stack(4);
            }
            if (!resourceHelper.resourceManagerInited)
            {
                RuntimeHelpers.PrepareConstrainedRegions();
                try
                {
                }
                finally
                {
                    RuntimeHelpers.RunClassConstructor(typeof(ResourceManager).TypeHandle);
                    RuntimeHelpers.RunClassConstructor(typeof(ResourceReader).TypeHandle);
                    //RuntimeHelpers.RunClassConstructor(typeof(RuntimeResourceSet).TypeHandle);
                    RuntimeHelpers.RunClassConstructor(typeof(BinaryReader).TypeHandle);
                    resourceHelper.resourceManagerInited = true;
                }
            }
            resourceHelper.currentlyLoading.Push(key);
            if (resourceHelper.SystemResMgr == null)
            {
                resourceHelper.SystemResMgr = new ResourceManager(m_name, typeof(object).Assembly);
            }
            string @string = resourceHelper.SystemResMgr.GetString(key, null);
            resourceHelper.currentlyLoading.Pop();
            getResourceStringUserData.m_retVal = @string;
        }

        [PrePrepareMethod]
        private void GetResourceStringBackoutCode(object userDataIn, bool exceptionThrown)
        {
            GetResourceStringUserData getResourceStringUserData = (GetResourceStringUserData)userDataIn;
            ResourceHelper resourceHelper = getResourceStringUserData.m_resourceHelper;
            if (exceptionThrown && getResourceStringUserData.m_lockWasTaken)
            {
                resourceHelper.SystemResMgr = null;
                resourceHelper.currentlyLoading = null;
            }
            if (getResourceStringUserData.m_lockWasTaken)
            {
                Monitor.Exit(resourceHelper);
            }
        }
    }

    /// <summary>Specifies options to use for getting the path to a special folder.</summary>
    public enum SpecialFolderOption
    {
        /// <summary>The path to the folder is verified. If the folder exists, the path is returned. If the folder does not exist, an empty string is returned. This is the default behavior.</summary>
        None = 0,
        /// <summary>The path to the folder is created if it does not already exist.</summary>
        Create = 32768,
        /// <summary>The path to the folder is returned without verifying whether the path exists. If the folder is located on a network, specifying this option can reduce lag time.</summary>
        DoNotVerify = 16384
    }

    /// <summary>Specifies enumerated constants used to retrieve directory paths to system special folders.</summary>
    [ComVisible(true)]
    public enum SpecialFolder
    {
        /// <summary>The directory that serves as a common repository for application-specific data for the current roaming user.</summary>
        ApplicationData = 26,
        /// <summary>The directory that serves as a common repository for application-specific data that is used by all users.</summary>
        CommonApplicationData = 35,
        /// <summary>The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.</summary>
        LocalApplicationData = 28,
        /// <summary>The directory that serves as a common repository for Internet cookies.</summary>
        Cookies = 33,
        /// <summary>The logical Desktop rather than the physical file system location.</summary>
        Desktop = 0,
        /// <summary>The directory that serves as a common repository for the user's favorite items.</summary>
        Favorites = 6,
        /// <summary>The directory that serves as a common repository for Internet history items.</summary>
        History = 34,
        /// <summary>The directory that serves as a common repository for temporary Internet files.</summary>
        InternetCache = 32,
        /// <summary>The directory that contains the user's program groups.</summary>
        Programs = 2,
        /// <summary>The My Computer folder.</summary>
        MyComputer = 17,
        /// <summary>The My Music folder.</summary>
        MyMusic = 13,
        /// <summary>The My Pictures folder.</summary>
        MyPictures = 39,
        /// <summary>The file system directory that serves as a repository for videos that belong to a user.  Added in the .NET Framework 4.</summary>
        MyVideos = 14,
        /// <summary>The directory that contains the user's most recently used documents.</summary>
        Recent = 8,
        /// <summary>The directory that contains the Send To menu items.</summary>
        SendTo = 9,
        /// <summary>The directory that contains the Start menu items.</summary>
        StartMenu = 11,
        /// <summary>The directory that corresponds to the user's Startup program group.</summary>
        Startup = 7,
        /// <summary>The System directory.</summary>
        System = 37,
        /// <summary>The directory that serves as a common repository for document templates.</summary>
        Templates = 21,
        /// <summary>The directory used to physically store file objects on the desktop.</summary>
        DesktopDirectory = 16,
        /// <summary>The directory that serves as a common repository for documents.</summary>
        Personal = 5,
        /// <summary>The My Documents folder.</summary>
        MyDocuments = 5,
        /// <summary>The program files directory.  
        ///  On a non-x86 system, passing <see cref="F:System.Environment.SpecialFolder.ProgramFiles" /> to the <see cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)" /> method returns the path for non-x86 programs. To get the x86 program files directory on a non-x86 system, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
        ProgramFiles = 38,
        /// <summary>The directory for components that are shared across applications.  
        ///  To get the x86 common program files directory on a non-x86 system, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
        CommonProgramFiles = 43,
        /// <summary>The file system directory that is used to store administrative tools for an individual user. The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user. Added in the .NET Framework 4.</summary>
        AdminTools = 48,
        /// <summary>The file system directory that acts as a staging area for files waiting to be written to a CD. Added in the .NET Framework 4.</summary>
        CDBurning = 59,
        /// <summary>The file system directory that contains administrative tools for all users of the computer. Added in the .NET Framework 4.</summary>
        CommonAdminTools = 47,
        /// <summary>The file system directory that contains documents that are common to all users. This special folder is valid for Windows NT systems, Windows 95, and Windows 98 systems with Shfolder.dll installed. Added in the .NET Framework 4.</summary>
        CommonDocuments = 46,
        /// <summary>The file system directory that serves as a repository for music files common to all users. Added in the .NET Framework 4.</summary>
        CommonMusic = 53,
        /// <summary>This value is recognized in Windows Vista for backward compatibility, but the special folder itself is no longer used. Added in the .NET Framework 4.</summary>
        CommonOemLinks = 58,
        /// <summary>The file system directory that serves as a repository for image files common to all users. Added in the .NET Framework 4.</summary>
        CommonPictures = 54,
        /// <summary>The file system directory that contains the programs and folders that appear on the Start menu for all users. This special folder is valid only for Windows NT systems. Added in the .NET Framework 4.</summary>
        CommonStartMenu = 22,
        /// <summary>A folder for components that are shared across applications. This special folder is valid only for Windows NT, Windows 2000, and Windows XP systems. Added in the .NET Framework 4.</summary>
        CommonPrograms = 23,
        /// <summary>The file system directory that contains the programs that appear in the Startup folder for all users. This special folder is valid only for Windows NT systems. Added in the .NET Framework 4.</summary>
        CommonStartup = 24,
        /// <summary>The file system directory that contains files and folders that appear on the desktop for all users. This special folder is valid only for Windows NT systems. Added in the .NET Framework 4.</summary>
        CommonDesktopDirectory = 25,
        /// <summary>The file system directory that contains the templates that are available to all users. This special folder is valid only for Windows NT systems.  Added in the .NET Framework 4.</summary>
        CommonTemplates = 45,
        /// <summary>The file system directory that serves as a repository for video files common to all users. Added in the .NET Framework 4.</summary>
        CommonVideos = 55,
        /// <summary>A virtual folder that contains fonts. Added in the .NET Framework 4.</summary>
        Fonts = 20,
        /// <summary>A file system directory that contains the link objects that may exist in the My Network Places virtual folder. Added in the .NET Framework 4.</summary>
        NetworkShortcuts = 19,
        /// <summary>The file system directory that contains the link objects that can exist in the Printers virtual folder. Added in the .NET Framework 4.</summary>
        PrinterShortcuts = 27,
        /// <summary>The user's profile folder. Applications should not create files or folders at this level; they should put their data under the locations referred to by <see cref="F:System.Environment.SpecialFolder.ApplicationData" />. Added in the .NET Framework 4.</summary>
        UserProfile = 40,
        /// <summary>The Program Files folder. Added in the .NET Framework 4.</summary>
        CommonProgramFilesX86 = 44,
        /// <summary>The x86 Program Files folder. Added in the .NET Framework 4.</summary>
        ProgramFilesX86 = 42,
        /// <summary>The file system directory that contains resource data. Added in the .NET Framework 4.</summary>
        Resources = 56,
        /// <summary>The file system directory that contains localized resource data. Added in the .NET Framework 4.</summary>
        LocalizedResources = 57,
        /// <summary>The Windows System folder. Added in the .NET Framework 4.</summary>
        SystemX86 = 41,
        /// <summary>The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% environment variables. Added in the .NET Framework 4.</summary>
        Windows = 36
    }

    private const int MaxEnvVariableValueLength = 32767;

    private const int MaxSystemEnvVariableLength = 1024;

    private const int MaxUserEnvVariableLength = 255;

    private static volatile ResourceHelper m_resHelper;

    private const int MaxMachineNameLength = 256;

    private static object s_InternalSyncObject;

    private static volatile OperatingSystem m_os;

    private static volatile bool s_IsWindows8OrAbove;

    private static volatile bool s_CheckedOSWin8OrAbove;

    private static volatile bool s_WinRTSupported;

    private static volatile bool s_CheckedWinRT;

    private static volatile IntPtr processWinStation;

    private static volatile bool isUserNonInteractive;

    private static object InternalSyncObject
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        get
        {
            if (s_InternalSyncObject == null)
            {
                object value = new object();
                Interlocked.CompareExchange<object>(ref s_InternalSyncObject, value, (object)null);
            }
            return s_InternalSyncObject;
        }
    }

    /// <summary>Gets the number of milliseconds elapsed since the system started.</summary>
    /// <returns>A 32-bit signed integer containing the amount of time in milliseconds that has passed since the last time the computer was started.</returns>
    [__DynamicallyInvokable]
    public static extern int TickCount
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        [__DynamicallyInvokable]
        get;
    }

    internal static extern long TickCount64
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        get;
    }

    /// <summary>Gets or sets the exit code of the process.</summary>
    /// <returns>A 32-bit signed integer containing the exit code. The default value is 0 (zero), which indicates that the process completed successfully.</returns>
    public static extern int ExitCode
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        get;
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        set;
    }

    internal static bool IsCLRHosted
    {
        [SecuritySafeCritical]
        get
        {
            return GetIsCLRHosted();
        }
    }

    ///// <summary>Gets the command line for this process.</summary>
    ///// <returns>A string containing command-line arguments.</returns>
    //public static string CommandLine
    //{
    //    [SecuritySafeCritical]
    //    get
    //    {
    //        new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Demand();
    //        string s = null;
    //        GetCommandLine(JitHelpers.GetStringHandleOnStack(ref s));
    //        return s;
    //    }
    //}

    /// <summary>Gets or sets the fully qualified path of the current working directory.</summary>
    /// <returns>A string containing a directory path.</returns>
    /// <exception cref="T:System.ArgumentException">Attempted to set to an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">Attempted to set to <see langword="null." /></exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate permission.</exception>
    public static string CurrentDirectory
    {
        get
        {
            return Directory.GetCurrentDirectory();
        }
        set
        {
            Directory.SetCurrentDirectory(value);
        }
    }

    ///// <summary>Gets the fully qualified path of the system directory.</summary>
    ///// <returns>A string containing a directory path.</returns>
    //public static string SystemDirectory
    //{
    //    [SecuritySafeCritical]
    //    get
    //    {
    //        StringBuilder stringBuilder = new StringBuilder(260);
    //        if (Win32Native.GetSystemDirectory(stringBuilder, 260) == 0)
    //        {
    //            __Error.WinIOError();
    //        }
    //        string text = stringBuilder.ToString();
    //        FileIOPermission.QuickDemand(FileIOPermissionAccess.PathDiscovery, text);
    //        return text;
    //    }
    //}

    //internal static string InternalWindowsDirectory
    //{
    //    [SecurityCritical]
    //    get
    //    {
    //        StringBuilder stringBuilder = new StringBuilder(260);
    //        if (Win32Native.GetWindowsDirectory(stringBuilder, 260) == 0)
    //        {
    //            __Error.WinIOError();
    //        }
    //        return stringBuilder.ToString();
    //    }
    //}

    /// <summary>Gets the NetBIOS name of this local computer.</summary>
    /// <returns>A string containing the name of this computer.</returns>
    /// <exception cref="T:System.InvalidOperationException">The name of this computer cannot be obtained.</exception>
    public static string MachineName
    {
        [SecuritySafeCritical]
        get
        {
            new EnvironmentPermission(EnvironmentPermissionAccess.Read, "COMPUTERNAME").Demand();
            StringBuilder stringBuilder = new StringBuilder(256);
            int bufferSize = 256;
            if (Win32Native.GetComputerName(stringBuilder, ref bufferSize) == 0)
            {
                throw new InvalidOperationException(GetResourceString("InvalidOperation_ComputerName"));
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>Gets the number of processors on the current machine.</summary>
    /// <returns>The 32-bit signed integer that specifies the number of processors on the current machine. There is no default. If the current machine contains multiple processor groups, this property returns the number of logical processors that are available for use by the common language runtime (CLR).</returns>
    [__DynamicallyInvokable]
    public static int ProcessorCount
    {
        [SecuritySafeCritical]
        [__DynamicallyInvokable]
        get
        {
            return GetProcessorCount();
        }
    }

    /// <summary>Gets the number of bytes in the operating system's memory page.</summary>
    /// <returns>The number of bytes in the system memory page.</returns>
    public static int SystemPageSize
    {
        [SecuritySafeCritical]
        get
        {
            new EnvironmentPermission(PermissionState.Unrestricted).Demand();
            Win32Native.SYSTEM_INFO lpSystemInfo = default(Win32Native.SYSTEM_INFO);
            Win32Native.GetSystemInfo(ref lpSystemInfo);
            return lpSystemInfo.dwPageSize;
        }
    }

    /// <summary>Gets the newline string defined for this environment.</summary>
    /// <returns>A string containing "\r\n" for non-Unix platforms, or a string containing "\n" for Unix platforms.</returns>
    [__DynamicallyInvokable]
    public static string NewLine
    {
        [__DynamicallyInvokable]
        get
        {
            return "\r\n";
        }
    }

    /// <summary>Gets a <see cref="T:System.Version" /> object that describes the major, minor, build, and revision numbers of the common language runtime.</summary>
    /// <returns>An object that displays the version of the common language runtime.</returns>
    public static Version Version => new Version(4, 0, 30319, 42000);

    /// <summary>Gets the amount of physical memory mapped to the process context.</summary>
    /// <returns>A 64-bit signed integer containing the number of bytes of physical memory mapped to the process context.</returns>
    public static long WorkingSet
    {
        [SecuritySafeCritical]
        get
        {
            new EnvironmentPermission(PermissionState.Unrestricted).Demand();
            return GetWorkingSet();
        }
    }

    /// <summary>Gets an <see cref="T:System.OperatingSystem" /> object that contains the current platform identifier and version number.</summary>
    /// <returns>An object that contains the platform identifier and version number.</returns>
    /// <exception cref="T:System.InvalidOperationException">This property was unable to obtain the system version.  
    ///  -or-  
    ///  The obtained platform identifier is not a member of <see cref="T:System.PlatformID" /></exception>
    //public static OperatingSystem OSVersion
    //{
    //    [SecuritySafeCritical]
    //    get
    //    {
    //        if (m_os == null)
    //        {
    //            Win32Native.OSVERSIONINFO oSVERSIONINFO = new Win32Native.OSVERSIONINFO();
    //            if (!GetVersion(oSVERSIONINFO))
    //            {
    //                throw new InvalidOperationException(GetResourceString("InvalidOperation_GetVersion"));
    //            }
    //            Win32Native.OSVERSIONINFOEX oSVERSIONINFOEX = new Win32Native.OSVERSIONINFOEX();
    //            if (!GetVersionEx(oSVERSIONINFOEX))
    //            {
    //                throw new InvalidOperationException(GetResourceString("InvalidOperation_GetVersion"));
    //            }
    //            PlatformID platform = PlatformID.Win32NT;
    //            Version version = new Version(oSVERSIONINFO.MajorVersion, oSVERSIONINFO.MinorVersion, oSVERSIONINFO.BuildNumber, (oSVERSIONINFOEX.ServicePackMajor << 16) | oSVERSIONINFOEX.ServicePackMinor);
    //            m_os = new OperatingSystem(platform, version, oSVERSIONINFO.CSDVersion);
    //        }
    //        return m_os;
    //    }
    //}

    //internal static bool IsWindows8OrAbove
    //{
    //    get
    //    {
    //        if (!s_CheckedOSWin8OrAbove)
    //        {
    //            OperatingSystem oSVersion = OSVersion;
    //            s_IsWindows8OrAbove = oSVersion.Platform == PlatformID.Win32NT && ((oSVersion.Version.Major == 6 && oSVersion.Version.Minor >= 2) || oSVersion.Version.Major > 6);
    //            s_CheckedOSWin8OrAbove = true;
    //        }
    //        return s_IsWindows8OrAbove;
    //    }
    //}

    internal static bool IsWinRTSupported
    {
        [SecuritySafeCritical]
        get
        {
            if (!s_CheckedWinRT)
            {
                s_WinRTSupported = WinRTSupported();
                s_CheckedWinRT = true;
            }
            return s_WinRTSupported;
        }
    }

    /// <summary>Gets current stack trace information.</summary>
    /// <returns>A string containing stack trace information. This value can be <see cref="F:System.String.Empty" />.</returns>
    //[__DynamicallyInvokable]
    //public static string StackTrace
    //{
    //    [SecuritySafeCritical]
    //    [__DynamicallyInvokable]
    //    get
    //    {
    //        new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //        return GetStackTrace(null, needFileInfo: true);
    //    }
    //}

    /// <summary>Determines whether the current process is a 64-bit process.</summary>
    /// <returns>
    ///   <see langword="true" /> if the process is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitProcess => true;

    /// <summary>Determines whether the current operating system is a 64-bit operating system.</summary>
    /// <returns>
    ///   <see langword="true" /> if the operating system is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitOperatingSystem
    {
        [SecuritySafeCritical]
        get
        {
            return true;
        }
    }

    /// <summary>Gets a value that indicates whether the current application domain is being unloaded or the common language runtime (CLR) is shutting down.</summary>
    /// <returns>
    ///   <see langword="true" /> if the current application domain is being unloaded or the CLR is shutting down; otherwise, <see langword="false" />.</returns>
    [__DynamicallyInvokable]
    public static extern bool HasShutdownStarted
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        [SecuritySafeCritical]
        [__DynamicallyInvokable]
        get;
    }

    /// <summary>Gets the user name of the person who is currently logged on to the operating system.</summary>
    /// <returns>The user name of the person who is logged on to the operating system.</returns>
    public static string UserName
    {
        [SecuritySafeCritical]
        get
        {
            new EnvironmentPermission(EnvironmentPermissionAccess.Read, "UserName").Demand();
            StringBuilder stringBuilder = new StringBuilder(256);
            int nSize = stringBuilder.Capacity;
            if (Win32Native.GetUserName(stringBuilder, ref nSize))
            {
                return stringBuilder.ToString();
            }
            return string.Empty;
        }
    }

    /// <summary>Gets a value indicating whether the current process is running in user interactive mode.</summary>
    /// <returns>
    ///   <see langword="true" /> if the current process is running in user interactive mode; otherwise, <see langword="false" />.</returns>
    public static bool UserInteractive
    {
        [SecuritySafeCritical]
        get
        {
            IntPtr processWindowStation = Win32Native.GetProcessWindowStation();
            if (processWindowStation != IntPtr.Zero && processWinStation != processWindowStation)
            {
                int lpnLengthNeeded = 0;
                Win32Native.USEROBJECTFLAGS uSEROBJECTFLAGS = new Win32Native.USEROBJECTFLAGS();
                if (Win32Native.GetUserObjectInformation(processWindowStation, 1, uSEROBJECTFLAGS, Marshal.SizeOf(uSEROBJECTFLAGS), ref lpnLengthNeeded) && (uSEROBJECTFLAGS.dwFlags & 1) == 0)
                {
                    isUserNonInteractive = true;
                }
                processWinStation = processWindowStation;
            }
            return !isUserNonInteractive;
        }
    }

    /// <summary>Gets the network domain name associated with the current user.</summary>
    /// <returns>The network domain name associated with the current user.</returns>
    /// <exception cref="T:System.PlatformNotSupportedException">The operating system does not support retrieving the network domain name.</exception>
    /// <exception cref="T:System.InvalidOperationException">The network domain name cannot be retrieved.</exception>
    public static string UserDomainName
    {
        [SecuritySafeCritical]
        get
        {
            new EnvironmentPermission(EnvironmentPermissionAccess.Read, "UserDomain").Demand();
            byte[] array = new byte[1024];
            int sidLen = array.Length;
            StringBuilder stringBuilder = new StringBuilder(1024);
            uint domainNameLen = (uint)stringBuilder.Capacity;
            byte userNameEx = Win32Native.GetUserNameEx(2, stringBuilder, ref domainNameLen);
            if (userNameEx == 1)
            {
                string text = stringBuilder.ToString();
                int num = text.IndexOf('\\');
                if (num != -1)
                {
                    return text.Substring(0, num);
                }
            }
            domainNameLen = (uint)stringBuilder.Capacity;
            if (!Win32Native.LookupAccountName(null, UserName, array, ref sidLen, stringBuilder, ref domainNameLen, out var _))
            {
                int lastWin32Error = Marshal.GetLastWin32Error();
                throw new InvalidOperationException(Win32Native.GetMessage(lastWin32Error));
            }
            return stringBuilder.ToString();
        }
    }

    /// <summary>Gets a unique identifier for the current managed thread.</summary>
    /// <returns>An integer that represents a unique identifier for this managed thread.</returns>
    [__DynamicallyInvokable]
    public static int CurrentManagedThreadId
    {
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [__DynamicallyInvokable]
        get
        {
            return Thread.CurrentThread.ManagedThreadId;
        }
    }

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    internal static extern void _Exit(int exitCode);

    /// <summary>Terminates this process and returns an exit code to the operating system.</summary>
    /// <param name="exitCode">The exit code to return to the operating system. Use 0 (zero) to indicate that the process completed successfully.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have sufficient security permission to perform this function.</exception>
    [SecuritySafeCritical]
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static void Exit(int exitCode)
    {
        _Exit(exitCode);
    }

    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    [__DynamicallyInvokable]
    public static extern void FailFast(string message);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern void FailFast(string message, uint exitCode);

    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message and exception information in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    /// <param name="exception">An exception that represents the error that caused the termination. This is typically the exception in a <see langword="catch" /> block.</param>
    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    [__DynamicallyInvokable]
    public static extern void FailFast(string message, Exception exception);

#if !NET6_0_OR_GREATER
    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
    internal static extern void TriggerCodeContractFailure(ContractFailureKind failureKind, string message, string condition, string exceptionAsString);
#endif

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetIsCLRHosted();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    private static extern void GetCommandLine(StringHandleOnStack retString);

    ///// <summary>Replaces the name of each environment variable embedded in the specified string with the string equivalent of the value of the variable, then returns the resulting string.</summary>
    ///// <param name="name">A string containing the names of zero or more environment variables. Each environment variable is quoted with the percent sign character (%).</param>
    ///// <returns>A string with each environment variable replaced by its value.</returns>
    ///// <exception cref="T:System.ArgumentNullException">
    /////   <paramref name="name" /> is <see langword="null" />.</exception>
    //[SecuritySafeCritical]
    //[__DynamicallyInvokable]
    //public static string ExpandEnvironmentVariables(string name)
    //{
    //    if (name == null)
    //    {
    //        throw new ArgumentNullException("name");
    //    }
    //    if (name.Length == 0)
    //    {
    //        return name;
    //    }
    //    if (AppDomain.IsAppXModel() && !AppDomain.IsAppXDesignMode())
    //    {
    //        return name;
    //    }
    //    int num = 100;
    //    StringBuilder stringBuilder = new StringBuilder(num);
    //    bool flag = CodeAccessSecurityEngine.QuickCheckForAllDemands();
    //    string[] array = name.Split('%');
    //    StringBuilder stringBuilder2 = (flag ? null : new StringBuilder());
    //    bool flag2 = false;
    //    int num2;
    //    for (int i = 1; i < array.Length - 1; i++)
    //    {
    //        if (array[i].Length == 0 || flag2)
    //        {
    //            flag2 = false;
    //            continue;
    //        }
    //        stringBuilder.Length = 0;
    //        string text = "%" + array[i] + "%";
    //        num2 = Win32Native.ExpandEnvironmentStrings(text, stringBuilder, num);
    //        if (num2 == 0)
    //        {
    //            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
    //        }
    //        while (num2 > num)
    //        {
    //            num = (stringBuilder.Capacity = num2);
    //            stringBuilder.Length = 0;
    //            num2 = Win32Native.ExpandEnvironmentStrings(text, stringBuilder, num);
    //            if (num2 == 0)
    //            {
    //                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
    //            }
    //        }
    //        if (!flag)
    //        {
    //            string text2 = stringBuilder.ToString();
    //            flag2 = text2 != text;
    //            if (flag2)
    //            {
    //                stringBuilder2.Append(array[i]);
    //                stringBuilder2.Append(';');
    //            }
    //        }
    //    }
    //    if (!flag)
    //    {
    //        new EnvironmentPermission(EnvironmentPermissionAccess.Read, stringBuilder2.ToString()).Demand();
    //    }
    //    stringBuilder.Length = 0;
    //    num2 = Win32Native.ExpandEnvironmentStrings(name, stringBuilder, num);
    //    if (num2 == 0)
    //    {
    //        Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
    //    }
    //    while (num2 > num)
    //    {
    //        num = (stringBuilder.Capacity = num2);
    //        stringBuilder.Length = 0;
    //        num2 = Win32Native.ExpandEnvironmentStrings(name, stringBuilder, num);
    //        if (num2 == 0)
    //        {
    //            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
    //        }
    //    }
    //    return stringBuilder.ToString();
    //}


    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    private static extern int GetProcessorCount();

    /// <summary>Returns a string array containing the command-line arguments for the current process.</summary>
    /// <returns>An array of string where each element contains a command-line argument. The first element is the executable file name, and the following zero or more elements contain the remaining command-line arguments.</returns>
    /// <exception cref="T:System.NotSupportedException">The system does not support command-line arguments.</exception>
    [SecuritySafeCritical]
    public static string[] GetCommandLineArgs()
    {
        new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Demand();
        return GetCommandLineArgsNative();
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    private static extern string[] GetCommandLineArgsNative();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern string nativeGetEnvironmentVariable(string variable);

    /// <summary>Retrieves the value of an environment variable from the current process.</summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <returns>The value of the environment variable specified by <paramref name="variable" />, or <see langword="null" /> if the environment variable is not found.</returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    //[SecuritySafeCritical]
    //[__DynamicallyInvokable]
    //public static string GetEnvironmentVariable(string variable)
    //{
    //    if (variable == null)
    //    {
    //        throw new ArgumentNullException("variable");
    //    }
    //    if (AppDomain.IsAppXModel() && !AppDomain.IsAppXDesignMode())
    //    {
    //        return null;
    //    }
    //    new EnvironmentPermission(EnvironmentPermissionAccess.Read, variable).Demand();
    //    StringBuilder stringBuilder = StringBuilderCache.Acquire(128);
    //    int environmentVariable = Win32Native.GetEnvironmentVariable(variable, stringBuilder, stringBuilder.Capacity);
    //    if (environmentVariable == 0 && Marshal.GetLastWin32Error() == 203)
    //    {
    //        StringBuilderCache.Release(stringBuilder);
    //        return null;
    //    }
    //    while (environmentVariable > stringBuilder.Capacity)
    //    {
    //        stringBuilder.Capacity = environmentVariable;
    //        stringBuilder.Length = 0;
    //        environmentVariable = Win32Native.GetEnvironmentVariable(variable, stringBuilder, stringBuilder.Capacity);
    //    }
    //    return StringBuilderCache.GetStringAndRelease(stringBuilder);
    //}

    ///// <summary>Retrieves the value of an environment variable from the current process or from the Windows operating system registry key for the current user or local machine.</summary>
    ///// <param name="variable">The name of an environment variable.</param>
    ///// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values.</param>
    ///// <returns>The value of the environment variable specified by the <paramref name="variable" /> and <paramref name="target" /> parameters, or <see langword="null" /> if the environment variable is not found.</returns>
    ///// <exception cref="T:System.ArgumentNullException">
    /////   <paramref name="variable" /> is <see langword="null" />.</exception>
    ///// <exception cref="T:System.ArgumentException">
    /////   <paramref name="target" /> is not a valid <see cref="T:System.EnvironmentVariableTarget" /> value.</exception>
    ///// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    //[SecuritySafeCritical]
    //public static string GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    //{
    //    if (variable == null)
    //    {
    //        throw new ArgumentNullException("variable");
    //    }
    //    if (target == EnvironmentVariableTarget.Process)
    //    {
    //        return GetEnvironmentVariable(variable);
    //    }
    //    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //    switch (target)
    //    {
    //        case EnvironmentVariableTarget.Machine:
    //            {
    //                using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", writable: false);
    //                if (registryKey2 == null)
    //                {
    //                    return null;
    //                }
    //                return registryKey2.GetValue(variable) as string;
    //            }
    //        case EnvironmentVariableTarget.User:
    //            {
    //                using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Environment", writable: false);
    //                if (registryKey == null)
    //                {
    //                    return null;
    //                }
    //                return registryKey.GetValue(variable) as string;
    //            }
    //        default:
    //            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)target));
    //    }
    //}

    //[SecurityCritical]
    //private unsafe static char[] GetEnvironmentCharArray()
    //{
    //    char[] array = null;
    //    RuntimeHelpers.PrepareConstrainedRegions();
    //    try
    //    {
    //    }
    //    finally
    //    {
    //        char* ptr = null;
    //        try
    //        {
    //            ptr = Win32Native.GetEnvironmentStrings();
    //            if (ptr == null)
    //            {
    //                throw new OutOfMemoryException();
    //            }
    //            char* ptr2;
    //            for (ptr2 = ptr; *ptr2 != 0 || ptr2[1] != 0; ptr2++)
    //            {
    //            }
    //            int num = (int)(ptr2 - ptr + 1);
    //            array = new char[num];
    //            fixed (char* dmem = array)
    //            {
    //                string.wstrcpy(dmem, ptr, num);
    //            }
    //        }
    //        finally
    //        {
    //            if (ptr != null)
    //            {
    //                Win32Native.FreeEnvironmentStrings(ptr);
    //            }
    //        }
    //    }
    //    return array;
    //}

    /// <summary>Retrieves all environment variable names and their values from the current process.</summary>
    /// <returns>A dictionary that contains all environment variable names and their values; otherwise, an empty dictionary if no environment variables are found.</returns>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The buffer is out of memory.</exception>
    //[SecuritySafeCritical]
    //[__DynamicallyInvokable]
    //public static IDictionary GetEnvironmentVariables()
    //{
    //    if (AppDomain.IsAppXModel() && !AppDomain.IsAppXDesignMode())
    //    {
    //        return new Hashtable(0);
    //    }
    //    bool flag = CodeAccessSecurityEngine.QuickCheckForAllDemands();
    //    StringBuilder stringBuilder = (flag ? null : new StringBuilder());
    //    bool flag2 = true;
    //    char[] environmentCharArray = GetEnvironmentCharArray();
    //    Hashtable hashtable = new Hashtable(20);
    //    for (int i = 0; i < environmentCharArray.Length; i++)
    //    {
    //        int num = i;
    //        for (; environmentCharArray[i] != '=' && environmentCharArray[i] != 0; i++)
    //        {
    //        }
    //        if (environmentCharArray[i] == '\0')
    //        {
    //            continue;
    //        }
    //        if (i - num == 0)
    //        {
    //            for (; environmentCharArray[i] != 0; i++)
    //            {
    //            }
    //            continue;
    //        }
    //        string text = new string(environmentCharArray, num, i - num);
    //        i++;
    //        int num2 = i;
    //        for (; environmentCharArray[i] != 0; i++)
    //        {
    //        }
    //        string value = new string(environmentCharArray, num2, i - num2);
    //        hashtable[text] = value;
    //        if (!flag)
    //        {
    //            if (flag2)
    //            {
    //                flag2 = false;
    //            }
    //            else
    //            {
    //                stringBuilder.Append(';');
    //            }
    //            stringBuilder.Append(text);
    //        }
    //    }
    //    if (!flag)
    //    {
    //        new EnvironmentPermission(EnvironmentPermissionAccess.Read, stringBuilder.ToString()).Demand();
    //    }
    //    return hashtable;
    //}

    internal static IDictionary GetRegistryKeyNameValuePairs(RegistryKey registryKey)
    {
        Hashtable hashtable = new Hashtable(20);
        if (registryKey != null)
        {
            string[] valueNames = registryKey.GetValueNames();
            string[] array = valueNames;
            foreach (string text in array)
            {
                string value = registryKey.GetValue(text, "").ToString();
                hashtable.Add(text, value);
            }
        }
        return hashtable;
    }

    /// <summary>Retrieves all environment variable names and their values from the current process, or from the Windows operating system registry key for the current user or local machine.</summary>
    /// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values.</param>
    /// <returns>A dictionary that contains all environment variable names and their values from the source specified by the <paramref name="target" /> parameter; otherwise, an empty dictionary if no environment variables are found.</returns>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation for the specified value of <paramref name="target" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="target" /> contains an illegal value.</exception>
    //[SecuritySafeCritical]
    //public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
    //{
    //    if (target == EnvironmentVariableTarget.Process)
    //    {
    //        return GetEnvironmentVariables();
    //    }
    //    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //    switch (target)
    //    {
    //        case EnvironmentVariableTarget.Machine:
    //            {
    //                using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", writable: false);
    //                return GetRegistryKeyNameValuePairs(registryKey2);
    //            }
    //        case EnvironmentVariableTarget.User:
    //            {
    //                using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Environment", writable: false);
    //                return GetRegistryKeyNameValuePairs(registryKey);
    //            }
    //        default:
    //            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)target));
    //    }
    //}

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").  
    /// -or-  
    /// The length of <paramref name="variable" /> or <paramref name="value" /> is greater than or equal to 32,767 characters.  
    /// -or-  
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    //[SecuritySafeCritical]
    //[__DynamicallyInvokable]
    //public static void SetEnvironmentVariable(string variable, string value)
    //{
    //    CheckEnvironmentVariableName(variable);
    //    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //    if (string.IsNullOrEmpty(value) || value[0] == '\0')
    //    {
    //        value = null;
    //    }
    //    else if (value.Length >= 32767)
    //    {
    //        throw new ArgumentException(GetResourceString("Argument_LongEnvVarValue"));
    //    }
    //    if (AppDomain.IsAppXModel() && !AppDomain.IsAppXDesignMode())
    //    {
    //        throw new PlatformNotSupportedException();
    //    }
    //    if (!Win32Native.SetEnvironmentVariable(variable, value))
    //    {
    //        int lastWin32Error = Marshal.GetLastWin32Error();
    //        switch (lastWin32Error)
    //        {
    //            case 203:
    //                break;
    //            case 206:
    //                throw new ArgumentException(GetResourceString("Argument_LongEnvVarValue"));
    //            default:
    //                throw new ArgumentException(Win32Native.GetMessage(lastWin32Error));
    //        }
    //    }
    //}

    private static void CheckEnvironmentVariableName(string variable)
    {
        if (variable == null)
        {
            throw new ArgumentNullException("variable");
        }
        if (variable.Length == 0)
        {
            throw new ArgumentException(GetResourceString("Argument_StringZeroLength"), "variable");
        }
        if (variable[0] == '\0')
        {
            throw new ArgumentException(GetResourceString("Argument_StringFirstCharIsZero"), "variable");
        }
        if (variable.Length >= 32767)
        {
            throw new ArgumentException(GetResourceString("Argument_LongEnvVarValue"));
        }
        if (variable.IndexOf('=') != -1)
        {
            throw new ArgumentException(GetResourceString("Argument_IllegalEnvVarName"));
        }
    }

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process or in the Windows operating system registry key reserved for the current user or local machine.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <param name="target">One of the enumeration values that specifies the location of the environment variable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    ///   <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").  
    /// -or-  
    /// The length of <paramref name="variable" /> is greater than or equal to 32,767 characters.  
    /// -or-  
    /// <paramref name="target" /> is not a member of the <see cref="T:System.EnvironmentVariableTarget" /> enumeration.  
    /// -or-  
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Machine" /> or <see cref="F:System.EnvironmentVariableTarget.User" />, and the length of <paramref name="variable" /> is greater than or equal to 255.  
    /// -or-  
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Process" /> and the length of <paramref name="value" /> is greater than or equal to 32,767 characters.  
    /// -or-  
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    //[SecuritySafeCritical]
    //public static void SetEnvironmentVariable(string variable, string value, EnvironmentVariableTarget target)
    //{
    //    if (target == EnvironmentVariableTarget.Process)
    //    {
    //        SetEnvironmentVariable(variable, value);
    //        return;
    //    }
    //    CheckEnvironmentVariableName(variable);
    //    if (variable.Length >= 1024)
    //    {
    //        throw new ArgumentException(GetResourceString("Argument_LongEnvVarName"));
    //    }
    //    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //    if (string.IsNullOrEmpty(value) || value[0] == '\0')
    //    {
    //        value = null;
    //    }
    //    switch (target)
    //    {
    //        case EnvironmentVariableTarget.Machine:
    //            {
    //                using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("System\\CurrentControlSet\\Control\\Session Manager\\Environment", writable: true))
    //                {
    //                    if (registryKey2 != null)
    //                    {
    //                        if (value == null)
    //                        {
    //                            registryKey2.DeleteValue(variable, throwOnMissingValue: false);
    //                        }
    //                        else
    //                        {
    //                            registryKey2.SetValue(variable, value);
    //                        }
    //                    }
    //                }
    //                break;
    //            }
    //        case EnvironmentVariableTarget.User:
    //            {
    //                if (variable.Length >= 255)
    //                {
    //                    throw new ArgumentException(GetResourceString("Argument_LongEnvVarValue"));
    //                }
    //                using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Environment", writable: true))
    //                {
    //                    if (registryKey != null)
    //                    {
    //                        if (value == null)
    //                        {
    //                            registryKey.DeleteValue(variable, throwOnMissingValue: false);
    //                        }
    //                        else
    //                        {
    //                            registryKey.SetValue(variable, value);
    //                        }
    //                    }
    //                }
    //                break;
    //            }
    //        default:
    //            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)target));
    //    }
    //    IntPtr intPtr = Win32Native.SendMessageTimeout(new IntPtr(65535), 26, IntPtr.Zero, "Environment", 0u, 1000u, IntPtr.Zero);
    //    _ = intPtr == IntPtr.Zero;
    //}

    /// <summary>Returns an array of string containing the names of the logical drives on the current computer.</summary>
    /// <returns>An array of strings where each element contains the name of a logical drive. For example, if the computer's hard drive is the first logical drive, the first element returned is "C:\".</returns>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    //[SecuritySafeCritical]
    //public static string[] GetLogicalDrives()
    //{
    //    new EnvironmentPermission(PermissionState.Unrestricted).Demand();
    //    int logicalDrives = Win32Native.GetLogicalDrives();
    //    if (logicalDrives == 0)
    //    {
    //        __Error.WinIOError();
    //    }
    //    uint num = (uint)logicalDrives;
    //    int num2 = 0;
    //    while (num != 0)
    //    {
    //        if ((num & (true ? 1u : 0u)) != 0)
    //        {
    //            num2++;
    //        }
    //        num >>= 1;
    //    }
    //    string[] array = new string[num2];
    //    char[] array2 = new char[3] { 'A', ':', '\\' };
    //    num = (uint)logicalDrives;
    //    num2 = 0;
    //    while (num != 0)
    //    {
    //        if ((num & (true ? 1u : 0u)) != 0)
    //        {
    //            array[num2++] = new string(array2);
    //        }
    //        num >>= 1;
    //        array2[0] += '\u0001';
    //    }
    //    return array;
    //}

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    private static extern long GetWorkingSet();

    [DllImport("QCall", CharSet = CharSet.Unicode)]
    [SecurityCritical]
    [SuppressUnmanagedCodeSecurity]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WinRTSupported();

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern bool GetVersion(Win32Native.OSVERSIONINFO osVer);

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern bool GetVersionEx(Win32Native.OSVERSIONINFOEX osVer);

    //internal static string GetStackTrace(Exception e, bool needFileInfo)
    //{
    //    StackTrace stackTrace = ((e != null) ? new StackTrace(e, needFileInfo) : new StackTrace(needFileInfo));
    //    return stackTrace.ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
    //}

    [SecuritySafeCritical]
    private static void InitResourceHelper()
    {
        bool lockTaken = false;
        RuntimeHelpers.PrepareConstrainedRegions();
        try
        {
#if NET45_OR_GREATER
            Monitor.Enter(InternalSyncObject, ref lockTaken);
#else
            MonitorEx.Enter(InternalSyncObject, ref lockTaken);
#endif
            if (m_resHelper == null)
            {
                ResourceHelper resHelper = new ResourceHelper("mscorlib");
                Thread.MemoryBarrier();
                m_resHelper = resHelper;
            }
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(InternalSyncObject);
            }
        }
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern string GetResourceFromDefault(string key);

    internal static string GetResourceStringLocal(string key)
    {
        if (m_resHelper == null)
        {
            InitResourceHelper();
        }
        return m_resHelper.GetResourceString(key);
    }

    [SecuritySafeCritical]
    internal static string GetResourceString(string key)
    {
        return GetResourceFromDefault(key);
    }

    [SecuritySafeCritical]
    internal static string GetResourceString(string key, params object[] values)
    {
        string resourceString = GetResourceString(key);
        return string.Format(CultureInfo.CurrentCulture, resourceString, values);
    }

    internal static string GetRuntimeResourceString(string key)
    {
        return GetResourceString(key);
    }

    internal static string GetRuntimeResourceString(string key, params object[] values)
    {
        return GetResourceString(key, values);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    [SecurityCritical]
    internal static extern bool GetCompatibilityFlag(CompatibilityFlag flag);

    /// <summary>Gets the path to the system special folder that is identified by the specified enumeration.</summary>
    /// <param name="folder">One of enumeration values that identifies a system special folder.</param>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").  
    ///  A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    [SecuritySafeCritical]
    public static string GetFolderPath(SpecialFolder folder)
    {
        if (!Enum.IsDefined(typeof(SpecialFolder), folder))
        {
            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)folder));
        }
        return InternalGetFolderPath(folder, SpecialFolderOption.None);
    }

    /// <summary>Gets the path to the system special folder that is identified by the specified enumeration, and uses a specified option for accessing special folders.</summary>
    /// <param name="folder">One of the enumeration values that identifies a system special folder.</param>
    /// <param name="option">One of the enumeration values taht specifies options to use for accessing a special folder.</param>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").  
    ///  A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    /// <exception cref="T:System.ArgumentException">
    ///   <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.
    /// -or-
    /// <paramref name="options" /> is not a member of <see cref="T:System.Environment.SpecialFolderOption" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    [SecuritySafeCritical]
    public static string GetFolderPath(SpecialFolder folder, SpecialFolderOption option)
    {
        if (!Enum.IsDefined(typeof(SpecialFolder), folder))
        {
            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)folder));
        }
        if (!Enum.IsDefined(typeof(SpecialFolderOption), option))
        {
            throw new ArgumentException(GetResourceString("Arg_EnumIllegalVal", (int)option));
        }
        return InternalGetFolderPath(folder, option);
    }

    [SecurityCritical]
    internal static string UnsafeGetFolderPath(SpecialFolder folder)
    {
        return InternalGetFolderPath(folder, SpecialFolderOption.None, suppressSecurityChecks: true);
    }

    [SecurityCritical]
    private static string InternalGetFolderPath(SpecialFolder folder, SpecialFolderOption option, bool suppressSecurityChecks = false)
    {
        if (option == SpecialFolderOption.Create && !suppressSecurityChecks)
        {
            FileIOPermission fileIOPermission = new FileIOPermission(PermissionState.None);
            fileIOPermission.AllFiles = FileIOPermissionAccess.Write;
            fileIOPermission.Demand();
        }
        StringBuilder stringBuilder = new StringBuilder(260);
        int num = Win32Native.SHGetFolderPath(IntPtr.Zero, (int)folder | (int)option, IntPtr.Zero, 0, stringBuilder);
        string text;
        if (num < 0)
        {
            if (num == -2146233031)
            {
                throw new PlatformNotSupportedException();
            }
            text = string.Empty;
        }
        else
        {
            text = stringBuilder.ToString();
        }
        if (!suppressSecurityChecks)
        {
            new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
        }
        return text;
    }
}
