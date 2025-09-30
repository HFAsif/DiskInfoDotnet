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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;



[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
class InfoExtractorStaticAttributes : System.Attribute
{
    public bool GetDriveInfos { get; }
    public InfoExtractorStaticAttributes(bool getDriveInfos) => GetDriveInfos = getDriveInfos;
}

[SomeElementsInfos("Static load information")]
[InfoExtractorStaticAttributes(GetDriveInfoBool.GetDriveInfos)]
internal static class InfoExtractorStatic
{
    static readonly ReadOnlyCollectionBuilder<object> smManagerList;
    static LoadMScopModule loadMScopModule;
    public static readonly string extractTime;
    static readonly object ataLists;

    static InfoExtractorStatic()
    {

        loadMScopModule = LoadMScopModule.Create();
        smManagerList = new ReadOnlyCollectionBuilder<object>();
        var getCusAttrs = typeof(InfoExtractorStatic).GetCustomAttribute<InfoExtractorStaticAttributes>();
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

        var stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        MainEntry.Run(out ataLists, loadMScopModule.win32_DiskDrive_Infos_List);
        stopwatch.Stop();
        extractTime = stopwatch.Elapsed.ToString();
    }

    public static void ExtractOptimizedInfos(out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder, out LoadMScopModule loadMScopModule)
    {
        loadMScopModule = InfoExtractorStatic.loadMScopModule ?? throw new GettingExceptions(typeof(InfoExtractorStatic), $"{nameof(LoadMScopModule) } null");
        DynamicCast(OutPutInfos.OptimizedInfos, out OptimizedListBuilder);
    }

    public static void ExtractFullInfos(out ObservableCollection<ExtendedInfosStruct> vals, out LoadMScopModule loadMScopModule)
    {
        loadMScopModule = InfoExtractorStatic.loadMScopModule ?? throw new GettingExceptions(typeof(InfoExtractorStatic), $"{nameof(LoadMScopModule)} null");
        DynamicCast(OutPutInfos.ExtendedInfos, out vals);
    }

    static void DynamicCast<T>(OutPutInfos outPutInfos, out T vals)
    {
        ExtracIFullInfos(outPutInfos, out var OptimizedListBuilder, out var extendedInfos);

        if (outPutInfos == OutPutInfos.OptimizedInfos && OptimizedListBuilder is T optList)
        {
            vals = optList;
        }
        else if (outPutInfos == OutPutInfos.ExtendedInfos && extendedInfos is T exInfos)
        {
            vals = exInfos;
        }
        else throw new GettingExceptions("invalid typecast ");
    }

    static void ExtracIFullInfos(
        OutPutInfos outPutInfos,
        out ReadOnlyCollectionBuilder<ObservableCollection<KeyValuePair<string, string>>> OptimizedListBuilder,
        out ObservableCollection<ExtendedInfosStruct> extendedInfos
        )
    {
        var crystalDiskInfoDotnet = new CrystalDiskInfoDotnetLoadInformation()
        {
            ataInfos = ataLists,
            InfoType = outPutInfos
        };

        crystalDiskInfoDotnet.LoadInformation(out extendedInfos, out OptimizedListBuilder);
    }

}
