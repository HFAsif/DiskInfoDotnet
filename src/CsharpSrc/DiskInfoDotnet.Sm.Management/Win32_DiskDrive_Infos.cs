

namespace DiskInfoDotnet.Sm.Management;

[Win32_]
public record Win32_DiskDrive_Infos
{
    public int? BytesPerSector { get; set; }
    public int[]? Capabilities { get; set; }
    public string[]? CapabilityDescriptions { get; set; }
    public string? Caption { get; set; }
    public int ConfigManagerErrorCode { get; set; }
    public bool ConfigManagerUserConfig { get; set; }
    public string? CreationClassName { get; set; }
    public string? Description { get; set; }
    public string? DeviceID { get; set; }
    public string? FirmwareRevision { get; set; }
    public int Index { get; set; }
    public string? InterfaceType { get; set; }
    public string? Manufacturer { get; set; }
    public bool MediaLoaded { get; set; }
    public string? MediaType { get; set; }
    public string? Model { get; set; }
    public string? Name { get; set; }
    public int Partitions { get; set; }
    public string? PNPDeviceID { get; set; }
    public int SCSIBus { get; set; }
    public int SCSILogicalUnit { get; set; }
    public int SCSIPort { get; set; }
    public int SCSITargetId { get; set; }
    public int SectorsPerTrack { get; set; }
    public string? SerialNumber { get; set; }
    public string? Size { get; set; }
    public string? Status { get; set; }
    public string? SystemCreationClassName { get; set; }
    public string? SystemName { get; set; }
    public string? TotalCylinders { get; set; }
    public int TotalHeads { get; set; }
    public string? TotalSectors { get; set; }
    public string? TotalTracks { get; set; }
    public int TracksPerCylinder { get; set; }
}
