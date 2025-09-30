namespace DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Win32_]
public record Win32_BIOS_Infos
{
    public List<int>? BiosCharacteristics { get; set; }
    public List<string>? BIOSVersion { get; set; }
    public string? Caption { get; set; }
    public string? Description { get; set; }
    public int? EmbeddedControllerMajorVersion { get; set; }
    public int? EmbeddedControllerMinorVersion { get; set; }
    public string? Manufacturer { get; set; }
    public string? Name { get; set; }
    public bool? PrimaryBIOS { get; set; }
    public string? ReleaseDate { get; set; }
    public string? SerialNumber { get; set; }
    public string? SMBIOSBIOSVersion { get; set; }
    public int? SMBIOSMajorVersion { get; set; }
    public int? SMBIOSMinorVersion { get; set; }
    public bool? SMBIOSPresent { get; set; }
    public string? SoftwareElementID { get; set; }
    public int? SoftwareElementState { get; set; }
    public string? Status { get; set; }
    public int? SystemBiosMajorVersion { get; set; }
    public int? SystemBiosMinorVersion { get; set; }
    public int? TargetOperatingSystem { get; set; }
    public string? Version { get; set; }
}
