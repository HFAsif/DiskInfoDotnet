namespace DiskInfoDotnet.Library;

using DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IDiskInfosWorker
{
    Win32_DiskDrive_Infos win32_DiskDrive_Infos { get; set; }

    bool PrimarySetup();

    void FinalStep(out Dis.ATA_SMART_INFO ObjList);
}