﻿using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using DiskInfoDotnet;
using DiskInfoDotnetParse.Shared.CheckDiskInfos;
using HelperClass;
using Newtonsoft.Json;
using DiskInfoDotnet.Sm.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

internal class Program
{
    [STAThread]
    private static async Task Main(string[] args)
    {

        ReadOnlyCollectionBuilder<object> SmmanagerList;
        ReadOnlyCollectionBuilder<object> atareadOnlyCollectionBuilder = [];

        bool NativeTest = false;
        string DiskInfoExtractedTime = string.Empty;

        var options = new Cac.Options() { args = args };
        var cmParser = new Cmd.CommandLineParser() { cacOptions = options };
        cmParser.Parse(options);

        //cmParser.cacOptions.outPutInfos = OutPutInfos.OptimizedInfos;

        ExtractionType extractionType = ExtractionType.None;

        if (extractionType == ExtractionType.StaticExtraction)
        {
            LoadMScopModule loadMScopModule;
            if (cmParser.cacOptions.outPutInfos == OutPutInfos.OptimizedInfos)
                InfoExtractorStatic.ExtractOptimizedInfos(out var vals, out loadMScopModule);
            else if (cmParser.cacOptions.outPutInfos == OutPutInfos.ExtendedInfos)
                InfoExtractorStatic.ExtractOptimizedInfos(out var vals, out loadMScopModule);
            else if (cmParser.cacOptions.outPutInfos == OutPutInfos.FullInfos)
                goto NextStep;
            else
                throw new GettingExceptions(typeof(Program), "invalid outputTypes");

            if (loadMScopModule is null)
                throw new GettingExceptions(typeof(InfoExtractorStatic), "LoadMScopModule null");

            StaticMethods.GetSMManagerList(out SmmanagerList, loadMScopModule);
            DiskInfoExtractedTime = InfoExtractorStatic.extractTime ?? throw new GettingExceptions(typeof(InfoExtractorStatic), "invalid extraction");
            goto OutPutFullInformation;
        }
        else
        {
            goto NextStep;
        }

    //return;

    NextStep:
        {
            InfoExtractorClass extractorClass = new InfoExtractorClass()
            {
                args = args,
                extractionType = extractionType,
                NativeTest = false
            };

            extractorClass.ExtractSmManagement(out SmmanagerList, out var loadMScopModule);

            object ataLists;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            Unsafe.SkipInit(out ataLists);
            if ((Attribute.GetCustomAttribute(typeof(MainEntry), typeof(HelperClass.WindowsVersionCheckerAttribute)) is HelperClass.WindowsVersionCheckerAttribute winvAttr
                && winvAttr is not null
                && winvAttr.WindowsVesionChecker()
                && loadMScopModule.win32_DiskDrive_Infos_List.Count is not 0
                ))
            {
                MainEntry.Run(NativeTest, winvAttr, out ataLists, loadMScopModule.win32_DiskDrive_Infos_List, args);
                stopwatch.Stop();
                DiskInfoExtractedTime = stopwatch.Elapsed.ToString();
            }



            else throw new GettingExceptions(typeof(Program), "invalid ataLists");



            if (ataLists is not null)
            {
                if (cmParser.cacOptions.outPutInfos == OutPutInfos.ExtendedInfos || cmParser.cacOptions.outPutInfos == OutPutInfos.OptimizedInfos)
                {
                    CrystalDiskInfoDotnetLoadInformation crystalDiskInfoDotnet = new CrystalDiskInfoDotnetLoadInformation()
                    {
                        ataInfos = ataLists,
                        InfoType = cmParser.cacOptions.outPutInfos
                    };

                    crystalDiskInfoDotnet.LoadInformation(out var infoForCasts, out var OptimizedListBuilder);

                    if (infoForCasts != null)
                        atareadOnlyCollectionBuilder.Add(infoForCasts);
                    else if (OptimizedListBuilder != null)
                        atareadOnlyCollectionBuilder.Add(OptimizedListBuilder);

                    await OutPutFullInfos(null, atareadOnlyCollectionBuilder);
                    goto OutPutCastInformation;
                }
                else if (cmParser.cacOptions.outPutInfos == OutPutInfos.FullInfos)
                {
                    atareadOnlyCollectionBuilder.Add(ataLists);
                    if (atareadOnlyCollectionBuilder.Count is 0)
                        throw new GettingExceptions(typeof(Program), "0 list");
                    goto OutPutFullInformation;
                }
                else
                {
                    throw new GettingExceptions(typeof(Program), "invalid outputTypes");
                }
            }
            else
            {
                Debugger.Break();
                throw new GettingExceptions(typeof(Program), "ata infos null");
            }


        }

    OutPutFullInformation:

        await OutPutFullInfos(SmmanagerList, atareadOnlyCollectionBuilder);

    OutPutCastInformation:
        Console.WriteLine($"DiskInfo Extracted Time: {DiskInfoExtractedTime}");

        CastInfos.GenereteCastInfos();


        Console.ReadLine();

    }

    static async Task OutPutFullInfos(ReadOnlyCollectionBuilder<object> SmmanagerList, ReadOnlyCollectionBuilder<object> readOnlyCollectionBuilder)
    {
        IEnumerable<object> unionList;
        if (SmmanagerList is not null)
        {
            unionList = SmmanagerList.Union(readOnlyCollectionBuilder);
        }
        else if (readOnlyCollectionBuilder is not null)
        {
            unionList = readOnlyCollectionBuilder;
        }
        else
        {
            throw new GettingExceptions(typeof(Program), "invalid list");
        }
        foreach (var item in unionList)
        {

            if (item is IEnumerable objects and not null)
            {

                var enumerator = objects.GetEnumerator();
                if (enumerator.MoveNext())
                {
                    do
                    {
                        var current = enumerator.Current;

                        if (current is not null and ObservableCollection<KeyValuePair<string, string>> valuePairs)
                        {
                            foreach (var valuePair in valuePairs)
                            {
                                var valKey = valuePair.Key;
                                var valValue = valuePair.Value;
                                Console.WriteLine(valKey + " " + valValue);
                            }
                            
                        }

                        else if (current is not null)
                        {
                            
                            var json = JsonConvert.SerializeObject(current, Formatting.Indented);
                            Console.WriteLine(json);
                        }
                        else throw new GettingExceptions(typeof(Program), "invalid collection");
                        //await Task.Delay(500);

                        Console.WriteLine(Environment.NewLine + Environment.NewLine);
                    }
                    while (enumerator.MoveNext());
                }
            }
            else if (item is not null && Attribute.IsDefined(item.GetType(), typeof(Win32_Attribute)))
            {
                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                Console.WriteLine(json);
                //await Task.Delay(500);
            }
            else
            {
                throw new GettingExceptions(typeof(Program), "item is null or not valid type");
            }
        }

    }



}



//using CrystalDiskInfoDotnet;
//using DiskInfoDotnetParse.Shared.CheckDiskInfos;
//using HelperClass;
//using Newtonsoft.Json;
//using DiskInfoDotnet.Sm.Management;
//using System;
//using System.Collections;
//using System.Linq;
//using System.Threading.Tasks;

//internal class Program
//{
//    [STAThread]
//    private static void Main(string[] args)
//    {
//        try
//        {
//            var options = new Cac.Options() { args = args };
//            var cmParser = new Cmd.CommandLineParser() { cacOptions = options };
//            cmParser.Parse(options);

//            InfoExtractorClass extractorClass = new InfoExtractorClass()
//            {
//                args = args,
//                extractionType = ExtractionType.None,
//                NativeTest = false,
//                InfoType = cmParser.cacOptions.outPutInfos
//            };

//            extractorClass.ExtractSmManagement(out var SmmanagerList, out var loadMScopModule);

//            if (SmmanagerList.Count is 0)
//            {
//                throw new GettingExceptions(typeof(Program), "SmmanagerList.Count is 0");
//            }


//            extractorClass.ExtractDiskInfoSecondWay(out var DiskInfoList, loadMScopModule, out var diskInfoExtractedTime);

//            foreach (var item in SmmanagerList.Union(DiskInfoList))
//            {
//                if (item is IEnumerable objects and not null)
//                {
//                    var enumerator = objects.GetEnumerator();
//                    if (enumerator.MoveNext())
//                    {
//                        do
//                        {
//                            var current = enumerator.Current;
//                            if (current is not null)
//                            {
//                                var json = JsonConvert.SerializeObject(current, Formatting.Indented);
//                                Console.WriteLine(json);
//                            }
//                        }
//                        while (enumerator.MoveNext());
//                    }
//                }
//                else if (item is not null && Attribute.IsDefined(item.GetType(), typeof(Win32_Attribute)))
//                {
//                    var json = JsonConvert.SerializeObject(item, Formatting.Indented);
//                    Console.WriteLine(json);
//                }
//                else
//                {
//                    throw new GettingExceptions(typeof(Program), "item is null or not valid type");
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine(ex.Message);
//        }

//        Console.ReadLine();

//    }



//}