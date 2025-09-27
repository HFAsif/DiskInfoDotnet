namespace HelperClass;
using System;
using static ImportantElements_Properties;
[SomeElementsInfos("Has some enums which get from crystalDiskInfos")]
public class ImportantElements_Enums
{
    [Flags]
    public enum INTERFACE_TYPE
    {
        INTERFACE_TYPE_UNKNOWN = 0,
        INTERFACE_TYPE_PATA,
        INTERFACE_TYPE_SATA,
        INTERFACE_TYPE_USB,
        INTERFACE_TYPE_IEEE1394,
        //	INTERFACE_TYPE_UASP,
        INTERFACE_TYPE_SCSI,
        INTERFACE_TYPE_NVME,
        INTERFACE_TYPE_AMD_RC2,// +AMD_RC2
                               //	INTERFACE_TYPE_USB_NVME,
    };

    [Flags]
    public enum VENDOR_ID
    {
        HDD_GENERAL = 0,
        SSD_GENERAL = 1,
        SSD_VENDOR_MTRON = 2,
        SSD_VENDOR_INDILINX = 3,
        SSD_VENDOR_JMICRON = 4,
        SSD_VENDOR_INTEL = 5,
        SSD_VENDOR_SAMSUNG = 6,
        SSD_VENDOR_SANDFORCE = 7,
        SSD_VENDOR_MICRON = 8,
        SSD_VENDOR_OCZ = 9,
        SSD_VENDOR_SEAGATE = 10,
        SSD_VENDOR_WDC = 11,
        SSD_VENDOR_PLEXTOR = 12,
        SSD_VENDOR_SANDISK = 13,
        SSD_VENDOR_OCZ_VECTOR = 14,
        SSD_VENDOR_TOSHIBA = 15,
        SSD_VENDOR_CORSAIR = 16,
        SSD_VENDOR_KINGSTON = 17,
        SSD_VENDOR_MICRON_MU03 = 18,
        SSD_VENDOR_NVME = 19,
        SSD_VENDOR_REALTEK = 20,
        SSD_VENDOR_SKHYNIX = 21,
        SSD_VENDOR_KIOXIA = 22,
        SSD_VENDOR_SSSTC = 23,
        SSD_VENDOR_INTEL_DC = 24,
        SSD_VENDOR_APACER = 25,
        SSD_VENDOR_SILICONMOTION = 26,
        SSD_VENDOR_PHISON = 27,
        SSD_VENDOR_MARVELL = 28,
        SSD_VENDOR_MAXIOTEK = 29,
        SSD_VENDOR_YMTC = 30,
        SSD_VENDOR_SCY = 31,

        SSD_VENDOR_JMICRON_60X = 32,
        SSD_VENDOR_JMICRON_61X = 33,
        SSD_VENDOR_JMICRON_66X = 34,
        SSD_VENDOR_SEAGATE_IRON_WOLF = 35,
        SSD_VENDOR_SEAGATE_BARRA_CUDA = 36,
        SSD_VENDOR_SANDISK_GB = 37,
        SSD_VENDOR_KINGSTON_SUV = 38,
        SSD_VENDOR_KINGSTON_KC600 = 39,
        SSD_VENDOR_KINGSTON_DC500 = 40,
        SSD_VENDOR_KINGSTON_SA400 = 41,

        SSD_VENDOR_RECADATA = 42,

        SSD_VENDOR_SANDISK_DELL = 43,
        SSD_VENDOR_SANDISK_HP = 44,
        SSD_VENDOR_SANDISK_HP_VENUS = 45,
        SSD_VENDOR_SANDISK_LENOVO = 46,
        SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS = 47,
        SSD_VENDOR_SANDISK_CLOUD = 48,

        SSD_VENDOR_SILICONMOTION_CVC = 49,
        SSD_VENDOR_ADATA_INDUSTRIAL = 50,

        SSD_VENDOR_MAX = 99,

        VENDOR_UNKNOWN = 0x0000,
        USB_VENDOR_BUFFALO = 0x0411,
        USB_VENDOR_IO_DATA = 0x04BB,
        USB_VENDOR_LOGITEC = 0x0789,
        USB_VENDOR_INITIO = 0x13FD,
        USB_VENDOR_SUNPLUS = 0x04FC,
        USB_VENDOR_JMICRON = 0x152D,
        USB_VENDOR_CYPRESS = 0x04B4,
        USB_VENDOR_OXFORD = 0x0928,
        USB_VENDOR_PROLIFIC = 0x067B,
        USB_VENDOR_REALTEK = 0x0BDA,
        USB_VENDOR_ALL = 0xFFFF,
    };

    [Flags]
    public enum HOST_READS_WRITES_UNIT
    {
        HOST_READS_WRITES_UNKNOWN = 0,
        HOST_READS_WRITES_512B,
        HOST_READS_WRITES_1MB,
        HOST_READS_WRITES_16MB,
        HOST_READS_WRITES_32MB,
        HOST_READS_WRITES_GB,
    };


    [Flags]
    public enum POWER_ON_HOURS_UNIT
    {
        POWER_ON_UNKNOWN = 0,
        POWER_ON_HOURS,
        POWER_ON_MINUTES,
        POWER_ON_HALF_MINUTES,
        POWER_ON_SECONDS,
        POWER_ON_10_MINUTES,
        POWER_ON_MILLI_SECONDS,
    };

    [Flags]
    public enum TRANSFER_MODE
    {
        TRANSFER_MODE_UNKNOWN = 0,
        TRANSFER_MODE_PIO,
        TRANSFER_MODE_PIO_DMA,
        TRANSFER_MODE_ULTRA_DMA_16,
        TRANSFER_MODE_ULTRA_DMA_25,
        TRANSFER_MODE_ULTRA_DMA_33,
        TRANSFER_MODE_ULTRA_DMA_44,
        TRANSFER_MODE_ULTRA_DMA_66,
        TRANSFER_MODE_ULTRA_DMA_100,
        TRANSFER_MODE_ULTRA_DMA_133,
        TRANSFER_MODE_SATA_150,
        TRANSFER_MODE_SATA_300,
        TRANSFER_MODE_SATA_600
    };


    [Flags]
    public enum COMMAND_TYPE
    {
        CMD_TYPE_UNKNOWN = 0,
        CMD_TYPE_PHYSICAL_DRIVE,
        CMD_TYPE_SCSI_MINIPORT,
        CMD_TYPE_SILICON_IMAGE,
        CMD_TYPE_SAT,           // SAT = SCSI_ATA_TRANSLATION
        CMD_TYPE_SUNPLUS,
        CMD_TYPE_IO_DATA,
        CMD_TYPE_LOGITEC,
        CMD_TYPE_PROLIFIC,
        CMD_TYPE_JMICRON,
        CMD_TYPE_CYPRESS,
        CMD_TYPE_SAT_ASM1352R,  // AMS1352 2nd drive
        CMD_TYPE_SAT_REALTEK9220DP,
        CMD_TYPE_CSMI,              // CSMI = Common Storage Management Interface
        CMD_TYPE_CSMI_PHYSICAL_DRIVE, // CSMI = Common Storage Management Interface 
        CMD_TYPE_WMI,
        CMD_TYPE_NVME_SAMSUNG,
        CMD_TYPE_NVME_INTEL,
        CMD_TYPE_NVME_STORAGE_QUERY,
        CMD_TYPE_NVME_JMICRON,
        CMD_TYPE_NVME_ASMEDIA,
        CMD_TYPE_NVME_REALTEK,
        CMD_TYPE_NVME_REALTEK9220DP,
        CMD_TYPE_NVME_INTEL_RST,
        CMD_TYPE_NVME_INTEL_VROC,
        CMD_TYPE_MEGARAID,
        CMD_TYPE_AMD_RC2,// +AMD_RC2
        CMD_TYPE_JMS56X,
        CMD_TYPE_JMB39X,
        CMD_TYPE_JMS586_20,
        CMD_TYPE_JMS586_40,
        CMD_TYPE_DEBUG
    };


    public enum IO_CONTROL_CODE
    {
        DFP_SEND_DRIVE_COMMAND = 0x0007C084,
        DFP_RECEIVE_DRIVE_DATA = 0x0007C088,
        IOCTL_SCSI_MINIPORT = 0x0004D008,
        IOCTL_IDE_PASS_THROUGH = 0x0004D028, // 2000 or later
        IOCTL_ATA_PASS_THROUGH = 0x0004D02C, // XP SP2 and 2003 or later
    };


    [Flags]
    public enum LocalMemoryFlags
    {
        LMEM_FIXED = 0x0000,
        LMEM_MOVEABLE = 0x0002,
        LMEM_NOCOMPACT = 0x0010,
        LMEM_NODISCARD = 0x0020,
        LMEM_ZEROINIT = 0x0040,
        LMEM_MODIFY = 0x0080,
        LMEM_DISCARDABLE = 0x0F00,
        LMEM_VALID_FLAGS = 0x0F72,
        LMEM_INVALID_HANDLE = 0x8000,
        LHND = (LMEM_MOVEABLE | LMEM_ZEROINIT),
        LPTR = (LMEM_FIXED | LMEM_ZEROINIT),
        NONZEROLHND = (LMEM_MOVEABLE),
        NONZEROLPTR = (LMEM_FIXED)
    }

    [Flags]
    public enum TStoragePropertyId
    {
        StorageDeviceProperty = 0,
        StorageAdapterProperty,
        StorageDeviceIdProperty,
        StorageDeviceUniqueIdProperty,
        StorageDeviceWriteCacheProperty,
        StorageMiniportProperty,
        StorageAccessAlignmentProperty,
        StorageDeviceSeekPenaltyProperty,
        StorageDeviceTrimProperty,
        StorageDeviceWriteAggregationProperty,
        StorageDeviceDeviceTelemetryProperty,
        StorageDeviceLBProvisioningProperty,
        StorageDevicePowerProperty,
        StorageDeviceCopyOffloadProperty,
        StorageDeviceResiliencyProperty,
        StorageDeviceMediumProductType,
        StorageDeviceRpmbProperty,
        StorageDeviceIoCapabilityProperty = 48,
        StorageAdapterProtocolSpecificProperty,
        StorageDeviceProtocolSpecificProperty,
        StorageAdapterTemperatureProperty,
        StorageDeviceTemperatureProperty,
        StorageAdapterPhysicalTopologyProperty,
        StorageDevicePhysicalTopologyProperty,
        StorageDeviceAttributesProperty,
    }

    [Flags]
    public enum TStorageQueryType
    {
        PropertyStandardQuery = 0,
        PropertyExistsQuery,
        PropertyMaskQuery,
        PropertyQueryMaxDefined
    }

    [Flags]
    public enum TStorageProtocolNVMeDataType
    {
        NVMeDataTypeUnknown = 0,
        NVMeDataTypeIdentify,
        NVMeDataTypeLogPage,
        NVMeDataTypeFeature,
    }

    [Flags]
    public enum TStroageProtocolType
    {
        ProtocolTypeUnknown = 0x00,
        ProtocolTypeScsi,
        ProtocolTypeAta,
        ProtocolTypeNvme,
        ProtocolTypeSd,
        ProtocolTypeProprietary = 0x7E,
        ProtocolTypeMaxReserved = 0x7F
    }

    [Flags]
    public enum MutantAccess : uint
    {
        MUTANT_ALL_ACCESS = (uint)(STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | MUTANT_QUERY_STATE)
    }
}
