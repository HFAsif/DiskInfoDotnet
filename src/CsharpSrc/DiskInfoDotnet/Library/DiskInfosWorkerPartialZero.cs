#nullable disable
namespace DiskInfoDotnet.Library;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Inm;
using static Impp;
using static Dis;
using static Hlp;
using static Ale;
using static Stm;
using System.Diagnostics;

internal partial class DiskInfosWorker
{
    bool SendAtaCommandPd(int physicalDriveId, byte target, byte main, byte sub, byte param, ref IDENTIFY_DEVICE data, uint dataSize)
    {
        bool bRet = false;
        IntPtr hIoCtrl = IntPtr.Zero;
        uint dwReturned = 0;

        hIoCtrl = GetIoCtrlHandle(physicalDriveId);
        if (hIoCtrl == IntPtr.Zero || hIoCtrl == INVALID_HANDLE_VALUE)
        {
            return false;
        }

        if (m_bAtaPassThrough)
        {
            ATA_PASS_THROUGH_EX_WITH_BUFFERS ab;
            Unsafe.SkipInit(out ab);

            //ZeroMemory(ref ab, Marshal.SizeOf(typeof(ATA_PASS_THROUGH_EX_WITH_BUFFERS)));

            StructToZeroStruct(ref ab);

            //.ZeroMemory(&ab, sizeof(ab));
            ab.Apt.Length = (ushort)Marshal.SizeOf(typeof(ATA_PASS_THROUGH_EX));
            ab.Apt.TimeOutValue = 2;
            uint size = (uint)Marshal.OffsetOf(typeof(ATA_PASS_THROUGH_EX_WITH_BUFFERS), "Buf");
            ab.Apt.DataBufferOffset = size;

            if (dataSize > 0)
            {
                if (dataSize > ab.Buf.Length)
                {
                    return false;
                }
                ab.Apt.AtaFlags = ATA_FLAGS_DATA_IN;
                ab.Apt.DataTransferLength = dataSize;
                ab.Buf[0] = 0xCF; // magic number
                size += dataSize;
            }

            ab.Apt.CurrentTaskFile.bFeaturesReg = sub;
            ab.Apt.CurrentTaskFile.bSectorCountReg = param;
            ab.Apt.CurrentTaskFile.bDriveHeadReg = target;
            ab.Apt.CurrentTaskFile.bCommandReg = main;

            if (main == SMART_CMD)
            {
                ab.Apt.CurrentTaskFile.bCylLowReg = SMART_CYL_LOW;
                ab.Apt.CurrentTaskFile.bCylHighReg = SMART_CYL_HI;
                ab.Apt.CurrentTaskFile.bSectorCountReg = 1;
                ab.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            }

            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.IOCTL_ATA_PASS_THROUGH,
                ref ab, size, ref ab, size, ref dwReturned, IntPtr.Zero);

            Dip.safeCloseHandle(hIoCtrl);


            if (bRet && dataSize != 0 && data.B.Bin == null)
            {
                MemCpyStructToStruct(ref ab.Buf, ref data.A, ref data.B, ref data.N);
            }
        }


        return bRet;
    }

    bool DoIdentifyDeviceNVMeStorageQuery(int physicalDriveId, int scsiPort, int scsiTargetId, ref IDENTIFY_DEVICE data, ref uint diskSize)
    {
        string path;
        path = string.Format("\\\\.\\PhysicalDrive{0}", physicalDriveId);

        IntPtr hIoCtrl = Dip.CreateFile(path, (uint)(GENERIC_READ | GENERIC_WRITE),
            FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

        TStorageQueryWithBuffer nptwb;
        Unsafe.SkipInit(out nptwb);
        bool bRet = false;

        //ZeroMemory(ref nptwb, Marshal.SizeOf(nptwb));
        StructToZeroStruct(ref nptwb);

        nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
        nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = 0;
        nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 1;
        nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
        uint dwReturned = 0;

        bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)(IOCTL_STORAGE_QUERY_PROPERTY),
        ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);

        if (bRet)
        {
            ulong totalLBA = BitConverter.ToUInt64(nptwb.Buffer, 0);
            int sectorSize = 1 << nptwb.Buffer[130];
            diskSize = (uint)((totalLBA * (uint)sectorSize) / 1000 / 1000);
        }

        StructToZeroStruct(ref nptwb);
        //ZeroMemory(ref nptwb, Marshal.SizeOf(nptwb));

        //ZeroMemory(&nptwb, sizeof(nptwb));
        nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
        nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
        nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = 1; /*NVME_IDENTIFY_CNS_CONTROLLER*/
        nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0;
        dwReturned = 0;

        bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)(IOCTL_STORAGE_QUERY_PROPERTY),
            ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);

        Dip.safeCloseHandle(hIoCtrl);

        ////MemCpyStructToStruct(ref nptwb.Buffer, ref data.A, ref data.B, ref data.N);

        //ATA_IDENTIFY_DEVICE _aTA_IDENTIFY_DEVICE;
        //Unsafe.SkipInit(out _aTA_IDENTIFY_DEVICE);

        //BIN_IDENTIFY_DEVICE _bIN_IDENTIFY_DEVICE;
        //Unsafe.SkipInit(out _bIN_IDENTIFY_DEVICE);

        //NVME_IDENTIFY_DEVICE _nVME_IDENTIFY_DEVICE;
        //Unsafe.SkipInit(out _nVME_IDENTIFY_DEVICE);

        ////IDENTIFY_DEVICE _iDENTIFY_DEVICE;
        ////Unsafe.SkipInit(out _iDENTIFY_DEVICE);

        //CopyMemory(ref _aTA_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));
        //CopyMemory(ref _bIN_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(BIN_IDENTIFY_DEVICE)));
        //CopyMemory(ref _nVME_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

        //data.A = _aTA_IDENTIFY_DEVICE;
        //data.B = _bIN_IDENTIFY_DEVICE;
        //data.N = _nVME_IDENTIFY_DEVICE;

        MemCpyTripleStruct(ref nptwb.Buffer, ref data.A, ref data.B, ref data.N);

        return bRet;
    }


    bool GetSmartAttributeNVMeStorageQuery(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        string path;
        path = string.Format("\\\\.\\PhysicalDrive{0}", physicalDriveId);

        var hIoCtrl = Dip.CreateFile(path, (uint)(GENERIC_READ | GENERIC_WRITE),
            FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        bool bRet = false;

        TStorageQueryWithBuffer nptwb;
        Unsafe.SkipInit(out nptwb);


        nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeLogPage;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = 2; // SMART Health Information
        nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0x00000000;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
        nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
        nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
        uint dwReturned = 0;

        bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IOCTL_STORAGE_QUERY_PROPERTY,
            ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);
        if (!bRet)
        {
            nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0xFFFFFFFF;
            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IOCTL_STORAGE_QUERY_PROPERTY,
                ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);
        }

        Dip.safeCloseHandle(hIoCtrl);
        asi.SmartReadData = new byte[512];

        //CopyMemory(ref asi.SmartReadData[0], ref nptwb.Buffer[0], 512);

        Buffer.BlockCopy(nptwb.Buffer, 0, asi.SmartReadData, 0, 512);

        //MemCpyByteArrayToArray(ref asi.SmartReadData, ref nptwb.Buffer, 512);

        //memcpy_s(&(asi.SmartReadData), 512, nptwb.Buffer, 512);

        byte[] NvmeIdentifyControllerData = new byte[4096];

        if (GetNvMeIdentifyControllerData(physicalDriveId, NvmeIdentifyControllerData))
        {
            asi.IsNvmeThresholdSupported = IsNVMeTemperatureThresholdDefined(NvmeIdentifyControllerData);
            asi.IsNvmeThermalManagementSupported = IsNVMeThermalManagementTemperatureDefined(NvmeIdentifyControllerData);
        }

        return bRet;
    }


    bool AddDiskNVMe(ref ATA_SMART_INFO asi, int physicalDriveId, int scsiPort, int scsiTargetId, int scsiBus, byte target, COMMAND_TYPE commandType, ref IDENTIFY_DEVICE identify
        , ref uint? diskSize, string pnpDeviceId
        , ref NVME_PORT_20? nvmePort20, ref NVME_PORT_40? nvmePort40, ref NVME_ID? nvmeId)
    {

        //var chs = Marshal.SizeOf(typeof(SMART_ATTRIBUTE));

        //ATA_SMART_INFO asi;
        //Unsafe.SkipInit(out asi);

        //asi.IdentifyDevice = identify;

        //CopyMemory(ref asi.IdentifyDevice, ref identify, (uint)Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

        MemCpyStructToStruct(ref asi.IdentifyDevice, ref identify);

        asi.PhysicalDriveId = physicalDriveId;
        asi.ScsiBus = scsiBus;
        asi.ScsiPort = scsiPort;
        asi.ScsiTargetId = scsiTargetId;
        asi.CommandType = commandType;
        asi.SsdVendorString = "";
        asi.CommandTypeString = commandTypeString[(int)commandType];

        asi.IsSmartEnabled = true;
        asi.IsIdInfoIncorrect = false;
        asi.IsSmartCorrect = true;
        asi.IsThresholdCorrect = true;
        asi.IsWord88 = false;
        asi.IsWord64_76 = false;
        asi.IsCheckSumError = false;
        asi.IsRawValues8 = false;
        asi.IsRawValues7 = false;
        asi.Is9126MB = false;
        asi.IsThresholdBug = false;

        asi.IsSmartSupported = true;
        asi.IsLba48Supported = false;
        asi.IsNcqSupported = false;
        asi.IsAamSupported = false;
        asi.IsApmSupported = false;
        asi.IsAamEnabled = false;
        asi.IsApmEnabled = false;
        asi.IsNvCacheSupported = false;
        asi.IsNvmeThresholdSupported = false;
        asi.IsNvmeThermalManagementSupported = false;
        asi.IsDeviceSleepSupported = false;
        asi.IsStreamingSupported = false;
        asi.IsGplSupported = false;

        asi.IsMaxtorMinute = false;
        asi.IsSsd = true;
        asi.IsTrimSupported = false;
        asi.IsVolatileWriteCachePresent = false;

        asi.TotalDiskSize = 0;
        asi.Cylinder = 0;
        asi.Head = 0;
        asi.Sector = 0;
        asi.Sector28 = 0;
        asi.Sector48 = 0;
        asi.NumberOfSectors = 0;
        asi.DiskSizeChs = 0;
        asi.DiskSizeLba28 = 0;
        asi.DiskSizeLba48 = 0;
        asi.LogicalSectorSize = 512;
        asi.PhysicalSectorSize = 512;
        asi.DiskSizeWmi = 0;
        asi.BufferSize = 0;
        asi.NvCacheSize = 0;
        asi.TransferModeType = 0;
        asi.DetectedTimeUnitType = 0;
        asi.MeasuredTimeUnitType = 0;
        asi.AttributeCount = 0;
        asi.DetectedPowerOnHours = -1;
        asi.MeasuredPowerOnHours = -1;
        asi.PowerOnRawValue = -1;
        asi.PowerOnStartRawValue = -1;
        asi.PowerOnCount = 0;
        asi.Temperature = -1000;
        asi.TemperatureMultiplier = 1.0;
        asi.NominalMediaRotationRate = 1;
        //	asi.Speed = 0.0;
        asi.Life = -1;
        asi.HostWrites = -1;
        asi.HostReads = -1;
        asi.GBytesErased = -1;
        asi.NandWrites = -1;
        asi.WearLevelingCount = -1;
        //	asi.PlextorNandWritesUnit = 0;

        asi.Major = 0;
        asi.Minor = 0;

        asi.DiskStatus = 0;
        asi.DriveLetterMap = 0;

        asi.AlarmTemperature = 0;
        asi.IsNVMe = true;

        if (commandType == COMMAND_TYPE.CMD_TYPE_NVME_JMICRON || commandType == COMMAND_TYPE.CMD_TYPE_NVME_ASMEDIA || commandType == COMMAND_TYPE.CMD_TYPE_NVME_REALTEK || commandType == COMMAND_TYPE.CMD_TYPE_NVME_REALTEK9220DP)
        {
            asi.InterfaceType = INTERFACE_TYPE.INTERFACE_TYPE_USB;
        }
        else
        {
            asi.InterfaceType = INTERFACE_TYPE.INTERFACE_TYPE_NVME;
        }

        asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;

        asi.DiskVendorId = (uint)VENDOR_ID.VENDOR_UNKNOWN;
        asi.UsbVendorId = (uint)VENDOR_ID.VENDOR_UNKNOWN;
        asi.UsbProductId = 0;
        asi.Target = target;

        asi.SerialNumber = ("");
        asi.FirmwareRev = ("");
        asi.Model = ("");
        asi.ModelReverse = ("");
        asi.ModelWmi = ("");
        asi.ModelSerial = ("");
        asi.DriveMap = ("");
        asi.MaxTransferMode = ("");
        asi.CurrentTransferMode = ("");
        asi.MajorVersion = ("");
        asi.MinorVersion = ("");
        asi.Interface = ("");
        asi.Enclosure = ("");
        asi.DeviceNominalFormFactor = ("");
        asi.PnpDeviceId = pnpDeviceId;
        asi.MinorVersion = ("");

        if (nvmePort20.Equals(default(NVME_PORT_20)))
        {
            asi.Model = nvmePort20?.ModelName;
            asi.SerialNumber = nvmePort20?.SerialNumber;
            asi.Model.TrimRight(ref asi.Model);
            asi.SerialNumber.TrimRight(ref asi.SerialNumber);
            asi.TotalDiskSize = (uint)((((UInt64)nvmePort20?.Capacity << 32) + (UInt64)nvmePort20?.CapacityOffset) * (UInt64)nvmePort20?.SectorSize / 1000 / 1000);
        }
        else if (nvmePort40.Equals(default(NVME_PORT_40)) && nvmeId.Equals(default(NVME_ID)))
        {
            asi.Model = nvmePort40?.ModelName;
            asi.FirmwareRev = nvmeId?.FirmwareRevision;
            asi.SerialNumber = nvmePort40?.SerialNumber;
            asi.Model.TrimRight(ref asi.Model);
            asi.SerialNumber.TrimRight(ref asi.SerialNumber);
            asi.TotalDiskSize = (uint)((((UInt64)nvmePort40?.Capacity << 32) + (UInt64)nvmePort40?.CapacityOffset) * (UInt64)nvmePort40?.SectorSize / 1000 / 1000);
        }
        else
        {
            asi.Model = asi.IdentifyDevice.N.Model;
            asi.Model = asi.Model.Mid(0, 40);
            asi.Model.TrimRight(ref asi.Model);

            if (asi.Model.IsEmpty())
            {
                return false;
            }

            asi.SerialNumber = asi.IdentifyDevice.N.SerialNumber;
            asi.SerialNumber = asi.SerialNumber.Mid(0, 20);
            asi.SerialNumber.TrimRight(ref asi.SerialNumber);

            asi.FirmwareRev = asi.IdentifyDevice.N.FirmwareRev;
            asi.FirmwareRev = asi.FirmwareRev.Mid(0, 8);
            asi.FirmwareRev.TrimRight(ref asi.FirmwareRev);
        }

        asi.ModelSerial = GetModelSerial(ref asi.Model, ref asi.SerialNumber);

        if (diskSize != null)
        {
            asi.TotalDiskSize = (uint)diskSize;
        }


        if ((asi.IdentifyDevice.B.Bin[520] & 0x4) != 0) // for Dataset Management Command support
        {
            asi.IsTrimSupported = true;
        }

        if ((asi.IdentifyDevice.B.Bin[525] & 0x1) != 0) // for Volatile Write Cache
        {
            asi.IsVolatileWriteCachePresent = true;
        }

        //// Check duplicate device
        //for (int i = 0; i < vars.GetCount(); i++)
        //{
        //    if (((commandType == CMD_TYPE_JMS586_20 || commandType == CMD_TYPE_JMS586_40) && asi.SerialNumber.Compare(vars[i].SerialNumber) == 0)
        //    || (asi.Model.Compare(vars[i].Model) == 0 && asi.SerialNumber.Compare(vars[i].SerialNumber) == 0)
        //    )
        //    {
        //        return false;
        //    }
        //}

        if (asi.Model.IsEmpty())
        {
            return false;
        }

        if (
#if !(_M_ARM) && !(_M_ARM64)
    (commandType == COMMAND_TYPE.CMD_TYPE_AMD_RC2 && GetSmartDataAMD_RC2(scsiBus, ref asi)) ||// +AMD_RC2
#endif
    (m_bNVMeStorageQuery && commandType == COMMAND_TYPE.CMD_TYPE_NVME_STORAGE_QUERY && GetSmartAttributeNVMeStorageQuery(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_INTEL && GetSmartAttributeNVMeIntel(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_INTEL_RST && GetSmartAttributeNVMeIntelRst(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_INTEL_VROC && GetSmartAttributeNVMeIntelVroc(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_SAMSUNG && GetSmartAttributeNVMeSamsung(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_SAMSUNG && GetSmartAttributeNVMeSamsung951(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_JMICRON && GetSmartAttributeNVMeJMicron(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_ASMEDIA && GetSmartAttributeNVMeASMedia(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_REALTEK && GetSmartAttributeNVMeRealtek(physicalDriveId, scsiPort, scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_NVME_REALTEK9220DP && GetSmartAttributeNVMeRealtek9220DP(physicalDriveId, scsiPort, scsiTargetId, ref asi))
#if JMICRON_USB_RAID_SUPPORT
|| (commandType == COMMAND_TYPE.CMD_TYPE_JMS586_40 && GetSmartAttributeNVMeJMS586_40((byte)scsiPort, (byte)scsiTargetId, ref asi))
|| (commandType == COMMAND_TYPE.CMD_TYPE_JMS586_20 && GetSmartAttributeNVMeJMS586_20(scsiPort, scsiTargetId, ref asi))
#endif

    )
        {
            asi.IsSmartSupported = true;
            asi.Temperature = asi.SmartReadData[0x2] * 256 + asi.SmartReadData[0x1] - 273;
            if (asi.Temperature == -273 || asi.Temperature > 100)
            {
                asi.Temperature = -1000;
            }

            asi.Life = 100 - asi.SmartReadData[0x05];
            if (asi.Life < 0)
            {
                asi.Life = 0;
            }



            var _HostRead_bit = BitConverter.ToUInt64(asi.SmartReadData, 0x20);
            var _HostWrites_bit = BitConverter.ToUInt64(asi.SmartReadData, 0x30);
            var _PowerOnCount_bit = BitConverter.ToUInt64(asi.SmartReadData, 0x70);
            var _MeasuredPowerOnHours_bit = BitConverter.ToUInt64(asi.SmartReadData, 0x80);

            asi.HostReads = (int)((_HostRead_bit * 1000) >> 21);// * 512 * 1000 / 1024 / 1024 / 1024
            asi.HostWrites = (int)((_HostWrites_bit * 1000) >> 21);// * 512 * 1000 / 1024 / 1024 / 1024
            asi.PowerOnCount = (uint)_PowerOnCount_bit;
            asi.MeasuredPowerOnHours = asi.DetectedPowerOnHours = (int)_MeasuredPowerOnHours_bit;


            //asi.HostReads = (int)((*((UInt64*)&asi.SmartReadData[0x20]) * 1000) >> 21);// * 512 * 1000 / 1024 / 1024 / 1024
            //asi.HostWrites = (int)((*((UInt64*)&asi.SmartReadData[0x30]) * 1000) >> 21);// * 512 * 1000 / 1024 / 1024 / 1024
            //asi.PowerOnCount = (uint) * ((UInt64*)&asi.SmartReadData[0x70]);
            //asi.MeasuredPowerOnHours = asi.DetectedPowerOnHours = (int) * ((UInt64*)&asi.SmartReadData[0x80]);


            //NVMeSmartToATASmart(asi.SmartReadData, ref asi.Attribute);
        }



        return true;
    }

    bool AddDisk(ref ATA_SMART_INFO asi, int physicalDriveId, int scsiPort, int scsiTargetId, int scsiBus, byte target, COMMAND_TYPE commandType, ref IDENTIFY_DEVICE identify, int siliconImageType,
        ref CSMI_SAS_PHY_ENTITY sasPhyEntity, string pnpDeviceId = default,
        uint TotalDiskSize = default // +AMD_RC2
        )
    {
        //if (vars.GetCount() >= MAX_DISK)
        //{
        //    return false;
        //}
        ATA_SMART_INFO asiCheck;
        Unsafe.SkipInit(out asiCheck);


        if (!asi.IdentifyDevice.Equals(default(IDENTIFY_DEVICE)))
        {
            StructToZeroStruct(ref asi);
        }

        MemCpyStructToStruct(ref asi.IdentifyDevice, ref identify);

        asi.PhysicalDriveId = physicalDriveId;
        asi.ScsiBus = scsiBus;
        asi.ScsiPort = scsiPort;
        asi.ScsiTargetId = scsiTargetId;
        asi.SiliconImageType = siliconImageType;
        asi.CommandType = commandType;
        asiCheck.CommandType = commandType;
        asi.SsdVendorString = ("");

        if (!sasPhyEntity.Equals(default(CSMI_SAS_PHY_ENTITY)))
        {
            //not tested
            Debugger.Break();
            MemCpyStructToStruct(ref asi.sasPhyEntity, ref sasPhyEntity);
            //memcpy(&(asi.sasPhyEntity), sasPhyEntity, sizeof(CSMI_SAS_PHY_ENTITY));
        }

        if (commandType == COMMAND_TYPE.CMD_TYPE_PHYSICAL_DRIVE || COMMAND_TYPE.CMD_TYPE_SAT <= commandType && commandType <= COMMAND_TYPE.CMD_TYPE_SAT_REALTEK9220DP)
        {
            if (target == 0xB0)
            {
                asi.CommandTypeString = string.Format("{0}2", commandTypeString[(int)commandType]);
                //asi.CommandTypeString.Format(("%s2"), commandTypeString[(int)commandType]);
            }
            else
            {
                asi.CommandTypeString = string.Format("{0}1", commandTypeString[(int)commandType]);
                //asi.CommandTypeString.Format(("%s1"), commandTypeString[commandType]);
            }
        }
        else
        {
            if (commandType >= COMMAND_TYPE.CMD_TYPE_UNKNOWN && commandType <= COMMAND_TYPE.CMD_TYPE_DEBUG)
            {
                asi.CommandTypeString = commandTypeString[(uint)commandType];
            }
            else
            {
                asi.CommandTypeString = "";//unknown
            }
        }

        asi.Attribute = new SMART_ATTRIBUTE[MAX_ATTRIBUTE];
        asi.Threshold = new SMART_THRESHOLD[MAX_ATTRIBUTE];
        for (int i = 0; i < MAX_ATTRIBUTE; i++)
        {
            StructToZeroStruct(ref asi.Attribute[i]);
            StructToZeroStruct(ref asi.Threshold[i]);
            //.ZeroMemory(&(asi.Attribute[i]), sizeof(SMART_ATTRIBUTE));
            //.ZeroMemory(&(asi.Threshold[i]), sizeof(SMART_THRESHOLD));
        }

        asi.SmartReadData = new byte[512];
        asi.SmartReadThreshold = new byte[512];
        for (int i = 0; i < 512; i++)
        {
            asi.SmartReadData[i] = 0x00;
            asi.SmartReadThreshold[i] = 0x00;
        }

        asi.IsSmartEnabled = false;
        asi.IsIdInfoIncorrect = false;
        asi.IsSmartCorrect = false;
        asi.IsThresholdCorrect = false;
        asi.IsWord88 = false;
        asi.IsWord64_76 = false;
        asi.IsCheckSumError = false;
        asi.IsRawValues8 = false;
        asi.IsRawValues7 = false;
        asi.Is9126MB = false;
        asi.IsThresholdBug = false;

        asi.IsSmartSupported = false;
        asi.IsLba48Supported = false;
        asi.IsNcqSupported = false;
        asi.IsAamSupported = false;
        asi.IsApmSupported = false;
        asi.IsAamEnabled = false;
        asi.IsApmEnabled = false;
        asi.IsNvCacheSupported = false;
        asi.IsDeviceSleepSupported = false;
        asi.IsStreamingSupported = false;
        asi.IsGplSupported = false;

        asi.IsMaxtorMinute = false;
        asi.IsSsd = false;
        asi.IsTrimSupported = false;
        asi.IsVolatileWriteCachePresent = false;

        asi.IsNVMe = false;
        asi.IsUasp = false;

        asi.TotalDiskSize = 0;
        asi.Cylinder = 0;
        asi.Head = 0;
        asi.Sector = 0;
        asi.Sector28 = 0;
        asi.Sector48 = 0;
        asi.NumberOfSectors = 0;
        asi.DiskSizeChs = 0;
        asi.DiskSizeLba28 = 0;
        asi.DiskSizeLba48 = 0;
        asi.LogicalSectorSize = 512;
        asi.PhysicalSectorSize = 512;
        asi.DiskSizeWmi = 0;
        asi.BufferSize = 0;
        asi.NvCacheSize = 0;
        asi.TransferModeType = 0;
        asi.DetectedTimeUnitType = 0;
        asi.MeasuredTimeUnitType = 0;
        asi.AttributeCount = 0;
        asi.DetectedPowerOnHours = -1;
        asi.MeasuredPowerOnHours = -1;
        asi.PowerOnRawValue = -1;
        asi.PowerOnStartRawValue = -1;
        asi.PowerOnCount = 0;
        asi.Temperature = -1000;
        asi.TemperatureMultiplier = 1.0;
        asi.NominalMediaRotationRate = 0;
        //	asi.Speed = 0.0;
        asi.Life = -1;
        asi.FlagLifeNoReport = false;
        asi.FlagLifeRawValue = false;
        asi.FlagLifeRawValueIncrement = false;
        asi.FlagLifeSanDiskUsbMemory = false;
        asi.FlagLifeSanDisk0_1 = false;
        asi.FlagLifeSanDisk1 = false;
        asi.FlagLifeSanDiskCloud = false;
        asi.FlagLifeSanDiskLenovo = false;
        asi.HostWrites = -1;
        asi.HostReads = -1;
        asi.GBytesErased = -1;
        asi.NandWrites = -1;
        asi.WearLevelingCount = -1;
        //	asi.PlextorNandWritesUnit = 0;

        asi.Major = 0;
        asi.Minor = 0;

        asi.DiskStatus = 0;
        asi.DriveLetterMap = 0;

        asi.AlarmTemperature = 0;

        asi.InterfaceType = INTERFACE_TYPE.INTERFACE_TYPE_UNKNOWN;
        asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_UNKNOWN;

        asi.DiskVendorId = (uint)VENDOR_ID.VENDOR_UNKNOWN;
        asi.UsbVendorId = (uint)VENDOR_ID.VENDOR_UNKNOWN;
        asi.UsbProductId = 0;
        asi.Target = target;

        asi.SerialNumber = ("");
        asi.FirmwareRev = ("");
        asi.Model = ("");
        asi.ModelReverse = ("");
        asi.ModelWmi = ("");
        asi.ModelSerial = ("");
        asi.DriveMap = ("");
        asi.MaxTransferMode = ("");
        asi.CurrentTransferMode = ("");
        asi.MajorVersion = ("");
        asi.MinorVersion = ("");
        asi.Interface = ("");
        asi.Enclosure = ("");
        asi.DeviceNominalFormFactor = ("");
        asi.PnpDeviceId = pnpDeviceId;
        asi.MinorVersion = ("");

        //CHAR buf[64] = { };
        byte[] buf = new byte[64];

        // Check Sum Error
        byte sum = 0;
        byte[] checkSum = new byte[IDENTIFY_BUFFER_SIZE];

        //CopyMemory(ref checkSum[0], ref identify, IDENTIFY_BUFFER_SIZE);

        MemCpyByteArrayToStruct(ref identify, ref checkSum, IDENTIFY_BUFFER_SIZE);

        //memcpy(checkSum, (void*)identify, IDENTIFY_BUFFER_SIZE);

        for (int j = 0; j < IDENTIFY_BUFFER_SIZE; j++)
        {
            sum += checkSum[j];
        }
        if (sum != 0)
        {
            asi.IsCheckSumError = true;
        }

        if (CheckAsciiStringError(ref identify.A.SerialNumber, (uint)identify.A.SerialNumber.Length)

        || CheckAsciiStringError(ref identify.A.FirmwareRev, (uint)identify.A.FirmwareRev.Length)

       || CheckAsciiStringError(ref identify.A.Model, (uint)identify.A.Model.Length))
        {
            asi.IsIdInfoIncorrect = true;
            //	Logs.MyLogs(("CheckAsciiStringError"));
            return false;
        }

        Dip.strncpy_s(buf, 64, identify.A.SerialNumber, identify.A.SerialNumber.Length);


        asi.SerialNumberReverse = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.SerialNumberReverse.TrimLeft(ref asi.SerialNumberReverse);
        asi.SerialNumberReverse.TrimRight(ref asi.SerialNumberReverse);


        Dip.strncpy_s(buf, 64, identify.A.FirmwareRev, identify.A.FirmwareRev.Length);
        asi.FirmwareRevReverse = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.FirmwareRevReverse.TrimLeft(ref asi.FirmwareRevReverse);
        asi.FirmwareRevReverse.TrimRight(ref asi.FirmwareRevReverse);
        Dip.strncpy_s(buf, 64, identify.A.Model, identify.A.Model.Length);
        asi.ModelReverse = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.ModelReverse.TrimLeft(ref asi.ModelReverse);
        asi.ModelReverse.TrimRight(ref asi.ModelReverse);


        ChangeByteOrder(ref identify.A.SerialNumber, (uint)identify.A.SerialNumber.Length);
        ChangeByteOrder(ref identify.A.FirmwareRev, (uint)identify.A.FirmwareRev.Length);
        ChangeByteOrder(ref identify.A.Model, (uint)identify.A.Model.Length);

        // Normal
        Dip.strncpy_s(buf, 64, identify.A.SerialNumber, identify.A.SerialNumber.Length);
        asi.SerialNumber = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.SerialNumber.TrimLeft(ref asi.SerialNumber);
        asi.SerialNumber.TrimRight(ref asi.SerialNumber);
        Dip.strncpy_s(buf, 64, identify.A.FirmwareRev, identify.A.FirmwareRev.Length);
        asi.FirmwareRev = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.FirmwareRev.TrimLeft(ref asi.FirmwareRev);
        asi.FirmwareRev.TrimRight(ref asi.FirmwareRev);
        Dip.strncpy_s(buf, 64, identify.A.Model, identify.A.Model.Length);
        asi.Model = System.Text.Encoding.ASCII.GetString(buf).TrimEnd('\0');
        asi.Model.TrimLeft(ref asi.Model);
        asi.Model.TrimRight(ref asi.Model);

        if (asi.Model.IsEmpty() || asi.FirmwareRev.IsEmpty())
        {
            //Logs.MyLogs(("asi.Model.IsEmpty() || asi.FirmwareRev.IsEmpty()"));
            asi.IsIdInfoIncorrect = true;
            return false;
        }

        int duplicatedId = -1;
        Unsafe.SkipInit(out duplicatedId);
        // Check duplicate device

        //for (int i = 0; i < vars.GetCount(); i++)
        //{
        //    if (asi.Model.Compare(vars[i].Model) == 0 && asi.SerialNumber.Compare(vars[i].SerialNumber) == 0)
        //    {
        //        // for CSMI devices
        //        if (vars[i].PhysicalDriveId == -1)
        //        {
        //            vars[i].PhysicalDriveId = asi.PhysicalDriveId;
        //            if (CsmiType == CSMI_TYPE_ENABLE_AUTO)
        //            {
        //                duplicatedId = i;
        //                Logs.MyLogs(("vars[i].CommandType = CMD_TYPE_CSMI_PHYSICAL_DRIVE"));
        //                vars[i].CommandType = CMD_TYPE_CSMI_PHYSICAL_DRIVE;
        //                vars[i].CommandTypeString = commandTypeString[vars[i].CommandType];
        //                break;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        else if (vars[i].ModelWmi.IsEmpty())
        //        {
        //            duplicatedId = i;
        //        }
        //        else if (asi.ModelWmi.IsEmpty())
        //        {
        //            return false;
        //        }
        //    }

        //    if (asi.ModelReverse.Compare(vars[i].Model) == 0 && asi.SerialNumberReverse.Compare(vars[i].SerialNumber) == 0)
        //    {
        //        string cstr;
        //        cstr.Format("Duplicate Check: %s:%s", asi.Model.GetString(), asi.SerialNumber.GetString());
        //        Logs.MyLogs(cstr);
        //        return false;
        //    }
        //}

        string firmwareRevInt = asi.FirmwareRev;
        firmwareRevInt.Replace(("."), (""));

        //if (asi.Model.IndexOf(("ADATA SSD")) == 0 && _wtoi(firmwareRevInt) == 346)
        //{
        //    asi.TemperatureMultiplier = 0.5;
        //}

        if (asi.Model.IndexOf("ADATA SSD") == 0 && int.Parse(firmwareRevInt) == 346)
        {
            asi.TemperatureMultiplier = 0.5;
        }

        asi.ModelSerial = GetModelSerial(ref asi.Model, ref asi.SerialNumber);

        // +AMD_RC2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (commandType == COMMAND_TYPE.CMD_TYPE_AMD_RC2)
        {
            asi.Major = 3;
            asi.IsSsd = (identify.A.SerialAtaCapabilities & 1) != 0;
            asi.IsSmartSupported = true;
            asi.Interface = (identify.A.CurrentMediaSerialNo[0] == 'N' ? ("AMD_RC2") : ("AMD_RC2 (Serial ATA)"));
            asi.CurrentTransferMode = System.Text.Encoding.ASCII.GetString(identify.A.CurrentMediaSerialNo);//tmp
            asi.CurrentTransferMode.Replace("HDD", "");
            asi.CurrentTransferMode.Replace("SSD", "");
            asi.CurrentTransferMode.Replace("SATA", "");
            asi.CurrentTransferMode.Replace("6Gb", "SATA/600");
            asi.CurrentTransferMode.Replace("3Gb", "SATA/300");
            asi.CurrentTransferMode.Replace("1.5Gb", "SATA/150");
            asi.CurrentTransferMode.Replace("1Gb", "SATA/150");
            asi.CurrentTransferMode.Replace(" ", "");
            asi.MaxTransferMode = "----";

            // asi.MajorVersion = identify.A.CurrentMediaSerialNo;//tmp
            identify.A.CurrentMediaSerialNo[0] = (byte)'\0';
            identify.A.SerialAtaCapabilities = 0;
        }

#if JMICRON_USB_RAID_SUPPORT
        else if (commandType == COMMAND_TYPE.CMD_TYPE_JMS56X)
        {
            asi.Major = 0;
            asi.IsSmartSupported = true;
            asi.Interface = "USB (JMicron JMS56X)";
            asi.CurrentTransferMode = "---";
            asi.MaxTransferMode = "----";

            if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0002) != 0) { asi.MaxTransferMode = "SATA/150"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0004) != 0) { asi.MaxTransferMode = "SATA/300"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0008) != 0) { asi.MaxTransferMode = "SATA/600"; }
        }
        else if (commandType == COMMAND_TYPE.CMD_TYPE_JMB39X)
        {
            asi.Major = 0;
            asi.IsSmartSupported = true;
            asi.Interface = "USB (JMicron JMB39X)";
            asi.CurrentTransferMode = "---";
            asi.MaxTransferMode = "----";

            if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0002) != 0) { asi.MaxTransferMode = "SATA/150"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0004) != 0) { asi.MaxTransferMode = "SATA/300"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0008) != 0) { asi.MaxTransferMode = "SATA/600"; }
        }
        else if (commandType == COMMAND_TYPE.CMD_TYPE_JMS586_40)
        {
            asi.Major = 0;
            asi.IsSmartSupported = true;
            asi.Interface = "USB (JMicron JMS586 NewFW)";
            asi.CurrentTransferMode = "---";
            asi.MaxTransferMode = "----";

            if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0002) != 0) { asi.MaxTransferMode = "SATA/150"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0004) != 0) { asi.MaxTransferMode = "SATA/300"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0008) != 0) { asi.MaxTransferMode = "SATA/600"; }
        }
        else if (commandType == COMMAND_TYPE.CMD_TYPE_JMS586_20)
        {
            asi.Major = 0;
            asi.IsSmartSupported = true;
            asi.Interface = "USB (JMicron JMS586)";
            asi.CurrentTransferMode = "---";
            asi.MaxTransferMode = "----";

            if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0002) != 0) { asi.MaxTransferMode = "SATA/150"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0004) != 0) { asi.MaxTransferMode = "SATA/300"; }
            else if ((asi.IdentifyDevice.A.SerialAtaCapabilities & 0x0008) != 0) { asi.MaxTransferMode = "SATA/600"; }
        }
#endif
        else
        {
            // +AMD_RC2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

            asi.Major = GetAtaMajorVersion(identify.A.MajorVersion, ref asi.MajorVersion);
            GetAtaMinorVersion(identify.A.MinorVersion, ref asi.MinorVersion);

            asi.TransferModeType = GetTransferMode(identify.A.MultiWordDma, identify.A.SerialAtaCapabilities,
                                identify.A.SerialAtaAdditionalCapabilities,
                                identify.A.UltraDmaMode, ref asi.CurrentTransferMode, ref asi.MaxTransferMode,
                                ref asi.Interface, ref asi.InterfaceType);
            // +AMD_RC2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        }
        // +AMD_RC2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        asi.DetectedTimeUnitType = GetTimeUnitType(asi.Model, asi.FirmwareRev, asi.Major, asi.TransferModeType);

        if (asi.DetectedTimeUnitType == (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS)
        {
            asi.MeasuredTimeUnitType = (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS;
        }
        else if (asi.DetectedTimeUnitType == (uint)POWER_ON_HOURS_UNIT.POWER_ON_10_MINUTES)
        {
            asi.MeasuredTimeUnitType = (uint)POWER_ON_HOURS_UNIT.POWER_ON_10_MINUTES;
        }

        // Feature
        if (asi.Major >= 3 && (asi.IdentifyDevice.A.CommandSetSupported1 & (1 << 0)) != 0)
        {
            asi.IsSmartSupported = true;
        }
        if (asi.Major >= 3 && (asi.IdentifyDevice.A.CommandSetSupported2 & (1 << 3)) != 3)
        {
            asi.IsApmSupported = true;
            if ((asi.IdentifyDevice.A.CommandSetEnabled2 & (1 << 3)) != 3)
            {
                asi.IsApmEnabled = true;
            }
        }
        if (asi.Major >= 5 && (asi.IdentifyDevice.A.CommandSetSupported2 & (1 << 9)) != 9)
        {
            asi.IsAamSupported = true;
            if ((asi.IdentifyDevice.A.CommandSetEnabled2 & (1 << 9)) != 9)
            {
                asi.IsAamEnabled = true;
            }
        }

        if (asi.Major >= 5 && (asi.IdentifyDevice.A.CommandSetSupported2 & (1 << 10)) != 10)
        {
            asi.IsLba48Supported = true;
        }

        if (asi.Major >= 6 && (asi.IdentifyDevice.A.SerialAtaCapabilities & (1 << 8)) != 8)
        {
            asi.IsNcqSupported = true;
        }

        if (asi.Major >= 7 && (asi.IdentifyDevice.A.NvCacheCapabilities & (1 << 0)) != 0)
        {
            asi.IsNvCacheSupported = true;
        }

        if (asi.Major >= 7 && (asi.IdentifyDevice.A.DataSetManagement & (1 << 0)) != 0)
        {
            asi.IsTrimSupported = true;
        }

        // 8.17.4 ATA Streaming feature set
        if (asi.Major >= 7 && (asi.IdentifyDevice.A.CommandSetSupported3 & (1 << 4)) != 4)
        {
            asi.IsStreamingSupported = true;
        }
        // 8.17.4 GPL feature set
        if (asi.Major >= 7 && (asi.IdentifyDevice.A.CommandSetSupported3 & (1 << 5)) != 5)
        {
            asi.IsGplSupported = true;
        }

        // http://ascii.jp/elem/000/000/203/203345/img.html
        // "NominalMediaRotationRate" is supported by ATA8-ACS but a part of ATA/ATAPI-7 devices support this field.
        if (asi.Major >= 7 && asi.IdentifyDevice.A.NominalMediaRotationRate == 0x01)
        {
            asi.IsSsd = true;
            asi.NominalMediaRotationRate = 1;
        }

        if (asi.Major >= 7 && (asi.IdentifyDevice.A.NominalMediaRotationRate >= 0x401
        && asi.IdentifyDevice.A.NominalMediaRotationRate < 0xFFFF))
        {
            asi.NominalMediaRotationRate = asi.IdentifyDevice.A.NominalMediaRotationRate;
        }

        if (asi.Major >= 7
        && (asi.IdentifyDevice.A.DeviceNominalFormFactor & 0xF) > 0
        && (asi.IdentifyDevice.A.DeviceNominalFormFactor & 0xF) <= 5
        )
        {
            asi.DeviceNominalFormFactor = string.Format(("{0}"),
                deviceFormFactorString[asi.IdentifyDevice.A.DeviceNominalFormFactor & 0xF]);
            //	AfxMessageBox(asi.DeviceNominalFormFactor);
        }

        if (asi.Major >= 7 && (asi.IdentifyDevice.A.SerialAtaFeaturesSupported & (1 << 8)) != 8)
        {
            asi.IsDeviceSleepSupported = true;
        }

        string model = asi.Model;
        model = model.ToUpper();
        if (model.IndexOf("MAXTOR") == 0 && asi.DetectedTimeUnitType == (uint)POWER_ON_HOURS_UNIT.POWER_ON_MINUTES)
        {
            asi.IsMaxtorMinute = true;
        }

        // DiskSize & BufferSize
        if (identify.A.LogicalCylinders > 16383)
        {
            identify.A.LogicalCylinders = 16383;
            asi.IsIdInfoIncorrect = true;
        }
        if (identify.A.LogicalHeads > 16)
        {
            identify.A.LogicalHeads = 16;
            asi.IsIdInfoIncorrect = true;
        }
        if (identify.A.LogicalSectors > 63)
        {
            identify.A.LogicalSectors = 63;
            asi.IsIdInfoIncorrect = true;
        }


        asi.Cylinder = identify.A.LogicalCylinders;
        asi.Head = identify.A.LogicalHeads;
        asi.Sector = identify.A.LogicalSectors;
        asi.Sector28 = 0x0FFFFFFF & identify.A.TotalAddressableSectors;
        asi.Sector48 = 0x0000FFFFFFFFFFFF & identify.A.MaxUserLba;

        if ((identify.A.SectorSize & 0xC000) == 0x4000) // bit 14-15, bit14=1, bit15=0
        {
            if ((identify.A.SectorSize & 0x000F) == 0x3) // bit 0-3
            {
                asi.LogicalSectorSize = 512;
                asi.PhysicalSectorSize = 4096;
            }
            else if ((identify.A.SectorSize & 0x1000) == 0x1000) // bit 12=1
            {
                if (identify.A.WordsPerLogicalSector == 256 || identify.A.WordsPerLogicalSector == 0)
                {
                    asi.LogicalSectorSize = 512;
                }
                else
                {
                    asi.LogicalSectorSize = identify.A.WordsPerLogicalSector * 2;
                }
            }
        }

        if (asi.PhysicalSectorSize < asi.LogicalSectorSize)
        {
            asi.PhysicalSectorSize = asi.LogicalSectorSize;
        }

        if (identify.A.TotalAddressableSectors == 0x01100003) // 9126807040 bytes
        {
            asi.Is9126MB = true;
        }

        if (commandType == COMMAND_TYPE.CMD_TYPE_AMD_RC2) // +AMD_RC2
        {
            asi.IsLba48Supported = true;
            asi.DiskSizeChs = 0;
        }
        else if (commandType == COMMAND_TYPE.CMD_TYPE_JMS56X || commandType == COMMAND_TYPE.CMD_TYPE_JMB39X || commandType == COMMAND_TYPE.CMD_TYPE_JMS586_20 || commandType == COMMAND_TYPE.CMD_TYPE_JMS586_40)
        {
            asi.IsLba48Supported = true;
            asi.DiskSizeChs = 0;
        }
        else if (identify.A.LogicalCylinders == 0 || identify.A.LogicalHeads == 0 || identify.A.LogicalSectors == 0)
        {
            //	return false;
            //	asi.DiskSizeChs   = 0;
            // Realteck RTL9210 support (2024/01/19)
            if (identify.A.Capabilities1 == 0 && identify.A.Capabilities2 == 0 && commandType == COMMAND_TYPE.CMD_TYPE_SAT)
            {
                return false;
            }
            else
            {
                asi.DiskSizeChs = 0;
            }
        }
        else if (((ulong)((ulong)identify.A.LogicalCylinders * identify.A.LogicalHeads * identify.A.LogicalSectors * 512) / 1000 / 1000) > 1000)
        {
            asi.DiskSizeChs = (uint)(((ulong)identify.A.LogicalCylinders * identify.A.LogicalHeads * identify.A.LogicalSectors * 512) / 1000 / 1000 - 49);
        }
        else
        {
            asi.DiskSizeChs = (uint)(((ulong)identify.A.LogicalCylinders * identify.A.LogicalHeads * identify.A.LogicalSectors * 512) / 1000 / 1000);
        }

        asi.NumberOfSectors = (ulong)identify.A.LogicalCylinders * identify.A.LogicalHeads * identify.A.LogicalSectors;
        if (asi.Sector28 > 0 && ((ulong)asi.Sector28 * 512) / 1000 / 1000 > 49)
        {
            asi.DiskSizeLba28 = (uint)(((ulong)asi.Sector28 * 512) / 1000 / 1000 - 49);
            asi.NumberOfSectors = asi.Sector28;
        }
        else
        {
            asi.DiskSizeLba28 = 0;
        }

        if (asi.IsLba48Supported && (asi.Sector48 * asi.LogicalSectorSize) / 1000 / 1000 > 49)
        {
            asi.DiskSizeLba48 = (uint)((asi.Sector48 * asi.LogicalSectorSize) / 1000 / 1000 - 49);
            asi.NumberOfSectors = asi.Sector48;
        }
        else
        {
            asi.DiskSizeLba48 = 0;
        }

        asi.BufferSize = (uint)(identify.A.BufferSize * 512);
        if (asi.IsNvCacheSupported)
        {
            asi.NvCacheSize = (ulong)identify.A.NvCacheSizeLogicalBlocks * 512;
        }

        // +AMD_RC2 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (commandType == COMMAND_TYPE.CMD_TYPE_AMD_RC2)
        {
            asi.TotalDiskSize = TotalDiskSize;
        }
        else
        // +AMD_RC2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (identify.A.TotalAddressableSectors > 0x0FFFFFFF)
        {
            asi.TotalDiskSize = 0;
        }
        else if (asi.DiskSizeLba48 > asi.DiskSizeLba28)
        {
            asi.TotalDiskSize = asi.DiskSizeLba48;
        }
        else if (asi.DiskSizeLba28 > asi.DiskSizeChs)
        {
            asi.TotalDiskSize = asi.DiskSizeLba28;
        }
        else
        {
            asi.TotalDiskSize = asi.DiskSizeChs;
        }

        // Error Check for External ATA Controller
        if (asi.IsLba48Supported && (identify.A.TotalAddressableSectors < 268435455 && asi.DiskSizeLba28 != asi.DiskSizeLba48))
        {
            asi.DiskSizeLba48 = 0;
        }


        string debug;
        // Check S.M.A.R.T. Enabled or Diabled
        if (asi.IsSmartSupported || asi.Is9126MB || IsAdvancedDiskSearch)
        {
            switch (asi.CommandType)
            {
                case COMMAND_TYPE.CMD_TYPE_PHYSICAL_DRIVE:
                    debug = string.Format(("GetSmartAttributePd({0}) - 1"), physicalDriveId);
                    Logs.MyLogs(debug);
                    if (GetSmartAttributePd(physicalDriveId, asi.Target, ref asi))
                    {
                        CheckSsdSupport(ref asi);

                        //// my conversion was ended up here , i have Got my ssd health percent in csharp format so i dont need to proceed anymore , 
                        //Debugger.Break();
                        // here can be bugs if you want to go ahead u have to start from here 
                        GetSmartAttributePd(physicalDriveId, asi.Target, ref asiCheck);
                        //Debugger.Break();


                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            debug = string.Format(("GetSmartAttributePd({0}) - 1A"), physicalDriveId);
                            asi.IsSmartCorrect = true;
                        }

                        if (GetSmartThresholdPd(physicalDriveId, asi.Target, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartCorrect && ControlSmartStatusPd(physicalDriveId, asi.Target, ENABLE_SMART))
                    {
                        //debug.Format(("GetSmartAttributePd(%d) - 2"), physicalDriveId);
                        //Logs.MyLogs(debug);
                        if (GetSmartAttributePd(physicalDriveId, asi.Target, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            GetSmartAttributePd(physicalDriveId, asi.Target, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                debug = string.Format(("GetSmartAttributePd({0}) - 2A"), physicalDriveId);
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdPd(physicalDriveId, asi.Target, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                            asi.IsSmartEnabled = true;
                        }
                    }

                    // 2012/9/12 - https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=821;id=diskinfo#821
                    // 2013/12/2 - https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1330;id=diskinfo#1330

                    //if (memcmp(asi.SmartReadData, asi.SmartReadThreshold, 512) == 0 && asi.DiskVendorId != SSD_VENDOR_INDILINX)

                    var smartreadPtr = Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadData, 0);
                    var SmartReadThresholdPtr = Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadThreshold, 0);

                    var IsZero = Dip.Memcmp(smartreadPtr, SmartReadThresholdPtr, 512);

                    if ((IsZero == 0) && asi.DiskVendorId != (uint)VENDOR_ID.SSD_VENDOR_INDILINX)
                    {
                        //Logs.MyLogs(("asi.SmartReadData == asi.SmartReadThreshold"));
                        asi.IsSmartCorrect = false;
                        asi.IsThresholdCorrect = false;
                        asi.IsSmartEnabled = false;

                        /* 2013/04/12 Disabled
                        m_bAtaPassThroughSmart = true; // Force Enable ATA_PASS_THROUGH

                        debug.Format(("GetSmartAttributePd(%d) - 1"), physicalDriveId);
                        Logs.MyLogs(debug);
                        if(GetSmartAttributePd(physicalDriveId, asi.Target, &asi))
                        {
                            CheckSsdSupport(asi);
                            GetSmartAttributePd(physicalDriveId, asi.Target, &asiCheck);
                            if(CheckSmartAttributeCorrect(&asi, &asiCheck))
                            {
                                debug.Format(("GetSmartAttributePd(%d) - 1A"), physicalDriveId);
                                asi.IsSmartCorrect = true;
                            }

                            if(GetSmartThresholdPd(physicalDriveId, asi.Target, &asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                            asi.IsSmartEnabled = true;
                        }

                        if(! asi.IsSmartCorrect && ControlSmartStatusPd(physicalDriveId, asi.Target, ENABLE_SMART))
                        {
                            debug.Format(("GetSmartAttributePd(%d) - 2"), physicalDriveId);
                            Logs.MyLogs(debug);
                            if(GetSmartAttributePd(physicalDriveId, asi.Target, &asi))
                            {
                                CheckSsdSupport(asi);
                                GetSmartAttributePd(physicalDriveId, asi.Target, &asiCheck);
                                if(CheckSmartAttributeCorrect(&asi, &asiCheck))
                                {
                                    debug.Format(("GetSmartAttributePd(%d) - 2A"), physicalDriveId);
                                    asi.IsSmartCorrect = true;
                                }
                                if(GetSmartThresholdPd(physicalDriveId, asi.Target, &asi))
                                {
                                    asi.IsThresholdCorrect = true;
                                }
                            //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                                asi.IsSmartEnabled = true;
                            }
                        }
                        */
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_SCSI_MINIPORT:
                    if (GetSmartAttributeScsi(scsiPort, scsiTargetId, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        GetSmartAttributeScsi(scsiPort, scsiTargetId, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                        }
                        if (GetSmartThresholdScsi(scsiPort, scsiTargetId, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartCorrect && ControlSmartStatusScsi(scsiPort, scsiTargetId, ENABLE_SMART))
                    {
                        if (GetSmartAttributeScsi(scsiPort, scsiTargetId, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            GetSmartAttributeScsi(scsiPort, scsiTargetId, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdScsi(scsiPort, scsiTargetId, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                            asi.IsSmartEnabled = true;
                        }
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_SILICON_IMAGE:
                    Logs.MyLogs(("GetSmartAttributeSi(physicalDriveId, &asi)"));
                    if (GetSmartAttributeSi(physicalDriveId, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        Logs.MyLogs(("GetSmartAttributeSi(physicalDriveId, &asiCheck)"));
                        GetSmartAttributeSi(physicalDriveId, ref asiCheck);
                        Logs.MyLogs(("CheckSmartAttributeCorrect(&asi, &asiCheck) - 1"));
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                            // Compare Si and Pd 
                            GetSmartAttributePd(physicalDriveId, 0xA0, ref asiCheck);
                            //Logs.MyLogs(("CheckSmartAttributeCorrect(&asi, &asiCheck) - 2"));
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                //Logs.MyLogs(("GetSmartThresholdPd"));
                                //// Does not support GetSmartThresholdSi
                                GetSmartThresholdPd(physicalDriveId, 0xA0, ref asi);
                                asi.IsThresholdCorrect = true;
                            }
                        }

                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                        asi.IsSmartEnabled = true;
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_CSMI:
                    if (GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                        }
                        if (GetSmartThresholdCsmi(scsiPort, sasPhyEntity, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartCorrect && ControlSmartStatusCsmi(scsiPort, sasPhyEntity, ENABLE_SMART))
                    {
                        if (GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdCsmi(scsiPort, sasPhyEntity, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            asi.IsSmartEnabled = true;
                        }
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_CSMI_PHYSICAL_DRIVE:
                    debug = string.Format(("GetSmartAttributePd({0}) - 1 CSMI"), physicalDriveId);
                    Logs.MyLogs(debug);
                    if (GetSmartAttributePd(physicalDriveId, asi.Target, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        GetSmartAttributePd(physicalDriveId, asi.Target, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            debug = string.Format(("GetSmartAttributePd({0}) - 1A CSMI"), physicalDriveId);
                            asi.IsSmartCorrect = true;
                        }

                        if (GetSmartThresholdPd(physicalDriveId, asi.Target, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartEnabled || !asi.IsSmartCorrect || !asi.IsThresholdCorrect)
                    {
                        debug = string.Format(("GetSmartAttributeCsmi - 1 CSMI"));
                        Logs.MyLogs(debug);
                        if (GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdCsmi(scsiPort, sasPhyEntity, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            asi.IsSmartEnabled = true;
                        }

                        if (asi.IsSmartEnabled && asi.IsSmartCorrect && asi.IsThresholdCorrect)
                        {
                            asi.CommandType = COMMAND_TYPE.CMD_TYPE_CSMI;
                        }

                        if (!asi.IsSmartCorrect && ControlSmartStatusCsmi(scsiPort, sasPhyEntity, ENABLE_SMART))
                        {
                            if (GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asi))
                            {
                                CheckSsdSupport(ref asi);
                                GetSmartAttributeCsmi(scsiPort, sasPhyEntity, ref asiCheck);
                                if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                                {
                                    asi.IsSmartCorrect = true;
                                }
                                if (GetSmartThresholdCsmi(scsiPort, sasPhyEntity, ref asi))
                                {
                                    asi.IsThresholdCorrect = true;
                                }
                                asi.IsSmartEnabled = true;
                                asi.CommandType = COMMAND_TYPE.CMD_TYPE_CSMI;
                            }
                        }
                    }
                    break;

                case COMMAND_TYPE.CMD_TYPE_SAT:
                case COMMAND_TYPE.CMD_TYPE_SAT_ASM1352R:
                case COMMAND_TYPE.CMD_TYPE_SAT_REALTEK9220DP:
                case COMMAND_TYPE.CMD_TYPE_SUNPLUS:
                case COMMAND_TYPE.CMD_TYPE_IO_DATA:
                case COMMAND_TYPE.CMD_TYPE_LOGITEC:
                case COMMAND_TYPE.CMD_TYPE_PROLIFIC:
                case COMMAND_TYPE.CMD_TYPE_JMICRON:
                case COMMAND_TYPE.CMD_TYPE_CYPRESS:
                    debug = string.Format(("GetSmartAttributeSat({0}) - 1 [{1}]"), physicalDriveId, commandTypeString[(int)asi.CommandType]);
                    Logs.MyLogs(debug);
                    if (GetSmartAttributeSat(physicalDriveId, asi.Target, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        Logs.MyLogs(("GetSmartAttributeSat - 1A"));
                        GetSmartAttributeSat(physicalDriveId, asi.Target, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                        }
                        if (GetSmartThresholdSat(physicalDriveId, asi.Target, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartCorrect && ControlSmartStatusSat(physicalDriveId, asi.Target, ENABLE_SMART, asi.CommandType))
                    {
                        Logs.MyLogs(("GetSmartAttributeSat - 2"));
                        if (GetSmartAttributeSat(physicalDriveId, asi.Target, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            Logs.MyLogs(("GetSmartAttributeSat - 2A"));
                            GetSmartAttributeSat(physicalDriveId, asi.Target, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdSat(physicalDriveId, asi.Target, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                            asi.IsSmartEnabled = true;
                        }
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_WMI:
                    asi.IsSmartCorrect = GetSmartAttributeWmi(ref asi);
                    asi.IsThresholdCorrect = GetSmartThresholdWmi(ref asi);
                    if (asi.IsSmartCorrect)
                    {
                        CheckSsdSupport(ref asi);
                        asi.IsSmartEnabled = true;
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_MEGARAID:
                    if (GetSmartAttributeMegaRAID(scsiPort, scsiTargetId, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        GetSmartAttributeMegaRAID(scsiPort, scsiTargetId, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                        }
                        if (GetSmartThresholdMegaRAID(scsiPort, scsiTargetId, ref asi))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                        asi.IsSmartEnabled = true;
                    }

                    if (!asi.IsSmartCorrect && ControlSmartStatusMegaRAID(scsiPort, scsiTargetId, ENABLE_SMART))
                    {
                        if (GetSmartAttributeMegaRAID(scsiPort, scsiTargetId, ref asi))
                        {
                            CheckSsdSupport(ref asi);
                            GetSmartAttributeMegaRAID(scsiPort, scsiTargetId, ref asiCheck);
                            if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                            {
                                asi.IsSmartCorrect = true;
                            }
                            if (GetSmartThresholdMegaRAID(scsiPort, scsiTargetId, ref asi))
                            {
                                asi.IsThresholdCorrect = true;
                            }
                            //	asi.DiskStatus = CheckDiskStatus(asi.Attribute, asi.Threshold, asi.AttributeCount, asi.DiskVendorId, asi.IsSmartCorrect, asi.IsSsd);
                            asi.IsSmartEnabled = true;
                        }
                    }
                    break;
#if !(_M_ARM) && !(_M_ARM64)
                case COMMAND_TYPE.CMD_TYPE_AMD_RC2:// +AMD_RC2
                    if (GetSmartDataAMD_RC2(scsiBus, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        GetSmartDataAMD_RC2(scsiBus, ref asiCheck);
                        if (CheckSmartAttributeCorrect(ref asi, ref asiCheck))
                        {
                            asi.IsSmartCorrect = true;
                        }
                        if (GetSmartThresholdAMD_RC2(scsiBus, ref asiCheck))
                        {
                            asi.IsThresholdCorrect = true;
                        }
                        asi.IsSmartSupported = true;
                        asi.IsSmartEnabled = true;
                    }
                    break;
#endif
#if JMICRON_USB_RAID_SUPPORT
                case COMMAND_TYPE.CMD_TYPE_JMS56X:
                    if (GetSmartInfoJMS56X(scsiBus, scsiPort, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        // GetSmartInfoJMicronUsbRaid(scsiBus, scsiPort, &asiCheck);
                        // if (CheckSmartAttributeCorrect(&asi, &asiCheck)){}
                        asi.IsSmartSupported = true;
                        asi.IsSmartCorrect = true;
                        asi.IsThresholdCorrect = true;
                        asi.IsSmartEnabled = true;
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_JMB39X:
                    if (GetSmartInfoJMB39X(scsiBus, (byte)scsiPort, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        // GetSmartInfoJMicronUsbRaid(scsiBus, scsiPort, &asiCheck);
                        // if (CheckSmartAttributeCorrect(&asi, &asiCheck)){}
                        asi.IsSmartSupported = true;
                        asi.IsSmartCorrect = true;
                        asi.IsThresholdCorrect = true;
                        asi.IsSmartEnabled = true;
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_JMS586_40:
                    if (GetSmartInfoJMS586_40(scsiBus, scsiPort, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        // GetSmartInfoJMicronUsbRaid(scsiBus, scsiPort, &asiCheck);
                        // if (CheckSmartAttributeCorrect(&asi, &asiCheck)){}
                        asi.IsSmartSupported = true;
                        asi.IsSmartCorrect = true;
                        asi.IsThresholdCorrect = true;
                        asi.IsSmartEnabled = true;
                    }
                    break;
                case COMMAND_TYPE.CMD_TYPE_JMS586_20:

                    if (GetSmartInfoJMS586_20(scsiBus, (byte)scsiPort, ref asi))
                    {
                        CheckSsdSupport(ref asi);
                        // GetSmartInfoJMicronUsbRaid(scsiBus, scsiPort, &asiCheck);
                        // if (CheckSmartAttributeCorrect(&asi, &asiCheck)){}
                        asi.IsSmartSupported = true;
                        asi.IsSmartCorrect = true;
                        asi.IsThresholdCorrect = true;
                        asi.IsSmartEnabled = true;
                    }
                    break;
#endif
                default:
                    return false;
                    //break;
            }
        }
        return true;
    }

    bool GetNvMeIdentifyControllerData(int physicalDriveId, byte[] outBuffer)
    {
        string path;
        path = string.Format("\\\\.\\PhysicalDrive{0}", physicalDriveId);

        var hIoCtrl = Dip.CreateFile(path, (uint)(GENERIC_READ | GENERIC_WRITE),
            FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
        TStorageQueryWithBuffer nptwb;
        Unsafe.SkipInit(out nptwb);

        bool bRet = false;

        //ZeroMemory(ref nptwb, Marshal.SizeOf(nptwb));

        StructToZeroStruct(ref nptwb);

        if (hIoCtrl == INVALID_HANDLE_VALUE)
        {
            return false;
        }

        nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
        nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
        nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
        nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
        nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
        nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
        nptwb.ProtocolSpecific.ProtocolDataRequestValue = 1; /*NVME_IDENTIFY_CNS_CONTROLLER*/
        nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0;

        uint dwReturned = 0;

        bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IOCTL_STORAGE_QUERY_PROPERTY,
            ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);

        Dip.safeCloseHandle(hIoCtrl);

        //CopyMemory(ref outBuffer[0], ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

        Buffer.BlockCopy(nptwb.Buffer, 0, outBuffer, 0, Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

        //memcpy_s(outBuffer, sizeof(NVME_IDENTIFY_DEVICE), nptwb.Buffer, sizeof(NVME_IDENTIFY_DEVICE));

        return bRet;
    }




    bool SendAtaCommandPd(int physicalDriveId, byte target, byte main, byte sub, byte param, ref byte[] data, uint dataSize)
    {
        bool bRet = false;
        IntPtr hIoCtrl = IntPtr.Zero;
        uint dwReturned = 0;

        hIoCtrl = GetIoCtrlHandle(physicalDriveId);
        if (hIoCtrl == IntPtr.Zero || hIoCtrl == INVALID_HANDLE_VALUE)
        {
            return false;
        }

        if (m_bAtaPassThrough)
        {
            ATA_PASS_THROUGH_EX_WITH_BUFFERS ab;
            Unsafe.SkipInit(out ab);

            //ZeroMemory(ref ab, Marshal.SizeOf(ab));
            ab.StructToZeroHollowCast();

            ab.Apt.Length = (ushort)Marshal.SizeOf(typeof(ATA_PASS_THROUGH_EX));
            ab.Apt.TimeOutValue = 2;
            uint size = (uint)Marshal.OffsetOf(typeof(ATA_PASS_THROUGH_EX_WITH_BUFFERS), "Buf");
            ab.Apt.DataBufferOffset = size;

            if (dataSize > 0)
            {
                if (dataSize > ab.Buf.Length)
                {
                    return false;
                }
                ab.Apt.AtaFlags = ATA_FLAGS_DATA_IN;
                ab.Apt.DataTransferLength = dataSize;
                ab.Buf[0] = 0xCF; // magic number
                size += dataSize;
            }

            ab.Apt.CurrentTaskFile.bFeaturesReg = sub;
            ab.Apt.CurrentTaskFile.bSectorCountReg = param;
            ab.Apt.CurrentTaskFile.bDriveHeadReg = target;
            ab.Apt.CurrentTaskFile.bCommandReg = main;

            if (main == SMART_CMD)
            {
                ab.Apt.CurrentTaskFile.bCylLowReg = SMART_CYL_LOW;
                ab.Apt.CurrentTaskFile.bCylHighReg = SMART_CYL_HI;
                ab.Apt.CurrentTaskFile.bSectorCountReg = 1;
                ab.Apt.CurrentTaskFile.bSectorNumberReg = 1;
            }

            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.IOCTL_ATA_PASS_THROUGH,
                ref ab, size, ref ab, size, ref dwReturned, IntPtr.Zero);
            Dip.safeCloseHandle(hIoCtrl);
            if (bRet == true && dataSize != 0 && !data.Equals(default(ATA_SMART_INFO)))
            {
                //var dataAlloc = Marshal.AllocHGlobal(data.Length);
                //Marshal.Copy(ab.Buf, 0, dataAlloc, (int)dataSize);
                //Marshal.Copy(dataAlloc, data, 0, (int)dataSize);
                //dataAlloc.FreePtr();

                Buffer.BlockCopy(ab.Buf, 0, data, 0, (int)dataSize);

                #region MyRegion
                //[MethodImpl(MethodImplOptions.AggressiveInlining)]
                //[System.Runtime.Versioning.NonVersionable]
                //public static void CopyBlock(ref byte destination, ref byte source, uint byteCount)
                //{
                //    // IL cpblk instruction
                //    Unsafe.CopyBlock(ref destination, ref source, byteCount);
                //}
                //Copy(byte[] source, int startIndex, IntPtr destination, int length)

                //CopyMemory(ref data.SmartReadData[0], ref ab.Buf[0], dataSize);
                //Buffer.BlockCopy(ab.Buf, 0, data, 0, (int)dataSize);

                //Buffer.BlockCopy(ab.Buf, 0, data.SmartReadData, 0, (int)dataSize);



                //Marshal.Copy(ab.Buf, 0, data, (int)dataSize);

                //memcpy_s(data, dataSize, ab.Buf, dataSize); 
                #endregion
            }
        }
        else
        {
            uint size = (uint)(Marshal.SizeOf(typeof(CMD_IDE_PATH_THROUGH)) - 1 + dataSize);

            /*CMD_IDE_PATH_THROUGH[]*/
            var buf = Dni.VirtualAlloc(IntPtr.Zero, size, MEM_COMMIT, PAGE_READWRITE);

            //var buf = Marshal.PtrToStructure<CMD_IDE_PATH_THROUGH>(vAllocs);
            if (!buf.Equals(default))
            {
                buf.reg.bFeaturesReg = sub;
                buf.reg.bSectorCountReg = param;
                buf.reg.bSectorNumberReg = 0;
                buf.reg.bCylLowReg = 0;
                buf.reg.bCylHighReg = 0;
                buf.reg.bDriveHeadReg = target;
                buf.reg.bCommandReg = main;
                buf.reg.bReserved = 0;
                buf.length = dataSize;

                bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.IOCTL_IDE_PASS_THROUGH,
                    ref buf, size, ref buf, size, ref dwReturned, IntPtr.Zero);
            }
            Dip.safeCloseHandle(hIoCtrl);
            if (bRet && dataSize != 0 && !data.Equals(default(ATA_SMART_INFO)))
            {
                //memcpy_s(data, dataSize, buf.buffer, dataSize);

                //Buffer.BlockCopy(buf.buffer, 0, data.SmartReadData, 0, (int)dataSize);
                //CopyMemory(ref data.SmartReadData[0], ref buf.buffer[0], dataSize);
            }
            Dni.safeVirtualFree(ref buf, (UIntPtr)0, MEM_RELEASE);
        }

        return bRet;
    }

    bool GetSmartThresholdPd(int physicalDriveId, byte target, ref ATA_SMART_INFO asi)
    {
        bool bRet = false;
        IntPtr hIoCtrl = INVALID_HANDLE_VALUE;
        uint dwReturned = 0;

        SMART_READ_DATA_OUTDATA sendCmdOutParam;
        Unsafe.SkipInit(out sendCmdOutParam);
        SENDCMDINPARAMS sendCmd;
        Unsafe.SkipInit(out sendCmd);

        if (m_bAtaPassThrough && m_bAtaPassThroughSmart)
        {
            Logs.MyLogs(("SendAtaCommandPd - SMART_READ_THRESHOLDS (ATA_PASS_THROUGH)"));

            #region MyRegion
            ////bRet = SendAtaCommandPd(physicalDriveId, target, 0xEC, 0x00, 0x00, ref data, (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));
            //if (asi.Threshold == null)
            //{
            //    Unsafe.SkipInit(out asi);

            //    //ZeroMemory(ref asi, Marshal.SizeOf(asi));
            //    //asi.StructToZeroHollowCast();

            //} 
            #endregion

            //var AllocReadThreshold = Marshal.AllocHGlobal(asi.SmartReadThreshold.Length);

            bRet = SendAtaCommandPd(physicalDriveId, target, SMART_CMD, READ_THRESHOLDS, 0x00,
             /*(PBYTE) & (asi.SmartReadThreshold)*/ ref asi.SmartReadThreshold, (uint)asi.SmartReadThreshold.Length);

            //Marshal.Copy(AllocReadThreshold, asi.SmartReadThreshold, 0, asi.SmartReadThreshold.Length);
            //AllocReadThreshold.FreePtr();
        }

        if (!bRet)
        {
            hIoCtrl = GetIoCtrlHandle(physicalDriveId);
            if (hIoCtrl != IntPtr.Zero || hIoCtrl == INVALID_HANDLE_VALUE)
            {
                return false;
            }

            //::ZeroMemory(&sendCmdOutParam, sizeof(SMART_READ_DATA_OUTDATA));
            //::ZeroMemory(&sendCmd, sizeof(SENDCMDINPARAMS));

            sendCmd.irDriveRegs.bFeaturesReg = READ_THRESHOLDS;
            sendCmd.irDriveRegs.bSectorCountReg = 1;
            sendCmd.irDriveRegs.bSectorNumberReg = 1;
            sendCmd.irDriveRegs.bCylLowReg = SMART_CYL_LOW;
            sendCmd.irDriveRegs.bCylHighReg = SMART_CYL_HI;
            sendCmd.irDriveRegs.bDriveHeadReg = target;
            sendCmd.irDriveRegs.bCommandReg = SMART_CMD;
            sendCmd.cBufferSize = READ_THRESHOLD_BUFFER_SIZE;

            Logs.MyLogs(("SendAtaCommandPd - SMART_READ_THRESHOLDS"));

            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.DFP_RECEIVE_DRIVE_DATA,
                ref sendCmd, (uint)Marshal.SizeOf(typeof(SENDCMDINPARAMS)),
                ref sendCmdOutParam, (uint)Marshal.SizeOf(typeof(SMART_READ_DATA_OUTDATA)),
                ref dwReturned, IntPtr.Zero);

            Dip.safeCloseHandle(hIoCtrl);

            if (bRet == false || dwReturned != Marshal.SizeOf(typeof(SMART_READ_DATA_OUTDATA)))
            {
                return false;
            }

            Buffer.BlockCopy(asi.SmartReadThreshold, 0, sendCmdOutParam.SendCmdOutParam.bBuffer, 0, (int)512);
            //memcpy_s(&(asi.SmartReadThreshold), 512, &(sendCmdOutParam.SendCmdOutParam.bBuffer), 512);
        }

        return FillSmartThreshold(ref asi);
    }

    bool GetSmartAttributePd(int physicalDriveId, byte target, ref ATA_SMART_INFO asi)
    {
        bool bRet = false;
        IntPtr hIoCtrl = IntPtr.Zero;
        uint dwReturned = 0;

        SMART_READ_DATA_OUTDATA sendCmdOutParam;
        Unsafe.SkipInit(out sendCmdOutParam);
        SENDCMDINPARAMS sendCmd;
        Unsafe.SkipInit(out sendCmd);


        //SendAtaCommandPd(physicalDriveId, target, SMART_CMD, READ_ATTRIBUTES, 0x00,
        //Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadData, 0), Marshal.SizeOf(asi.SmartReadData));

        if (m_bAtaPassThrough && m_bAtaPassThroughSmart)
        {
            Logs.MyLogs(("SendAtaCommandPd - SMART_READ_DATA (ATA_PASS_THROUGH)"));

            //IntPtr pbytecheck = Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadData, 0);

            if (asi.SmartReadData == null)
            {
                Unsafe.SkipInit(out asi);

                //ZeroMemory(ref asi, Marshal.SizeOf(asi));
                asi.StructToZeroHollowCast();
            }

            //var AllocSmartReader = Marshal.AllocHGlobal(asi.SmartReadData.Length);

            bRet = SendAtaCommandPd(physicalDriveId, target, SMART_CMD, READ_ATTRIBUTES, 0x00,
            ref asi.SmartReadData, (uint)asi.SmartReadData.Length);

            // 2. Create a managed byte array to receive the data
            //byte[] managedByteArray = new byte[asi.SmartReadData.Length];
            //Marshal.Copy(AllocSmartReader, asi.SmartReadData, 0, asi.SmartReadData.Length);

        }

        if (!bRet)
        {
            hIoCtrl = GetIoCtrlHandle(physicalDriveId);
            if (hIoCtrl == IntPtr.Zero || hIoCtrl == INVALID_HANDLE_VALUE)
            {
                return false;
            }

            //::ZeroMemory(&sendCmdOutParam, sizeof(SMART_READ_DATA_OUTDATA));
            //::ZeroMemory(&sendCmd, sizeof(SENDCMDINPARAMS));

            sendCmd.irDriveRegs.bFeaturesReg = READ_ATTRIBUTES;
            sendCmd.irDriveRegs.bSectorCountReg = 1;
            sendCmd.irDriveRegs.bSectorNumberReg = 1;
            sendCmd.irDriveRegs.bCylLowReg = SMART_CYL_LOW;
            sendCmd.irDriveRegs.bCylHighReg = SMART_CYL_HI;
            sendCmd.irDriveRegs.bDriveHeadReg = target;
            sendCmd.irDriveRegs.bCommandReg = SMART_CMD;
            sendCmd.cBufferSize = READ_ATTRIBUTE_BUFFER_SIZE;

            Logs.MyLogs(("SendAtaCommandPd - SMART_READ_DATA"));

            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.DFP_RECEIVE_DRIVE_DATA,
                ref sendCmd, (uint)Marshal.SizeOf(typeof(SENDCMDINPARAMS)),
                ref sendCmdOutParam, (uint)Marshal.SizeOf(typeof(SMART_READ_DATA_OUTDATA)),
                ref dwReturned, IntPtr.Zero);

            Dip.safeCloseHandle(hIoCtrl);

            if (bRet == false || dwReturned != Marshal.SizeOf(typeof(SMART_READ_DATA_OUTDATA)))
            {
                return false;
            }

            Buffer.BlockCopy(sendCmdOutParam.SendCmdOutParam.bBuffer, 0, asi.SmartReadData, 0, 512);
            //CopyMemory(ref asi.SmartReadData[0], 512, ref sendCmdOutParam.SendCmdOutParam.bBuffer[0], 512);

            //memcpy_s(&(asi.SmartReadData), 512, &(sendCmdOutParam.SendCmdOutParam.bBuffer), 512);
        }

        return FillSmartData(ref asi);
    }

    bool FillSmartThreshold(ref ATA_SMART_INFO asi)
    {
        // 2016/04/18
        // https://github.com/hiyohiyo/CrystalDiskInfo/issues/1
        int count = 0;
        for (int i = 0; i < MAX_ATTRIBUTE; i++)
        {
            //var Target_Byte = asi.SmartReadThreshold[i * Marshal.SizeOf(typeof(SMART_THRESHOLD)) + 2];
            //// Create a Span<byte> from a byte array
            var _unsafePinned = Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadThreshold, i * Marshal.SizeOf(typeof(SMART_THRESHOLD)) + 2);

            SMART_THRESHOLD pst = Marshal.PtrToStructure<SMART_THRESHOLD>(_unsafePinned);


            //SMART_THRESHOLD pst = Marshal.PtrToStructure<SMART_THRESHOLD>(
            //Marshal.UnsafeAddrOfPinnedArrayElement(asi.SmartReadThreshold, i * Marshal.SizeOf<SMART_THRESHOLD>() + 2));

            //var pstPosition = asi.Threshold;


            //var pst = /*(SMART_THRESHOLD*)&*/(asi.SmartReadThreshold[i * Marshal.SizeOf(typeof(SMART_THRESHOLD)) + 2]);
            if (pst.Id != 0)
            {
                for (uint j = 0; j < asi.AttributeCount; j++)
                {
                    if (pst.Id == asi.Attribute[j].Id)
                    {
                        //memcpy(&(asi.Threshold[j]), pst, sizeof(SMART_THRESHOLD));
                        MemCpyStructToStruct<SMART_THRESHOLD>(ref asi.Threshold[j], ref pst);
                        count++;
                    }
                }
            }
        }

        // 2013/04/13 Added P400e SSD SMART Implementation support
        // Threshold = Attribute[].Reserved
        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON && count == 0)
        {
            for (int i = 0; i < MAX_ATTRIBUTE; i++)
            {
                if (asi.Attribute[i].Reserved > 0)
                {
                    asi.Threshold[i].Id = asi.Attribute[i].Id;
                    asi.Threshold[i].ThresholdValue = asi.Attribute[i].Reserved;
                    count++;
                }
            }
        }

        // 2023/02/19 Disabled Threshold Check
        // if(count > 0)
        if (asi.AttributeCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}
