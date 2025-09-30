namespace DiskInfoDotnet.Sm.Management;

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management;
using Nj = Newtonsoft.Json;

public class LoadMScopModule
{
    //Win32_BIOS_Infos
    public ObservableCollection<Win32_DiskDrive_Infos>? win32_DiskDrive_Infos_List { get; protected set; }
    public ObservableCollection<Win32_OperatingSystem_Infos>? win32_OperatingSystem_Infos_List { get; protected set; }
    public ObservableCollection<Win32_ComputerSystem_Infos>? win32_ComputerSystem_Infos_List { get; protected set; }
    public ObservableCollection<Win32_Processor_Infos>? win32_Processor_Infos_List { get; protected set; }
    public ObservableCollection<Win32_BIOS_Infos>? Win32_BIOS_Infos_List { get; protected set; }
    public ObservableCollection<Win32_USBHub_Infos>? Win32_USBHub_Infos_List { get; protected set; }
    public ObservableCollection<SystemInformationItem>? SystemInformationItems { get; protected set; }

    public LoadMScopModule()
    {
        SystemInformationItems = [];
        win32_DiskDrive_Infos_List = [];
        win32_OperatingSystem_Infos_List = [];
        win32_ComputerSystem_Infos_List = [];
        win32_Processor_Infos_List = [];
        Win32_BIOS_Infos_List = [];
        Win32_USBHub_Infos_List = []; 
    }

    public static LoadMScopModule Create()
    {
        return new LoadMScopModule();
    }

    public bool LoadInfos(bool getDriverInfos)
    {
        LoadManagementDisk();
        GetOperatingSystem();
        GetComputerSystem();
        GetCPU();
        GetBios();
        GetPointingDevice();
        if (getDriverInfos)
            GetDriverVersions();

        return true;
    }

    string JSQueryPersonal(ManagementBaseObject? vals, string intanceStr)
    {
        var _itemInfo = vals?.GetText(TextFormat.Mof);
        var _itemInfoJs = _itemInfo?
            .Replace($"\ninstance of {intanceStr}\n", "").Replace(";", ",").Replace("=", ":");
        _itemInfoJs = _itemInfoJs?.Remove(_itemInfoJs.Length - 2).Replace("FALSE", "false").Replace("TRUE", "true");
        _itemInfoJs = _itemInfoJs?.Replace(": {", ": [").Replace("},", "],");

        return _itemInfoJs ?? throw new HelperClass.GettingExceptions($"get exception at {nameof(LoadMScopModule)}");

        #region MyRegion
        //var _driveInfoJs = vals.Insert(vals.IndexOf('}', 0), "]");
        //_driveInfoJs = _driveInfoJs.Replace("]},", "],");

        //_driveInfoJs = _driveInfoJs.Insert(_driveInfoJs.IndexOf('}', 0), "]");
        //_driveInfoJs = _driveInfoJs.Replace("]},", "],");

        //return _driveInfoJs = _driveInfoJs.Remove(_driveInfoJs.Length - 2).Replace("FALSE", "false").Replace("TRUE", "true"); 
        #endregion
    }

    T JsWorker<T>(ManagementBaseObject? managementBases, string intanceStr)
    {
        if (Nj.JsonConvert.DeserializeObject<T>(JSQueryPersonal(managementBases, intanceStr)) is T win32_OperatingSystem_Infos and not null)
        {
            //values?.Add(win32_OperatingSystem_Infos);
            return win32_OperatingSystem_Infos;
        }

        throw new HelperClass.GettingExceptions($"get exception to this class {nameof(LoadMScopModule)}");
    }

    private void LoadManagementDisk()
    {
        ManagementScope scope = new ManagementScope("\\\\.\\root\\cimv2");
        scope.Connect();
        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
        ManagementObjectCollection queryCollection = searcher.Get();

        foreach (var _drive in queryCollection)
        {
            win32_DiskDrive_Infos_List?.Add(JsWorker<Win32_DiskDrive_Infos>(_drive, "Win32_DiskDrive"));
        }
    }

    private void GetOperatingSystem()
    {
        foreach (ManagementObject item in new ManagementObjectSearcher(new SelectQuery("Win32_OperatingSystem")).Get())
        {
            win32_OperatingSystem_Infos_List?.Add(JsWorker<Win32_OperatingSystem_Infos>(item, "Win32_OperatingSystem"));

            string propertyValue = GetPropertyValue(item.Properties, "Name");
            string propertyValue2 = GetPropertyValue(item.Properties, "Version");
            string propertyValue3 = GetPropertyValue(item.Properties, "ServicePackMajorVersion");
            if (propertyValue != "")
            {
                SystemInformationItem systemInformationItem = new SystemInformationItem();
                systemInformationItem.Field = "OS";
                systemInformationItem.SubField = "NAME";
                systemInformationItem.Value = propertyValue.Substring(0, propertyValue.IndexOf('|'));
                SystemInformationItems?.Add(systemInformationItem);
            }
            if (propertyValue2 != "")
            {
                SystemInformationItem systemInformationItem2 = new SystemInformationItem();
                systemInformationItem2.SubField = "VER";
                systemInformationItem2.Value = propertyValue2;
                SystemInformationItems?.Add(systemInformationItem2);
            }
            if (propertyValue3 != "")
            {
                SystemInformationItem systemInformationItem3 = new SystemInformationItem();
                systemInformationItem3.SubField = "SP";
                systemInformationItem3.Value = propertyValue3;
                SystemInformationItems?.Add(systemInformationItem3);
            }
        }
    }

    private void GetComputerSystem()
    {

        foreach (ManagementObject item in new ManagementObjectSearcher(new SelectQuery("Win32_ComputerSystem")).Get())
        {
            win32_ComputerSystem_Infos_List?.Add(JsWorker<Win32_ComputerSystem_Infos>(item, "Win32_ComputerSystem"));

            string propertyValue = GetPropertyValue(item.Properties, "Manufacturer");
            string propertyValue2 = GetPropertyValue(item.Properties, "Model");
            if (propertyValue != "")
            {
                SystemInformationItem systemInformationItem = new SystemInformationItem();
                systemInformationItem.Field = "COMPUTER";
                systemInformationItem.SubField = "MANU";
                systemInformationItem.Value = propertyValue;
                SystemInformationItems?.Add(systemInformationItem);
            }
            if (propertyValue2 != "")
            {
                SystemInformationItem systemInformationItem2 = new SystemInformationItem();
                systemInformationItem2.SubField = "MOD";
                systemInformationItem2.Value = propertyValue2;
                SystemInformationItems?.Add(systemInformationItem2);
            }
        }
    }

    private void GetCPU()
    {
        foreach (ManagementObject item in new ManagementObjectSearcher(new SelectQuery("Win32_Processor")).Get())
        {
            win32_Processor_Infos_List?.Add(JsWorker<Win32_Processor_Infos>(item, "Win32_Processor"));

            string propertyValue = GetPropertyValue(item.Properties, "Name");
            if (propertyValue != "")
            {
                SystemInformationItem systemInformationItem = new SystemInformationItem();
                systemInformationItem.Field = "CPU";
                systemInformationItem.Value = propertyValue;
                SystemInformationItems?.Add(systemInformationItem);
            }
        }
    }

    private void GetBios()
    {
        foreach (ManagementObject item in new ManagementObjectSearcher(new SelectQuery("Win32_BIOS")).Get())
        {
            Win32_BIOS_Infos_List?.Add(JsWorker<Win32_BIOS_Infos>(item, "Win32_BIOS"));
        }
    }

    private void GetPointingDevice()
    {
        //CimSession session = CimSession.Create("localHost");
        //IEnumerable<CimInstance> queryInstance = session.QueryInstances(@"root\cimv2", "WQL", "SELECT * FROM Win32_ComputerSystem");
        //foreach (CimInstance cimObj in queryInstance)
        //{
        //    Console.WriteLine(cimObj.CimInstanceProperties["Name"].ToString());
        //}


        foreach (ManagementObject item in new ManagementObjectSearcher(new SelectQuery("Win32_USBHub")).Get())
        {
            Win32_USBHub_Infos_List?.Add(JsWorker<Win32_USBHub_Infos>(item, "Win32_USBHub"));
        }
    }

#nullable disable
    private void GetDriverVersions()
    {
        foreach (string item in new List<string> { "Win32_IDEController", "Win32_SCSIController" })
        {
            foreach (ManagementObject item2 in new ManagementObjectSearcher(new SelectQuery(item)).Get())
            {
                string propertyValue = GetPropertyValue(item2.Properties, "DeviceID");
                string propertyValue2 = GetPropertyValue(item2.Properties, "Description");
                if (propertyValue2 != "")
                {
                    SystemInformationItem systemInformationItem = new SystemInformationItem();
                    systemInformationItem.Field = "CONTROLLER";
                    systemInformationItem.SubField = "NAME";
                    systemInformationItem.Value = propertyValue2;
                    SystemInformationItems?.Add(systemInformationItem);
                    if (propertyValue != "")
                    {
                        string name = "System\\CurrentControlSet\\Enum\\" + propertyValue;
                        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
                        object value = registryKey.GetValue("Driver");
                        registryKey.Close();
                        name = "System\\CurrentControlSet\\Control\\Class\\" + value.ToString();
                        RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(name);
                        object value2 = registryKey2.GetValue("DriverVersion");
                        registryKey2.Close();
                        systemInformationItem = new SystemInformationItem();
                        systemInformationItem.SubField = "DRIVERVER";
                        systemInformationItem.Value = value2.ToString();
                        SystemInformationItems?.Add(systemInformationItem);
                    }
                }
            }
        }
    }
#nullable enable

    private string GetPropertyValue(PropertyDataCollection data, string propertyName)
    {
        string? result = "";
        foreach (PropertyData datum in data)
        {
            if (datum is not null && datum.Value is not null && datum.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) && datum.Value != null)
            {
                result = datum.Value.ToString();
                break;
            }
        }
        if (result is not null)
        {
            return result;
        }
        throw new HelperClass.GettingExceptions($"get exception to this class {nameof(LoadMScopModule)}");
    }



}
