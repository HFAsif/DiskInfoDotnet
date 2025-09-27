namespace HelperClass;
using System;

public static class ImportantElements_Properties
{
    public const int PAGE_NOACCESS = 0x01;
    public const int PAGE_READONLY = 0x02;
    public const int PAGE_READWRITE = 0x04;
    public const int PAGE_WRITECOPY = 0x08;
    public const int PAGE_EXECUTE = 0x10;
    public const int PAGE_EXECUTE_READ = 0x20;
    public const int PAGE_EXECUTE_READWRITE = 0x40;
    public const int PAGE_EXECUTE_WRITECOPY = 0x80;
    public const int PAGE_GUARD = 0x100;
    public const int PAGE_NOCACHE = 0x200;
    public const int PAGE_WRITECOMBINE = 0x400;
    public const int PAGE_GRAPHICS_NOACCESS = 0x0800;
    public const int PAGE_GRAPHICS_READONLY = 0x1000;
    public const int PAGE_GRAPHICS_READWRITE = 0x2000;
    public const int PAGE_GRAPHICS_EXECUTE = 0x4000;
    public const int PAGE_GRAPHICS_EXECUTE_READ = 0x8000;
    public const int PAGE_GRAPHICS_EXECUTE_READWRITE = 0x10000;
    public const int PAGE_GRAPHICS_COHERENT = 0x20000;
    public const int PAGE_GRAPHICS_NOCACHE = 0x40000;
    public const uint PAGE_ENCLAVE_THREAD_CONTROL = 0x80000000;
    public const uint PAGE_REVERT_TO_FILE_MAP = 0x80000000;
    public const int PAGE_TARGETS_NO_UPDATE = 0x40000000;
    public const int PAGE_TARGETS_INVALID = 0x40000000;
    public const int PAGE_ENCLAVE_UNVALIDATED = 0x20000000;
    public const int PAGE_ENCLAVE_MASK = 0x10000000;
    public const int PAGE_ENCLAVE_DECOMMIT = (PAGE_ENCLAVE_MASK | 0);
    public const int PAGE_ENCLAVE_SS_FIRST = (PAGE_ENCLAVE_MASK | 1);
    public const int PAGE_ENCLAVE_SS_REST = (PAGE_ENCLAVE_MASK | 2);
    public const int MEM_COMMIT = 0x00001000;
    public const int MEM_RESERVE = 0x00002000;
    public const int MEM_REPLACE_PLACEHOLDER = 0x00004000;
    public const int MEM_RESERVE_PLACEHOLDER = 0x00040000;
    public const int MEM_RESET = 0x00080000;
    public const int MEM_TOP_DOWN = 0x00100000;
    public const int MEM_WRITE_WATCH = 0x00200000;
    public const int MEM_PHYSICAL = 0x00400000;
    public const int MEM_ROTATE = 0x00800000;
    public const int MEM_DIFFERENT_IMAGE_BASE_OK = 0x00800000;
    public const int MEM_RESET_UNDO = 0x01000000;
    public const int MEM_LARGE_PAGES = 0x20000000;
    public const uint MEM_4MB_PAGES = 0x80000000;
    public const int MEM_64K_PAGES = (MEM_LARGE_PAGES | MEM_PHYSICAL);
    public const int MEM_UNMAP_WITH_TRANSIENT_BOOST = 0x00000001;
    public const int MEM_COALESCE_PLACEHOLDERS = 0x00000001;
    public const int MEM_PRESERVE_PLACEHOLDER = 0x00000002;
    public const int MEM_DECOMMIT = 0x00004000;
    public const int MEM_RELEASE = 0x00008000;
    public const int MEM_FREE = 0x00010000;

    public const int READ_ATTRIBUTES = 0xD0;
    public const int READ_THRESHOLDS = 0xD1;
    public const int ENABLE_DISABLE_AUTOSAVE = 0xD2;
    public const int SAVE_ATTRIBUTE_VALUES = 0xD3;
    public const int EXECUTE_OFFLINE_DIAGS = 0xD4;
    public const int SMART_READ_LOG = 0xD5;
    public const int SMART_WRITE_LOG = 0xd6;
    public const int ENABLE_SMART = 0xD8;
    public const int DISABLE_SMART = 0xD9;
    public const int RETURN_SMART_STATUS = 0xDA;
    public const int ENABLE_DISABLE_AUTO_OFFLINE = 0xDB;



    public const int ATAPI_ID_CMD = 0xA1;            // Returns ID sector for ATAPI.
    public const int ID_CMD = 0xEC;            // Returns ID sector for ATA.
    public const int SMART_CMD = 0xB0;            // Performs SMART cmd.

    public const int SMART_CYL_LOW = 0x4F;
    public const int SMART_CYL_HI = 0xC2;

    public const int READ_ATTRIBUTE_BUFFER_SIZE = 512;
    public const int IDENTIFY_BUFFER_SIZE = 512;
    public const int READ_THRESHOLD_BUFFER_SIZE = 512;
    public const int SMART_LOG_SECTOR_SIZE = 512;

    public const int ATA_FLAGS_DRDY_REQUIRED = 0x01;
    public const int ATA_FLAGS_DATA_IN = 0x02;
    public const int ATA_FLAGS_DATA_OUT = 0x04;
    public const int ATA_FLAGS_48BIT_COMMAND = 0x08;


    public const int FILE_DEVICE_BEEP = 0x00000001;
    public const int FILE_DEVICE_CD_ROM = 0x00000002;
    public const int FILE_DEVICE_CD_ROM_FILE_SYSTEM = 0x00000003;
    public const int FILE_DEVICE_CONTROLLER = 0x00000004;
    public const int FILE_DEVICE_DATALINK = 0x00000005;
    public const int FILE_DEVICE_DFS = 0x00000006;
    public const int FILE_DEVICE_DISK = 0x00000007;
    public const int FILE_DEVICE_DISK_FILE_SYSTEM = 0x00000008;
    public const int FILE_DEVICE_FILE_SYSTEM = 0x00000009;
    public const int FILE_DEVICE_INPORT_PORT = 0x0000000a;
    public const int FILE_DEVICE_KEYBOARD = 0x0000000b;
    public const int FILE_DEVICE_MAILSLOT = 0x0000000c;
    public const int FILE_DEVICE_MIDI_IN = 0x0000000d;
    public const int FILE_DEVICE_MIDI_OUT = 0x0000000e;
    public const int FILE_DEVICE_MOUSE = 0x0000000f;
    public const int FILE_DEVICE_MULTI_UNC_PROVIDER = 0x00000010;
    public const int FILE_DEVICE_NAMED_PIPE = 0x00000011;
    public const int FILE_DEVICE_NETWORK = 0x00000012;
    public const int FILE_DEVICE_NETWORK_BROWSER = 0x00000013;
    public const int FILE_DEVICE_NETWORK_FILE_SYSTEM = 0x00000014;
    public const int FILE_DEVICE_NULL = 0x00000015;
    public const int FILE_DEVICE_PARALLEL_PORT = 0x00000016;
    public const int FILE_DEVICE_PHYSICAL_NETCARD = 0x00000017;
    public const int FILE_DEVICE_PRINTER = 0x00000018;
    public const int FILE_DEVICE_SCANNER = 0x00000019;
    public const int FILE_DEVICE_SERIAL_MOUSE_PORT = 0x0000001a;
    public const int FILE_DEVICE_SERIAL_PORT = 0x0000001b;
    public const int FILE_DEVICE_SCREEN = 0x0000001c;
    public const int FILE_DEVICE_SOUND = 0x0000001d;
    public const int FILE_DEVICE_STREAMS = 0x0000001e;
    public const int FILE_DEVICE_TAPE = 0x0000001f;
    public const int FILE_DEVICE_TAPE_FILE_SYSTEM = 0x00000020;
    public const int FILE_DEVICE_TRANSPORT = 0x00000021;
    public const int FILE_DEVICE_UNKNOWN = 0x00000022;
    public const int FILE_DEVICE_VIDEO = 0x00000023;
    public const int FILE_DEVICE_VIRTUAL_DISK = 0x00000024;
    public const int FILE_DEVICE_WAVE_IN = 0x00000025;
    public const int FILE_DEVICE_WAVE_OUT = 0x00000026;
    public const int FILE_DEVICE_8042_PORT = 0x00000027;
    public const int FILE_DEVICE_NETWORK_REDIRECTOR = 0x00000028;
    public const int FILE_DEVICE_BATTERY = 0x00000029;
    public const int FILE_DEVICE_BUS_EXTENDER = 0x0000002a;
    public const int FILE_DEVICE_MODEM = 0x0000002b;
    public const int FILE_DEVICE_VDM = 0x0000002c;
    public const int FILE_DEVICE_MASS_STORAGE = 0x0000002d;
    public const int FILE_DEVICE_SMB = 0x0000002e;
    public const int FILE_DEVICE_KS = 0x0000002f;
    public const int FILE_DEVICE_CHANGER = 0x00000030;
    public const int FILE_DEVICE_SMARTCARD = 0x00000031;
    public const int FILE_DEVICE_ACPI = 0x00000032;
    public const int FILE_DEVICE_DVD = 0x00000033;
    public const int FILE_DEVICE_FULLSCREEN_VIDEO = 0x00000034;
    public const int FILE_DEVICE_DFS_FILE_SYSTEM = 0x00000035;
    public const int FILE_DEVICE_DFS_VOLUME = 0x00000036;
    public const int FILE_DEVICE_SERENUM = 0x00000037;
    public const int FILE_DEVICE_TERMSRV = 0x00000038;
    public const int FILE_DEVICE_KSEC = 0x00000039;
    public const int FILE_DEVICE_FIPS = 0x0000003A;
    public const int FILE_DEVICE_INFINIBAND = 0x0000003B;
    public const int FILE_DEVICE_VMBUS = 0x0000003E;
    public const int FILE_DEVICE_CRYPT_PROVIDER = 0x0000003F;
    public const int FILE_DEVICE_WPD = 0x00000040;
    public const int FILE_DEVICE_BLUETOOTH = 0x00000041;
    public const int FILE_DEVICE_MT_COMPOSITE = 0x00000042;
    public const int FILE_DEVICE_MT_TRANSPORT = 0x00000043;
    public const int FILE_DEVICE_BIOMETRIC = 0x00000044;
    public const int FILE_DEVICE_PMI = 0x00000045;
    public const int FILE_DEVICE_EHSTOR = 0x00000046;
    public const int FILE_DEVICE_DEVAPI = 0x00000047;
    public const int FILE_DEVICE_GPIO = 0x00000048;
    public const int FILE_DEVICE_USBEX = 0x00000049;
    public const int FILE_DEVICE_CONSOLE = 0x00000050;
    public const int FILE_DEVICE_NFP = 0x00000051;
    public const int FILE_DEVICE_SYSENV = 0x00000052;
    public const int FILE_DEVICE_VIRTUAL_BLOCK = 0x00000053;
    public const int FILE_DEVICE_POINT_OF_SERVICE = 0x00000054;
    public const int FILE_DEVICE_STORAGE_REPLICATION = 0x00000055;
    public const int FILE_DEVICE_TRUST_ENV = 0x00000056;
    public const int FILE_DEVICE_UCM = 0x00000057;
    public const int FILE_DEVICE_UCMTCPCI = 0x00000058;
    public const int FILE_DEVICE_PERSISTENT_MEMORY = 0x00000059;
    public const int FILE_DEVICE_NVDIMM = 0x0000005a;
    public const int FILE_DEVICE_HOLOGRAPHIC = 0x0000005b;
    public const int FILE_DEVICE_SDFXHCI = 0x0000005c;
    public const int FILE_DEVICE_UCMUCSI = 0x0000005d;
    public const int FILE_DEVICE_PRM = 0x0000005e;
    public const int FILE_DEVICE_EVENT_COLLECTOR = 0x0000005f;
    public const int FILE_DEVICE_USB4 = 0x00000060;
    public const int FILE_DEVICE_SOUNDWIRE = 0x00000061;

    public const int VER_MINORVERSION = 0x0000001;
    public const int VER_MAJORVERSION = 0x0000002;
    public const int VER_BUILDNUMBER = 0x0000004;
    public const int VER_PLATFORMID = 0x0000008;
    public const int VER_SERVICEPACKMINOR = 0x0000010;
    public const int VER_SERVICEPACKMAJOR = 0x0000020;
    public const int VER_SUITENAME = 0x0000040;
    public const int VER_PRODUCT_TYPE = 0x0000080;

    public const int IOCTL_STORAGE_BASE = FILE_DEVICE_MASS_STORAGE;


    public const int VER_EQUAL = 1;
    public const int VER_GREATER = 2;
    public const int VER_GREATER_EQUAL = 3;
    public const int VER_LESS = 4;
    public const int VER_LESS_EQUAL = 5;
    public const int VER_AND = 6;
    public const int VER_OR = 7;
    public const int VER_CONDITION_MASK = 7;
    public const int VER_NUM_BITS_PER_CONDITION_MASK = 3;


    public const int SECURITY_DESCRIPTOR_REVISION = (1);
    public const long SECURITY_WORLD_RID = (0x00000000L);
    public const int ACL_REVISION = (2);
    public const long STANDARD_RIGHTS_REQUIRED = (0x000F0000L);
    public const long SYNCHRONIZE = (0x00100000L);
    public const int MUTANT_QUERY_STATE = 0x0001;
    public const long READ_CONTROL = (0x00020000L);

    public const int DISKNAME_LENGTH = (40 + 1);
    public const int MODELNAME_LENGTH = (20 + 1);
    public const int SERIALNUMBER_LENGTH = (20 + 1);
    public const int DISK_FIRMWAREVERSION_LENGTH = 9;

    public const long GENERIC_READ = (0x80000000L);
    public const long GENERIC_WRITE = (0x40000000L);
    public const long GENERIC_EXECUTE = (0x20000000L);
    public const long GENERIC_ALL = (0x10000000L);

    public const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
    public const int FILE_SHARE_READ = 0x00000001;
    public const int FILE_SHARE_WRITE = 0x00000002;

    public const int CREATE_NEW = 1;
    public const int CREATE_ALWAYS = 2;
    public const int OPEN_EXISTING = 3;
    public const int OPEN_ALWAYS = 4;
    public const int TRUNCATE_EXISTING = 5;

    public const int METHOD_BUFFERED = 0;
    public const int METHOD_IN_DIRECT = 1;
    public const int METHOD_OUT_DIRECT = 2;
    public const int METHOD_NEITHER = 3;

    public const int FILE_ANY_ACCESS = 0;
    public const int FILE_SPECIAL_ACCESS = FILE_ANY_ACCESS;
    public const int FILE_READ_ACCESS = (0x0001);  // file & pipe
    public const int FILE_WRITE_ACCESS = (0x0002);    // file & pipe

    //public const int FILE_DEVICE_CONTROLLER = 0x00000004;
    public const int IOCTL_SCSI_BASE = FILE_DEVICE_CONTROLLER;

    public static IntPtr INVALID_HANDLE_VALUE = ((IntPtr)(-1));

    public const string NVME_SIG_STR = "NvmeMini";
    public const int NVME_SIG_STR_LEN = 8;
    public const int NVME_FROM_DEV_TO_HOST = 2;
    public const int NVME_IOCTL_VENDOR_SPECIFIC_DW_SIZE = 6;
    public const int NVME_IOCTL_CMD_DW_SIZE = 16;
    public const int NVME_IOCTL_COMPLETE_DW_SIZE = 4;
    public const int NVME_PT_TIMEOUT = 40;

    public const int NVME_STORPORT_DRIVER = 0xE000;

    //
    // Define values for pass-through DataIn field.
    //
    public const int SCSI_IOCTL_DATA_OUT = 0;
    public const int SCSI_IOCTL_DATA_IN = 1;
    public const int SCSI_IOCTL_DATA_UNSPECIFIED = 2;

    public const int FILE_DEVICE_SCSI = 0x0000001b;
    public const int IOCTL_SCSI_MINIPORT_IDENTIFY = ((FILE_DEVICE_SCSI << 16) + 0x0501);
    public const int IOCTL_SCSI_MINIPORT_READ_SMART_ATTRIBS = ((FILE_DEVICE_SCSI << 16) + 0x0502);
    public const int IOCTL_SCSI_MINIPORT_READ_SMART_THRESHOLDS = ((FILE_DEVICE_SCSI << 16) + 0x0503);
    public const int IOCTL_SCSI_MINIPORT_ENABLE_SMART = ((FILE_DEVICE_SCSI << 16) + 0x0504);
    public const int IOCTL_SCSI_MINIPORT_DISABLE_SMART = ((FILE_DEVICE_SCSI << 16) + 0x0505);

    //#define IOCTL_STORAGE_QUERY_PROPERTY                CTL_CODE(IOCTL_STORAGE_BASE, 0x0500, METHOD_BUFFERED, FILE_ANY_ACCESS)

    public const int NVME_ATTRIBUTE = 30;

    public const int MAX_DISK = 80;// FIX
    public const int MAX_ATTRIBUTE = 30; // FIX
    public const int MAX_SEARCH_PHYSICAL_DRIVE = 56;
    public const int MAX_SEARCH_SCSI_PORT = 16;
    public const int MAX_SEARCH_SCSI_TARGET_ID = 8;

    public const int SCSI_MINIPORT_BUFFER_SIZE = 512;


}