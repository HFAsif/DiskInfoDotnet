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
using System.Text;

internal partial class DiskInfosWorker
{
    private bool GetSmartAttributeSi(int physicalDriveId, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool ControlSmartStatusScsi(int scsiPort, int scsiTargetId, int eNABLE_SMART)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdScsi(int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool ControlSmartStatusMegaRAID(int scsiPort, int scsiTargetId, int eNABLE_SMART)
    {
        throw new NotImplementedException();
    }

    private bool ControlSmartStatusCsmi(int scsiPort, CSMI_SAS_PHY_ENTITY? sasPhyEntity, int eNABLE_SMART)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartAttributeCsmi(int scsiPort, CSMI_SAS_PHY_ENTITY? sasPhyEntity, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdCsmi(int scsiPort, CSMI_SAS_PHY_ENTITY? sasPhyEntity, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool ControlSmartStatusSat(int physicalDriveId, byte target, int eNABLE_SMART, COMMAND_TYPE commandType)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartAttributeScsi(int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdSat(int physicalDriveId, byte target, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdWmi(ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartAttributeWmi(ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartAttributeMegaRAID(int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asiCheck)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdMegaRAID(int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartThresholdAMD_RC2(int scsiBus, ref ATA_SMART_INFO asiCheck)
    {
        throw new NotImplementedException();
    }

    private bool GetSmartInfoJMS56X(int scsiBus, int scsiPort, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    private bool GetSmartInfoJMS586_40(int scsiBus, int scsiPort, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartInfoJMB39X(int index, byte port, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool ControlSmartStatusPd(int physicalDriveId, byte target, byte command)
    {
        return true;
    }

    bool GetSmartInfoJMS586_20(int index, byte port, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeSat(int PhysicalDriveId, byte target, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool CheckSmartAttributeCorrect(ref ATA_SMART_INFO asi1, ref ATA_SMART_INFO asi2)
    {
        if (asi1.AttributeCount != asi2.AttributeCount)
        {
            Logs.MyLogs(("asi1.AttributeCount != asi2.AttributeCount"));
            return false;
        }

        for (uint i = 0; i < asi1.AttributeCount; i++)
        {
            if (asi1.Attribute[i].Id != asi2.Attribute[i].Id)
            {
                Logs.MyLogs(("asi1.Attribute[i].Id != asi2.Attribute[i].Id"));
                return false;
            }
        }

        return true; // Correct
    }



    bool IsSsdOld(ref ATA_SMART_INFO asi)
    {
        return asi.Model.IndexOf(("OCZ")) == 0
            || asi.Model.IndexOf(("SPCC")) == 0
            || asi.Model.IndexOf(("PATRIOT")) == 0
            || asi.Model.IndexOf(("Solid")) >= 0
            || asi.Model.IndexOf(("SSD")) >= 0
            || asi.Model.IndexOf(("SiliconHardDisk")) >= 0
            || asi.Model.IndexOf(("PHOTOFAST")) == 0
            || asi.Model.IndexOf(("STT_FTM")) == 0
            || asi.Model.IndexOf(("Super Talent")) == 0
            ;
    }

    bool IsSsdAdataIndustrial(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (modelUpper.IndexOf(("ADATA_IM2S")) == 0
        || modelUpper.IndexOf(("ADATA_IMSS")) == 0
        || modelUpper.IndexOf(("ADATA_ISSS")) == 0
        || modelUpper.IndexOf(("IM2S")) == 0
        || modelUpper.IndexOf(("IMSS")) == 0
        || modelUpper.IndexOf(("ISSS")) == 0
        )
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
        }

        return flagSmartType;
    }

    //bool IsSsdSanDisk(ref ATA_SMART_INFO asi)
    //{
    //    bool flagSmartType = false;

    //    // 2013/10/7
    //    // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1425;id=diskinfo#1425
    //    // 2020/07/25
    //    // 
    //    if (asi.Model.IndexOf(("SanDisk")) >= 0 || asi.Model.IndexOf(("SD Ultra")) >= 0 || asi.Model.IndexOf(("SDLF1")) >= 0)
    //    {
    //        asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK; // Default Vendor ID for SanDisk
    //        flagSmartType = true;

    //        if (
    //              (asi.Model.IndexOf(("X600")) >= 0 && asi.Model.IndexOf(("2280")) >= 0) // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2123;id=#2123
    //            || asi.Model.IndexOf(("X400")) >= 0
    //            || asi.Model.IndexOf(("X300")) >= 0
    //            || asi.Model.IndexOf(("X110")) >= 0
    //            || asi.Model.IndexOf(("SD5")) >= 0
    //            )
    //        {
    //            if (asi.Attribute[2].Id == 0xAF || asi.Attribute[3].Id == 0xAF)
    //            {
    //                asi.SmartKeyName = ("SmartSanDiskDell");
    //            }
    //            else
    //            {
    //                asi.SmartKeyName = ("SmartSanDiskGb");
    //            }
    //            asi.FlagLifeSanDisk1 = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;

    //        }
    //        else if (asi.Model.IndexOf(("Z400")) >= 0)
    //        {
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //            asi.SmartKeyName = ("SmartSanDiskDell");
    //        }
    //        // 2022/04/24
    //        // https://osdn.net/projects/crystaldiskinfo/ticket/44354
    //        else if (asi.Model.IndexOf(("1006")) > 0) // HP OEM
    //        {
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB;
    //            if (asi.Model.IndexOf(("8U")) > 0)
    //            {
    //                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS;
    //                asi.SmartKeyName = ("SmartSanDiskHpVenus");
    //            }
    //            else
    //            {
    //                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP;
    //                asi.SmartKeyName = ("SmartSanDiskHp");
    //            }
    //        }
    //        else if (asi.Model.IndexOf(("G1001")) >= 0) // Lenovo OEM
    //        {
    //            asi.FlagLifeSanDiskLenovo = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //            if (asi.Model.IndexOf(("6S")) > 0 || asi.Model.IndexOf(("7S")) > 0 || asi.Model.IndexOf(("8U")) > 0)
    //            {
    //                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS;
    //                asi.SmartKeyName = ("SmartSanDiskLenovoHelenVenus");
    //            }
    //            else
    //            {
    //                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO;
    //                asi.SmartKeyName = ("SmartSanDiskLenovo");
    //            }
    //        }
    //        else if (asi.Model.IndexOf(("G1012")) >= 0 || asi.Model.IndexOf(("Z400s 2.5")) >= 0) // DELL OEM
    //        {
    //            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //            asi.SmartKeyName = ("SmartSanDiskDell");
    //        }
    //        else if (asi.Model.IndexOf(("SSD P4")) >= 0)
    //        {
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
    //            asi.FlagLifeSanDiskUsbMemory = true; // No Life Report
    //            asi.SmartKeyName = ("SmartSanDisk");
    //        }
    //        else if (asi.Model.IndexOf(("iSSD P4")) >= 0)
    //        {
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
    //            asi.SmartKeyName = ("SmartSanDiskGb");
    //        }
    //        else if (
    //           asi.Model.IndexOf(("SDSSDP")) >= 0
    //        || asi.Model.IndexOf(("SDSSDRC")) >= 0
    //        )
    //        {
    //            asi.FlagLifeSanDisk0_1 = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
    //            asi.SmartKeyName = ("SmartSanDisk");
    //        }
    //        else if (
    //           asi.Model.IndexOf(("SSD U100")) >= 0
    //        || asi.Model.IndexOf(("SSD U110")) >= 0
    //        || asi.Model.IndexOf(("SSD i100")) >= 0
    //        || asi.Model.IndexOf(("SSD i110")) >= 0
    //        || asi.Model.IndexOf(("pSSD")) >= 0
    //            )
    //        {
    //            asi.FlagLifeSanDiskUsbMemory = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
    //            asi.SmartKeyName = ("SmartSanDisk");
    //        }
    //        else if (
    //            // CloudSpeed ECO Gen II Eco SSD
    //            asi.Model.IndexOf(("SDLF1CRR-")) >= 0
    //            || asi.Model.IndexOf(("SDLF1DAR-")) >= 0
    //            // CloudSpeed ECO Gen II Ultra SSD
    //            || asi.Model.IndexOf(("SDLF1CRM-")) >= 0
    //            || asi.Model.IndexOf(("SDLF1DAM-")) >= 0
    //            )
    //        {
    //            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD;
    //            asi.FlagLifeSanDiskCloud = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //            asi.SmartKeyName = ("SmartSanDiskCloud");
    //        }
    //        else
    //        {
    //            asi.FlagLifeSanDisk1 = true;
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //            asi.SmartKeyName = ("SmartSanDiskGb");
    //        }
    //    }

    //    return flagSmartType;
    //}

    bool IsSsdWdc(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Model.IndexOf(("WDC ")) == 0 || asi.Model.IndexOf(("WD ")) == 0)
        {
            flagSmartType = true;

            if (asi.Model.IndexOf("SA530") >= 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB;
            }
            else
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
        }

        return flagSmartType;
    }

    bool IsSsdSeagate(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0x64
            && asi.Attribute[5].Id == 0x66
            && asi.Attribute[6].Id == 0x67
            && asi.Attribute[7].Id == 0xAA
            && asi.Attribute[8].Id == 0xAB
            && asi.Attribute[9].Id == 0xAC
            && asi.Attribute[10].Id == 0xAD
            && asi.Attribute[11].Id == 0xAE
            && asi.Attribute[12].Id == 0xB1
            && asi.Attribute[13].Id == 0xB7
            && asi.Attribute[14].Id == 0xBB
            )
        {
            flagSmartType = true;
            asi.SmartKeyName = ("SmartSeagateIronWolf");
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SEAGATE;
        }
        else if (asi.Model.IndexOf("Seagate") == 0
            || (asi.Model.IndexOf("STT") != 0 && asi.Model.IndexOf("ST") == 0)
            || (asi.Model.IndexOf("ZA") == 0)
            )
        {
            flagSmartType = true;
            if (asi.Model.IndexOf("BarraCuda") >= 0)
            {
                asi.SmartKeyName = ("SmartSeagateBarraCuda");
                asi.FlagLifeRawValue = true;
            }
            else if (asi.Model.IndexOf("HM") >= 0 || asi.Model.IndexOf("FP") >= 0)
            {
                asi.SmartKeyName = ("SmartSeagate");
                asi.FlagLifeRawValue = false;
            }
            else
            {
                asi.SmartKeyName = ("SmartSeagate");
                asi.FlagLifeRawValue = true;
            }
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SEAGATE;
        }

        return flagSmartType;
    }

    bool IsSsdMarvell(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA1
            && asi.Attribute[4].Id == 0xA4
            && asi.Attribute[5].Id == 0xA5
            && asi.Attribute[6].Id == 0xA6
            && asi.Attribute[7].Id == 0xA7
            )
        {
            flagSmartType = true;
        }
        else if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA4
            && asi.Attribute[4].Id == 0xA5
            && asi.Attribute[5].Id == 0xA6
            && asi.Attribute[6].Id == 0xA7
            )
        {
            flagSmartType = true;
        }

        if (flagSmartType)
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }

        // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2476;id=#2476
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (modelUpper.IndexOf("LEXAR") == 0 && asi.FirmwareRev.IndexOf("SN") == 0)
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
        }

        // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2523;id=#2523
        if ((modelUpper.IndexOf("HANYE-Q55") == 0))
        {
            return false;
        }

        return flagSmartType;
    }

    bool IsSsdMaxiotek(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (modelUpper.IndexOf(("MAXIO")) == 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }
        else if (modelUpper.IndexOf(("CUSO C5S-EVO")) == 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
        }
        // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2523;id=#2523
        else if (modelUpper.IndexOf("HANYE-Q55") == 0
            && asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA4
            && asi.Attribute[4].Id == 0xA5
            && asi.Attribute[5].Id == 0xA6
            && asi.Attribute[6].Id == 0xA7
            )
        {
            flagSmartType = true;
        }
        else if (
               asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA7
            && asi.Attribute[4].Id == 0xA8
            && asi.Attribute[5].Id == 0xA9
            )
        {
            flagSmartType = true;
        }

        return flagSmartType;
    }

    //bool IsSsdAdataIndustrial(ref ATA_SMART_INFO asi)
    //{
    //    bool flagSmartType = false;

    //    string modelUpper = asi.Model;
    //    modelUpper = modelUpper.ToUpper();

    //    if (modelUpper.IndexOf(("ADATA_IM2S")) == 0
    //    || modelUpper.IndexOf(("ADATA_IMSS")) == 0
    //    || modelUpper.IndexOf(("ADATA_ISSS")) == 0
    //    || modelUpper.IndexOf(("IM2S")) == 0
    //    || modelUpper.IndexOf(("IMSS")) == 0
    //    || modelUpper.IndexOf(("ISSS")) == 0
    //    )
    //    {
    //        flagSmartType = true;
    //        asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
    //    }

    //    return flagSmartType;
    //}


    bool IsSsdGeneral(ref ATA_SMART_INFO asi)
    {
        asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_UNKNOWN;

        if (
            asi.Model.IndexOf("ADATA SP580") == 0
        )
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
        }
        else if (
           asi.Model.IndexOf("LITEON IT LMT") == 0
        )
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
        }
        else if (
           asi.Model.IndexOf("LITEON S960") == 0
        )
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }

        return asi.IsSsd;
    }

    bool IsSsdMtron(ref ATA_SMART_INFO asi)
    {
        return ((asi.Attribute[0].Id == 0xBB && asi.AttributeCount == 1) || asi.Model.IndexOf(("MTRON")) == 0);
    }

    bool IsSsdToshiba(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (modelUpper.IndexOf(("TOSHIBA")) >= 0 && asi.IsSsd)
        {
            flagSmartType = true;
            if (asi.Model.IndexOf(("THNSNC")) >= 0 || asi.Model.IndexOf(("THNSNJ")) >= 0 || asi.Model.IndexOf(("THNSNK")) >= 0
            || asi.Model.IndexOf(("KSG60")) >= 0
            || asi.Model.IndexOf(("TL100")) >= 0 || asi.Model.IndexOf(("TR150")) >= 0 || asi.Model.IndexOf(("TR200")) >= 0
            )
            {
                // TOSHIBA HG3
                // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1106;id=diskinfo#1106
                // TOSHIBA KSG60ZMV
                // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2425;id=#2425
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
            }
            else
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
        }

        return flagSmartType;
    }

    bool IsSsdJMicron61x(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x02
        && asi.Attribute[2].Id == 0x03
        && asi.Attribute[3].Id == 0x05
        && asi.Attribute[4].Id == 0x07
        && asi.Attribute[5].Id == 0x08
        && asi.Attribute[6].Id == 0x09
        && asi.Attribute[7].Id == 0x0A
        && asi.Attribute[8].Id == 0x0C
        && asi.Attribute[9].Id == 0xA8
        && asi.Attribute[10].Id == 0xAF
        && asi.Attribute[11].Id == 0xC0
        && asi.Attribute[12].Id == 0xC2
        //	&& asi.Attribute[13].Id == 0xF0
        //	&& asi.Attribute[14].Id == 0xAA
        //	&& asi.Attribute[15].Id == 0xAD
        )
        {
            flagSmartType = true;
        }

        return flagSmartType;
    }

    bool IsSsdJMicron66x(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x02
        && asi.Attribute[2].Id == 0x03
        && asi.Attribute[3].Id == 0x05
        && asi.Attribute[4].Id == 0x07
        && asi.Attribute[5].Id == 0x08
        && asi.Attribute[6].Id == 0x09
        && asi.Attribute[7].Id == 0x0A
        && asi.Attribute[8].Id == 0x0C
        && asi.Attribute[9].Id == 0xA7
        && asi.Attribute[10].Id == 0xA8
        && asi.Attribute[11].Id == 0xA9
        && asi.Attribute[12].Id == 0xAA
        && asi.Attribute[13].Id == 0xAD
        && asi.Attribute[14].Id == 0xAF
            )
        {
            flagSmartType = true;
        }
        else if (asi.Model.IndexOf(("ADATA SU700")) == 0)
        {
            flagSmartType = true;
        }

        return flagSmartType;
    }

    bool IsSsdJMicron60x(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x0C
        && asi.Attribute[1].Id == 0x09
        && asi.Attribute[2].Id == 0xC2
        && asi.Attribute[3].Id == 0xE5
        && asi.Attribute[4].Id == 0xE8
        && asi.Attribute[5].Id == 0xE9
        //	&& asi.Attribute[ 6].Id == 0xEA
        //	&& asi.Attribute[ 7].Id == 0xEB
        )
        {
            flagSmartType = true;
        }

        return flagSmartType;
    }


    bool IsSsdIndilinx(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x09
        && asi.Attribute[2].Id == 0x0C
        && asi.Attribute[3].Id == 0xB8
        && asi.Attribute[4].Id == 0xC3
        && asi.Attribute[5].Id == 0xC4
        //	&& asi.Attribute[ 6].Id == 0xC5
        //	&& asi.Attribute[ 7].Id == 0xC6
        //	&& asi.Attribute[ 8].Id == 0xC7
        //	&& asi.Attribute[ 9].Id == 0xC8
        //	&& asi.Attribute[10].Id == 0xC9
        //	&& asi.Attribute[11].Id == 0xCA
        //	&& asi.Attribute[12].Id == 0xCB
        //	&& asi.Attribute[13].Id == 0xCC
        //	&& asi.Attribute[14].Id == 0xCD
        //	&& asi.Attribute[15].Id == 0xCE
        //	&& asi.Attribute[16].Id == 0xCF
        //	&& asi.Attribute[17].Id == 0xD0
        //	&& asi.Attribute[18].Id == 0xD1
        //	&& asi.Attribute[19].Id == 0xD2
        //	&& asi.Attribute[20].Id == 0xD3
        )
        {
            flagSmartType = true;
        }

        /*
            asi.Model.IndexOf(("OCZ-VERTEX")) == 0
        || asi.Model.IndexOf(("G-Monster-V3")) == 0
        || asi.Model.IndexOf(("G-Monster-V5")) == 0 
        || (asi.Model.IndexOf(("STT_FTM")) == 0 && asi.Model.IndexOf(("GX")) > 0)
        || asi.Model.IndexOf(("Solidata")) == 0
        */

        return flagSmartType;

    }

    bool IsSsdIntelDc(ref ATA_SMART_INFO asi)
    {
        // https://github.com/hiyohiyo/CrystalDiskInfo/issues/18
        return (asi.Model.IndexOf(("INTEL SSDSCKHB")) >= 0);
    }

    bool IsSsdIntel(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (asi.Attribute[0].Id == 0x03
        && asi.Attribute[1].Id == 0x04
        && asi.Attribute[2].Id == 0x05
        && asi.Attribute[3].Id == 0x09
        && asi.Attribute[4].Id == 0x0C
        )
        {
            if (asi.Attribute[5].Id == 0xC0 && asi.Attribute[6].Id == 0xE8 && asi.Attribute[7].Id == 0xE9)
            {
                flagSmartType = true;
            }
            else if (asi.Attribute[5].Id == 0xC0 && asi.Attribute[6].Id == 0xE1)
            {
                flagSmartType = true;
            }
            else if (asi.Attribute[5].Id == 0xAA && asi.Attribute[6].Id == 0xAB && asi.Attribute[7].Id == 0xAC)
            {
                flagSmartType = true;
            }
        }

        return (modelUpper.IndexOf(("INTEL")) >= 0 || modelUpper.IndexOf(("SOLIDIGM")) >= 0 || flagSmartType == true);
    }


    // http://www.samsung.com/us/business/oem-solutions/pdfs/General_NSSD_25_SATA_III_Spec_0.2.pdf
    bool IsSsdSamsung(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        // SM951
        if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xAA
            && asi.Attribute[4].Id == 0xAB
            && asi.Attribute[5].Id == 0xAC
            && asi.Attribute[6].Id == 0xAD
            && asi.Attribute[7].Id == 0xAE
            && asi.Attribute[8].Id == 0xB2
            && asi.Attribute[9].Id == 0xB4
            )
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }
        else if (asi.Attribute[0].Id == 0x09
        && asi.Attribute[1].Id == 0x0C
        && asi.Attribute[2].Id == 0xB2
        && asi.Attribute[3].Id == 0xB3
        && asi.Attribute[4].Id == 0xB4
        )
        {
            flagSmartType = true;
        }
        else
        if (asi.Attribute[0].Id == 0x09
        && asi.Attribute[1].Id == 0x0C
        && asi.Attribute[2].Id == 0xB1
        && asi.Attribute[3].Id == 0xB2
        && asi.Attribute[4].Id == 0xB3
        && asi.Attribute[5].Id == 0xB4
        && asi.Attribute[6].Id == 0xB7
        )
        {
            flagSmartType = true;
        }
        else
        if (asi.Attribute[0].Id == 0x09
        && asi.Attribute[1].Id == 0x0C
        && asi.Attribute[2].Id == 0xAF
        && asi.Attribute[3].Id == 0xB0
        && asi.Attribute[4].Id == 0xB1
        && asi.Attribute[5].Id == 0xB2
        && asi.Attribute[6].Id == 0xB3
        && asi.Attribute[7].Id == 0xB4
        )
        {
            flagSmartType = true;
        }
        else
        if (asi.Attribute[0].Id == 0x05
        && asi.Attribute[1].Id == 0x09
        && asi.Attribute[2].Id == 0x0C
        && asi.Attribute[3].Id == 0xB1
        && asi.Attribute[4].Id == 0xB3
        && asi.Attribute[5].Id == 0xB5
        && asi.Attribute[6].Id == 0xB6
        )
        {
            flagSmartType = true;
        }

        return ((asi.Model.IndexOf(("SAMSUNG")) >= 0 && asi.IsSsd) || (asi.Model.IndexOf(("MZ-")) >= 0 && asi.IsSsd) || flagSmartType == true);
    }

    bool IsSsdSandForce(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x05
        && asi.Attribute[2].Id == 0x09
        && asi.Attribute[3].Id == 0x0C
        && asi.Attribute[4].Id == 0x0D
        && asi.Attribute[5].Id == 0x64
        && asi.Attribute[6].Id == 0xAA
        )
        {
            flagSmartType = true;
        }

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x05
        && asi.Attribute[2].Id == 0x09
        && asi.Attribute[3].Id == 0x0C
        && asi.Attribute[4].Id == 0xAB
        && asi.Attribute[5].Id == 0xAC
        )
        {
            flagSmartType = true;
        }

        // TOSHIBA + SandForce
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1116;id=diskinfo#1116
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1136;id=diskinfo#1136
        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x02
        && asi.Attribute[2].Id == 0x03
        && asi.Attribute[3].Id == 0x05
        && asi.Attribute[4].Id == 0x07
        && asi.Attribute[5].Id == 0x08
        && asi.Attribute[6].Id == 0x09
        && asi.Attribute[7].Id == 0x0A
        && asi.Attribute[8].Id == 0x0C
        && asi.Attribute[9].Id == 0xA7
        && asi.Attribute[10].Id == 0xA8
        && asi.Attribute[11].Id == 0xA9
        && asi.Attribute[12].Id == 0xAA
        && asi.Attribute[13].Id == 0xAD
        && asi.Attribute[14].Id == 0xAF
        && asi.Attribute[15].Id == 0xB1
        )
        {
            flagSmartType = true;
        }

        return (asi.Model.IndexOf(("SandForce")) >= 0 || flagSmartType == true);
    }

    // Micron Crucial
    bool IsSsdMicronMU03(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();


        if ((
               modelUpper.IndexOf(("MICRON_M600")) == 0 || modelUpper.IndexOf(("MICRON M600")) == 0
            || modelUpper.IndexOf(("MICRON_M550")) == 0 || modelUpper.IndexOf(("MICRON M550")) == 0
            || modelUpper.IndexOf(("MICRON_M510")) == 0 || modelUpper.IndexOf(("MICRON M510")) == 0
            || modelUpper.IndexOf(("MICRON_M500")) == 0 || modelUpper.IndexOf(("MICRON M500")) == 0
            || modelUpper.IndexOf(("MICRON_1300")) == 0 || modelUpper.IndexOf(("MICRON 1300")) == 0
            || modelUpper.IndexOf(("MICRON_1100")) == 0 || modelUpper.IndexOf(("MICRON 1100")) == 0 || modelUpper.IndexOf(("MTFDDA")) == 0))
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
            flagSmartType = true;
        }
        else if (
            (modelUpper.IndexOf(("M500SSD")) >= 0
            || modelUpper.IndexOf(("MX500SSD")) >= 0 || modelUpper.IndexOf(("BX500SSD")) >= 0
            || modelUpper.IndexOf(("MX300SSD")) >= 0 || modelUpper.IndexOf(("BX300SSD")) >= 0
            || modelUpper.IndexOf(("MX200SSD")) >= 0 || modelUpper.IndexOf(("BX200SSD")) >= 0
            || modelUpper.IndexOf(("MX100SSD")) >= 0 || modelUpper.IndexOf(("BX100SSD")) >= 0
            || modelUpper.IndexOf("MTFD") == 0)
            && asi.FirmwareRev.IndexOf("MU01") == -1)//&& !asi.FirmwareRev.IndexOf("MU01") == 0)
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
            flagSmartType = true;
        }

        return flagSmartType;
    }

    // Micron RealSSD & Crucial
    bool IsSsdMicron(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x05
        && asi.Attribute[2].Id == 0x09
        && asi.Attribute[3].Id == 0x0C
        && asi.Attribute[4].Id == 0xAA
        && asi.Attribute[5].Id == 0xAB
        && asi.Attribute[6].Id == 0xAC
        && asi.Attribute[7].Id == 0xAD
        && asi.Attribute[8].Id == 0xAE
        && asi.Attribute[9].Id == 0xB5
        && asi.Attribute[10].Id == 0xB7
        )
        {
            flagSmartType = true;
        }

        return modelUpper.IndexOf(("P600")) == 0 || modelUpper.IndexOf(("C600")) == 0
            || modelUpper.IndexOf(("M6-")) == 0 || modelUpper.IndexOf(("M600")) == 0
            || modelUpper.IndexOf(("P500")) == 0
            || (modelUpper.IndexOf(("C500")) == 0 && asi.FirmwareRev.IndexOf(("H")) != 0) // workaround for Maxiotek C500
            || modelUpper.IndexOf(("M5-")) == 0 || modelUpper.IndexOf(("M500")) == 0
            || modelUpper.IndexOf(("P400")) == 0 || modelUpper.IndexOf(("C400")) == 0
            || modelUpper.IndexOf(("M4-")) == 0 || modelUpper.IndexOf(("M400")) == 0
            || modelUpper.IndexOf(("P300")) == 0 || modelUpper.IndexOf(("C300")) == 0
            || modelUpper.IndexOf(("M3-")) == 0 || modelUpper.IndexOf(("M300")) == 0
            || (modelUpper.IndexOf(("CT")) == 0 && modelUpper.IndexOf(("SSD")) != -1)
            || modelUpper.IndexOf(("CRUCIAL")) == 0 || modelUpper.IndexOf(("MICRON")) == 0
            || modelUpper.IndexOf("MTFD") == 0
            || flagSmartType == true;
    }

    bool IsSsdOcz(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        // OCZ-TRION100 2015/11/25
        if (modelUpper.IndexOf(("OCZ-TRION")) == 0)
        {
            flagSmartType = true;
        }
        // 2012/3/11
        // OCZ-PETROL - https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=553;id=diskinfo#553
        // OCZ-OCTANE S2 - https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=577;id=diskinfo#577
        // OCZ-VERTEX 4 - http://imageshack.us/a/img269/7506/ocz2.png
        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x03
        && asi.Attribute[2].Id == 0x04
        && asi.Attribute[3].Id == 0x05
        && asi.Attribute[4].Id == 0x09
        && asi.Attribute[5].Id == 0x0C
        && asi.Attribute[6].Id == 0xE8
        && asi.Attribute[7].Id == 0xE9
        )
        {
            flagSmartType = true;
        }

        return (modelUpper.IndexOf(("OCZ")) == 0 && flagSmartType == true);
    }

    bool IsSsdOczVector(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        string modelUpper = asi.Model;
        modelUpper = modelUpper.ToUpper();

        // Radeon R7 2024/06/13
        if (modelUpper.IndexOf(("RADEON R7")) == 0)
        {
            flagSmartType = true;
            return true;
        }

        /*
        // 2013/1/19
        // OCZ-VECTOR - https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1031;id=diskinfo#1031
        if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xAB
            && asi.Attribute[4].Id == 0xAE
            && asi.Attribute[5].Id == 0xBB
            && asi.Attribute[6].Id == 0xC3
            && asi.Attribute[7].Id == 0xC4
            )
        {
            flagSmartType = true;
        }
        // 2013/3/24
        // OCZ-VECTOR - FW 2.0
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1185;id=diskinfo#1185
        if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xAB
            && asi.Attribute[4].Id == 0xAE
            && asi.Attribute[5].Id == 0xC3
            && asi.Attribute[6].Id == 0xC4
            )
        {
            flagSmartType = true;
        }
        */

        // 2015/11/25
        // PANASONIC RP-SSB240GAK
        // https://crystalmark.info/board/c-board.cgi?cmd=one;no=500;id=#500
        if (asi.Attribute[0].Id == 0x05
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xAB
            && asi.Attribute[4].Id == 0xAE
            && asi.Attribute[5].Id == 0xC3
            && asi.Attribute[6].Id == 0xC4
            && asi.Attribute[7].Id == 0xC5
            && asi.Attribute[8].Id == 0xC6
            )
        {
            flagSmartType = true;
        }
        if (modelUpper.IndexOf(("PANASONIC RP-SSB")) == 0)
        {
            flagSmartType = true;
        }

        return (modelUpper.IndexOf(("OCZ")) == 0 || flagSmartType == true);
    }

    bool IsSsdSsstc(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Model.IndexOf("CV8-") >= 0 || asi.Model.IndexOf("CVB-") >= 0 || asi.Model.IndexOf("ER2-") >= 0)
        {
            flagSmartType = true;
        }

        return flagSmartType;
    }

    bool IsSsdPlextor(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        // 2012/10/10
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=739;id=diskinfo#739
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=829;id=diskinfo#829
        if (asi.Attribute[0].Id == 0x01
        && asi.Attribute[1].Id == 0x05
        && asi.Attribute[2].Id == 0x09
        && asi.Attribute[3].Id == 0x0C
        && asi.Attribute[4].Id == 0xB1
        && asi.Attribute[5].Id == 0xB2
        && asi.Attribute[6].Id == 0xB5
        && asi.Attribute[7].Id == 0xB6
        )
        {
            flagSmartType = true;
        }

        // Added CFD's SSD
        // Added LITEON CV6-CQ (2018/9/17)
        return asi.Model.IndexOf(("PLEXTOR")) == 0 || asi.Model.IndexOf(("LITEON")) == 0 || asi.Model.IndexOf(("CV6-CQ")) == 0 || asi.Model.IndexOf(("CSSD-S6T128NM3PQ")) == 0 || asi.Model.IndexOf(("CSSD-S6T256NM3PQ")) == 0
            || flagSmartType == true;
    }

    bool IsSsdSanDisk(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        // 2013/10/7
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1425;id=diskinfo#1425
        // 2020/07/25
        // 
        if (asi.Model.IndexOf(("SanDisk")) >= 0 || asi.Model.IndexOf(("SD Ultra")) >= 0 || asi.Model.IndexOf(("SDLF1")) >= 0)
        {
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK; // Default Vendor ID for SanDisk
            flagSmartType = true;

            if (
                  (asi.Model.IndexOf(("X600")) >= 0 && asi.Model.IndexOf(("2280")) >= 0) // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2123;id=#2123
                || asi.Model.IndexOf(("X400")) >= 0
                || asi.Model.IndexOf(("X300")) >= 0
                || asi.Model.IndexOf(("X110")) >= 0
                || asi.Model.IndexOf(("SD5")) >= 0
                )
            {
                if (asi.Attribute[2].Id == 0xAF || asi.Attribute[3].Id == 0xAF)
                {
                    asi.SmartKeyName = ("SmartSanDiskDell");
                }
                else
                {
                    asi.SmartKeyName = ("SmartSanDiskGb");
                }
                asi.FlagLifeSanDisk1 = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;

            }
            else if (asi.Model.IndexOf(("Z400")) >= 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
                asi.SmartKeyName = ("SmartSanDiskDell");
            }
            // 2022/04/24
            // https://osdn.net/projects/crystaldiskinfo/ticket/44354
            else if (asi.Model.IndexOf(("1006")) > 0) // HP OEM
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB;
                if (asi.Model.IndexOf(("8U")) > 0)
                {
                    asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS;
                    asi.SmartKeyName = ("SmartSanDiskHpVenus");
                }
                else
                {
                    asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP;
                    asi.SmartKeyName = ("SmartSanDiskHp");
                }
            }
            else if (asi.Model.IndexOf(("G1001")) >= 0) // Lenovo OEM
            {
                asi.FlagLifeSanDiskLenovo = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
                if (asi.Model.IndexOf(("6S")) > 0 || asi.Model.IndexOf(("7S")) > 0 || asi.Model.IndexOf(("8U")) > 0)
                {
                    asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS;
                    asi.SmartKeyName = ("SmartSanDiskLenovoHelenVenus");
                }
                else
                {
                    asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO;
                    asi.SmartKeyName = ("SmartSanDiskLenovo");
                }
            }
            else if (asi.Model.IndexOf(("G1012")) >= 0 || asi.Model.IndexOf(("Z400s 2.5")) >= 0) // DELL OEM
            {
                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
                asi.SmartKeyName = ("SmartSanDiskDell");
            }
            else if (asi.Model.IndexOf(("SSD P4")) >= 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
                asi.FlagLifeSanDiskUsbMemory = true; // No Life Report
                asi.SmartKeyName = ("SmartSanDisk");
            }
            else if (asi.Model.IndexOf(("iSSD P4")) >= 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
                asi.SmartKeyName = ("SmartSanDiskGb");
            }
            else if (
               asi.Model.IndexOf(("SDSSDP")) >= 0
            || asi.Model.IndexOf(("SDSSDRC")) >= 0
            )
            {
                asi.FlagLifeSanDisk0_1 = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
                asi.SmartKeyName = ("SmartSanDisk");
            }
            else if (
               asi.Model.IndexOf(("SSD U100")) >= 0
            || asi.Model.IndexOf(("SSD U110")) >= 0
            || asi.Model.IndexOf(("SSD i100")) >= 0
            || asi.Model.IndexOf(("SSD i110")) >= 0
            || asi.Model.IndexOf(("pSSD")) >= 0
                )
            {
                asi.FlagLifeSanDiskUsbMemory = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
                asi.SmartKeyName = ("SmartSanDisk");
            }
            else if (
                // CloudSpeed ECO Gen II Eco SSD
                asi.Model.IndexOf(("SDLF1CRR-")) >= 0
                || asi.Model.IndexOf(("SDLF1DAR-")) >= 0
                // CloudSpeed ECO Gen II Ultra SSD
                || asi.Model.IndexOf(("SDLF1CRM-")) >= 0
                || asi.Model.IndexOf(("SDLF1DAM-")) >= 0
                )
            {
                asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD;
                asi.FlagLifeSanDiskCloud = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
                asi.SmartKeyName = ("SmartSanDiskCloud");
            }
            else
            {
                asi.FlagLifeSanDisk1 = true;
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
                asi.SmartKeyName = ("SmartSanDiskGb");
            }
        }

        return flagSmartType;
    }

    bool IsSsdKingston(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Model.IndexOf("KINGSTON") >= 0)
        {
            if (asi.Model.IndexOf("SM2280") >= 0 || asi.Model.IndexOf("SEDC400") >= 0 || asi.Model.IndexOf("SKC310") >= 0 || asi.Model.IndexOf("SHSS") >= 0 || asi.Model.IndexOf("SUV300") >= 0 || asi.Model.IndexOf("SKC400") >= 0)
            {
                flagSmartType = true;
                asi.SmartKeyName = ("SmartKingston");
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
            else if (asi.Model.IndexOf("SA400") >= 0)
            {
                flagSmartType = true;
                // https://github.com/hiyohiyo/CrystalDiskInfo/issues/162
                if (asi.FirmwareRev.IndexOf("03070009") == 0)
                {
                    asi.FlagLifeRawValue = false;
                }
                // https://github.com/hiyohiyo/CrystalDiskInfo/issues/201
                else if (asi.FirmwareRev.IndexOf("SBFK62C3") == 0)
                {
                    //	asi.FlagLifeNoReport = true;
                    asi.FlagLifeRawValue = true;
                }
                else
                {
                    asi.FlagLifeRawValue = true;
                }
                asi.SmartKeyName = ("SmartKingstonSA400");
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
            else if (asi.Model.IndexOf("KC600") >= 0)
            {
                flagSmartType = true;
                asi.SmartKeyName = ("SmartKingstonKC600");
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
            }
            else if (asi.Model.IndexOf("DC500") >= 0)
            {
                flagSmartType = true;
                asi.SmartKeyName = ("SmartKingstonDC500");
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
            else if (asi.Model.IndexOf("SUV400") >= 0 || asi.Model.IndexOf("SUV500") >= 0)
            {
                flagSmartType = true;
                asi.SmartKeyName = ("SmartKingstonSuv");
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
        }

        return flagSmartType;
    }

    bool IsSsdCorsair(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Model.IndexOf("Corsair") == 0)
        {
            flagSmartType = true;
            if (asi.Model.IndexOf(("Voyager GTX")) >= 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB;
            }
        }

        return flagSmartType;
    }

    //bool IsSsdToshiba(ref ATA_SMART_INFO asi)
    //{
    //    bool flagSmartType = false;
    //    string modelUpper = asi.Model;
    //    modelUpper = modelUpper.ToUpper();

    //    if (modelUpper.IndexOf(("TOSHIBA")) >= 0 && asi.IsSsd)
    //    {
    //        flagSmartType = true;
    //        if (asi.Model.IndexOf(("THNSNC")) >= 0 || asi.Model.IndexOf(("THNSNJ")) >= 0 || asi.Model.IndexOf(("THNSNK")) >= 0
    //        || asi.Model.IndexOf(("KSG60")) >= 0
    //        || asi.Model.IndexOf(("TL100")) >= 0 || asi.Model.IndexOf(("TR150")) >= 0 || asi.Model.IndexOf(("TR200")) >= 0
    //        )
    //        {
    //            // TOSHIBA HG3
    //            // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1106;id=diskinfo#1106
    //            // TOSHIBA KSG60ZMV
    //            // https://crystalmark.info/board/c-board.cgi?cmd=one;no=2425;id=#2425
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
    //        }
    //        else
    //        {
    //            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
    //        }
    //    }

    //    return flagSmartType;
    //}

    bool IsSsdRealtek(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0xA1
            && asi.Attribute[5].Id == 0xA2
            && asi.Attribute[6].Id == 0xA3
            && asi.Attribute[7].Id == 0xA4
            && asi.Attribute[8].Id == 0xA6
            && asi.Attribute[9].Id == 0xA7
            )
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            asi.SmartKeyName = ("SmartRealtek");
        }

        return flagSmartType;
    }

    bool IsSsdSKhynix(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("SK hynix")) >= 0 || asi.Model.IndexOf(("HFS")) == 0 || asi.Model.IndexOf(("SHG")) == 0)
        {
            flagSmartType = true;
            asi.SmartKeyName = ("SmartSKhynix");
        }

        // https://crystalmark.info/board/c-board.cgi?cmd=one;no=1772;id=#1772
        if (
           (asi.Model.IndexOf(("HFS")) >= 0 && asi.Model.IndexOf(("TND")) >= 0) // SL300
        || (asi.Model.IndexOf(("HFS")) >= 0 && asi.Model.IndexOf(("MND")) >= 0) // SC210
        )
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            asi.FlagLifeRawValueIncrement = true;
        }
        else if (asi.Model.IndexOf(("HFS")) >= 0 && asi.Model.IndexOf(("TNF")) >= 0)
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            asi.FlagLifeRawValue = true;
        }
        else if (asi.Model.IndexOf(("SC311")) >= 0 || asi.Model.IndexOf(("SC401")) >= 0)
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
            asi.FlagLifeRawValue = true;
        }
        else
        {
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }

        return flagSmartType;
    }

    bool IsSsdKioxia(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("KIOXIA")) >= 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
            asi.SmartKeyName = ("SmartKioxia");
        }
        return flagSmartType;
    }

    bool IsSsdApacer(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("Apacer")) == 0
        || asi.Model.IndexOf(("ZADAK")) == 0
        || asi.FirmwareRev.IndexOf("AP") == 0
        || asi.FirmwareRev.IndexOf("SF") == 0
        || asi.FirmwareRev.IndexOf("PN") == 0
        )
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
            asi.FlagLifeRawValue = true;
            asi.SmartKeyName = ("SmartApacer");
        }
        return flagSmartType;
    }

    bool IsSsdYmtc(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("ZHITAI")) >= 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B;
            asi.SmartKeyName = ("SmartYmtc");
        }
        return flagSmartType;
    }

    bool IsSsdScy(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("SCY")) == 0) // SCY S500
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB;
            asi.SmartKeyName = ("SmartScy");
        }
        return flagSmartType;
    }

    bool IsSsdRecadata(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("RECADATA")) == 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }

        return flagSmartType;
    }

    bool IsSsdSiliconMotionCVC(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;
        if (asi.Model.IndexOf(("CVC-")) >= 0)
        {
            flagSmartType = true;
            asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
        }

        return flagSmartType;
    }

    bool IsSsdSiliconMotion(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0xA0
            && asi.Attribute[5].Id == 0xA1
            && asi.Attribute[6].Id == 0xA3
            && asi.Attribute[7].Id == 0xA4
            && asi.Attribute[8].Id == 0xA5
            && asi.Attribute[9].Id == 0xA6
            && asi.Attribute[10].Id == 0xA7
            && asi.Attribute[11].Id == 0xA8
            && asi.Attribute[12].Id == 0xA9
            && asi.Attribute[13].Id == 0xAF
            && asi.Attribute[14].Id == 0xB0
            && asi.Attribute[15].Id == 0xB1
            && asi.Attribute[16].Id == 0xB2
            && asi.Attribute[17].Id == 0xB5
            && asi.Attribute[18].Id == 0xB6
            && asi.Attribute[19].Id == 0xC0
            )
        {
            flagSmartType = true;
        }
        else if ( // ADATA SX950 https://crystalmark.info/board/c-board.cgi?cmd=one;no=1819;id=
               asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0xA0
            && asi.Attribute[5].Id == 0xA1
            && asi.Attribute[6].Id == 0xA3
            && asi.Attribute[7].Id == 0xA4
            && asi.Attribute[8].Id == 0xA5
            && asi.Attribute[9].Id == 0xA6
            && asi.Attribute[10].Id == 0xA7
            && asi.Attribute[11].Id == 0x94
            && asi.Attribute[12].Id == 0x95
            && asi.Attribute[13].Id == 0x96
            && asi.Attribute[14].Id == 0x97
            && asi.Attribute[15].Id == 0xA9
            && asi.Attribute[16].Id == 0xB1
            && asi.Attribute[17].Id == 0xB5
            && asi.Attribute[18].Id == 0xB6
            && asi.Attribute[19].Id == 0xBB
            )
        {
            flagSmartType = true;
        }
        else if (
               asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0x94
            && asi.Attribute[5].Id == 0x95
            && asi.Attribute[6].Id == 0x96
            && asi.Attribute[7].Id == 0x97
            && asi.Attribute[8].Id == 0x9F
            && asi.Attribute[9].Id == 0xA0
            && asi.Attribute[10].Id == 0xA1
            )
        {
            flagSmartType = true;
        }
        else if (
               asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0xA0
            && asi.Attribute[5].Id == 0xA1
            && asi.Attribute[6].Id == 0xA3
            && asi.Attribute[7].Id == 0xA4
            && asi.Attribute[8].Id == 0xA5
            && asi.Attribute[9].Id == 0xA6
            && asi.Attribute[10].Id == 0xA7
            )
        {
            flagSmartType = true;
        }
        else if (
               asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x05
            && asi.Attribute[2].Id == 0x09
            && asi.Attribute[3].Id == 0x0C
            && asi.Attribute[4].Id == 0xA0
            && asi.Attribute[5].Id == 0xA1
            && asi.Attribute[6].Id == 0xA3
            && asi.Attribute[7].Id == 0x94
            && asi.Attribute[8].Id == 0x95
            && asi.Attribute[9].Id == 0x96
            && asi.Attribute[10].Id == 0x97
            )
        {
            flagSmartType = true;
        }

        // Transcend
        else if (asi.Model.IndexOf(("TS")) == 0)
        {
            if ((asi.SmartReadData[400] == 'T' && asi.SmartReadData[401] == 'S') // Transcend
            || asi.SmartReadData[400] == 'S' && asi.SmartReadData[401] == 'M') // Silicon Motion
            {
                flagSmartType = true;
            }
        }
        else if (asi.Model.IndexOf(("ADATA SX950")) == 0)
        {
            flagSmartType = true;
        }

        if (flagSmartType)
        {
            if (asi.Model.IndexOf(("SSD")) == 0 && asi.FirmwareRev.IndexOf(("FW")) == 0) // for Goldenfir SSD
            {
                // Disabled for harmful to other products (v8.12.8)
                //	asi.FlagLifeRawValueIncrement = true;
            }
            // WINTEC
            else if (asi.Model.IndexOf(("WT200")) == 0 || asi.Model.IndexOf(("WT100")) == 0 || asi.Model.IndexOf(("WT ")) == 0)
            {

            }
            else if (asi.Model.IndexOf(("tecmiyo")) == 0) // https://github.com/hiyohiyo/CrystalDiskInfo/issues/191
            {

            }
            else if (asi.Model.IndexOf(("ADATA SU650")) == 0 && asi.FirmwareRev.IndexOf(("XD0R3C0A")) == 0) // Twitter DM
            {

            }
            else
            {
                asi.FlagLifeRawValue = true;
            }
        }

        return flagSmartType;
    }

    bool IsSsdPhison(ref ATA_SMART_INFO asi)
    {
        bool flagSmartType = false;

        if (asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA8
            && asi.Attribute[4].Id == 0xAA
            && asi.Attribute[5].Id == 0xAD
            && asi.Attribute[6].Id == 0xC0
            && asi.Attribute[7].Id == 0xC2 // with Temperature Sensor
            && asi.Attribute[8].Id == 0xDA
            && asi.Attribute[9].Id == 0xE7
            && asi.Attribute[10].Id == 0xF1
        )
        {
            flagSmartType = true;
        }
        else if (
               asi.Attribute[0].Id == 0x01
            && asi.Attribute[1].Id == 0x09
            && asi.Attribute[2].Id == 0x0C
            && asi.Attribute[3].Id == 0xA8
            && asi.Attribute[4].Id == 0xAA
            && asi.Attribute[5].Id == 0xAD
            && asi.Attribute[6].Id == 0xC0
            && asi.Attribute[7].Id == 0xDA
            && asi.Attribute[8].Id == 0xE7
            && asi.Attribute[9].Id == 0xF1
            )
        {
            flagSmartType = true;
        }

        if (flagSmartType)
        {
            asi.FlagLifeRawValue = true;
            if (asi.FirmwareRev.IndexOf("S9") == 0)
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB;
            }
            else
            {
                asi.HostReadsWritesUnit = HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB;
            }
        }

        return flagSmartType;
    }




    void CheckSsdSupport(ref ATA_SMART_INFO asi)
    {
        // Old SSD Detection
        if (IsSsdOld(ref asi))
        {
            asi.IsSsd = true;
        }

        if (!asi.IsSsd) // HDD
        {
            asi.SmartKeyName = ("Smart");
            asi.DiskVendorId = (uint)VENDOR_ID.HDD_GENERAL;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdAdataIndustrial(ref asi))
        {
            asi.SmartKeyName = ("SmartAdataIndustrial");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSanDisk(ref asi))
        {
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdWdc(ref asi))
        {
            asi.SmartKeyName = ("SmartWdc");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_WDC;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSeagate(ref asi))
        {
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdMtron(ref asi))
        {
            asi.SmartKeyName = ("SmartMtron");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_MTRON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdToshiba(ref asi))
        {
            asi.SmartKeyName = ("SmartToshiba");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdJMicron66x(ref asi))
        {
            asi.SmartKeyName = ("SmartJMicron66x");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_JMICRON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdJMicron61x(ref asi))
        {
            asi.SmartKeyName = ("SmartJMicron61x");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_JMICRON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdJMicron60x(ref asi))
        {
            asi.SmartKeyName = ("SmartJMicron60x");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_JMICRON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
            asi.IsRawValues8 = true;
        }
        else if (IsSsdIndilinx(ref asi))
        {
            asi.SmartKeyName = ("SmartIndilinx");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_INDILINX;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
            asi.IsRawValues8 = true;
        }
        else if (IsSsdIntelDc(ref asi))
        {
            asi.SmartKeyName = ("SmartIntelDc");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdIntel(ref asi))
        {
            asi.SmartKeyName = ("SmartIntel");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_INTEL;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSamsung(ref asi))
        {
            asi.SmartKeyName = ("SmartSamsung");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdMicronMU03(ref asi))
        {
            asi.SmartKeyName = ("SmartMicronMU03");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdMicron(ref asi))
        {
            asi.SmartKeyName = ("SmartMicron");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_MICRON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSandForce(ref asi))
        {
            asi.SmartKeyName = ("SmartSandForce");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
            asi.IsRawValues7 = true;
        }
        else if (IsSsdOcz(ref asi))
        {
            asi.SmartKeyName = ("SmartOcz");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_OCZ;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdOczVector(ref asi))
        {
            asi.SmartKeyName = ("SmartOczVector");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSsstc(ref asi))
        {
            asi.SmartKeyName = ("SmartSsstc");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SSSTC;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdPlextor(ref asi))
        {
            asi.SmartKeyName = ("SmartPlextor");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdKingston(ref asi))
        {
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_KINGSTON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdCorsair(ref asi))
        {
            asi.SmartKeyName = ("SmartCorsair");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_CORSAIR;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdRealtek(ref asi))
        {
            asi.SmartKeyName = ("SmartRealtek");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_REALTEK;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSKhynix(ref asi))
        {
            asi.SmartKeyName = ("SmartSKhynix");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdKioxia(ref asi))
        {
            asi.SmartKeyName = ("SmartKioxia");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_KIOXIA;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSiliconMotionCVC(ref asi))
        {
            asi.SmartKeyName = ("SmartSiliconMotionCVC");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdSiliconMotion(ref asi))
        {
            asi.SmartKeyName = ("SmartSiliconMotion");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdPhison(ref asi))
        {
            asi.SmartKeyName = ("SmartPhison");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_PHISON;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdMarvell(ref asi))
        {
            asi.SmartKeyName = ("SmartMarvell");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_MARVELL;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdMaxiotek(ref asi))
        {
            asi.SmartKeyName = ("SmartMaxiotek");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdApacer(ref asi))
        {
            asi.SmartKeyName = ("SmartApacer");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_APACER;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdYmtc(ref asi))
        {
            asi.SmartKeyName = ("SmartYmtc");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_YMTC;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdScy(ref asi))
        {
            asi.SmartKeyName = ("SmartScy");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_SCY;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdRecadata(ref asi))
        {
            asi.SmartKeyName = ("SmartRecadata");
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_VENDOR_RECADATA;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
        }
        else if (IsSsdGeneral(ref asi))
        {
            asi.DiskVendorId = (uint)VENDOR_ID.SSD_GENERAL;
            asi.SsdVendorString = ssdVendorString[asi.DiskVendorId];
            asi.SmartKeyName = ("SmartSsd");
            return;
        }
        else
        {
            asi.DiskVendorId = (uint)VENDOR_ID.HDD_GENERAL;
            asi.SmartKeyName = ("Smart");
            return;
        }

        // Update Life
        for (uint j = 0; j < asi.AttributeCount; j++)
        {
            switch (asi.Attribute[j].Id)
            {
                case 0xBB:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MTRON)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xCA:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03 || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xD1:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INDILINX)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xC9:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS)
                    {
                        int life = -1;
                        life = asi.Attribute[j].CurrentValue;
                        if (life < 0) { life = -1; }

                        asi.Life = life;
                    }
                    break;
                case 0xE6:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                    {
                        int life = -1;

                        if (asi.FlagLifeSanDiskUsbMemory)
                        {
                            life = -1;
                        }
                        else if (asi.FlagLifeSanDisk0_1)
                        {
                            life = 100 - (asi.Attribute[j].RawValue[1] * 256 + asi.Attribute[j].RawValue[0]) / 100;
                        }
                        else if (asi.FlagLifeSanDisk1)
                        {
                            life = 100 - asi.Attribute[j].RawValue[1];
                        }
                        else if (asi.FlagLifeSanDiskLenovo)
                        {
                            life = asi.Attribute[j].CurrentValue;
                        }
                        else
                        {
                            life = 100 - asi.Attribute[j].RawValue[1];
                        }

                        if (life < 0) { life = -1; }

                        asi.Life = life;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL)
                    {
                        int life = -1;
                        life = asi.Attribute[j].CurrentValue;
                        if (life < 0) { life = -1; }

                        asi.Life = life;
                    }
                    break;
                case 0xE8:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    break;
                case 0xE9:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX)
                    {
                        if (asi.FlagLifeRawValue)
                        {
                            asi.Life = asi.Attribute[j].RawValue[0];
                        }
                        else
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                        }
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    else if ((asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO) && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC)
                    {
                        asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK)
                    {
                        if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                        }
                    }
                    break;
                case 0xE1:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    break;
                case 0xEA:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE
                        || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        )
                    {
                        asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    break;
                case 0xEB:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    break;
                case 0xF1:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_GENERAL)
                    {
                        if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 64);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                        }
                        else
                        {
                        }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA ||
                        asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KIOXIA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PHISON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MARVELL
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL
                    )
                    {
                        if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 64);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                        }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_APACER || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    break;
                case 0xF2:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_GENERAL)
                    {
                        if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 64);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                        }
                        else
                        {
                        }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                    {
                        asi.HostReads = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MARVELL
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03
                          || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL
                    )
                    {
                        if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 64);
                        }
                        else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                        }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON)
                    {
                        asi.HostReads = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                    {
                        asi.HostReads = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    {
                        asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                    {
                        asi.HostReads = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xF3:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC)
                    {
                        if (asi.Attribute[j].RawValue[0] > 0)
                        {
                            asi.Temperature = asi.Attribute[j].RawValue[0];
                        }

                        if (asi.Temperature >= 100)
                        {
                            asi.Temperature = -1000;
                        }
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    break;
                case 0xF9:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                    || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS)
                    {
                        asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            * 16 / 1024 / 1024);
                    }
                    break;
                case 0xFA:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK)
                    {
                        asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    break;
                case 0x64:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE)
                    {
                        asi.GBytesErased = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    break;
                case 0xAD:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KIOXIA)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue - 100;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;

                case 0xB1:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG)
                    {
                        asi.WearLevelingCount = (int)MAKELONG(
                            MAKEWORD(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1]),
                            MAKEWORD(asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3])
                            );
                        asi.Life = asi.Attribute[j].CurrentValue;
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xE7:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                    || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_APACER || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PHISON
                    || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL)
                    {
                        if (asi.FlagLifeNoReport)
                        {
                            asi.Life = -1;
                        }
                        else if (asi.FlagLifeRawValueIncrement)
                        {
                            asi.Life = 100 - asi.Attribute[j].RawValue[0];
                        }
                        else if (asi.FlagLifeRawValue)
                        {
                            asi.Life = asi.Attribute[j].RawValue[0];
                        }
                        else
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                        }
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xA9:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB) || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                    {
                        if (asi.FlagLifeRawValueIncrement)
                        {
                            asi.Life = 100 - asi.Attribute[j].RawValue[0];
                        }
                        else if (asi.FlagLifeRawValue)
                        {
                            asi.Life = asi.Attribute[j].RawValue[0];
                        }
                        else
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                        }
                        if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                    }
                    break;
                case 0xC6:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                    {
                        asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    break;
                case 0xC7:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                    {
                        asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)(B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]));
                    }
                    break;
                case 0xF5:
                    // Percent Drive Life Remaining (SanDisk/WD CloudSpeed)
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                    {
                        asi.Life = asi.Attribute[j].CurrentValue;
                    }
                    // NAND Page Size = 8KBytes
                    // http://www.overclock.net/t/1145150/official-crucial-ssd-owners-club/1290
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            * 8 / 1024 / 1024);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32);
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                    }
                    else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA)
                    {
                        asi.NandWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5]));
                    }
                    break;
                case 0xF6:
                    if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03)
                    {
                        asi.HostWrites = (int)(
                            B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                            / 2 / 1024 / 1024);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    uint GetTimeUnitType(string model, string firmware, uint major, uint transferMode)
    {
        model = model.ToUpper();

        if (model.IndexOf(("FUJITSU")) == 0)
        {
            if (major >= 8)
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS;
            }
            else
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_SECONDS;
            }
        }
        else if (model.IndexOf(("HITACHI_DK")) == 0)
        {
            return (uint)POWER_ON_HOURS_UNIT.POWER_ON_MINUTES;
        }
        else if (model.IndexOf(("MAXTOR")) == 0)
        {
            if (transferMode >= (uint)TRANSFER_MODE.TRANSFER_MODE_SATA_300
            || model.IndexOf(("MAXTOR 6H")) == 0     // Maxtor DiamondMax 11 family
            || model.IndexOf(("MAXTOR 7H500")) == 0  // Maxtor MaXLine Pro 500 family
            || model.IndexOf(("MAXTOR 6L0")) == 0    // Maxtor DiamondMax Plus D740X family
            || model.IndexOf(("MAXTOR 4K")) == 0     // Maxtor DiamondMax D540X-4K family
            )
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS;
            }
            else
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_MINUTES;
            }
        }
        else if (model.IndexOf(("SAMSUNG")) == 0)
        {
            if (transferMode >= (uint)TRANSFER_MODE.TRANSFER_MODE_SATA_300)
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS;
            }

            else if (-23 >= int.Parse(firmware.Substring(firmware.Length - 3)) && int.Parse(firmware.Substring(firmware.Length - 3)) >= -39)
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HALF_MINUTES;
            }
            else if (model.IndexOf(("SAMSUNG SV")) == 0
            || model.IndexOf(("SAMSUNG SP")) == 0
            || model.IndexOf(("SAMSUNG HM")) == 0
            || model.IndexOf(("SAMSUNG MP")) == 0
            )
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HALF_MINUTES;
            }
            else
            {
                return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS;
            }
        }
        // 2012/1/15
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=504;id=diskinfo#504
        // http://sourceforge.jp/ticket/browse.php?group_id=4394&tid=27443
        else if (
           ((model.IndexOf(("CFD_CSSD-S6TM128NMPQ")) == 0 || model.IndexOf(("CFD_CSSD-S6TM256NMPQ")) == 0) && (firmware.IndexOf(("VM21")) == 0 || firmware.IndexOf(("VN21")) == 0))
        || ((model.IndexOf(("PX-128M2P")) != -1 || model.IndexOf(("PX-256M2P")) != -1) && int.Parse(firmware) < 1.059)
        || (model.IndexOf(("Corsair Performance Pro")) == 0 && int.Parse(firmware) < 1.059)
        )
        {
            return (uint)POWER_ON_HOURS_UNIT.POWER_ON_10_MINUTES;
        }
        // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=1174;id=diskinfo#1174
        else if (
               (model.IndexOf(("INTEL SSDSC2CW")) == 0 && model.IndexOf(("A3")) > 0) // Intel SSD 520 Series
            || (model.IndexOf(("INTEL SSDSC2BW")) == 0 && model.IndexOf(("A3")) > 0) // Intel SSD 520 Series
            || (model.IndexOf(("INTEL SSDSC2CT")) == 0 && model.IndexOf(("A3")) > 0) // Intel SSD 330 Series
            )
        {
            return (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS;
        }
        else
        {
            return (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS;
        }
    }



    uint GetTransferMode(ushort w63, ushort w76, ushort w77, ushort w88, ref string current, ref string max, ref string type, ref INTERFACE_TYPE interfaceType)
    {
        uint tm = (uint)TRANSFER_MODE.TRANSFER_MODE_UNKNOWN;
        current = max = ("");
        type = ("");
        interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_UNKNOWN;

        // Multiword DMA or PIO
        if ((w63 & 0x0700) != 0)
        {
            tm = (uint)TRANSFER_MODE.TRANSFER_MODE_PIO_DMA;
            current = max = ("PIO/DMA");
        }

        if ((w88 & 0x7F) != 0)
        {
            type = ("Parallel ATA");
            interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_PATA;
        }

        // Ultara DMA Max Transfer Mode
        if ((w88 & 0x0040) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_133; max = ("UDMA/133"); }
        else if ((w88 & 0x0020) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_100; max = ("UDMA/100"); }
        else if ((w88 & 0x0010) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_66; max = ("UDMA/66"); }
        else if ((w88 & 0x0008) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_44; max = ("UDMA/44"); }
        else if ((w88 & 0x0004) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_33; max = ("UDMA/33"); }
        else if ((w88 & 0x0002) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_25; max = ("UDMA/25"); }
        else if ((w88 & 0x0001) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_ULTRA_DMA_16; max = ("UDMA/16"); }

        // Ultara DMA Current Transfer Mode
        if ((w88 & 0x4000) != 0) { current = ("UDMA/133"); }
        else if ((w88 & 0x2000) != 0) { current = ("UDMA/100"); }
        else if ((w88 & 0x1000) != 0) { current = ("UDMA/66"); }
        else if ((w88 & 0x0800) != 0) { current = ("UDMA/44"); }
        else if ((w88 & 0x0400) != 0) { current = ("UDMA/33"); }
        else if ((w88 & 0x0200) != 0) { current = ("UDMA/25"); }
        else if ((w88 & 0x0100) != 0) { current = ("UDMA/16"); }

        // Serial ATA
        if (w76 != 0x0000 && w76 != 0xFFFF)
        {
            current = max = ("SATA/150");
            type = ("Serial ATA");
            interfaceType = INTERFACE_TYPE.INTERFACE_TYPE_SATA;
        }

        if ((w76 & 0x0010) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_UNKNOWN; current = max = ("----"); }
        else if ((w76 & 0x0008) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_SATA_600; current = ("----"); max = ("SATA/600"); }
        else if ((w76 & 0x0004) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_SATA_300; current = ("----"); max = ("SATA/300"); }
        else if ((w76 & 0x0002) != 0) { tm = (uint)TRANSFER_MODE.TRANSFER_MODE_SATA_150; current = ("----"); max = ("SATA/150"); }

        // 2013/5/1 ACS-3
        if (((w77 & 0x000E) >> 1) == 3) { current = ("SATA/600"); }
        else if (((w77 & 0x000E) >> 1) == 2) { current = ("SATA/300"); }
        else if (((w77 & 0x000E) >> 1) == 1) { current = ("SATA/150"); }

        return tm;
    }



    // Last Update : 2011/03/21
    // Reference : http://www.t13.org/Documents/MinutesDefault.aspx?DocumentType=4&DocumentStage=2
    //           - d2161r0-ATAATAPI_Command_Set_-_3.pdf
    //           - d1153r18-ATA-ATAPI-4.pdf 
    void GetAtaMinorVersion(ushort w81, ref string minor)
    {
        switch (w81)
        {
            case 0x0000:
            case 0xFFFF:
                //	minor = ("Not Reported");									break;
                minor = ("----"); break;
            case 0x0001: minor = ("ATA (ATA-1) X3T9.2 781D prior to revision 4"); break;
            case 0x0002: minor = ("ATA-1 published, ANSI X3.221-1994"); break;
            case 0x0003: minor = ("ATA (ATA-1) X3T10 781D revision 4"); break;
            case 0x0004: minor = ("ATA-2 published, ANSI X3.279-1996"); break;
            case 0x0005: minor = ("ATA-2 X3T10 948D prior to revision 2k"); break;
            case 0x0006: minor = ("ATA-3 X3T10 2008D revision 1"); break;
            case 0x0007: minor = ("ATA-2 X3T10 948D revision 2k"); break;
            case 0x0008: minor = ("ATA-3 X3T10 2008D revision 0"); break;
            case 0x0009: minor = ("ATA-2 X3T10 948D revision 3"); break;
            case 0x000A: minor = ("ATA-3 published, ANSI X3.298-199x"); break;
            case 0x000B: minor = ("ATA-3 X3T10 2008D revision 6"); break;
            case 0x000C: minor = ("ATA-3 X3T13 2008D revision 7 and 7a"); break;
            case 0x000D: minor = ("ATA/ATAPI-4 X3T13 1153D version 6"); break;
            case 0x000E: minor = ("ATA/ATAPI-4 T13 1153D version 13"); break;
            case 0x000F: minor = ("ATA/ATAPI-4 X3T13 1153D version 7"); break;
            case 0x0010: minor = ("ATA/ATAPI-4 T13 1153D version 18"); break;
            case 0x0011: minor = ("ATA/ATAPI-4 T13 1153D version 15"); break;
            case 0x0012: minor = ("ATA/ATAPI-4 published, ANSI INCITS 317-1998"); break;
            case 0x0013: minor = ("ATA/ATAPI-5 T13 1321D version 3"); break;
            case 0x0014: minor = ("ATA/ATAPI-4 T13 1153D version 14"); break;
            case 0x0015: minor = ("ATA/ATAPI-5 T13 1321D version 1"); break;
            case 0x0016: minor = ("ATA/ATAPI-5 published, ANSI INCITS 340-2000"); break;
            case 0x0017: minor = ("ATA/ATAPI-4 T13 1153D version 17"); break;
            case 0x0018: minor = ("ATA/ATAPI-6 T13 1410D version 0"); break;
            case 0x0019: minor = ("ATA/ATAPI-6 T13 1410D version 3a"); break;
            case 0x001A: minor = ("ATA/ATAPI-7 T13 1532D version 1"); break;
            case 0x001B: minor = ("ATA/ATAPI-6 T13 1410D version 2"); break;
            case 0x001C: minor = ("ATA/ATAPI-6 T13 1410D version 1"); break;
            case 0x001D: minor = ("ATA/ATAPI-7 published ANSI INCITS 397-2005."); break;
            case 0x001E: minor = ("ATA/ATAPI-7 T13 1532D version 0"); break;
            case 0x001F: minor = ("ACS-3 Revision 3b"); break;
            case 0x0021: minor = ("ATA/ATAPI-7 T13 1532D version 4a"); break;
            case 0x0022: minor = ("ATA/ATAPI-6 published, ANSI INCITS 361-2002"); break;
            case 0x0027: minor = ("ATA8-ACS version 3c"); break;
            case 0x0028: minor = ("ATA8-ACS version 6"); break;
            case 0x0029: minor = ("ATA8-ACS version 4"); break;
            case 0x0031: minor = ("ACS-2 Revision 2"); break;
            case 0x0033: minor = ("ATA8-ACS version 3e"); break;
            case 0x0039: minor = ("ATA8-ACS version 4c"); break;
            case 0x0042: minor = ("ATA8-ACS version 3f"); break;
            case 0x0052: minor = ("ATA8-ACS version 3b"); break;
            case 0x005E: minor = ("ACS-4 Revision 5"); break;
            case 0x006D: minor = ("ACS-3 Revision 5"); break;
            case 0x0082: minor = ("ACS-2 published, ANSI INCITS 482-2012"); break;
            case 0x009C: minor = ("ACS-4 published, ANSI INCITS 529-2018"); break;
            case 0x0107: minor = ("ATA8-ACS version 2d"); break;
            case 0x010A: minor = ("ACS-3 published, ANSI INCITS 522-2014"); break;
            case 0x0110: minor = ("ACS-2 Revision 3"); break;
            case 0x011B: minor = ("ACS-3 Revision 4"); break;
            default:    //	minor.Format(("Reserved [%04Xh]"), w81);					break;
                        //minor = minor.Format(("---- [%04Xh]"), w81); break;
                minor = string.Format(("---- [{0:X4}h]"), w81); break;
        }
    }



    uint GetAtaMajorVersion(ushort w80, ref string majorVersion)
    {
        uint major = 0;

        if (w80 == 0x0000 || w80 == 0xFFFF)
        {
            return 0;
        }

        for (int i = 14; i > 0; i--)
        {
            if (((w80 >> i) & 0x1) != 0)
            {
                major = (uint)i;
                break;
            }
        }

        if (major >= 9)
        {
            majorVersion = string.Format(("ACS-{0}"), major - 7);
        }
        else if (major == 8)
        {
            majorVersion = ("ATA8-ACS");
        }
        else if (major >= 4)
        {
            majorVersion = string.Format(("ATA/ATAPI-{0}"), major);
        }
        else if (major == 0)
        {
            majorVersion = ("----");
        }
        else
        {
            majorVersion = string.Format(("ATA-{0}"), major);
        }

        return major;
    }


    void ChangeByteOrder(ref byte[] str, uint length)
    {
        byte temp;
        for (uint i = 0; i < length; i += 2)
        {
            temp = str[i];
            str[i] = str[i + 1];
            str[i + 1] = temp;
        }
    }


    bool CheckAsciiStringError(ref byte[] str, uint length)
    {
        bool flag = false;
        for (uint i = 0; i < length; i++)
        {
            if ((0x00 < str[i] && str[i] < 0x20))
            {
                str[i] = 0x20;
                break;
            }
            else if (str[i] >= 0x7f)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }


   

    bool DoIdentifyDevicePd(int physicalDriveId, byte target, ref IDENTIFY_DEVICE data)
    {
        bool bRet = false;
        IntPtr hIoCtrl = IntPtr.Zero;
        uint dwReturned = 0;
        string cstr;
        Unsafe.SkipInit(out cstr);

        IDENTIFY_DEVICE_OUTDATA sendCmdOutParam;
        Unsafe.SkipInit(out sendCmdOutParam);
        SENDCMDINPARAMS sendCmd;
        Unsafe.SkipInit(out sendCmd);

        if (data.B.Bin != null)
        {
            return false;
        }

        if (m_bAtaPassThrough && m_bAtaPassThroughSmart)
        {
            //Logs.MyLogs(("SendAtaCommandPd - IDENTIFY_DEVICE (ATA_PASS_THROUGH)"));
            bRet = SendAtaCommandPd(physicalDriveId, target, 0xEC, 0x00, 0x00, ref data, (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));
            cstr = Encoding.ASCII.GetString(data.A.Model);
        }


        if (bRet == false || cstr.IsEmpty())
        {
            data.StructToZeroHollowCast();
            //ZeroMemory(ref data, Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));

            hIoCtrl = GetIoCtrlHandle(physicalDriveId);
            if (hIoCtrl == IntPtr.Zero || hIoCtrl == INVALID_HANDLE_VALUE)
            {
                return false;
            }
            //.ZeroMemory(&sendCmdOutParam, sizeof(IDENTIFY_DEVICE_OUTDATA));
            //.ZeroMemory(&sendCmd, sizeof(SENDCMDINPARAMS));

            sendCmd.irDriveRegs.bCommandReg = ID_CMD;
            sendCmd.irDriveRegs.bSectorCountReg = 1;
            sendCmd.irDriveRegs.bSectorNumberReg = 1;
            sendCmd.irDriveRegs.bDriveHeadReg = target;
            sendCmd.cBufferSize = IDENTIFY_BUFFER_SIZE;

            //Logs.MyLogs(("SendAtaCommandPd - IDENTIFY_DEVICE"));
            bRet = Dni.DeviceIoControlSpecific(hIoCtrl, (uint)IO_CONTROL_CODE.DFP_RECEIVE_DRIVE_DATA,
                ref sendCmd, (uint)Marshal.SizeOf(typeof(SENDCMDINPARAMS)),
                ref sendCmdOutParam, (uint)Marshal.SizeOf(typeof(IDENTIFY_DEVICE_OUTDATA)),
                ref dwReturned, IntPtr.Zero);

            Dip.safeCloseHandle(hIoCtrl);

            if (bRet == false || dwReturned != Marshal.SizeOf(typeof(IDENTIFY_DEVICE_OUTDATA)))
            {
                return false;
            }

            //not tested
            //Debugger.Break();
            MemCpyStructToStruct(ref data, ref sendCmdOutParam.SendCmdOutParam.bBuffer, 0, (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));

            //memcpy_s(data, sizeof(ATA_IDENTIFY_DEVICE), sendCmdOutParam.SendCmdOutParam.bBuffer, sizeof(ATA_IDENTIFY_DEVICE));
        }

        return true;
    }


    //bool CopyMemV(ref IDENTIFY_DEVICE data, byte buf)
    //{
    //    ATA_IDENTIFY_DEVICE _aTA_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _aTA_IDENTIFY_DEVICE);

    //    BIN_IDENTIFY_DEVICE _bIN_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _bIN_IDENTIFY_DEVICE);

    //    NVME_IDENTIFY_DEVICE _nVME_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _nVME_IDENTIFY_DEVICE);

    //    CopyMemory(ref _aTA_IDENTIFY_DEVICE, ref buf, (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));
    //    CopyMemory(ref _bIN_IDENTIFY_DEVICE, ref buf, (uint)Marshal.SizeOf(typeof(BIN_IDENTIFY_DEVICE)));
    //    CopyMemory(ref _nVME_IDENTIFY_DEVICE, ref buf, (uint)Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

    //    data.A = _aTA_IDENTIFY_DEVICE;
    //    data.B = _bIN_IDENTIFY_DEVICE;
    //    data.N = _nVME_IDENTIFY_DEVICE;
    //    return true;
    //}

    IntPtr GetIoCtrlHandle(int/*BYTE*/ index)
    {
        string strDevice;
        strDevice = string.Format(("\\\\.\\PhysicalDrive{0}"), index);

        return Dip.CreateFile(strDevice, (uint)(GENERIC_READ | GENERIC_WRITE),
            FILE_SHARE_READ | FILE_SHARE_WRITE,
            IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
    }


    //bool DoIdentifyDeviceNVMeStorageQuery(int physicalDriveId, int scsiPort, int scsiTargetId, ref IDENTIFY_DEVICE data, ref uint diskSize)
    //{
    //    ////512
    //    //var _ATA_IDENTIFY_DEVICE = Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE));
    //    ////4096
    //    //var _NVME_IDENTIFY_DEVICE = Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE));
    //    ////4096
    //    //var _BIN_IDENTIFY_DEVICE = Marshal.SizeOf(typeof(BIN_IDENTIFY_DEVICE));
    //    ////4096
    //    ////var _IDENTIFY_DEVICE_Data = Marshal.SizeOf(typeof(IDENTIFY_DEVICE));


    //    string path;
    //    path = string.Format("\\\\.\\PhysicalDrive{0}", physicalDriveId);

    //    IntPtr hIoCtrl = Dip.CreateFile(path, (uint)(GENERIC_READ | GENERIC_WRITE),
    //        FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);

    //    TStorageQueryWithBuffer nptwb;
    //    Unsafe.SkipInit(out nptwb);
    //    bool bRet = false;

    //    ZeroMemory(ref nptwb, Marshal.SizeOf(nptwb));

    //    nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
    //    nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
    //    nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
    //    nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
    //    nptwb.ProtocolSpecific.ProtocolDataRequestValue = 0;
    //    nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 1;
    //    nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
    //    nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
    //    uint dwReturned = 0;

    //    bRet = Dip.DeviceIoControlSpecific(hIoCtrl, (uint)(IOCTL_STORAGE_QUERY_PROPERTY),
    //    ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);

    //    if (bRet)
    //    {
    //        ulong totalLBA = BitConverter.ToUInt64(nptwb.Buffer, 0);
    //        int sectorSize = 1 << nptwb.Buffer[130];
    //        diskSize = (uint)((totalLBA * (uint)sectorSize) / 1000 / 1000);
    //    }

    //    ZeroMemory(ref nptwb, Marshal.SizeOf(nptwb));

    //    //ZeroMemory(&nptwb, sizeof(nptwb));
    //    nptwb.ProtocolSpecific.ProtocolType = TStroageProtocolType.ProtocolTypeNvme;
    //    nptwb.ProtocolSpecific.DataType = (uint)TStorageProtocolNVMeDataType.NVMeDataTypeIdentify;
    //    nptwb.ProtocolSpecific.ProtocolDataOffset = (uint)Marshal.SizeOf(typeof(TStorageProtocolSpecificData));
    //    nptwb.ProtocolSpecific.ProtocolDataLength = 4096;
    //    nptwb.Query.PropertyId = TStoragePropertyId.StorageAdapterProtocolSpecificProperty;
    //    nptwb.Query.QueryType = TStorageQueryType.PropertyStandardQuery;
    //    nptwb.ProtocolSpecific.ProtocolDataRequestValue = 1; /*NVME_IDENTIFY_CNS_CONTROLLER*/
    //    nptwb.ProtocolSpecific.ProtocolDataRequestSubValue = 0;
    //    dwReturned = 0;

    //    bRet = Dip.DeviceIoControlSpecific(hIoCtrl, (uint)(IOCTL_STORAGE_QUERY_PROPERTY),
    //        ref nptwb, (uint)Marshal.SizeOf(nptwb), ref nptwb, (uint)Marshal.SizeOf(nptwb), ref dwReturned, IntPtr.Zero);

    //    Dip.safeCloseHandle(hIoCtrl);

    //    ATA_IDENTIFY_DEVICE _aTA_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _aTA_IDENTIFY_DEVICE);

    //    BIN_IDENTIFY_DEVICE _bIN_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _bIN_IDENTIFY_DEVICE);

    //    NVME_IDENTIFY_DEVICE _nVME_IDENTIFY_DEVICE;
    //    Unsafe.SkipInit(out _nVME_IDENTIFY_DEVICE);

    //    //IDENTIFY_DEVICE _iDENTIFY_DEVICE;
    //    //Unsafe.SkipInit(out _iDENTIFY_DEVICE);

    //    CopyMemory(ref _aTA_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(ATA_IDENTIFY_DEVICE)));
    //    CopyMemory(ref _bIN_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(BIN_IDENTIFY_DEVICE)));
    //    CopyMemory(ref _nVME_IDENTIFY_DEVICE, ref nptwb.Buffer[0], (uint)Marshal.SizeOf(typeof(NVME_IDENTIFY_DEVICE)));

    //    data.A = _aTA_IDENTIFY_DEVICE;
    //    data.B = _bIN_IDENTIFY_DEVICE;
    //    data.N = _nVME_IDENTIFY_DEVICE;


    //    return bRet;
    //}

    bool AddDiskNVMe(ref ATA_SMART_INFO asi, int physicalDriveId, int scsiPort, int scsiTargetId, int scsiBus, byte target, COMMAND_TYPE commandType, ref IDENTIFY_DEVICE identify)
    {
        NVME_PORT_20? nVME_PORT_20 = null;
        //Unsafe.SkipInit(out nVME_PORT_20);

        NVME_PORT_40? nVME_PORT_40 = null;
        //Unsafe.SkipInit(out nVME_PORT_40);

        NVME_ID? nVME_ID = null;
        //Unsafe.SkipInit(out nVME_ID);

        string? pnpDeviceId = null;

        uint? diskSize = null;

        return AddDiskNVMe(ref asi, physicalDriveId, scsiPort, scsiTargetId, scsiBus, target, commandType, ref identify, ref diskSize, pnpDeviceId, ref nVME_PORT_20, ref nVME_PORT_40, ref nVME_ID);
    }




    //private void NVMeSmartToATASmart(byte[] smartReadData, ref SMART_ATTRIBUTE[] attribute)
    //{
    //    throw new NotImplementedException();
    //}


    private void NVMeSmartToATASmart(byte[] NVMeSmartBuf, ref SMART_ATTRIBUTE_LIST ATASmartBufUncasted)
    {
        SMART_ATTRIBUTE_LIST ATASmartBuf = ATASmartBufUncasted;
        int IdxInBuf = 0;
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateCriticalWarningFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateTemperatureFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateAvailableSpareFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateAvailableSpareThresholdFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperatePercentageUsedFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateDataUnitsReadFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateDataUnitsWrittenFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateHostReadCommandsFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateHostWriteCommandsFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateControllerBusyTimeFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperatePowerCyclesFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperatePowerOnHoursFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateUnsafeShutdownsFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateMediaErrorsFrom(NVMeSmartBuf));
        AddToATASmartBuf(ref ATASmartBuf, IdxInBuf++, SeperateNumberOfErrorsFrom(NVMeSmartBuf));
        NVMeTemperatureSensorSmartToATASmart(NVMeSmartBuf, ref ATASmartBuf);
    }

    void NVMeTemperatureSensorSmartToATASmart(byte[] NVMeSmartBuf, ref SMART_ATTRIBUTE_LIST ATASmartBufUncasted)
    {
        SMART_ATTRIBUTE_LIST ATASmartBuf = ATASmartBufUncasted;
        int IdxInBuf = 17;
        const int TemperatureSensorStart = 200;
        const int MaxSensors = 8;

        for (int i = 0; i < MaxSensors; i++)
        {
            SMART_ATTRIBUTE attr;
            Unsafe.SkipInit(out attr);
            attr.Id = (byte)(++IdxInBuf);

            //not tested
            Debugger.Break();
            Buffer.BlockCopy(NVMeSmartBuf, TemperatureSensorStart + i * 2, attr.RawValue, 0, 2);
            //memcpy(attr.RawValue, &NVMeSmartBuf[TemperatureSensorStart + i * 2], 2);
            if (attr.RawValue[0] != 0 || attr.RawValue[1] != 0)
            {
                AddToATASmartBuf(ref ATASmartBuf, IdxInBuf, attr);
            }
        }
    }


    #region MyRegion

    SMART_ATTRIBUTE SeperateCriticalWarningFrom(byte[] NVMeSmartBuf)
    {
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 1;
        attr.RawValue[0] = NVMeSmartBuf[0];
        return attr;
    }

    SMART_ATTRIBUTE SeperateTemperatureFrom(byte[] NVMeSmartBuf)
    {
        const int TemperatureStart = 1;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 2;
        attr.RawValue[0] = NVMeSmartBuf[TemperatureStart];
        attr.RawValue[1] = NVMeSmartBuf[TemperatureStart + 1];
        return attr;
    }

    SMART_ATTRIBUTE SeperateAvailableSpareFrom(byte[] NVMeSmartBuf)
    {
        const int AvailableSpareStart = 3;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 3;
        attr.RawValue[0] = NVMeSmartBuf[AvailableSpareStart];
        return attr;
    }

    SMART_ATTRIBUTE SeperateAvailableSpareThresholdFrom(byte[] NVMeSmartBuf)
    {
        const int AvailableSpareThresholdStart = 4;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 4;
        attr.RawValue[0] = NVMeSmartBuf[AvailableSpareThresholdStart];
        return attr;
    }

    SMART_ATTRIBUTE SeperatePercentageUsedFrom(byte[] NVMeSmartBuf)
    {
        const int PercentageUsedStart = 5;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 5;
        attr.RawValue[0] = NVMeSmartBuf[PercentageUsedStart];
        return attr;
    }

    SMART_ATTRIBUTE SeperateDataUnitsReadFrom(byte[] NVMeSmartBuf)
    {
        const int DataUnitsReadStart = 32;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 6;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, DataUnitsReadStart, attr.RawValue, 0, attr.RawValue.Length);

        //memcpy(attr.RawValue, &NVMeSmartBuf[DataUnitsReadStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateDataUnitsWrittenFrom(byte[] NVMeSmartBuf)
    {
        const int DataUnitsWrittenStart = 48;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 7;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, DataUnitsWrittenStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[DataUnitsWrittenStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateHostReadCommandsFrom(byte[] NVMeSmartBuf)
    {
        const int ReadStart = 64;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 8;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, ReadStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[ReadStart], sizeof(attr.RawValue));

        return attr;
    }

    SMART_ATTRIBUTE SeperateHostWriteCommandsFrom(byte[] NVMeSmartBuf)
    {
        const int WriteStart = 80;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 9;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, WriteStart, attr.RawValue, 0, attr.RawValue.Length);

        //memcpy(attr.RawValue, &NVMeSmartBuf[WriteStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateControllerBusyTimeFrom(byte[] NVMeSmartBuf)
    {
        const int BusyTimeStart = 96;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 10;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, BusyTimeStart, attr.RawValue, 0, attr.RawValue.Length);

        //memcpy(attr.RawValue, &NVMeSmartBuf[BusyTimeStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperatePowerCyclesFrom(byte[] NVMeSmartBuf)
    {
        const int PowerCycleStart = 112;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 11;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, PowerCycleStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[PowerCycleStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperatePowerOnHoursFrom(byte[] NVMeSmartBuf)
    {
        const int PowerOnHoursStart = 128;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 12;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, PowerOnHoursStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[PowerOnHoursStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateUnsafeShutdownsFrom(byte[] NVMeSmartBuf)
    {
        const int UnsafeShutdownsStart = 144;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 13;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, UnsafeShutdownsStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[UnsafeShutdownsStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateMediaErrorsFrom(byte[] NVMeSmartBuf)
    {
        const int MediaErrorsStart = 160;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 14;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, MediaErrorsStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[MediaErrorsStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateNumberOfErrorsFrom(byte[] NVMeSmartBuf)
    {
        const int NumberOfErrorsStart = 176;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 15;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, NumberOfErrorsStart, attr.RawValue, 0, attr.RawValue.Length);
        //memcpy(attr.RawValue, &NVMeSmartBuf[NumberOfErrorsStart], sizeof(attr.RawValue));
        return attr;
    }

    SMART_ATTRIBUTE SeperateWarningCompositeTemperatureTime(byte[] NVMeSmartBuf)
    {
        const int TemperatureTimeStart = 192;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 16;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, TemperatureTimeStart, attr.RawValue, 0, 4);
        //memcpy(attr.RawValue, &NVMeSmartBuf[TemperatureTimeStart], 4);
        return attr;
    }

    SMART_ATTRIBUTE SeperateCriticalCompositeTemperatureTime(byte[] NVMeSmartBuf)
    {
        const int TemperatureTimeStart = 196;
        SMART_ATTRIBUTE attr;
        Unsafe.SkipInit(out attr);
        attr.Id = 17;

        //not tested
        Debugger.Break();
        Buffer.BlockCopy(NVMeSmartBuf, TemperatureTimeStart, attr.RawValue, 0, 4);
        //memcpy(attr.RawValue, &NVMeSmartBuf[TemperatureTimeStart], 4);
        return attr;
    }

    #endregion

    #region MyRegion
    void AddToATASmartBuf(ref SMART_ATTRIBUTE_LIST ATASmartBuf, int IdxInBuf, SMART_ATTRIBUTE AttrToAdd)
    {
        //(*ATASmartBuf)[IdxInBuf] = AttrToAdd;
        ATASmartBuf.Attributes[IdxInBuf] = AttrToAdd;
    }

    bool GetSmartAttributeNVMeJMS586_20(int index, int port, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeJMS586_40(byte index, byte port, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeRealtek9220DP(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeRealtek(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeASMedia(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeJMicron(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeSamsung951(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeSamsung(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeIntelVroc(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeIntelRst(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }

    bool GetSmartAttributeNVMeIntel(int physicalDriveId, int scsiPort, int scsiTargetId, ref ATA_SMART_INFO asi)
    {
        return true;
    }
    #endregion



    bool IsNVMeTemperatureThresholdDefined(byte[] identifyControllerData)
    {
        //354
        short WCTemp = BitConverter.ToInt16(identifyControllerData, 266);
        //354
        short CCTemp = BitConverter.ToInt16(identifyControllerData, 268);

        ////short WCTemp = *(short*)(identifyControllerData + 266);
        ////short CCTemp = *(short*)(identifyControllerData + 268);

        return (WCTemp != 0 || CCTemp != 0);
    }

    bool IsNVMeThermalManagementTemperatureDefined(byte[] identifyControllerData)
    {
        //352
        short minTMT = BitConverter.ToInt16(identifyControllerData, 324);
        //354
        short maxTMT = BitConverter.ToInt16(identifyControllerData, 326);

        ////short minTMT = *(short*)(identifyControllerData + 324);
        ////short maxTMT = *(short*)(identifyControllerData + 326);

        return (minTMT != 0 || maxTMT != 0);
    }




    bool FillSmartData(ref ATA_SMART_INFO asi)
    {
        string? str = default;
        Unsafe.SkipInit(out str);
        asi.AttributeCount = 0;
        int j = 0;
        for (int i = 0; i < MAX_ATTRIBUTE; i++)
        {
            uint rawValue = 0;

            //CopyMemory(ref asi.Attribute[j], ref asi.SmartReadData[i * Marshal.SizeOf(typeof(SMART_ATTRIBUTE)) + 2], (uint)Marshal.SizeOf(typeof(SMART_ATTRIBUTE)));

            MemCpyStructToStruct(ref asi.Attribute[j], ref asi.SmartReadData, (uint)(i * Marshal.SizeOf(typeof(SMART_ATTRIBUTE)) + 2), (uint)Marshal.SizeOf(typeof(SMART_ATTRIBUTE)));

            //memcpy(&(asi.Attribute[j]),
            //    &(asi.SmartReadData[i * sizeof(SMART_ATTRIBUTE) + 2]), sizeof(SMART_ATTRIBUTE));

            if (asi.Attribute[j].Id != 0)
            {
                switch (asi.Attribute[j].Id)
                {
                    case 0x09: // Power on Hours
                        rawValue = (uint)BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                        /*	MAKELONG(
                            MAKEWORD(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1]),
                            MAKEWORD(asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3])
                            );*/
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INDILINX)
                        {
                            rawValue = (uint)(asi.Attribute[j].WorstValue * 256 + asi.Attribute[j].CurrentValue);
                        }
                        // Intel SSD 520 Series and etc...
                        else if (
                            (asi.DetectedTimeUnitType == (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS)
                        || (asi.DetectedTimeUnitType == (uint)POWER_ON_HOURS_UNIT.POWER_ON_HOURS && rawValue >= 0x0DA000)
                        || (asi.Model?.IndexOf(("Intel")) == 0 && rawValue >= 0x0DA000)
                        )
                        {
                            asi.MeasuredTimeUnitType = (uint)POWER_ON_HOURS_UNIT.POWER_ON_MILLI_SECONDS;
                            int value = 0;
                            rawValue = (uint)(value = asi.Attribute[j].RawValue[2] * 256 * 256
                                             + asi.Attribute[j].RawValue[1] * 256
                                             + asi.Attribute[j].RawValue[0] - 0x0DA753); // https://crystalmark.info/bbs/c-board.cgi?cmd=one;no=560;id=diskinfo#560
                            if (value < 0)
                            {
                                rawValue = 0;
                            }
                        }

                        asi.PowerOnRawValue = (int)rawValue;
                        asi.DetectedPowerOnHours = (int)GetPowerOnHours(rawValue, asi.DetectedTimeUnitType);
                        asi.MeasuredPowerOnHours = (int)GetPowerOnHours(rawValue, asi.MeasuredTimeUnitType);
                        break;
                    case 0x0C: // Power On Count
                        rawValue = (uint)BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                        /*MAKELONG(
                            MAKEWORD(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1]),
                            MAKEWORD(asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3])
                            );*/
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INDILINX)
                        {
                            rawValue = (uint)(asi.Attribute[j].WorstValue * 256 + asi.Attribute[j].CurrentValue);
                        }
                        asi.PowerOnCount = rawValue;
                        break;
                    case 0xBE:
                        if (asi.Attribute[j].RawValue[0] > 0 && asi.Attribute[j].RawValue[0] < 100)
                        {
                            asi.Temperature = asi.Attribute[j].RawValue[0];
                        }
                        break;
                    case 0xBF: // Clean PowerOff Count for Sandisk/WD CloudSpeed SSD
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                        {
                            // Use Clean Shutdowns to calculate Power On Count
                            rawValue = (uint)BitConverter.ToInt16(asi.Attribute[j].RawValue, 0);
                            asi.PowerOnCount = rawValue + 1;
                        }
                        break;
                    case 0xC0: // UnClean PowerOff Count for Sandisk/WD CloudSpeed SSD
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                        {
                            // Use UnClean Shutdowns to calculate Power On Count
                            rawValue = (uint)BitConverter.ToInt16(asi.Attribute[j].RawValue, 0);
                            asi.PowerOnCount += rawValue;
                        }
                        break;
                    case 0xC2: // Temperature
                        if (asi.Model?.IndexOf(("SAMSUNG SV")) == 0 && (asi.Attribute[j].RawValue[1] != 0 || asi.Attribute[j].RawValue[0] > 70))
                        {
                            asi.Temperature = BitConverter.ToInt16(asi.Attribute[j].RawValue, 0) / 10;//MAKEWORD(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1]) / 10;			
                        }
                        else if (asi.Attribute[j].RawValue[0] > 0 && asi.TemperatureMultiplier < 1.0)//(asi.DiskVendorId == SSD_VENDOR_SANDFORCE)
                        {
                            asi.Temperature = (int)(uint)(asi.Attribute[j].RawValue[0] * asi.TemperatureMultiplier);
                        }
                        else if (asi.Attribute[j].RawValue[0] > 0)
                        {
                            asi.Temperature = asi.Attribute[j].RawValue[0];
                        }

                        if (asi.Temperature >= 100)
                        {
                            asi.Temperature = -1000;
                        }
                        break;
                    case 0xF3: // Temperature for YMTC
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC)
                        {
                            if (asi.Attribute[j].RawValue[0] > 0)
                            {
                                asi.Temperature = asi.Attribute[j].RawValue[0];
                            }

                            if (asi.Temperature >= 100)
                            {
                                asi.Temperature = -1000;
                            }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); //  65536 * 512 / 1024 / 1024 / 1024;
                        }
                        break;
                    case 0xBB:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MTRON)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xCA:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03 || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xD1:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INDILINX)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xC9:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS)
                        {
                            int life = -1;
                            if (life < 0 || life > 100) { life = -1; }
                            life = asi.Attribute[j].CurrentValue;
                            asi.Life = life;
                        }
                        break;
                    case 0xE6:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                        {
                            int life = -1;
                            if (asi.FlagLifeSanDiskUsbMemory)
                            {
                                life = -1;
                            }
                            else if (asi.FlagLifeSanDisk0_1)
                            {
                                life = 100 - (asi.Attribute[j].RawValue[1] * 256 + asi.Attribute[j].RawValue[0]) / 100;
                            }
                            else if (asi.FlagLifeSanDisk1)
                            {
                                life = 100 - asi.Attribute[j].RawValue[1];
                            }
                            else if (asi.FlagLifeSanDiskLenovo)
                            {
                                life = asi.Attribute[j].CurrentValue;
                            }
                            else
                            {
                                life = 100 - asi.Attribute[j].RawValue[1];
                            }

                            if (life < 0 || life > 100) { life = -1; }

                            asi.Life = life;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL)
                        {
                            int life = -1;
                            if (life < 0 || life > 100) { life = -1; }
                            life = asi.Attribute[j].CurrentValue;
                            asi.Life = life;
                        }
                        break;
                    case 0xE8:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        break;
                    case 0xE9:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX)
                        {
                            if (asi.FlagLifeRawValue)
                            {
                                asi.Life = asi.Attribute[j].RawValue[0];
                            }
                            else
                            {
                                asi.Life = asi.Attribute[j].CurrentValue;
                            }
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        else if ((asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK ||
                                asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO ||
                                asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                            && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);// (int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC)
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK)
                        {
                            if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                            {
                                asi.NandWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 2 / 1024 / 1024);
                            }
                            else
                            {
                                asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                            }
                        }
                        break;
                    case 0xE1:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); //  65536 * 512 / 1024 / 1024 / 1024;
                        }
                        break;
                    case 0xEA:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE
                            || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                            )
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        break;
                    case 0xEB:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        break;
                    case 0xF1:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_GENERAL)
                        {
                            if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 2 / 1024 / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 64);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                            {
                                asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                            }
                            else
                            {

                            }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL_DC)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KIOXIA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PHISON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MARVELL
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL
                            )
                        {
                            if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                            asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 2 / 1024 / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_1MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                            asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 64);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                            {
                                asi.HostWrites = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                            asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                            }
                            else
                            {
                                asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                            }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_APACER || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                 / 32);
                        }
                        else if ((asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK ||
                                asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                            && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        break;
                    case 0xF2:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_GENERAL)
                        {
                            if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 2 / 1024 / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 64);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                            {
                                asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                            }
                            else
                            {
                            }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION_CVC && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MARVELL
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS
                            || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_DELL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL
                            )
                        {
                            if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_512B)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                            asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 2 / 1024 / 1024);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_16MB)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 64);
                            }
                            else if (asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                            {
                                asi.HostReads = (int)(
                                    B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                            asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                    / 32); // 65536 * 512 / 1024 / 1024 / 1024;
                            }
                            else
                            {
                                asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                            }
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PLEXTOR)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 32);
                        }
                        else if ((asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK ||
                            asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                            && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK)
                        {
                            asi.HostReads = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        break;
                    case 0xF9:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_INTEL || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_WDC || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_GB)
                        || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_HP_VENUS || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_LENOVO_HELEN_VENUS)
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 64 / 1024); // 16 / 1024 / 1024
                        }
                        break;
                    case 0xFA:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK)
                        {
                            asi.NandWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        break;
                    case 0x64:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE)
                        {
                            asi.GBytesErased = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        break;
                    case 0xAD:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_TOSHIBA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KIOXIA)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue - 100;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xB1:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SAMSUNG)
                        {
                            asi.WearLevelingCount = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);
                            /*(int)MAKELONG(
                            MAKEWORD(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1]),
                            MAKEWORD(asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3])
                            );*/
                            asi.Life = asi.Attribute[j].CurrentValue;
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xE7:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDFORCE || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_CORSAIR || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SKHYNIX || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK
                        || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SSSTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_APACER || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_JMICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_PHISON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SEAGATE
                        || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MAXIOTEK || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_YMTC || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_ADATA_INDUSTRIAL)
                        {
                            if (asi.FlagLifeNoReport)
                            {
                                asi.Life = -1;
                            }
                            else if (asi.FlagLifeRawValueIncrement)
                            {
                                asi.Life = 100 - asi.Attribute[j].RawValue[0];
                            }
                            else if (asi.FlagLifeRawValue)
                            {
                                asi.Life = asi.Attribute[j].RawValue[0];
                            }
                            else
                            {
                                asi.Life = asi.Attribute[j].CurrentValue;
                            }
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xA9:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_REALTEK || (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB) || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                        {
                            if (asi.FlagLifeRawValueIncrement)
                            {
                                asi.Life = 100 - asi.Attribute[j].RawValue[0];
                            }
                            else if (asi.FlagLifeRawValue)
                            {
                                asi.Life = asi.Attribute[j].RawValue[0];
                            }
                            else
                            {
                                asi.Life = asi.Attribute[j].CurrentValue;
                            }
                            if (asi.Life < 0 || asi.Life > 100) { asi.Life = -1; }
                        }
                        break;
                    case 0xC6:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                        {
                            asi.HostReads = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        break;
                    case 0xC7:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_OCZ_VECTOR)
                        {
                            asi.HostWrites = BitConverter.ToInt32(asi.Attribute[j].RawValue, 0);//(int)B8toB32(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2], asi.Attribute[j].RawValue[3]);
                        }
                        break;
                    case 0xF5:
                        // Percent Drive Life Remaining (SanDisk/WD CloudSpeed)
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SANDISK_CLOUD)
                        {
                            asi.Life = asi.Attribute[j].CurrentValue;
                        }

                        // NAND Page Size = 8KBytes
                        // http://www.overclock.net/t/1145150/official-crucial-ssd-owners-club/1290
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                * 8 / 1024 / 1024);
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                ) / 32;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_KINGSTON && asi.HostReadsWritesUnit == HOST_READS_WRITES_UNIT.HOST_READS_WRITES_32MB)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                ) / 32;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SILICONMOTION)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                ) / 32;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_SCY)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                ) / 32;
                        }
                        else if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_RECADATA)
                        {
                            asi.NandWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                    asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                );
                        }
                        break;
                    case 0xF6:
                        if (asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON || asi.DiskVendorId == (uint)VENDOR_ID.SSD_VENDOR_MICRON_MU03)
                        {
                            asi.HostWrites = (int)(
                                B8toB64(asi.Attribute[j].RawValue[0], asi.Attribute[j].RawValue[1], asi.Attribute[j].RawValue[2],
                                        asi.Attribute[j].RawValue[3], asi.Attribute[j].RawValue[4], asi.Attribute[j].RawValue[5])
                                / 2 / 1024 / 1024);
                        }
                        break;
                    default:
                        break;
                }
                j++;
            }
        }
        asi.AttributeCount = (uint)j;

        if (asi.AttributeCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


        //return true;
    }

    //typedef bool(__stdcall* A_AMD_RC2_GetSmartData)(UINT diskNum, BYTE* SmartReadData, UINT SmartReadDataLen, BYTE* SmartReadThreshold, UINT SmartReadThresholdLen);
    //A_AMD_RC2_GetSmartData AMD_RC2_GetSmartData = NULL;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate bool A_AMD_RC2_GetSmartData(uint diskNum, byte[] SmartReadData, uint SmartReadDataLen, byte[] SmartReadThreshold, uint SmartReadThresholdLen);
    A_AMD_RC2_GetSmartData? AMD_RC2_GetSmartData = null;

    bool GetSmartDataAMD_RC2(int diskNum, ref ATA_SMART_INFO asi)
    {
#if !(_M_ARM) && !(_M_ARM64)
#if AMD_RC2
            if (!g_AMD_RC2_init) AMD_RC2_DLL_Load();
            if (!AMD_RC2_GetSmartData) return false;
#endif
        if (AMD_RC2_GetSmartData is not null && !AMD_RC2_GetSmartData((uint)diskNum, asi.SmartReadData, READ_ATTRIBUTE_BUFFER_SIZE, asi.SmartReadThreshold, READ_THRESHOLD_BUFFER_SIZE))
        {
            return false;
        }
        if (asi.IsNVMe) return true;
        return FillSmartData(ref asi);
#else
            return false;
#endif



    }

}
