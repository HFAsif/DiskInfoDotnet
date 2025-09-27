
namespace DiskInfoDotnet.Library;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using static Dis;
using static Inm;

internal partial class DiskInfosWorker : DiskInfosWorkerBase
{

    public bool GetDiskInfo(ref ATA_SMART_INFO asi, int physicalDriveId, int scsiPort, int scsiTargetId, INTERFACE_TYPE interfaceType, COMMAND_TYPE commandType,
        VENDOR_ID usbVendorId, uint productId, int scsiBus, uint siliconImageType, bool FlagNvidiaController, bool FlagMarvellController, string pnpDeviceId,
        bool flagNVMe, bool flagUsap)
    {
        IDENTIFY_DEVICE identify;
        Unsafe.SkipInit(out identify);
        CSMI_SAS_PHY_ENTITY cSMI_SAS_PHY_ENTITY;
        Unsafe.SkipInit(out cSMI_SAS_PHY_ENTITY);

        uint diskSize = 0;
        string debug = string.Empty;

        if (interfaceType == INTERFACE_TYPE.INTERFACE_TYPE_UNKNOWN || interfaceType == INTERFACE_TYPE.INTERFACE_TYPE_PATA || interfaceType == INTERFACE_TYPE.INTERFACE_TYPE_SATA)
        {
            if (siliconImageType != 0)
            {

            }
            if (physicalDriveId >= 0)
            {
                debug = string.Format(("DoIdentifyDevicePd(%d, 0xA0) - 1"), physicalDriveId);
                Logs.MyLogs(debug);
                if (!DoIdentifyDevicePd(physicalDriveId, 0xA0, ref identify))
                {
                    debug = string.Format(("WakeUp({0})"), physicalDriveId);
                    Logs.MyLogs(debug);
                    //WakeUp(physicalDriveId);

                    //debug.Format(("DoIdentifyDevicePd(%d, 0xA0) - 2"), physicalDriveId);
                    //Logs.MyLogs(debug);
                    //if (!DoIdentifyDevicePd(physicalDriveId, 0xA0, &identify))
                    //{
                    //    debug.Format(("DoIdentifyDevicePd(%d, 0xB0) - 3"), physicalDriveId);
                    //    Logs.MyLogs(debug);

                    //    if (!DoIdentifyDevicePd(physicalDriveId, 0xB0, &identify))
                    //    {
                    //        debug.Format(("DoIdentifyDeviceScsi(%d, %d) - 4"), scsiPort, scsiTargetId);
                    //        Logs.MyLogs(debug);

                    //        if ((FlagNvidiaController || FlagMarvellController || IsAdvancedDiskSearch) && scsiPort >= 0 && scsiTargetId >= 0 && DoIdentifyDeviceScsi(scsiPort, scsiTargetId, &identify))
                    //        {
                    //            debug.Format(("AddDisk(%d, %d, %d) - 5"), physicalDriveId, scsiPort, scsiTargetId);
                    //            Logs.MyLogs(debug);
                    //            return AddDisk(physicalDriveId, scsiPort, scsiTargetId, scsiBus, 0xA0, CMD_TYPE_SCSI_MINIPORT, &identify, siliconImageType, NULL, pnpDeviceId);
                    //        }
                    //        else
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //}
                }
                //debug.Format(("AddDisk(%d, %d, %d) - 6"), physicalDriveId, scsiPort, scsiTargetId);
                //Logs.MyLogs(debug);
                return AddDisk(ref asi, physicalDriveId, scsiPort, scsiTargetId, scsiBus, 0xA0, COMMAND_TYPE.CMD_TYPE_PHYSICAL_DRIVE, ref identify, (int)siliconImageType, ref cSMI_SAS_PHY_ENTITY, pnpDeviceId);
            }
        }
        else if (interfaceType == INTERFACE_TYPE.INTERFACE_TYPE_NVME)
        {
            //debug.Format(("DoIdentifyDeviceNVMeStorageQuery"));
            //Logs.MyLogs(debug);
            if (m_bNVMeStorageQuery && DoIdentifyDeviceNVMeStorageQuery(physicalDriveId, scsiPort, scsiTargetId, ref identify, ref diskSize))
            {
                ////debug.Format(("AddDiskNVMe - CMD_TYPE_NVME_STORAGE_QUERY"));
                ////Logs.MyLogs(debug);
                if (AddDiskNVMe(ref asi, physicalDriveId, scsiPort, scsiTargetId, scsiBus, (byte)scsiTargetId, COMMAND_TYPE.CMD_TYPE_NVME_STORAGE_QUERY, ref identify)) { return true; }
            }

            ////debug.Format(("DoIdentifyDeviceNVMeIntelVroc"));
            ////Logs.MyLogs(debug);
            //if (DoIdentifyDeviceNVMeIntelVroc(physicalDriveId, scsiPort, scsiTargetId, ref _identifyPtr, ref diskSize))
            //{
            //    //debug.Format(("AddDiskNVMe - CMD_TYPE_NVME_INTEL_VROC"));
            //    //Logs.MyLogs(debug);
            //    //if (AddDiskNVMe(physicalDriveId, scsiPort, scsiTargetId, scsiBus, (BYTE)scsiTargetId, CMD_TYPE_NVME_INTEL_VROC, &identify, &diskSize)) { return true; }
            //}

            ////debug.Format(("DoIdentifyDeviceNVMeIntelRst"));
            ////Logs.MyLogs(debug);
            //if (DoIdentifyDeviceNVMeIntelRst(physicalDriveId, scsiPort, scsiTargetId, ref _identifyPtr, ref diskSize))
            //{
            //    //debug.Format(("AddDiskNVMe - CMD_TYPE_NVME_INTEL_RST"));
            //    //Logs.MyLogs(debug);
            //    //if (AddDiskNVMe(physicalDriveId, scsiPort, scsiTargetId, scsiBus, (BYTE)scsiTargetId, CMD_TYPE_NVME_INTEL_RST, &identify, &diskSize)) { return true; }
            //}

            ////debug.Format(("DoIdentifyDeviceNVMeSamsung"));
            ////Logs.MyLogs(debug);
            //if (DoIdentifyDeviceNVMeSamsung(physicalDriveId, scsiPort, scsiTargetId, ref _identifyPtr))
            //{
            //    //_identify = Marshal.PtrToStructure<IDENTIFY_DEVICE>(_identifyPtr);

            //    //debug.Format(("AddDiskNVMe - CMD_TYPE_NVME_SAMSUNG"));
            //    //Logs.MyLogs(debug);

            //    //if (AddDiskNVMe(physicalDriveId, scsiPort, scsiTargetId, scsiBus, (byte)scsiTargetId, COMMAND_TYPE.CMD_TYPE_NVME_SAMSUNG, ref _identifyPtr)) { return true; }
            //}

            //////debug.Format(("DoIdentifyDeviceNVMeIntel"));
            //////Logs.MyLogs(debug);

            ////if (DoIdentifyDeviceNVMeIntel(physicalDriveId, scsiPort, scsiTargetId, &identify, &diskSize))
            ////{
            ////    //debug.Format(("AddDiskNVMe - CMD_TYPE_NVME_INTEL"));
            ////    //Logs.MyLogs(debug);
            ////    //if (AddDiskNVMe(physicalDriveId, scsiPort, scsiTargetId, scsiBus, (BYTE)scsiTargetId, CMD_TYPE_NVME_INTEL, &identify)) { return true; }
            ////}
        }


        return true;
    }

}