using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using DiskInfoDotnet;
using DiskInfoDotnet.Demo;
using DiskInfoDotnet.ImportantLib.Shared;
using Microsoft.Extensions.Logging;
using Os.Management;
using System;
using System.Runtime.CompilerServices;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ExtractionType extractionType = ExtractionType.None;

        LoadMScopModule loadMScopModule = new LoadMScopModule();
        if (!loadMScopModule.LoadInfos(false))
        {
            throw new HelperClass.GettingExceptions("Load Management Scops Failed");
        }

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

                        using var loggerFactory = LoggerFactory.Create(static builder =>
                        {
                            builder
                                .AddFilter("Microsoft", LogLevel.Warning)
                                .AddFilter("System", LogLevel.Warning)
                                .AddFilter("DiskInfoArtificial.Test.Program", LogLevel.Debug)
                                .AddConsole();
                        });

                        var logger = loggerFactory.CreateLogger<Program>();

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
            Console.ReadLine();
        }
    }
}