//#define UseStrings



namespace DiskInfoDotnet.Library
{
    using static Inm;
    using System.Runtime.InteropServices;
    using static Impp;
    using System;

    class DiskInfoDotnetManager_Structures 
    {
        public static readonly string[] commandTypeString =
        {
            ("un"),
            ("pd"),
            ("sm"),
            ("si"),
            ("sa"),
            ("sp"),
            ("io"),
            ("lo"),
            ("pr"),
            ("jm"),
            ("cy"),
            ("ar"), // ASM1352R
	        ("rr"), // Realtek 9220DP
	        ("cs"),
            ("cp"),
            ("wm"),
            ("ns"), // NVMe Samsung
	        ("ni"), // NVMe Intel
	        ("sq"), // NVMe Storage Query
	        ("nj"), // NVMe JMicron
	        ("na"), // NVMe ASMedia
	        ("nr"), // NVMe Realtek
	        ("nt"), // NVMe Intel RST
	        ("iv"), // NVMe Intel VROC
	        ("mr"), // MegaRAID SAS
	        ("rc"), // +AMD RC2
	        ("j5"), // JMS56X
	        ("j3"), // JMB39X
	        ("j6"), // JMS586_20
	        ("j4"), // JMS586_40
	        ("dg"), // Debug
        };

        public static readonly string[] ssdVendorString =
        {
            (""),
            (""),
            ("mt"), // MTron
	        ("ix"), // Indilinx
	        ("jm"), // JMicron
	        ("il"), // Intel
	        ("sg"), // SAMSUNG
	        ("sf"), // SandForce
	        ("mi"), // Micron
	        ("oz"), // OCZ
	        ("st"), // SEAGATE
	        ("wd"), // WDC
	        ("px"), // PLEXTOR
	        ("sd"), // SanDisk
	        ("oz"), // OCZ Vector
	        ("to"), // TOSHIABA
	        ("co"), // Corsair
	        ("ki"), // Kingston
	        ("m3"), // Micron MU03
	        ("nv"), // NVMe
	        ("re"), // Realtek
	        ("sk"), // SKhynix
	        ("ki"), // KIOXIA
	        ("ss"), // SSSTC
	        ("id"), // Intel DC
	        ("ap"), // Apacer
	        ("sm"), // SiliconMotion
	        ("ph"), // Phison
	        ("ma"), // Marvell
	        ("mk"), // Maxiotek
	        ("ym"), // YMTC
	        ("sc"), // SCY
	        (""),	  // _T("SmartJMicron60x"),
	        (""),	  // _T("SmartJMicron61x"),
	        (""),	  // _T("SmartJMicron66x"),
	        (""),	  // _T("SmartSeagateIronWolf"),
	        (""),	  // _T("SmartSeagateBarraCuda"),
	        (""),	  // _T("SmartSanDiskGb"),
	        (""),	  // _T("SmartKingstonSuv"),
	        (""),	  // _T("SmartKingstonKC600"),
	        (""),	  // _T("SmartKingstonDC500"),
	        (""),	  // _T("SmartKingstonSA400"),
	        ("re"), // RECADATA
	        (""),	  // _T("SmartSanDiskDell"),
	        (""),	  // _T("SmartSanDiskHp"),
	        (""),	  // _T("SmartSanDiskHpVenus"),
	        (""),	  // _T("SmartSanDiskLenovo"),
	        (""),	  // _T("SmartSanDiskLenovoHelenVenus"),
	        (""),	  // _T("SmartSanDiskCloud"),
	        ("mc"), // _T("SmartSiliconMotionCVC"),
	        ("ai"), // _T("SmartAdataIndustrial"),
        };

        public static readonly string[] attributeString =
        {
            ("Smart"),
            ("SmartSsd"),
            ("SmartMtron"),
            ("SmartIndilinx"),
            ("SmartJMicron"),
            ("SmartIntel"),
            ("SmartSamsung"),
            ("SmartSandForce"),
            ("SmartMicron"),
            ("SmartOcz"),
            ("SmartSeagate"),
            ("SmartWdc"),
            ("SmartPlextor"),
            ("SmartSanDisk"),
            ("SmartOczVector"),
            ("SmartToshiba"),
            ("SmartCorsair"),
            ("SmartKingston"),
            ("SmartMicronMU03"),
            ("SmartNVMe"),
            ("SmartRealtek"),
            ("SmartSKhynix"),
            ("SmartKioxia"),
            ("SmartSsstc"),
            ("SmartIntelDc"),
            ("SmartApacer"),
            ("SmartSiliconMotion"),
            ("SmartPhison"),
            ("SmartMarvell"),
            ("SmartMaxiotek"),
            ("SmartYmtc"),
            ("SmartScy"),
            ("SmartJMicron60x"),
            ("SmartJMicron61x"),
            ("SmartJMicron66x"),
            ("SmartSeagateIronWolf"),
            ("SmartSeagateBarraCuda"),
            ("SmartSanDiskGb"),
            ("SmartKingstonSuv"),
            ("SmartKingstonKC600"),
            ("SmartKingstonDC500"),
            ("SmartKingstonSA400"),
            ("SmartRecadata"),
            ("SmartSanDiskDell"),
            ("SmartSanDiskHp"),
            ("SmartSanDiskHpVenus"),
            ("SmartSanDiskLenovo"),
            ("SmartSanDiskLenovoHelenVenus"),
            ("SmartSanDiskCloud"),
            ("SmartSiliconMotionCVC"),
            ("SmartAdataIndustrial"),
        };

        public static readonly string[] deviceFormFactorString =
        {
            (""),
            ("5.25 inch"),
            ("3.5 inch"),
            ("2.5 inch"),
            ("1.8 inch"),
            ("< 1.8 inch")
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CMD_IDE_PATH_THROUGH
        {
            public IDEREGS reg;
            public uint length;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] buffer;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SMART_READ_DATA_OUTDATA
        {
            public SENDCMDOUTPARAMS SendCmdOutParam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = READ_ATTRIBUTE_BUFFER_SIZE - 1)]
            public byte[] Data;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ATA_PASS_THROUGH_EX
        {
            public ushort Length;
            public ushort AtaFlags;
            public byte PathId;
            public byte TargetId;
            public byte Lun;
            public byte ReservedAsUchar;
            public uint DataTransferLength;
            public uint TimeOutValue;
            public uint ReservedAsUlong;
            //	uint   DataBufferOffset;
#if _x64bit_ || _AnyCPU_
            public uint padding;
#endif
            public nuint DataBufferOffset;
            public IDEREGS PreviousTaskFile;
            public IDEREGS CurrentTaskFile;
        }

            
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ATA_PASS_THROUGH_EX_WITH_BUFFERS
        {
            public ATA_PASS_THROUGH_EX Apt;
            public uint Filer;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] Buf;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SMART_ATTRIBUTE_LIST
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTE)]
            public SMART_ATTRIBUTE[] Attributes;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct DRIVERSTATUS
        {
            public byte bDriverError;           // Error code from driver,
                                                // or 0 if no error.
            public byte bIDEError;                      // Contents of IDE Error register.
                                                        // Only valid when bDriverError
                                                        // is SMART_IDE_ERROR.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] bReserved;           // Reserved for future expansion.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public uint[] dwReserved;          // Reserved for future expansion.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IDEREGS
        {
            public byte bFeaturesReg;           // Used for specifying SMART "commands".
            public byte bSectorCountReg;        // IDE sector count register
            public byte bSectorNumberReg;       // IDE sector number register
            public byte bCylLowReg;             // IDE low order cylinder value
            public byte bCylHighReg;            // IDE high order cylinder value
            public byte bDriveHeadReg;          // IDE drive/head register
            public byte bCommandReg;            // Actual IDE command.
            public byte bReserved;                      // reserved for future use.  Must be zero.
        }

               
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SENDCMDINPARAMS
        {
            public uint cBufferSize;            // Buffer size in bytes
            public IDEREGS irDriveRegs;            // Structure with drive register values.
            public byte bDriveNumber;           // Physical drive number to send
                                                // command to (0,1,2,3).
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public byte[] bReserved;           // Reserved for future expansion.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public uint[] dwReserved;          // For future use.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] bBuffer;                     // Input buffer.
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SENDCMDOUTPARAMS
        {
            public uint cBufferSize;            // Size of bBuffer in bytes
            public DRIVERSTATUS DriverStatus;           // Driver status structure.
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public byte[] bBuffer;             // Buffer of arbitrary length in which to store the data read from the                                                                                  // drive.
        }
     
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IDENTIFY_DEVICE_OUTDATA
        {
            public SENDCMDOUTPARAMS SendCmdOutParam;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = IDENTIFY_BUFFER_SIZE - 1)]
            public byte[] Data;
        }

        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        //public struct SMART_ATTRIBUTE
        //{
        //    public byte Id;
        //    public ushort StatusFlags;
        //    public byte CurrentValue;
        //    public byte WorstValue;
        //    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        //    public byte[] RawValue;
        //    public byte Reserved;
        //}

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CSMI_SAS_IDENTIFY
        {
            public byte bDeviceType;
            public byte bRestricted;
            public byte bInitiatorPortProtocol;
            public byte bTargetPortProtocol;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] bRestricted2;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] bSASAddress;
            public byte bPhyIdentifier;
            public byte bSignalClass;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] bReserved;
        }

        //[StructLayout(LayoutKind.Sequential, Pack = 1)]
        //public struct CSMI_SAS_PHY_ENTITY
        //{
        //    public CSMI_SAS_IDENTIFY Identify;
        //    public byte bPortIdentifier;
        //    public byte bNegotiatedLinkRate;
        //    public byte bMinimumLinkRate;
        //    public byte bMaximumLinkRate;
        //    public byte bPhyChangeCount;
        //    public byte bAutoDiscover;
        //    public byte bPhyFeatures;
        //    public byte bReserved;
        //    public CSMI_SAS_IDENTIFY Attached;
        //}


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CSMI_SAS_PHY_ENTITY
        {
            public CSMI_SAS_IDENTIFY Identify;
            public byte bPortIdentifier;
            public byte bNegotiatedLinkRate;
            public byte bMinimumLinkRate;
            public byte bMaximumLinkRate;
            public byte bPhyChangeCount;
            public byte bAutoDiscover;
            public byte bPhyFeatures;
            public byte bReserved;
            public CSMI_SAS_IDENTIFY Attached;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SMART_THRESHOLD
        {
            public byte Id;
            public byte ThresholdValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public byte[] Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SMART_ATTRIBUTE
        {
            public byte Id;
            public ushort StatusFlags;
            public byte CurrentValue;
            public byte WorstValue;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public byte[] RawValue;
            public byte Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ATA_SMART_INFO
        {
            public IDENTIFY_DEVICE IdentifyDevice;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] SmartReadData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
            public byte[] SmartReadThreshold;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTE)]
            public SMART_ATTRIBUTE[] Attribute;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_ATTRIBUTE)]
            public SMART_THRESHOLD[] Threshold;

            public bool IsSmartEnabled;
            public bool IsIdInfoIncorrect;
            public bool IsSmartCorrect;
            public bool IsThresholdCorrect;
            public bool IsCheckSumError;
            public bool IsWord88;
            public bool IsWord64_76;
            public bool IsRawValues8;
            public bool IsRawValues7;
            public bool Is9126MB;
            public bool IsThresholdBug;
            public bool IsSmartSupported;
            public bool IsLba48Supported;
            public bool IsAamSupported;
            public bool IsApmSupported;
            public bool IsAamEnabled;
            public bool IsApmEnabled;
            public bool IsNcqSupported;
            public bool IsNvCacheSupported;
            public bool IsNvmeThresholdSupported;
            public bool IsNvmeThermalManagementSupported;
            public bool IsDeviceSleepSupported;
            public bool IsStreamingSupported;
            public bool IsGplSupported;
            public bool IsMaxtorMinute;
            public bool IsSsd;
            public bool IsTrimSupported;
            public bool IsVolatileWriteCachePresent;
            public bool IsNVMe;
            public bool IsUasp;
            public int PhysicalDriveId;
            public int ScsiPort;
            public int ScsiTargetId;
            public int ScsiBus;
            public int SiliconImageType;
            //		int					   AccessType;
            public uint TotalDiskSize;
            public uint Cylinder;
            public uint Head;
            public uint Sector;
            public uint Sector28;
            public ulong Sector48;
            public ulong NumberOfSectors;
            public uint DiskSizeChs;
            public uint DiskSizeLba28;
            public uint DiskSizeLba48;
            public uint LogicalSectorSize;
            public uint PhysicalSectorSize;
            public uint DiskSizeWmi;
            public uint BufferSize;
            public ulong NvCacheSize;
            public uint TransferModeType;
            public uint DetectedTimeUnitType;
            public uint MeasuredTimeUnitType;
            public uint AttributeCount;
            public int DetectedPowerOnHours;
            public int MeasuredPowerOnHours;
            public int PowerOnRawValue;
            public int PowerOnStartRawValue;
            public uint PowerOnCount;
            public int Temperature;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public int[] TemperatureNVMe;
            public double TemperatureMultiplier;
            public uint NominalMediaRotationRate;
            //		double				   Speed;
            public int HostWrites;
            public int HostReads;
            public int GBytesErased;
            public int NandWrites;
            public int WearLevelingCount;

            //		int					   PlextorNandWritesUnit;

            public int Life;
            public bool FlagLifeNoReport;
            public bool FlagLifeRawValue;
            public bool FlagLifeRawValueIncrement;
            public bool FlagLifeSanDiskUsbMemory;
            public bool FlagLifeSanDisk0_1;
            public bool FlagLifeSanDisk1;
            public bool FlagLifeSanDiskLenovo;
            public bool FlagLifeSanDiskCloud;

            public uint Major;
            public uint Minor;

            public uint DiskStatus;
            public uint DriveLetterMap;

            public int AlarmTemperature;
            public bool AlarmHealthStatus;

            public INTERFACE_TYPE InterfaceType;
            public COMMAND_TYPE CommandType;
            public HOST_READS_WRITES_UNIT HostReadsWritesUnit;

            public uint DiskVendorId;
            public uint UsbVendorId;
            public uint UsbProductId;
            public byte Target;

            public ushort Threshold05;
            public ushort ThresholdC5;
            public ushort ThresholdC6;
            public ushort ThresholdFF;

            public CSMI_SAS_PHY_ENTITY sasPhyEntity;

            public string SerialNumber;
            public string SerialNumberReverse;
            public string FirmwareRev;
            public string FirmwareRevReverse;
            public string Model;
            public string ModelReverse;
            public string ModelWmi;
            public string ModelSerial;
            public string DriveMap;
            public string MaxTransferMode;
            public string CurrentTransferMode;
            public string MajorVersion;
            public string MinorVersion;
            public string Interface;
            public string Enclosure;
            public string CommandTypeString;
            public string SsdVendorString;
            public string DeviceNominalFormFactor;
            public string PnpDeviceId;
            public string SmartKeyName;
        };


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NVME_ID
        {
            public ushort PCIeVID;
            public ushort PCIeSubSysVID;
            public SByte ControllerMultIO;
            public SByte MaxTransferSize;
            public ushort ControllerID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string FirmwareRevision;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string FGUID;
            public ushort WarnTemperatureThreshold;
            public ushort CriticalTemperatureThreshold;
            public ushort MinThermalTemperature;
            public ushort MaxThermalTemperature;
            public uint NumOFNamespace;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3)]
            public string IeeeOuiID;
        }


        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NVME_PORT_20
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
            public string ModelName;                                  /* Model name of disk		*/
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SERIALNUMBER_LENGTH)]
            public string SerialNumber;              /* Serial number of disk	*/
            public uint SectorSize;                                        /* 512 bytes or 4K          */
            public uint Capacity;                                      /* Disk capacity        	*/
            public uint CapacityOffset;                                    /* Disk capacity        	*/
            public byte DeviceState;                                    /* Device State				*/
            public byte RaidIndex;                                      /* RAID Index				*/
            public byte MemberIndex;                                    /* Member Index				*/
            public byte PortType;                                       /* Port Type				*/
            public byte PCIeSpeed;                                      /* PCIe Speed				*/
            public byte PCIeLANE;                                       /* PCIe LANE                */
            public ushort PortErrorStatus;                               /* NVMe port error status	*/
            public byte DiskType;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NVME_PORT_40
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
            public string ModelName;                                  /* Model name of disk		*/
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SERIALNUMBER_LENGTH)]
            public string SerialNumber;              /* Serial number of disk	*/
            public uint SectorSize;                                        /* 512 bytes or 4K          */
            public uint Capacity;                                      /* Disk capacity        	*/
            public uint CapacityOffset;                                    /* Disk capacity        	*/
            public byte DeviceState;                                    /* Device State				*/
            public byte RaidIndex;                                      /* RAID Index				*/
            public byte MemberIndex;                                    /* Member Index				*/
            public byte PortType;                                       /* Port Type				*/
            public byte PCIeSpeed;                                      /* PCIe Speed				*/
            public byte PCIeLANE;                                       /* PCIe LANE                */
            public ushort PortErrorStatus;                               /* NVMe port error status	*/
            public byte DiskType;
        }

#if UseStrings
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct ATA_IDENTIFY_DEVICE
        {
            public ushort GeneralConfiguration;                  //0
            public ushort LogicalCylinders;                      //1	Obsolete
            public ushort SpecificConfiguration;                 //2
            public ushort LogicalHeads;                          //3 Obsolete
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] Retired1;                           //4-5
            public ushort LogicalSectors;                            //6 Obsolete
            public uint ReservedForCompactFlash;              //7-8
            public ushort Retired2;                              //9
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public sbyte[] SerialNumber;                      //10-19
            public ushort Retired3;                              //20
            public ushort BufferSize;                                //21 Obsolete
            public ushort Obsolute4;                             //22
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public sbyte[] FirmwareRev;                            //23-26
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public sbyte[] Model;                             //27-46
            public ushort MaxNumPerInterupt;                     //47
            public ushort Reserved1;                             //48
            public ushort Capabilities1;                         //49
            public ushort Capabilities2;                         //50
            public uint Obsolute5;                                //51-52
            public ushort Field88and7064;                            //53
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ushort[] Obsolute6;                          //54-58
            public ushort MultSectorStuff;                       //59
            public uint TotalAddressableSectors;              //60-61
            public ushort Obsolute7;                             //62
            public ushort MultiWordDma;                          //63
            public ushort PioMode;                               //64
            public ushort MinMultiwordDmaCycleTime;              //65
            public ushort RecommendedMultiwordDmaCycleTime;      //66
            public ushort MinPioCycleTimewoFlowCtrl;             //67
            public ushort MinPioCycleTimeWithFlowCtrl;           //68
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public ushort[] Reserved2;                          //69-74
            public ushort QueueDepth;                                //75
            public ushort SerialAtaCapabilities;                 //76
            public ushort SerialAtaAdditionalCapabilities;       //77
            public ushort SerialAtaFeaturesSupported;                //78
            public ushort SerialAtaFeaturesEnabled;              //79
            public ushort MajorVersion;                          //80
            public ushort MinorVersion;                          //81
            public ushort CommandSetSupported1;                  //82
            public ushort CommandSetSupported2;                  //83
            public ushort CommandSetSupported3;                  //84
            public ushort CommandSetEnabled1;                        //85
            public ushort CommandSetEnabled2;                        //86
            public ushort CommandSetDefault;                     //87
            public ushort UltraDmaMode;                          //88
            public ushort TimeReqForSecurityErase;               //89
            public ushort TimeReqForEnhancedSecure;              //90
            public ushort CurrentPowerManagement;                    //91
            public ushort MasterPasswordRevision;                    //92
            public ushort HardwareResetResult;                   //93
            public ushort AcoustricManagement;                   //94
            public ushort StreamMinRequestSize;                  //95
            public ushort StreamingTimeDma;                      //96
            public ushort StreamingAccessLatency;                    //97
            public uint StreamingPerformance;                 //98-99

            public ulong MaxUserLba;                               //100-103

            public ushort StremingTimePio;                       //104
            public ushort Reserved3;                             //105
            public ushort SectorSize;                                //106
            public ushort InterSeekDelay;                            //107
            public ushort IeeeOui;                               //108
            public ushort UniqueId3;                             //109
            public ushort UniqueId2;                             //110
            public ushort UniqueId1;                             //111
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] Reserved4;                          //112-115
            public ushort Reserved5;                             //116
            public uint WordsPerLogicalSector;                    //117-118
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Reserved6;                          //119-126
            public ushort RemovableMediaStatus;                  //127
            public ushort SecurityStatus;                            //128
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            public ushort[] VendorSpecific;                        //129-159
            public ushort CfaPowerMode1;                         //160
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public ushort[] ReservedForCompactFlashAssociation; //161-167
            public ushort DeviceNominalFormFactor;               //168
            public ushort DataSetManagement;                     //169
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] AdditionalProductIdentifier;            //170-173
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] Reserved7;                          //174-175
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public sbyte[] CurrentMediaSerialNo;              //176-205
            public ushort SctCommandTransport;                   //206
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] ReservedForCeAta1;                  //207-208
            public ushort AlignmentOfLogicalBlocks;              //209
            public uint WriteReadVerifySectorCountMode3;      //210-211
            public uint WriteReadVerifySectorCountMode2;      //212-213
            public ushort NvCacheCapabilities;                   //214
            public uint NvCacheSizeLogicalBlocks;             //215-216
            public ushort NominalMediaRotationRate;              //217
            public ushort Reserved8;                             //218
            public ushort NvCacheOptions1;                       //219
            public ushort NvCacheOptions2;                       //220
            public ushort Reserved9;                             //221
            public ushort TransportMajorVersionNumber;           //222
            public ushort TransportMinorVersionNumber;           //223
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] ReservedForCeAta2;                 //224-233
            public ushort MinimumBlocksPerDownloadMicrocode;     //234
            public ushort MaximumBlocksPerDownloadMicrocode;     //235
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public ushort[] Reserved10;                            //236-254
            public ushort IntegrityWord;                         //255
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct BIN_IDENTIFY_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
            public byte[] Bin;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        public struct NVME_IDENTIFY_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public sbyte[] Reserved1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public sbyte[] SerialNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public sbyte[] Model;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public sbyte[] FirmwareRev;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
            public sbyte[] Reserved2;
            public sbyte MinorVersion;
            public short MajorVersion;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 428)]
            public sbyte[] Reserved3;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3584)]
            public sbyte[] Reserved4;
        };
#else
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ATA_IDENTIFY_DEVICE
        {
            public ushort GeneralConfiguration;                  //0
            public ushort LogicalCylinders;                      //1	Obsolete
            public ushort SpecificConfiguration;                 //2
            public ushort LogicalHeads;                          //3 Obsolete
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] Retired1;                           //4-5
            public ushort LogicalSectors;                            //6 Obsolete
            public uint ReservedForCompactFlash;              //7-8
            public ushort Retired2;                              //9
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] SerialNumber;                      //10-19
            public ushort Retired3;                              //20
            public ushort BufferSize;                                //21 Obsolete
            public ushort Obsolute4;                             //22
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] FirmwareRev;                            //23-26
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public byte[] Model;                             //27-46
            public ushort MaxNumPerInterupt;                     //47
            public ushort Reserved1;                             //48
            public ushort Capabilities1;                         //49
            public ushort Capabilities2;                         //50
            public uint Obsolute5;                                //51-52
            public ushort Field88and7064;                            //53
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public ushort[] Obsolute6;                          //54-58
            public ushort MultSectorStuff;                       //59
            public uint TotalAddressableSectors;              //60-61
            public ushort Obsolute7;                             //62
            public ushort MultiWordDma;                          //63
            public ushort PioMode;                               //64
            public ushort MinMultiwordDmaCycleTime;              //65
            public ushort RecommendedMultiwordDmaCycleTime;      //66
            public ushort MinPioCycleTimewoFlowCtrl;             //67
            public ushort MinPioCycleTimeWithFlowCtrl;           //68
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public ushort[] Reserved2;                          //69-74
            public ushort QueueDepth;                                //75
            public ushort SerialAtaCapabilities;                 //76
            public ushort SerialAtaAdditionalCapabilities;       //77
            public ushort SerialAtaFeaturesSupported;                //78
            public ushort SerialAtaFeaturesEnabled;              //79
            public ushort MajorVersion;                          //80
            public ushort MinorVersion;                          //81
            public ushort CommandSetSupported1;                  //82
            public ushort CommandSetSupported2;                  //83
            public ushort CommandSetSupported3;                  //84
            public ushort CommandSetEnabled1;                        //85
            public ushort CommandSetEnabled2;                        //86
            public ushort CommandSetDefault;                     //87
            public ushort UltraDmaMode;                          //88
            public ushort TimeReqForSecurityErase;               //89
            public ushort TimeReqForEnhancedSecure;              //90
            public ushort CurrentPowerManagement;                    //91
            public ushort MasterPasswordRevision;                    //92
            public ushort HardwareResetResult;                   //93
            public ushort AcoustricManagement;                   //94
            public ushort StreamMinRequestSize;                  //95
            public ushort StreamingTimeDma;                      //96
            public ushort StreamingAccessLatency;                    //97
            public uint StreamingPerformance;                 //98-99

            public ulong MaxUserLba;                               //100-103

            public ushort StremingTimePio;                       //104
            public ushort Reserved3;                             //105
            public ushort SectorSize;                                //106
            public ushort InterSeekDelay;                            //107
            public ushort IeeeOui;                               //108
            public ushort UniqueId3;                             //109
            public ushort UniqueId2;                             //110
            public ushort UniqueId1;                             //111
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] Reserved4;                          //112-115
            public ushort Reserved5;                             //116
            public uint WordsPerLogicalSector;                    //117-118
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public ushort[] Reserved6;                          //119-126
            public ushort RemovableMediaStatus;                  //127
            public ushort SecurityStatus;                            //128
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 31)]
            public ushort[] VendorSpecific;                        //129-159
            public ushort CfaPowerMode1;                         //160
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
            public ushort[] ReservedForCompactFlashAssociation; //161-167
            public ushort DeviceNominalFormFactor;               //168
            public ushort DataSetManagement;                     //169
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public ushort[] AdditionalProductIdentifier;            //170-173
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] Reserved7;                          //174-175
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] CurrentMediaSerialNo;              //176-205
            public ushort SctCommandTransport;                   //206
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] ReservedForCeAta1;                  //207-208
            public ushort AlignmentOfLogicalBlocks;              //209
            public uint WriteReadVerifySectorCountMode3;      //210-211
            public uint WriteReadVerifySectorCountMode2;      //212-213
            public ushort NvCacheCapabilities;                   //214
            public uint NvCacheSizeLogicalBlocks;             //215-216
            public ushort NominalMediaRotationRate;              //217
            public ushort Reserved8;                             //218
            public ushort NvCacheOptions1;                       //219
            public ushort NvCacheOptions2;                       //220
            public ushort Reserved9;                             //221
            public ushort TransportMajorVersionNumber;           //222
            public ushort TransportMinorVersionNumber;           //223
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public ushort[] ReservedForCeAta2;                 //224-233
            public ushort MinimumBlocksPerDownloadMicrocode;     //234
            public ushort MaximumBlocksPerDownloadMicrocode;     //235
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public ushort[] Reserved10;                            //236-254
            public ushort IntegrityWord;                         //255
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BIN_IDENTIFY_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)]
            public byte[] Bin;
        };

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct NVME_IDENTIFY_DEVICE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
            public string Reserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
            public string SerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
            public string Model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
            public string FirmwareRev;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
            public string Reserved2;
            public sbyte MinorVersion;
            public short MajorVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 428)]
            public string Reserved3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 3584)]
            public string Reserved4;
        };
#endif
        [StructLayout(LayoutKind.Sequential)]
        public struct IDENTIFY_DEVICE // definite assignment error in primary constructor
        {
            public ATA_IDENTIFY_DEVICE A;
            public NVME_IDENTIFY_DEVICE N;
            public BIN_IDENTIFY_DEVICE B;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TStorageProtocolSpecificData
        {
            public TStroageProtocolType ProtocolType;
            public uint DataType;
            public uint ProtocolDataRequestValue;
            public uint ProtocolDataRequestSubValue;
            public uint ProtocolDataOffset;
            public uint ProtocolDataLength;
            public uint FixedProtocolReturnData;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public uint[] Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TStoragePropertyQuery
        {
            public TStoragePropertyId PropertyId;
            public TStorageQueryType QueryType;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct TStorageQueryWithBuffer
        {
            public TStoragePropertyQuery Query;
            public TStorageProtocolSpecificData ProtocolSpecific;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4096)] // Placeholder for actual size
            public byte[] Buffer;
        }
    }
}
