#region ProgramDemo

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

#if LoggerExist
    readonly static ILoggerFactory loggerFactory;
    readonly static ILogger logger;
#endif

    static Program()
    {
#if LoggerExist
        var _ = CreateLogger<Program>();
        loggerFactory = _.Item1;
        logger = _.Item2;
#endif
    }

    
    [System.STAThread]
    private static
#if LoggerExist
        async Task
#else
        void
#endif
    MainEx(string[] args)
    {
        var IsElevated = DiskInfoDotnet.MainEntry.Run(out var ataLists, out var loadMScopModule, out var extractionResult, true);
        DiskInfoDotnet.Sm.Management.Sm_StaticViews.GetSMManagerList(out var SmmanagerList, loadMScopModule);
#if LoggerExist
        logger.LogInformation(extractionResult);
        logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(SmmanagerList, Newtonsoft.Json.Formatting.Indented));
        logger.LogInformation(System.Environment.NewLine + "Extracting Optimized Infos " + System.Environment.NewLine);
#else
        System.Console.WriteLine(extractionResult);
        System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(SmmanagerList, Newtonsoft.Json.Formatting.Indented));
        System.Console.WriteLine(System.Environment.NewLine + "Extracting Optimized Infos {0} ", System.Environment.NewLine);
#endif

        if (IsElevated && ataLists is System.Collections.IEnumerable collection)
        {
            foreach (var item in collection)
            {
                item.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).ToList().ForEach(field =>
                {
                    if (new Type[] { typeof(ushort), typeof(uint), typeof(ulong), typeof(byte), typeof(string), typeof(int) }.Contains(field.FieldType)
                            && !string.IsNullOrEmpty(field.GetValue(item)?.ToString()) && field.GetValue(item)?.ToString() is string)
                    {
                        if ((field.GetValue(item)?.ToString()).Contains("-")) return;
                        if ((field.GetValue(item)?.ToString()).All(System.Char.IsDigit) && int.TryParse((field.GetValue(item)?.ToString()), out var rs) && rs is 0) return;
#if LoggerExist
                        logger.LogInformation(field.Name + "  " + ((field.GetValue(item)?.ToString()) ?? "null"));
#else
                        System.Console.WriteLine(field.Name + "  " + ((field.GetValue(item)?.ToString()) ?? "null"));
#endif
                    }
                });
#if LoggerExist
                logger.LogInformation(System.Environment.NewLine);
                await Task.Delay(500);
#else
                System.Console.WriteLine(System.Environment.NewLine);
#endif

            }
        }


#if !LoggerExist
        System.Console.ReadLine();
#endif

    }

#if LoggerExist
    static (ILoggerFactory, ILogger) CreateLogger<T>() 
    {
        using (var loggerFactory = LoggerFactory.Create(static builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("DiskInfoArtificial.Test.Program", LogLevel.Debug)
                .AddConsole();
        }))
        {
            var logger = loggerFactory.CreateLogger<T>();
            return (loggerFactory, logger);
        }
            
    }
#endif

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

//#if LoggerExist
//        var _ = CreateLogger<Program>();
//        var loggerFactory = _.Item1;    
//        var logger = _.Item2;

//#endif

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
