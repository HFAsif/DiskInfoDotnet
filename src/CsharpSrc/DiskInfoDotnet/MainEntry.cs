namespace DiskInfoDotnet;

using DiskInfoDotnet.Library;
using DiskInfoDotnet.Sm.Management;
using HelperClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

[WindowsVersionChecker]
public class MainEntry
{
    static WindowsVersionCheckerAttribute RunWinvAttr([Optional] bool Fake_m_bNVMeStorageQuery)
    {
        if ((Attribute.GetCustomAttribute(typeof(MainEntry), typeof(WindowsVersionCheckerAttribute)) is WindowsVersionCheckerAttribute winvAttr && winvAttr is not null))
        {
            if (winvAttr.WindowsVesionChecker())
            {
                if (Fake_m_bNVMeStorageQuery)
                {
                    winvAttr.m_bNVMeStorageQuery = true;
                }

                return winvAttr;
            }
            else goto errCode;
        }

    errCode:
        throw new GettingExceptions("invalid operation");
    }

    public static void Run(out object ataLists, out ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List,
        [Optional] bool Fake_m_bNVMeStorageQuery, [Optional] bool getDriverInfos, [Optional] bool nativeTest, [Optional] params string[] args)
    {
        var loadMScopModule = LoadMScopModule.Create();
        loadMScopModule.LoadInfos(getDriverInfos);
        win32_DiskDrive_Infos_List = loadMScopModule.win32_DiskDrive_Infos_List;
        Run(nativeTest, RunWinvAttr(Fake_m_bNVMeStorageQuery), out ataLists, win32_DiskDrive_Infos_List);
    }

    static void ExtractionHelper(bool nativeTest, out object ataLists, ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List, params string[] args)
    {
        Run(nativeTest, RunWinvAttr(), out ataLists, win32_DiskDrive_Infos_List, args);
    }

    public static void Run(out object ataLists, ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List)
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        ExtractionHelper(false, out ataLists, win32_DiskDrive_Infos_List, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

    }

    [STAThread]
    public static void Run(bool nativeTest, WindowsVersionCheckerAttribute windowsVersionCheckerAttr, out object ataLists, ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List, params string[] args)
    {
        //ataLists = default;
        Unsafe.SkipInit(out ataLists);
        bool createdNew = false;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string name = Assembly.GetExecutingAssembly().GetName().Name;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        using Mutex mutex = new Mutex(initiallyOwned: true, name, out createdNew);
        if (createdNew)
        {
            Dis.ATA_SMART_INFO aTA_SMART_INFO;
            Unsafe.SkipInit(out aTA_SMART_INFO);

            DiskInfoDotnetManagerAbstract diskInfoArtificialManager;

            if (nativeTest)
            {
                //diskInfoArtificialManager = new DiskInfoArtificial_Internal_Native_Test()
                //{ args = args, m_bAtaPassThrough = m_bAtaPassThrough, m_bAtaPassThroughSmart = m_bAtaPassThroughSmart, m_bNVMeStorageQuery = m_bNVMeStorageQuery, hMutexJMicron = hMutexJMicron };
                //diskInfoArtificialManager.StartUp();
            }
            else
            {
                diskInfoArtificialManager = new DiskInfoDotnetManager()
                {
                    args = args,
                    windowsVersionChecker = windowsVersionCheckerAttr,
                    win32_DiskDrive_Infos_List = win32_DiskDrive_Infos_List
                };

                var _getType = diskInfoArtificialManager.GetType();
                var diskInfoArtificialManagerattr = _getType.GetCustomAttribute<DiskInfoDotnetManagerAttribute>();

                if (diskInfoArtificialManagerattr is not null && Attribute.IsDefined(_getType, diskInfoArtificialManagerattr.GetType()) && diskInfoArtificialManagerattr.BTAPassThroughSmart)
                {
                    if (diskInfoArtificialManager.PrimarySetup())
                    {
                        diskInfoArtificialManager.FinalWorker(out ataLists);
                    }
                    else Debugger.Break();
                }
            }
            mutex.ReleaseMutex();
        }
        else
        {
            Logs.MyLogs("An application instance is already running");
        }


    }
}
