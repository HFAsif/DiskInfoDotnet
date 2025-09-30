using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JunkCodes
{
    public class Class1
    {
        [STAThread]
        private static async Task MainEx(string[] args)
        {
            ExtractionType extractionType = ExtractionType.None;
            bool NativeTest = false;

            var options = new Cac.Options() { args = args };
            var cmParser = new Cmd.CommandLineParser() { cacOptions = options };
            cmParser.Parse(options);

            InfoExtractorClass extractorClass = new InfoExtractorClass()
            {
                args = args,
                extractionType = extractionType,
                NativeTest = false
            };

            using var loggerFactory = LoggerFactory.Create(static builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("DiskInfoArtificial.Test.Program", LogLevel.Debug)
                    .AddConsole();
            });

            var logger = loggerFactory.CreateLogger<Program>();

            extractorClass.ExtractSmManagement(out var SmmanagerList, out var loadMScopModule);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            ReadOnlyCollectionBuilder<object> readOnlyCollectionBuilder = [];
            object ataLists;
            string DiskInfoExtractedTime = string.Empty;

            Unsafe.SkipInit(out ataLists);

            if (extractionType == ExtractionType.StaticExtraction)
            {
                //CrystalDiskInfoDotnetLoad.ExtractOptimizedInfos(out var vals);
            }
            else if (extractionType == ExtractionType.None)
            {
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

                if (ataLists is not null)
                    readOnlyCollectionBuilder.Add(ataLists);

                //extractorClass.ExtractDiskInfoSecondWay(ref readOnlyCollectionBuilder);


            }
            else goto gettingError;
            if (ataLists is null) goto gettingError;

            //extractorClass.ExtractDiskInfoSecondWay(out var DiskInfoList, loadMScopModule, out var diskInfoExtractedTime);

            foreach (var item in SmmanagerList.Union(readOnlyCollectionBuilder))
            {
                if (item is IEnumerable objects and not null)
                {
                    var enumerator = objects.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        do
                        {
                            var current = enumerator.Current;
                            if (current is not null)
                            {
                                var json = JsonConvert.SerializeObject(current, Formatting.Indented);
                                logger.LogInformation(json);
                            }
                            await Task.Delay(500);
                        }
                        while (enumerator.MoveNext());
                    }
                }
                else if (item is not null && Attribute.IsDefined(item.GetType(), typeof(Win32_Attribute)))
                {
                    var json = JsonConvert.SerializeObject(item, Formatting.Indented);
                    logger.LogInformation(json);
                    await Task.Delay(500);
                }
                else
                {
                    throw new GettingExceptions(typeof(Program), "item is null or not valid type");
                }
            }

            Console.ReadLine();

        gettingError:
            throw new GettingExceptions(typeof(Program), "invalid operation");

        }


        static void ExtractSmManagement(ILoggerFactory loggerFactory, string[] args, out List<object> list)
        {
            list = new List<object>();
            var logger = loggerFactory.CreateLogger<Program>();

            LoadMScopModule loadMScopModule = new LoadMScopModule();
            if (!loadMScopModule.LoadInfos(false))
            {
                throw new HelperClass.GettingExceptions("Load Management Scops Failed");
            }

            var AllScopInfos = loadMScopModule.GetType().GetProperties();
            foreach (var prop in AllScopInfos)
            {
                if (prop is not null && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType.IsGenericType)
                {
                    if (Attribute.IsDefined(prop.PropertyType.GetGenericArguments().First(), typeof(Win32_Attribute)))
                    {
                        var val = prop.GetValue(loadMScopModule);

                        //ListWorker(args, string.Empty, val, genericType);

                        if (val is IEnumerable enumerable)
                        {
                            var enumerator = enumerable.GetEnumerator();
                            if (enumerator.MoveNext())
                            {
                                do
                                {
                                    var current = enumerator.Current;
                                    list.Add(current);

                                    //if (current is not null)
                                    //{
                                    //    logger.LogInformation(Environment.NewLine + Environment.NewLine);
                                    //    var json = JsonConvert.SerializeObject(current, Formatting.Indented);
                                    //    logger.LogInformation(json);
                                    //}
                                }
                                while (enumerator.MoveNext());

                            }


                        }
                    }

                }
            }

        }

        static void ExtractDiskInfo(ILoggerFactory loggerFactory, string[] args, out List<object> smManagerList)
        {
            ExtractionType extractionType = ExtractionType.None;

            if (extractionType == ExtractionType.NugetExtraction)
            {
                CrystalDiskInfoDotnetLoad.ExtractOptimizedInfos(out var vals, loadMScopModule.win32_DiskDrive_Infos_List, out var extractTime);
            }
            else
            {
                bool DiskInfosCheck = true;
                Unsafe.SkipInit(out DiskInfosCheck);

                if (DiskInfosCheck)
                {
                    var stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                    bool NativeTest = false;

                    if ((Attribute.GetCustomAttribute(typeof(MainEntry), typeof(HelperClass.WindowsVersionCheckerAttribute)) is HelperClass.WindowsVersionCheckerAttribute winvAttr && winvAttr is not null))
                    {
                        if (winvAttr.WindowsVesionChecker())
                        {
                            winvAttr.m_bNVMeStorageQuery = true;

                            MainEntry.Run(NativeTest, winvAttr, out var ataLists, loadMScopModule.win32_DiskDrive_Infos_List, args);

                            stopwatch.Stop();

                            var dcEnd = stopwatch.Elapsed;

                            //var wdInofsList = _win32_DiskDrive_Infos as List<DiskInfoDotnet.Library.Win32_DiskDrive_Infos>;

                            //logger.LogInformation(string.Join(Environment.NewLine, wdInofsList));
                            //System.Threading.Thread.Sleep(500);


                            logger.LogInformation($"Infos Extracted Ended in {dcEnd}");

                            var options = new Cac.Options() { args = args };
                            var cmParser = new Cmd.CommandLineParser() { cacOptions = options };


                            cmParser.Parse(options);

                            CrystalDiskInfoDotnetBase crystalDiskInfoDotnetBase = new FinishingClass()
                            {
                                DiskInfoArtificialEndedAt = dcEnd.ToString(),
                                logger = loggerFactory.CreateLogger<FinishingClass>(),
                                ataInfos = ataLists,
                                outPutInfos = cmParser.cacOptions.outPutInfos,
                            };

                            crystalDiskInfoDotnetBase.ExtracInformation();

                        }

                    }


                    //ICrystalDiskInfoDotnet crystalDiskInfoDotnet;
                    //crystalDiskInfoDotnet = new CrystalDiskInfoDotnetLoad()
                    //{
                    //    ataLists = ataLists,
                    //    DiskInfoArtificialEndedAt = dcEnd.ToString(),
                    //    logger = loggerFactory.CreateLogger<CrystalDiskInfoDotnetLoad>(),
                    //    Options = cmParser.cacOptions
                    //};
                    //crystalDiskInfoDotnet.LoadInformation();

                    //if (cmParser.cacOptions.outPutInfos == OutPutInfos.NugetTest)
                    //{
                    //    diskInfoArtificialManager = new CrystalDiskInfoDotnet() { ataLists = ataLists, 
                    //        DiskInfoArtificialEndedAt = dcEnd.ToString(), logger = loggerFactory.CreateLogger<CrystalDiskInfoDotnet>(), 
                    //        Options = cmParser.cacOptions
                    //    };

                    //    diskInfoArtificialManager.ExternalRun();
                    //}
                    //else
                    //{
                    //    diskInfoArtificialManager = new FinishingClass()
                    //    {
                    //        Options = cmParser.cacOptions,
                    //        logger = loggerFactory.CreateLogger<FinishingClass>(),
                    //        DiskInfoArtificialEndedAt = dcEnd.ToString(),
                    //        ataLists = ataLists,
                    //        args = args
                    //    };

                    //    diskInfoArtificialManager.ExternalRun();
                    //}

                }
                else
                {
                    //IDiskInfoArtificialManager diskInfoArtificialManager = new IntelLibrary() { args = args };
                    //diskInfoArtificialManager.ExternalRun();
                }

            }
        }


        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                ExtractionType extractionType = ExtractionType.NugetExtraction;

                LoadMScopModule loadMScopModule = new LoadMScopModule();
                if (!loadMScopModule.LoadInfos(false))
                {
                    throw new HelperClass.GettingExceptions("Load Management Scops Failed");
                }

                //var AllScopInfos = loadMScopModule.GetType().GetProperties();
                //foreach (var prop in AllScopInfos)
                //{
                //    if (prop is not null && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType.IsGenericType)
                //    {
                //        var genericType = prop.PropertyType.GetGenericArguments()[0] as dynamic;

                //        if (genericType is not null)
                //        {
                //            var val = prop.GetValue(loadMScopModule);
                //            if (val is IEnumerable enumerable)
                //            {
                //                var enumerator = enumerable.GetEnumerator();
                //                if (enumerator.MoveNext())
                //                {
                //                    do
                //                    {
                //                        var current = enumerator.Current;
                //                        if (current is not null)
                //                        {
                //                            var crPrt = current.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                //                            if (crPrt is not null && crPrt.Length > 0)
                //                            {

                //                            }
                //                        }
                //                    }
                //                    while (enumerator.MoveNext());
                //                }


                //            }
                //        }

                //    }
                //    //if (prop is not null && prop.PropertyType == typeof(ObservableCollection<Win32_DiskDrive_Infos>))
                //    //{
                //    //    var val = prop.GetValue(loadMScopModule);
                //    //    if (val is ObservableCollection<Win32_DiskDrive_Infos> win32_DiskDrive_Infos_List && win32_DiskDrive_Infos_List.Count > 0)
                //    //    {
                //    //        extractionType = ExtractionType.NugetExtraction;
                //    //        break;
                //    //    }
                //    //}
                //}

                if (extractionType == ExtractionType.NugetExtraction)
                {
                    var ataLists = CrystalDiskInfoDotnetLoad.ExtractOptimizedInfos(out var vals, loadMScopModule.win32_DiskDrive_Infos_List, out var extractTime);

                    var dcEnd = extractTime;

                    var options = new Cac.Options() { args = args };
                    var cmParser = new Cmd.CommandLineParser() { cacOptions = options };
                    cmParser.Parse(options);

                    CrystalDiskInfoDotnetBase crystalDiskInfoDotnetBase = new FinishingClass()
                    {
                        DiskInfoArtificialEndedAt = dcEnd.ToString(),
                        //logger = loggerFactory.CreateLogger<FinishingClass>(),
                        ataInfos = ataLists,
                        outPutInfos = cmParser.cacOptions.outPutInfos,
                    };

                    crystalDiskInfoDotnetBase.ExtracInformation();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadLine();

        }
    }
}
