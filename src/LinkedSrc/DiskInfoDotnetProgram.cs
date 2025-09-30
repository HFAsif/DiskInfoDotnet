﻿#region ProgramDemo

using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using DiskInfoDotnet;
using DiskInfoDotnetParse.Shared.CheckDiskInfos;
using HelperClass;

#if LoggerExist
using Microsoft.Extensions.Logging;
#endif

using Newtonsoft.Json;
using DiskInfoDotnet.Sm.Management;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
#if LoggerExist
using System.Threading.Tasks;
#endif

internal class Program
{
    [STAThread]
    private static

#if LoggerExist
        async Task
#else
        void
#endif
        Main(string[] args)
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

#if LoggerExist
        using var loggerFactory = LoggerFactory.Create(static builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("DiskInfoArtificial.Test.Program", LogLevel.Debug)
                .AddConsole();
        });

        var logger = loggerFactory.CreateLogger<Program>();
#endif

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

#if LoggerExist
                    await
#endif
                        OutPutFullInfos(null, atareadOnlyCollectionBuilder
#if LoggerExist
                        , logger
#endif

                        );
                    if(args.Length is 0)
                    {
                        goto OutPutFullInformation;
                    }
                    else
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
#if LoggerExist
        await
#endif
        OutPutFullInfos(SmmanagerList, atareadOnlyCollectionBuilder
#if LoggerExist
            , logger
#endif
            );

    OutPutCastInformation:

#if LoggerExist
        logger.LogInformation($"DiskInfo Extracted Time: {DiskInfoExtractedTime}");
        CastInfos.GenereteCastInfos(loggerFactory.CreateLogger<CastInfos>());
#else
        Console.WriteLine($"DiskInfo Extracted Time: {DiskInfoExtractedTime}");
        CastInfos.GenereteCastInfos();
#endif





        Console.ReadLine();

    }

    private static
#if LoggerExist
        async Task
#else
        void
#endif
        OutPutFullInfos(ReadOnlyCollectionBuilder<object> SmmanagerList, ReadOnlyCollectionBuilder<object> readOnlyCollectionBuilder
#if LoggerExist
            , ILogger logger
#endif
        )
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
#if LoggerExist
                                logger.LogInformation(valKey + " " + valValue);
#else
                                Console.WriteLine(valKey + " " + valValue);
#endif
                            }

                        }

                        else if (current is not null)
                        {
#if LoggerExist
                            logger.LogInformation(Environment.NewLine + Environment.NewLine);
                            var json = JsonConvert.SerializeObject(current, Formatting.Indented);
                            logger.LogInformation(json);
#else
                            
                            var json = JsonConvert.SerializeObject(current, Formatting.Indented);
                            Console.WriteLine(json);
#endif

                        }
                        else throw new GettingExceptions(typeof(Program), "invalid collection");
#if LoggerExist
                        await Task.Delay(500);
#endif
                        Console.WriteLine(Environment.NewLine + Environment.NewLine);
                    }
                    while (enumerator.MoveNext());
                }
            }
            else if (item is not null && Attribute.IsDefined(item.GetType(), typeof(Win32_Attribute)))
            {
                var json = JsonConvert.SerializeObject(item, Formatting.Indented);
#if LoggerExist
                logger.LogInformation(json);
#else
                Console.WriteLine(json);
#endif

#if LoggerExist
                await Task.Delay(500);
#endif
            }
            else
            {
                throw new GettingExceptions(typeof(Program), "item is null or not valid type");
            }
        }

    }



} 
#endregion