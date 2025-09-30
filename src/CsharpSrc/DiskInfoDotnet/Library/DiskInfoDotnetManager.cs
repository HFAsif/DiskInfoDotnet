
namespace DiskInfoDotnet.Library
{
    using HelperClass;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [DiskInfoDotnetManager]
    internal class DiskInfoDotnetManager : DiskInfoDotnetManagerAbstract, IDiskInfoDotnetManager
    {
        public override bool PrimarySetup() => base.Initialize();
        //public override bool LoadManagementScops() => base.LoadManagementScops();

        public required WindowsVersionCheckerAttribute windowsVersionChecker;

        public override bool FinalWorker(out object _aTA_SMART_INFO)
        {
            Unsafe.SkipInit(out _aTA_SMART_INFO);
            ObservableCollection<Dis.ATA_SMART_INFO> aTA_SMART_INFOs = [];

            if (win32_DiskDrive_Infos_List is not null)
            {
                foreach (var wdInfos in win32_DiskDrive_Infos_List)
                {
                    IDiskInfosWorker iDiskInfosWorkerZero = new DiskInfosWorker()
                    { 
                        win32_DiskDrive_Infos = wdInfos ,
                        m_bAtaPassThrough = windowsVersionChecker.m_bAtaPassThrough,
                        m_bAtaPassThroughSmart = windowsVersionChecker.m_bAtaPassThroughSmart,
                        m_bNVMeStorageQuery = windowsVersionChecker.m_bNVMeStorageQuery,
                        hMutexJMicron = windowsVersionChecker.hMutexJMicron
                    };

                    if (iDiskInfosWorkerZero.PrimarySetup())
                    {
                        iDiskInfosWorkerZero.FinalStep(out var objList);
                        aTA_SMART_INFOs.Add(objList);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException($"Getting exception at {nameof(win32_DiskDrive_Infos_List)}");
            }

            if (aTA_SMART_INFOs.Count > 0)
            {
                _aTA_SMART_INFO = aTA_SMART_INFOs;
            }
            else
            {
                Debugger.Break();
            }

            return true;
        }

        


    }
}
