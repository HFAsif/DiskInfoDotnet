namespace DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Win32_]
public record Win32_USBHub_Infos
{
    public string? Caption { get; set; }
    public int? ConfigManagerErrorCode { get; set; }
    public bool? ConfigManagerUserConfig { get; set; }
    public string? CreationClassName { get; set; }
    public string? Description { get; set; }
    public string? DeviceID { get; set; }
    public string? Name { get; set; }
    public string? PNPDeviceID { get; set; }
    public string? Status { get; set; }
    public string? SystemCreationClassName { get; set; }
    public string? SystemName { get; set; }
}
