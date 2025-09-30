namespace DiskInfoDotnet.Sm.Management;
using System.Collections.Generic;
[Win32_]
public record Win32_ComputerSystem_Infos
{
    public int? AdminPasswordStatus { get; set; }
    public bool? AutomaticManagedPagefile { get; set; }
    public bool? AutomaticResetBootOption { get; set; }
    public bool? AutomaticResetCapability { get; set; }
    public bool? BootROMSupported { get; set; }
    public List<int>? BootStatus { get; set; }
    public string? BootupState { get; set; }
    public string? Caption { get; set; }
    public int? ChassisBootupState { get; set; }
    public string? ChassisSKUNumber { get; set; }
    public string? CreationClassName { get; set; }
    public int? CurrentTimeZone { get; set; }
    public string? Description { get; set; }
    public string? DNSHostName { get; set; }
    public string? Domain { get; set; }
    public int? DomainRole { get; set; }
    public bool? EnableDaylightSavingsTime { get; set; }
    public int? FrontPanelResetStatus { get; set; }
    public bool? HypervisorPresent { get; set; }
    public bool? InfraredSupported { get; set; }
    public int? KeyboardPasswordStatus { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? Name { get; set; }
    public bool? NetworkServerModeEnabled { get; set; }
    public int? NumberOfLogicalProcessors { get; set; }
    public int? NumberOfProcessors { get; set; }
    public List<string>? OEMStringArray { get; set; }
    public bool? PartOfDomain { get; set; }
    public string? PauseAfterReset { get; set; }
    public int? PCSystemType { get; set; }
    public int? PCSystemTypeEx { get; set; }
    public int? PowerOnPasswordStatus { get; set; }
    public int? PowerState { get; set; }
    public int? PowerSupplyState { get; set; }
    public string? PrimaryOwnerName { get; set; }
    public int? ResetCapability { get; set; }
    public int? ResetCount { get; set; }
    public int? ResetLimit { get; set; }
    public List<string>? Roles { get; set; }
    public string? Status { get; set; }
    public string? SystemFamily { get; set; }
    public string? SystemSKUNumber { get; set; }
    public string? SystemType { get; set; }
    public int? ThermalState { get; set; }
    public string? TotalPhysicalMemory { get; set; }
    public string? UserName { get; set; }
    public int? WakeUpType { get; set; }
    public string? Workgroup { get; set; }
}
