using CrystalDiskInfoDotnet;
using CrystalDiskInfoDotnet.CheckDiskInfos;
using DiskInfoDotnet;
using DiskInfoDotnet.Demo.NetAll;
using Os.Management;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

internal class Program
{
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
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
       
        Console.ReadLine();

    }
}