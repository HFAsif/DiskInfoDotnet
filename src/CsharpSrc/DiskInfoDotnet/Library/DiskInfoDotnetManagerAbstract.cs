using HelperClass;
using DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using Nj = Newtonsoft.Json;

namespace DiskInfoDotnet.Library
{
    internal abstract class DiskInfoDotnetManagerAbstract
    {
        //string[] args { get; set; }

        public required string[] args { get; set; }

        public ObservableCollection<Dis.ATA_SMART_INFO>? aTA_SMART_INFOs { get; protected set; }
        public required ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List;
        public abstract bool PrimarySetup();
        public abstract bool FinalWorker(out object _aTA_SMART_INFO);

        //protected Dis.SMART_ATTRIBUTE[] SMART_ATTRIBUTE_LIST = new Dis.SMART_ATTRIBUTE[MAX_ATTRIBUTE];

        //public required nint hMutexJMicron { get; set; }
        //public required bool m_bAtaPassThrough;
        //public required bool m_bAtaPassThroughSmart;
        //public required bool m_bNVMeStorageQuery;



        //protected bool FlagUsbMemory = false;
        //protected bool IsAdvancedDiskSearch = false;
        //protected bool IsWorkaroundHD204UI = false;

        //protected bool FlagNvidiaController = false;
        //protected bool FlagMarvellController = false;

        //[DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        //[MethodImpl(MethodImplOptions.ForwardRef, MethodCodeType = MethodCodeType.Runtime)]
        //protected static extern void RtlZeroMemory_CopyMemory(ref Dis.ATA_IDENTIFY_DEVICE dest, ref byte src, uint count);

        //SMART_ATTRIBUTE

        //public virtual bool LoadManagementScops()
        //{
        //    return true;
        //}

        public virtual bool Initialize()
        {
            aTA_SMART_INFOs = new ObservableCollection<Dis.ATA_SMART_INFO>();
            return true;
        }

        
    }
}
