namespace DiskInfoDotnet.Related;

using HelperClass;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

[SomeElementsInfos($"class name {nameof(MainViewModule)} namespace {nameof(DiskInfoDotnet.Related)} learned from yck1509 class")]
public class MainViewModule : Task
{

#nullable disable
    public string SolutionDirectory { get; set; }
    public string GetOutPath { get; set; }
#nullable enable

    public override bool Execute()
    {
        //if (Attribute.IsDefined(typeof(MainViewModule), typeof(SomeElementsInfos)) && typeof(MainViewModule).GetCustomAttribute<SomeElementsInfos>() is not null and SomeElementsInfos someElementsAttr)
        //{
        //    File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "atext.txt"), $"{someElementsAttr.Details}");
        //}
        //else
        //{


        //}

        var directories = Directory.GetDirectories(GetOutPath);
        var netFourDirs = directories.ToList();

        var NetFrDirs = new List<string>();   

        foreach (var dir in netFourDirs)
        {
            var dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Name.StartsWith("net4"))
                continue;

            var CreatingDepDir = Path.Combine(dir, "Mydependencies");
            if (!Directory.Exists(CreatingDepDir))
            {
                Directory.CreateDirectory(CreatingDepDir);

                Log.LogMessage(MessageImportance.High, "Success full to create folder {0}", CreatingDepDir);
            }

            NetFrDirs.Add(dir);
            NetFrDirs.Add(CreatingDepDir);

            var netFrFiles = dirInfo.GetFiles();

            foreach (var dirFile in netFrFiles)
            {
                var asmName = dirFile.Name;

                if (!asmName.StartsWith("DiskInfoDotnet.Demo"))
                {
                    var MovingFile = Path.Combine(CreatingDepDir, dirFile.Name);
                    dirFile.MoveTo(MovingFile);

                    Log.LogMessage(MessageImportance.High, "Successfull to move to {0}", dirFile.FullName);

                }
            }

        }

        foreach (var dir in NetFrDirs)
        {
            var dirInfo = new DirectoryInfo(dir);
            var netFrFiles = dirInfo.GetFiles();
            foreach (var dirFile in netFrFiles)
            {

                var asmName = dirFile.Name;
                if (asmName.StartsWith("DiskInfoDotnet") && asmName.EndsWith(".exe") || asmName.StartsWith("DiskInfoDotnet") && asmName.EndsWith(".dll"))
                {
                    //Log.LogMessage(MessageImportance.High, "asdsad {0}", asmName);

                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe",
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true,
                        Arguments = @"-noe -c ""&{Import-Module """"""C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\Tools\Microsoft.VisualStudio.DevShell.dll""""""; Enter-VsDevShell 46b89eab}"""
                    };
                    //startInfo.Arguments += " && signtool";

                    Process devCmd = new Process();
                    devCmd.StartInfo = startInfo;
                    devCmd.Start();

                    //devCmd.StartInfo.Arguments += " && signtool";

                    var cPfxPath = "C:\\Users\\Admin\\Documents\\MyDrive\\MyStorecert.pfx";
                    var cPfxPass = "";

                    devCmd.StandardInput.WriteLine($"signtool.exe sign /f {cPfxPath} /p {cPfxPass} /fd SHA256 {dirFile.FullName}");
                    devCmd.StandardInput.Flush();
                    devCmd.StandardInput.Close();

                    while (!devCmd.StandardOutput.EndOfStream)
                    {
                        string line = devCmd.StandardOutput.ReadLine();
                        //Console.WriteLine(line);
                        // do something with line
                        Log.LogMessage(MessageImportance.High, line);
                    }

                    //Console.WriteLine(devCmd.StandardOutput.ReadToEnd());
                    devCmd.WaitForExit();


                    //Log.LogMessage(MessageImportance.High, "Successfull to move to {0}", dirFile.FullName);
                }
            }


        }

        return true;
    }

    //private static void Main( string[] args)
    //{
    //    new MainViewModule().Execute();
    //}
}
