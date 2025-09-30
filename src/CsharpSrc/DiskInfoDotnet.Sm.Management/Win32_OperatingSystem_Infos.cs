namespace DiskInfoDotnet.Sm.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[Win32_]
public record Win32_OperatingSystem_Infos
{
    public string? BootDevice { get; set; }
    public string? BuildNumber { get; set; }
    public string? BuildType { get; set; }
    public string? Caption { get; set; }
    public string? CodeSet { get; set; }
    public string? CountryCode { get; set; }
    public string? CreationClassName { get; set; }
    public string? CSCreationClassName { get; set; }
    public string? CSName { get; set; }
    public int? CurrentTimeZone { get; set; }
    public bool? DataExecutionPrevention_32BitApplications { get; set; }
    public bool? DataExecutionPrevention_Available { get; set; }
    public bool? DataExecutionPrevention_Drivers { get; set; }
    public int? DataExecutionPrevention_SupportPolicy { get; set; }
    public bool? Debug { get; set; }
    public string? Description { get; set; }
    public bool? Distributed { get; set; }
    public int? EncryptionLevel { get; set; }
    public int? ForegroundApplicationBoost { get; set; }
    public string? FreePhysicalMemory { get; set; }
    public string? FreeSpaceInPagingFiles { get; set; }
    public string? FreeVirtualMemory { get; set; }
    public string? InstallDate { get; set; }
    public string? LastBootUpTime { get; set; }
    public string? LocalDateTime { get; set; }
    public string? Locale { get; set; }
    public string? Manufacturer { get; set; }
    public long? MaxNumberOfProcesses { get; set; }
    public string? MaxProcessMemorySize { get; set; }
    public List<string>? MUILanguages { get; set; }
    public string? Name { get; set; }
    public int? NumberOfLicensedUsers { get; set; }
    public int? NumberOfProcesses { get; set; }
    public int? NumberOfUsers { get; set; }
    public int? OperatingSystemSKU { get; set; }
    public string? OSArchitecture { get; set; }
    public int? OSLanguage { get; set; }
    public int? OSProductSuite { get; set; }
    public int? OSType { get; set; }
    public bool? PortableOperatingSystem { get; set; }
    public bool? Primary { get; set; }
    public int? ProductType { get; set; }
    public string? RegisteredUser { get; set; }
    public string? SerialNumber { get; set; }
    public int? ServicePackMajorVersion { get; set; }
    public int? ServicePackMinorVersion { get; set; }
    public string? SizeStoredInPagingFiles { get; set; }
    public string? Status { get; set; }
    public int? SuiteMask { get; set; }
    public string? SystemDevice { get; set; }
    public string? SystemDirectory { get; set; }
    public string? SystemDrive { get; set; }
    public string? TotalVirtualMemorySize { get; set; }
    public string? TotalVisibleMemorySize { get; set; }
    public string? Version { get; set; }
    public string? WindowsDirectory { get; set; }
}
