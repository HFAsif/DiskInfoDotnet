#nullable enable
namespace CrystalDiskInfoDotnet.CheckDiskInfos;
using System;
using System.Runtime.InteropServices;

//[Serializable]
public record ExtendedInfosStruct
{
    public bool? IsSmartEnabled{ get; set; }
    public bool? IsIdInfoIncorrect{ get; set; }
    public bool? IsSmartCorrect{ get; set; }
    public bool? IsThresholdCorrect{ get; set; }
    public bool? IsCheckSumError{ get; set; }
    public bool? IsWord88{ get; set; }
    public bool? IsWord64_76{ get; set; }
    public bool? IsRawValues8{ get; set; }
    public bool? IsRawValues7{ get; set; }
    public bool? Is9126MB{ get; set; }
    public bool? IsThresholdBug{ get; set; }
    public bool? IsSmartSupported{ get; set; }
    public bool? IsLba48Supported{ get; set; }
    public bool? IsAamSupported{ get; set; }
    public bool? IsApmSupported{ get; set; }
    public bool? IsAamEnabled{ get; set; }
    public bool? IsApmEnabled{ get; set; }
    public bool? IsNcqSupported{ get; set; }
    public bool? IsNvCacheSupported{ get; set; }
    public bool? IsNvmeThresholdSupported{ get; set; }
    public bool? IsNvmeThermalManagementSupported{ get; set; }
    public bool? IsDeviceSleepSupported{ get; set; }
    public bool? IsStreamingSupported{ get; set; }
    public bool? IsGplSupported{ get; set; }
    public bool? IsMaxtorMinute{ get; set; }
    public bool? IsSsd{ get; set; }
    public bool? IsTrimSupported{ get; set; }
    public bool? IsVolatileWriteCachePresent{ get; set; }
    public bool? IsNVMe{ get; set; }
    public bool? IsUasp{ get; set; }
    public int? PhysicalDriveId{ get; set; }
    public int? ScsiPort{ get; set; }
    public int? ScsiTargetId{ get; set; }
    public int? ScsiBus{ get; set; }
    public int? SiliconImageType{ get; set; }
    //		int					   AccessType{ get; set; }
    public uint? TotalDiskSize{ get; set; }
    public uint? Cylinder{ get; set; }
    public uint? Head{ get; set; }
    public uint? Sector{ get; set; }
    public uint? Sector28{ get; set; }
    public ulong? Sector48{ get; set; }
    public ulong? NumberOfSectors{ get; set; }
    public uint? DiskSizeChs{ get; set; }
    public uint? DiskSizeLba28{ get; set; }
    public uint? DiskSizeLba48{ get; set; }
    public uint? LogicalSectorSize{ get; set; }
    public uint? PhysicalSectorSize{ get; set; }
    public uint? DiskSizeWmi{ get; set; }
    public uint? BufferSize{ get; set; }
    public ulong? NvCacheSize{ get; set; }
    public uint? TransferModeType{ get; set; }
    public uint? DetectedTimeUnitType{ get; set; }
    public uint? MeasuredTimeUnitType{ get; set; }
    public uint? AttributeCount{ get; set; }
    public int? DetectedPowerOnHours{ get; set; }
    public int? MeasuredPowerOnHours{ get; set; }
    public int? PowerOnRawValue{ get; set; }
    public int? PowerOnStartRawValue{ get; set; }
    public uint? PowerOnCount{ get; set; }
    public int? Temperature{ get; set; }


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public int[]? TemperatureNVMe;

    public double? TemperatureMultiplier{ get; set; }
    public uint? NominalMediaRotationRate{ get; set; }
    //		double				   Speed{ get; set; }
    public int? HostWrites{ get; set; }
    public int? HostReads{ get; set; }
    public int? GBytesErased{ get; set; }
    public int? NandWrites{ get; set; }
    public int? WearLevelingCount{ get; set; }

    //		int					   PlextorNandWritesUnit{ get; set; }

    public int? Life{ get; set; }
    public bool? FlagLifeNoReport{ get; set; }
    public bool? FlagLifeRawValue{ get; set; }
    public bool? FlagLifeRawValueIncrement{ get; set; }
    public bool? FlagLifeSanDiskUsbMemory{ get; set; }
    public bool? FlagLifeSanDisk0_1{ get; set; }
    public bool? FlagLifeSanDisk1{ get; set; }
    public bool? FlagLifeSanDiskLenovo{ get; set; }
    public bool? FlagLifeSanDiskCloud{ get; set; }

    public uint? Major{ get; set; }
    public uint? Minor{ get; set; }

    public uint? DiskStatus{ get; set; }
    public uint? DriveLetterMap{ get; set; }

    public int? AlarmTemperature{ get; set; }
    public bool? AlarmHealthStatus{ get; set; }

    public uint? DiskVendorId{ get; set; }
    public uint? UsbVendorId{ get; set; }
    public uint? UsbProductId{ get; set; }
    public byte? Target{ get; set; }

    public ushort? Threshold05{ get; set; }
    public ushort? ThresholdC5{ get; set; }
    public ushort? ThresholdC6{ get; set; }
    public ushort? ThresholdFF{ get; set; }

    public string? SerialNumber{ get; set; }
    public string? SerialNumberReverse{ get; set; }
    public string? FirmwareRev{ get; set; }
    public string? FirmwareRevReverse{ get; set; }
    public string? Model{ get; set; }
    public string? ModelReverse{ get; set; }
    public string? ModelWmi{ get; set; }
    public string? ModelSerial{ get; set; }
    public string? DriveMap{ get; set; }
    public string? MaxTransferMode{ get; set; }
    public string? CurrentTransferMode{ get; set; }
    public string? MajorVersion{ get; set; }
    public string? MinorVersion{ get; set; }
    public string? Interface{ get; set; }
    public string? Enclosure{ get; set; }
    public string? CommandTypeString{ get; set; }
    public string? SsdVendorString{ get; set; }
    public string? DeviceNominalFormFactor{ get; set; }
    public string? PnpDeviceId{ get; set; }
    public string? SmartKeyName{ get; set; }
}

