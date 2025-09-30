
namespace DiskInfoDotnet.Library;

using System;
using System.Runtime.CompilerServices;
using static Inm;
using static Dis;
using DiskInfoDotnet.Sm.Management;

internal partial class DiskInfosWorker : IDiskInfosWorker
{
    public required Win32_DiskDrive_Infos win32_DiskDrive_Infos { get; set; }

    public bool PrimarySetup()
    {
        diskSize = win32_DiskDrive_Infos.Size;

        deviceId = win32_DiskDrive_Infos.DeviceID;

        deviceId = deviceId?.Replace("\\", "\\\\");

        int.TryParse(deviceId?.Substring(deviceId.Length - 2), out var result);

        if (result >= 10)
        {
            int.TryParse(deviceId?.Substring(deviceId.Length - 2), out physicalDriveId);
        }
        else
        {
            int.TryParse(deviceId?.Substring(deviceId.Length - 1), out physicalDriveId);
        }

        model = win32_DiskDrive_Infos.Model;

        if (model is not null and not "")
        {
            firmware = win32_DiskDrive_Infos.FirmwareRevision;

            scsiPort = win32_DiskDrive_Infos.SCSIPort;
            scsiTargetId = win32_DiskDrive_Infos.SCSITargetId;
            scsiBus = win32_DiskDrive_Infos.SCSIBus;

            mediaType = win32_DiskDrive_Infos.MediaType;

            if (mediaType is not null and not "")
            {
                mediaType = mediaType.ToLower();

                // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=994;id=diskinfo#994
                if (model.Contains("SanDisk Extreme"))
                {
                    flagTarget = true;
                    detectUSBMemory = true;
                }
                // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1198;id=diskinfo#1198
                else if (model.Contains("Kingston DT Ultimate"))
                {
                    flagTarget = true;
                    detectUSBMemory = true;
                }
                else if (FlagUsbMemory)
                {
                    flagTarget = true;
                    detectUSBMemory = true;
                }
                else if (mediaType.Contains("removable") || string.IsNullOrEmpty(mediaType))
                {
                    flagTarget = false;
                }
                else
                {
                    flagTarget = true;
                }
            }
        }

        interfaceTypeWmi = win32_DiskDrive_Infos.InterfaceType;

        pnpDeviceId = win32_DiskDrive_Infos.PNPDeviceID;
        if (pnpDeviceId is not null and not "")
            pnpDeviceId = pnpDeviceId.ToUpper();

        return true;
    }

    public void FinalStep(out Dis.ATA_SMART_INFO asi)
    {
        Unsafe.SkipInit(out asi);

        if (!flagBlackList)
        {
            //string cstr;
            //cstr.Format(("DO:GetDiskInfo pd=%d, sp=%d, st=%d, mt=%s"), physicalDriveId, scsiPort, scsiTargetId, mediaType.GetString());

            //StringBuilder cstr = new StringBuilder();
            //cstr.AppendFormat("DO:GetDiskInfo pd={0}, sp={1}, st={2}, mt={3}", physicalDriveId, scsiPort, scsiTargetId, mediaType.ToString());
            //var ssse = cstr.ToString();
            string cstr = string.Format("DO:GetDiskInfo pd={0}, sp={1}, st={2}, mt={3}", physicalDriveId, scsiPort, scsiTargetId, mediaType?.ToString());

            INTERFACE_TYPE interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_UNKNOWN;
            COMMAND_TYPE commandType = COMMAND_TYPE.CMD_TYPE_UNKNOWN;
            Unsafe.SkipInit(out commandType);
            VENDOR_ID usbVendorId = VENDOR_ID.VENDOR_UNKNOWN;
            Unsafe.SkipInit(out usbVendorId);
            uint usbProductId = 0;
            Unsafe.SkipInit(out usbProductId);

            if (interfaceTypeWmi is not null && model is not null && pnpDeviceId is not null && firmware is not null)
            {
                if (interfaceTypeWmi.Contains("1394") || model.Contains(" IEEE 1394 SBP2 Device"))
                {
                    //Logs.MyLogs(("INTERFACE_TYPE_IEEE1394"));
                    interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_IEEE1394;
                }
                else if (interfaceTypeWmi.Contains("USB") || model.Contains(" USB Device") || flagUasp)
                {
                    //Logs.MyLogs(("INTERFACE_TYPE_USB"));
                    interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_USB;

                    if (model.Contains("NVMe") || pnpDeviceId.Contains("NVME"))
                    {
                        flagNVMe = true;
                    }
                }
                else if (model.Contains("NVMe") || pnpDeviceId.Contains("NVME") || model.Contains("Optane") || pnpDeviceId.Contains("OPTANE"))
                {
                    //Logs.MyLogs(("INTERFACE_TYPE_NVME"));
                    interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_NVME;
                    flagNVMe = true;
                }
                else
                {
                    flagTarget = true;
                }


                cstr = string.Format("InterfaceTypeId={0}", interfaceType);


                //for (int i = 0; i < externals.GetCount(); i++)
                //{
                //    if (model.IndexOf(externals.GetAt(i).Enclosure) == 0)
                //    {
                //        usbVendorId = (VENDOR_ID)externals[i].UsbVendorId;
                //        usbProductId = externals[i].UsbProductId;
                //        cstr.Format(("usbVendorId=%04X, usbProductId=%04X"), usbVendorId, usbProductId);
                //        Logs.MyLogs(cstr);
                //    }
                //}


                if (IsAdvancedDiskSearch && string.IsNullOrEmpty(mediaType))
                {
                    flagTarget = true;
                }

                // [2010/12/05] Workaround for SAMSUNG HD204UI
                // http://sourceforge.net/apps/trac/smartmontools/wiki/SamsungF4EGBadBlocks
                if ((model.Contains("SAMSUNG HD155UI") || model.Contains("SAMSUNG HD204UI")) && firmware.Contains("1AQ10003") && IsWorkaroundHD204UI)
                {
                    flagTarget = false;
                }

                // [2018/10/24] Workaround for FuzeDrive (AMDStoreMi)
                if (model.Contains("FuzeDrive") || model.Contains("StoreMI"))
                {
                    flagTarget = false;
                }

                //var getsizes = Marshal.SizeOf(typeof(BIN_IDENTIFY_DEVICE));
                //int previousCount = (int)vars.GetCount();

                try
                {

                    if (flagTarget && GetDiskInfo(ref asi, physicalDriveId, scsiPort, scsiTargetId, interfaceType, commandType, usbVendorId, usbProductId, scsiBus, siliconImageType, FlagNvidiaController, FlagMarvellController, pnpDeviceId, flagNVMe, flagUasp))
                    {
                        //Console.WriteLine($"{asi.Model} health {Environment.NewLine} {asi.Life} ");
                    }
                }
                catch (Exception ex)
                {
                    //Debugger.Break();
                    Logs.MyLogs(ex.Message);
                }
            }

        }
    }

}