namespace DiskInfoDotnetParse.Shared.CheckDiskInfos;

using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using DiskInfoDotnet;
using HelperClass;
using DiskInfoDotnet.Sm.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

interface IExtractorClass
{
    void ExtractSmManagement(out ReadOnlyCollectionBuilder<object> list, out LoadMScopModule loadMScopModule);

    //void ExtractDiskInfoSecondWay(out ReadOnlyCollectionBuilder<object> readOnlyCollectionBuilder, LoadMScopModule loadMScopModule, out string diskInfoExtractedTime);

    //void ExtractOptimizedInfos(
    //    out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder,
    //    ObservableCollection<Win32_DiskDrive_Infos> win32_DiskDrive_Infos_List,
    //    out object ataLists,
    //    out string extractTime
    //    );
}

[SomeElementsInfos("Inhance load information")]
[InfoExtractorStaticAttributes(GetDriveInfoBool.GetDriveInfos)]
internal class InfoExtractorClass : IExtractorClass
{
    public required string[] args { get; set; }

    public required bool NativeTest { get; set; }

    public required ExtractionType extractionType { get; set; }

    public void ExtractSmManagement(out ReadOnlyCollectionBuilder<object> SmmanagerList, out LoadMScopModule loadMScopModule)
    {
        loadMScopModule = new LoadMScopModule();

        var getCusAttrs = typeof(InfoExtractorClass).GetCustomAttribute<InfoExtractorStaticAttributes>();
        if (getCusAttrs is not null)
        {
            if (!loadMScopModule.LoadInfos(getCusAttrs.GetDriveInfos))
            {
                throw new HelperClass.GettingExceptions("Load Management Scops Failed");
            }
        }
        else
        {
            throw new GettingExceptions(typeof(InfoExtractorStatic), "invalid operation");
        }

        StaticMethods.GetSMManagerList(out SmmanagerList, loadMScopModule);

    }

    

    //public void ExtractDiskInfoSecondWay(out ReadOnlyCollectionBuilder<object> readOnlyCollectionBuilder, LoadMScopModule loadMScopModule, out string diskInfoExtractedTime)
    //{
    //    diskInfoExtractedTime = string.Empty;
    //    readOnlyCollectionBuilder = [];
    //    var stopwatch = new System.Diagnostics.Stopwatch();
    //    stopwatch.Start();

    //    if (extractionType is ExtractionType.None)
    //    {

    //        //if ((Attribute.GetCustomAttribute(typeof(MainEntry), typeof(HelperClass.WindowsVersionCheckerAttribute)) is HelperClass.WindowsVersionCheckerAttribute winvAttr
    //        //    && winvAttr is not null
    //        //    && winvAttr.WindowsVesionChecker()
    //        //    && loadMScopModule.win32_DiskDrive_Infos_List.Count is not 0
    //        //    ))
    //        //{
    //        //    MainEntry.Run(NativeTest, winvAttr, out var ataLists, loadMScopModule.win32_DiskDrive_Infos_List, args);
    //        //    readOnlyCollectionBuilder.Add(ataLists);
    //        //    stopwatch.Stop();
    //        //    var dcEnd = stopwatch.Elapsed;
    //        //    diskInfoExtractedTime = ($"Infos Extracted Ended in {dcEnd}");
    //        //}

    //    }
    //}

    //public void ExtractOptimizedInfos(
    //    out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder,
    //    ObservableCollection<Win32_DiskDrive_Infos> win32_DiskDrive_Infos_List,
    //    out object ataLists,
    //    out string extractTime
    //    )
    //{
    //    OptimizedListBuilder = [];
    //    var stopwatch = new System.Diagnostics.Stopwatch();
    //    stopwatch.Start();
    //    MainEntry.Run(out ataLists, win32_DiskDrive_Infos_List);
    //    stopwatch.Stop();
    //    extractTime = stopwatch.Elapsed.ToString();


    //    CrystalDiskInfoDotnetLoadInformation crystalDiskInfoDotnet = new CrystalDiskInfoDotnetLoadInformation()
    //    { ataInfos = ataLists, InfoType = InfoType };
    //    //crystalDiskInfoDotnet.LoadInformation(out infoForCasts, out OptimizedListBuilder);
    //}
}
