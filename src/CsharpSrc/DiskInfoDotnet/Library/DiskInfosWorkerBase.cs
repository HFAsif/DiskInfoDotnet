namespace DiskInfoDotnet.Library;
using System;
using System.Collections.ObjectModel;

internal class DiskInfosWorkerBase
{
    public required nint hMutexJMicron;
    public required bool m_bAtaPassThrough;
    public required bool m_bAtaPassThroughSmart;
    public required bool m_bNVMeStorageQuery;

    protected bool FlagUsbMemory;
    protected bool IsAdvancedDiskSearch;
    protected bool IsWorkaroundHD204UI;

    protected bool FlagNvidiaController;
    protected bool FlagMarvellController;

    protected bool flagBlackList;
    protected bool flagTarget;

    protected bool flagUasp;
    protected bool flagNVMe;

    protected bool detectUASPdisks;
    protected bool detectUSBMemory;

    protected string? model = default, deviceId, diskSize, mediaType = default, interfaceTypeWmi = default, pnpDeviceId = default, firmware = default;
    protected int physicalDriveId = -1, scsiPort = -1, scsiTargetId = -1, scsiBus = -1;
    protected uint siliconImageType = 0;

    
    protected IntPtr AllocMethod(int size)
    {
        // Allocate the memory, zeroing it in the progress
        IntPtr memPtr = Dip.LocalAlloc((uint)Inm.LocalMemoryFlags.LPTR, new UIntPtr((uint)size));
        // Throw an OutOfMemoryException if out of memory
        if (memPtr == IntPtr.Zero)
            throw new OutOfMemoryException();
        return memPtr;
    }

    protected uint GetPowerOnHours(uint rawValue, uint timeUnitType)
    {
        switch (timeUnitType)
        {
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_UNKNOWN:
                return 0;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_HOURS:
                return rawValue;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_MINUTES:
                return rawValue / 60;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_HALF_MINUTES:
                return rawValue / 120;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_SECONDS:
                return rawValue / 60 / 60;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_10_MINUTES:
                return rawValue / 6;
            //break;
            case (uint)Inm.POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS:
                return rawValue;
            //break;
            default:
                return rawValue;
                //break;
        }
    }

    //protected uint GetPowerOnHoursEx(uint i, uint timeUnitType)
    //{
    //    uint rawValue = vars[i].PowerOnRawValue;
    //    switch (timeUnitType)
    //    {
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_UNKNOWN:
    //            return 0;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS:
    //            return rawValue;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_MINUTES:
    //            return rawValue / 60;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_HALF_MINUTES:
    //            return rawValue / 120;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_SECONDS:
    //            return rawValue / 60 / 60;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_10_MINUTES:
    //            return rawValue / 6;
    //            break;
    //        case (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS:
    //            return rawValue;
    //            break;
    //        default:
    //            return rawValue;
    //            break;
    //    }
    //}

    protected ulong B8toB64(byte b0, byte b1, byte b2, byte b3, byte b4, byte b5 = 0, byte b6 = 0, byte b7 = 0)
    {
        ulong data =
              ((ulong)b7 << 56)
            + ((ulong)b6 << 48)
            + ((ulong)b5 << 40)
            + ((ulong)b4 << 32)
            + ((ulong)b3 << 24)
            + ((ulong)b2 << 16)
            + ((ulong)b1 << 8)
            + ((ulong)b0 << 0);

        return data;
    }

    protected uint B8toB32(byte b0, byte b1, byte b2, byte b3)
    {
        uint data =
              ((uint)b3 << 24)
            + ((uint)b2 << 16)
            + ((uint)b1 << 8)
            + ((uint)b0 << 0);

        return data;
    }

    protected string GetModelSerial(ref string model, ref string serialNumber)
    {
        string modelSerial;
        modelSerial = model + serialNumber;
        modelSerial.Replace(("\\"), (""));
        modelSerial.Replace(("/"), (""));
        modelSerial.Replace((":"), (""));
        modelSerial.Replace(("*"), (""));
        modelSerial.Replace(("?"), (""));
        modelSerial.Replace(("\""), (""));
        modelSerial.Replace(("<"), (""));
        modelSerial.Replace((">"), (""));
        modelSerial.Replace(("|"), (""));

        return modelSerial;
    }
}
